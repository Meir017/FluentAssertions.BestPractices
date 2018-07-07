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
    public class CollectionShouldHaveSameCountAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveSameCount;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveSameCount() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldHaveCountOtherCollectionCountSyntaxVisitor();
            }
        }

        public class ShouldHaveCountOtherCollectionCountSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldHaveCountOtherCollectionCountSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("HaveCount", HasArgumentInvokingCountMethod))
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

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveSameCountCodeFix)), Shared]
    public class CollectionShouldHaveSameCountCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveSameCountAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.RenameAndRemoveInvocationOfMethodOnFirstArgument("HaveCount", "HaveSameCount"));
        }
    }
}
