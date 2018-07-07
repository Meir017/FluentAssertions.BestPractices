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
    public class CollectionShouldOnlyHaveUniqueItemsAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldOnlyHaveUniqueItems;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().OnlyHaveUniqueItems() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldHaveSameCountThisCollectionDistinctSyntaxVisitor();
            }
        }

        public class ShouldHaveSameCountThisCollectionDistinctSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldHaveSameCountThisCollectionDistinctSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("HaveSameCount", ArgumentInvokesDistinctMethod))
            {
            }

            private static bool ArgumentInvokesDistinctMethod(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                if (arguments.First().Expression is InvocationExpressionSyntax invocation)
                {
                    var visitor = new FluentAssertionsCSharpSyntaxVisitor(new MemberValidator(nameof(Enumerable.Distinct)));
                    invocation.Accept(visitor);

                    return visitor.IsValid(invocation) && VariableNameExtractor.ExtractVariabeName(arguments.First()) == VariableNameExtractor.ExtractVariabeName(invocation);
                }
                return false;
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldOnlyHaveUniqueItemsCodeFix)), Shared]
    public class CollectionShouldOnlyHaveUniqueItemsCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldOnlyHaveUniqueItemsAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.RenameAndRemoveFirstArgument("HaveSameCount", "OnlyHaveUniqueItems"));
        }
    }
}
