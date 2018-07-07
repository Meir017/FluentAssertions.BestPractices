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
    public class CollectionShouldBeInAscendingOrderAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldBeInAscendingOrder;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeInAscendingOrder() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new OrderByShouldEqualSyntaxVisitor();
            }
        }

        public class OrderByShouldEqualSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public OrderByShouldEqualSyntaxVisitor() : base(MemberValidator.MathodContainingLambda("OrderBy"), MemberValidator.Should, MemberValidator.ArgumentIsVariable("Equal"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldBeInAscendingOrderCodeFix)), Shared]
    public class CollectionShouldBeInAscendingOrderCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldBeInAscendingOrderAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("OrderBy");
            var newExpression = GetNewExpression(expression, remove);

            newExpression = GetNewExpression(newExpression, NodeReplacement.RenameAndRemoveFirstArgument("Equal", "BeInAscendingOrder"));

            return GetNewExpression(newExpression, NodeReplacement.PrependArguments("BeInAscendingOrder", remove.Arguments));
        }
    }
}
