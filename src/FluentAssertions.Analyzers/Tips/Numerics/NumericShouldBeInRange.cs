
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
    public class NumericShouldBeInRangeAnalyzer : NumericAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Numeric.NumericShouldBeInRange;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .BeInRange() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new BeGreaterOrEqualToAndBeLessOrEqualToSyntaxVisitor();
                yield return new BeLessOrEqualToAndBeGreaterOrEqualToSyntaxVisitor();
            }
        }

        public class BeGreaterOrEqualToAndBeLessOrEqualToSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public BeGreaterOrEqualToAndBeLessOrEqualToSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("BeGreaterOrEqualTo"), MemberValidator.And, new MemberValidator("BeLessOrEqualTo"))
            {
            }
        }
        public class BeLessOrEqualToAndBeGreaterOrEqualToSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public BeLessOrEqualToAndBeGreaterOrEqualToSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("BeLessOrEqualTo"), MemberValidator.And, new MemberValidator("BeGreaterOrEqualTo"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NumericShouldBeInRangeCodeFix)), Shared]
    public class NumericShouldBeInRangeCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NumericShouldBeInRangeAnalyzer.DiagnosticId);

        protected override bool CanRewriteAssertion(ExpressionSyntax expression)
        {
            var visitor = new MemberAccessExpressionsCSharpSyntaxVisitor();
            expression.Accept(visitor);

            var beLessOrEqualTo = visitor.Members.Find(member => member.Name.Identifier.Text == "BeLessOrEqualTo");
            var beGreaterOrEqualTo = visitor.Members.Find(member => member.Name.Identifier.Text == "BeGreaterOrEqualTo");

            return !(beLessOrEqualTo.Parent is InvocationExpressionSyntax beLessOrEqualToInvocation && beLessOrEqualToInvocation.ArgumentList.Arguments.Count > 1
                && beGreaterOrEqualTo.Parent is InvocationExpressionSyntax beGreaterOrEqualToInvocation && beGreaterOrEqualToInvocation.ArgumentList.Arguments.Count > 1);
        }

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(NumericShouldBeInRangeAnalyzer.BeGreaterOrEqualToAndBeLessOrEqualToSyntaxVisitor))
            {
                var removeLess = NodeReplacement.RemoveAndExtractArguments("BeLessOrEqualTo");
                var newExpression = GetNewExpression(expression, removeLess);

                var renameGreater = NodeReplacement.RenameAndExtractArguments("BeGreaterOrEqualTo", "BeInRange");
                newExpression = GetNewExpression(newExpression, renameGreater);

                var arguments = renameGreater.Arguments.InsertRange(1, removeLess.Arguments);

                return GetNewExpression(newExpression, NodeReplacement.WithArguments("BeInRange", arguments));
            }
            else if (properties.VisitorName == nameof(NumericShouldBeInRangeAnalyzer.BeLessOrEqualToAndBeGreaterOrEqualToSyntaxVisitor))
            {
                var removeGreater = NodeReplacement.RemoveAndExtractArguments("BeGreaterOrEqualTo");
                var newExpression = GetNewExpression(expression, removeGreater);

                var renameLess = NodeReplacement.RenameAndExtractArguments("BeLessOrEqualTo", "BeInRange");
                newExpression = GetNewExpression(newExpression, renameLess);

                var arguments = removeGreater.Arguments.InsertRange(1, renameLess.Arguments);

                return GetNewExpression(newExpression, NodeReplacement.WithArguments("BeInRange", arguments));
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }
    }
}