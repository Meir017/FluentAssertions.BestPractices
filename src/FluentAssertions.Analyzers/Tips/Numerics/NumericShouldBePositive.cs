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
    public class NumericShouldBePositiveAnalyzer : NumericAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Numeric.NumericShouldBePositive;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .BePositive() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new NumericShouldBeBeGreaterThan0SyntaxVisitor();
            }
        }

        public class NumericShouldBeBeGreaterThan0SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public NumericShouldBeBeGreaterThan0SyntaxVisitor() : base(MemberValidator.Should, MemberValidator.ArgumentIsLiteral("BeGreaterThan", 0))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NumericShouldBePositiveCodeFix)), Shared]
    public class NumericShouldBePositiveCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NumericShouldBePositiveAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.RenameAndRemoveFirstArgument("BeGreaterThan", "BePositive"));
        }
    }
}