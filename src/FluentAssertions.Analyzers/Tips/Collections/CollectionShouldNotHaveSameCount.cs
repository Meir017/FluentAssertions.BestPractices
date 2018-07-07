using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldNotHaveSameCountAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotHaveSameCount;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotHaveSameCount() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldNotBeOtherCollectionCountSyntaxVisitor();
            }
        }

        public class CountShouldNotBeOtherCollectionCountSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldNotBeOtherCollectionCountSyntaxVisitor() : base(MemberValidator.HasNoArguments("Count"), MemberValidator.Should, new MemberValidator("NotBe", HasArgumentInvokingCountMethod))
            {
            }

            private static bool HasArgumentInvokingCountMethod(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                return arguments.First().Expression is InvocationExpressionSyntax invocation
                    && invocation.Expression is MemberAccessExpressionSyntax memberAccess
                    && memberAccess.Name.Identifier.Text == nameof(Enumerable.Count)
                    && memberAccess.Expression is IdentifierNameSyntax;

            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotHaveSameCountCodeFix)), Shared]
    public class CollectionShouldNotHaveSameCountCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotHaveSameCountAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression,
                NodeReplacement.Remove("Count"),
                NodeReplacement.RenameAndRemoveInvocationOfMethodOnFirstArgument("NotBe", "NotHaveSameCount")
            );
        }
    }
}
