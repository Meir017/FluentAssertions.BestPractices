using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldOnlyHaveUniqueItemsByComparerAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldOnlyHaveUniqueItemsByComparer;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().OnlyHaveUniqueItems() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new SelectShouldOnlyHaveUniqueItemsSyntaxVisitor();
            }
        }

        public class SelectShouldOnlyHaveUniqueItemsSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public SelectShouldOnlyHaveUniqueItemsSyntaxVisitor() : base(MemberValidator.MathodContainingLambda("Select"), MemberValidator.Should, new MemberValidator("OnlyHaveUniqueItems"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldOnlyHaveUniqueItemsByComparerCodeFix)), Shared]
    public class CollectionShouldOnlyHaveUniqueItemsByComparerCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldOnlyHaveUniqueItemsByComparerAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Select");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.PrependArguments("OnlyHaveUniqueItems", remove.Arguments));
        }
    }
}
