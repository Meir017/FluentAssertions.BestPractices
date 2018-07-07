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
    public class NumericShouldBeNegativeAnalyzer : NumericAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Numeric.NumericShouldBeNegative;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .BeNegative() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new NumericShouldBeBeLessThan0SyntaxVisitor();
            }
        }

        public class NumericShouldBeBeLessThan0SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public NumericShouldBeBeLessThan0SyntaxVisitor() : base(MemberValidator.Should, MemberValidator.ArgumentIsLiteral("BeLessThan", 0))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NumericShouldBeNegativeCodeFix)), Shared]
    public class NumericShouldBeNegativeCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NumericShouldBeNegativeAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.RenameAndRemoveFirstArgument("BeLessThan", "BeNegative"));
        }
    }
}