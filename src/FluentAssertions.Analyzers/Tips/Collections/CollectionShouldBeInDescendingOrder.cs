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
    public class CollectionShouldBeInDescendingOrderAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldBeInDescendingOrder;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeInDescendingOrder() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new OrderByDescendingShouldEqualSyntaxVisitor();
            }
        }

        public class OrderByDescendingShouldEqualSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public OrderByDescendingShouldEqualSyntaxVisitor() : base(MemberValidator.MathodContainingLambda("OrderByDescending"), MemberValidator.Should, MemberValidator.ArgumentIsVariable("Equal"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldBeInDescendingOrderCodeFix)), Shared]
    public class CollectionShouldBeInDescendingOrderCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldBeInDescendingOrderAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("OrderByDescending");
            var newExpression = GetNewExpression(expression, remove);

            newExpression = GetNewExpression(newExpression, NodeReplacement.RenameAndRemoveFirstArgument("Equal", "BeInDescendingOrder"));

            return GetNewExpression(newExpression, NodeReplacement.PrependArguments("BeInDescendingOrder", remove.Arguments));
        }
    }
}
