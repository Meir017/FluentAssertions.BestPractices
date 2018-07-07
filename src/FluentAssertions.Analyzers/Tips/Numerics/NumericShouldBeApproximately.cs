using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NumericShouldBeApproximatelyAnalyzer : NumericAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Numeric.NumericShouldBeApproximately;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .BeApproximately() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new MathAbsShouldBeLessOrEqualToSyntaxVisitor();
            }
        }

        private static readonly string[] ValidaTypeNames = { "double", "decimal", "float" };
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type)
            => ValidaTypeNames.Contains(type.ToDisplayString(), StringComparer.OrdinalIgnoreCase);

        public class MathAbsShouldBeLessOrEqualToSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public MathAbsShouldBeLessOrEqualToSyntaxVisitor() : base(new MemberValidator("Abs", IsSubtractExpression), MemberValidator.Should, new MemberValidator("BeLessOrEqualTo"))
            {
            }

            private static bool IsSubtractExpression(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (arguments.Count != 1) return false;

                return arguments[0].Expression is BinaryExpressionSyntax subtractExpression
                    && subtractExpression.IsKind(SyntaxKind.SubtractExpression)
                    && subtractExpression.Right.IsKind(SyntaxKind.IdentifierName);
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NumericShouldBeApproximatelyCodeFix)), Shared]
    public class NumericShouldBeApproximatelyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NumericShouldBeApproximatelyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Abs");
            var newExpression = GetNewExpression(expression, remove);

            var subtractExpression = (BinaryExpressionSyntax)remove.Arguments[0].Expression;

            var actual = subtractExpression.Right as IdentifierNameSyntax;
            var expected = subtractExpression.Left;

            newExpression = GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeLessOrEqualTo", "BeApproximately", new SeparatedSyntaxList<ArgumentSyntax>().Add(SyntaxFactory.Argument(expected))));

            newExpression = RenameIdentifier(newExpression, "Math", actual.Identifier.Text);

            return newExpression;
        }
    }
}