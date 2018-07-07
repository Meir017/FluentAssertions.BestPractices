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
    public class CollectionShouldEqualOtherCollectionByComparerAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldEqualOtherCollectionByComparer;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().Equal() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new SelectShouldEqualOtherCollectionSelectSyntaxVisitor();
            }
        }

        public class SelectShouldEqualOtherCollectionSelectSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public SelectShouldEqualOtherCollectionSelectSyntaxVisitor()
                : base(MemberValidator.MathodContainingLambda("Select"), MemberValidator.Should, new MemberValidator("Equal", MathodContainingArgumentInvokingLambda))
            {
            }

            private static bool MathodContainingArgumentInvokingLambda(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                return arguments[0].Expression is InvocationExpressionSyntax invocation
                    && MemberValidator.MethodContainingLambdaPredicate(invocation.ArgumentList.Arguments);
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldEqualOtherCollectionByComparerCodeFix)), Shared]
    public class CollectionShouldEqualOtherCollectionByComparerCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldEqualOtherCollectionByComparerAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var removeMethodContainingFirstLambda = NodeReplacement.RemoveAndExtractArguments("Select");
            var newExpression = GetNewExpression(expression, removeMethodContainingFirstLambda);

            var removeArgument = NodeReplacement.RemoveFirstArgument("Equal");
            newExpression = GetNewExpression(newExpression, removeArgument);

            var argumentInvocation = (InvocationExpressionSyntax)removeArgument.Argument.Expression;
            var identifier = ((MemberAccessExpressionSyntax)argumentInvocation.Expression).Expression;

            var firstLambda = (SimpleLambdaExpressionSyntax)removeMethodContainingFirstLambda.Arguments[0].Expression;
            var secondLambda = (SimpleLambdaExpressionSyntax)argumentInvocation.ArgumentList.Arguments[0].Expression;

            var newArguments = SyntaxFactory.SeparatedList<ArgumentSyntax>()
                .Add(removeArgument.Argument.WithExpression(identifier))
                .Add(removeArgument.Argument.WithExpression(CombineLambdas(firstLambda, secondLambda).NormalizeWhitespace()
            ));

            return GetNewExpression(newExpression, NodeReplacement.PrependArguments("Equal", newArguments));
        }

        private ParenthesizedLambdaExpressionSyntax CombineLambdas(SimpleLambdaExpressionSyntax left, SimpleLambdaExpressionSyntax right) => SyntaxFactory.ParenthesizedLambdaExpression(
            parameterList: SyntaxFactory.ParameterList().AddParameters(left.Parameter, right.Parameter),
            body: SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression,
                left: (ExpressionSyntax)left.Body,
                right: (ExpressionSyntax)right.Body)
        );
    }
}
