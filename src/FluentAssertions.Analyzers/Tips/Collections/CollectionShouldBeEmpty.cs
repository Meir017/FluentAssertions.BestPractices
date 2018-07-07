using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldBeEmptyAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionsShouldBeEmpty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AnyShouldBeFalseSyntaxVisitor();
                yield return new ShouldHaveCount0SyntaxVisitor();
            }
        }

        public class AnyShouldBeFalseSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public AnyShouldBeFalseSyntaxVisitor() : base(MemberValidator.MathodNotContainingLambda("Any"), MemberValidator.Should, new MemberValidator("BeFalse"))
            {
            }
        }

        public class ShouldHaveCount0SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldHaveCount0SyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("HaveCount", HaveCountArgumentsValidator))
            {
            }

            private static bool HaveCountArgumentsValidator(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                return arguments[0].Expression is LiteralExpressionSyntax literal
                    && literal.Token.Value is int argument
                    && argument == 0;
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldBeEmptyCodeFix)), Shared]
    public class CollectionShouldBeEmptyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldBeEmptyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            switch (properties.VisitorName)
            {
                case nameof(CollectionShouldBeEmptyAnalyzer.AnyShouldBeFalseSyntaxVisitor):
                    return GetNewExpression(expression, NodeReplacement.Remove("Any"), NodeReplacement.Rename("BeFalse", "BeEmpty"));
                case nameof(CollectionShouldBeEmptyAnalyzer.ShouldHaveCount0SyntaxVisitor):
                    return GetNewExpression(expression, new HaveCountNodeReplacement());
                default:
                    throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
            }
        }

        private class HaveCountNodeReplacement : NodeReplacement
        {
            public override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Name.Identifier.Text == "HaveCount";
            public override SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Parent;
            public override SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode)
            {
                var invocation = (InvocationExpressionSyntax)listNode.Value.Parent;

                invocation = invocation.ReplaceNode(listNode.Value, listNode.Value.WithName(SyntaxFactory.IdentifierName("BeEmpty")));

                // remove the 0 argument
                var arguments = invocation.ArgumentList.Arguments.RemoveAt(0);

                return invocation.WithArgumentList(invocation.ArgumentList.WithArguments(arguments));
            }
        }
    }
}
