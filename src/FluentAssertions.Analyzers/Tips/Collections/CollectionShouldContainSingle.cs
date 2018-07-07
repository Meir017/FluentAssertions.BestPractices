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
    public class CollectionShouldContainSingleAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldContainSingle;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().ContainSingle() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new WhereShouldHaveCount1SyntaxVisitor();
                yield return new ShouldHaveCount1SyntaxVisitor();
            }
        }

        public class WhereShouldHaveCount1SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public WhereShouldHaveCount1SyntaxVisitor() : base(MemberValidator.MathodContainingLambda("Where"), MemberValidator.Should, MemberValidator.ArgumentIsLiteral("HaveCount", 1))
            {
            }
        }

        public class ShouldHaveCount1SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldHaveCount1SyntaxVisitor() : base(MemberValidator.Should, MemberValidator.ArgumentIsLiteral("HaveCount", 1))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldContainSingleCodeFix)), Shared]
    public class CollectionShouldContainSingleCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldContainSingleAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var newExpression = GetNewExpression(expression, NodeReplacement.RenameAndRemoveFirstArgument("HaveCount", "ContainSingle"));
            if (properties.VisitorName == nameof(CollectionShouldContainSingleAnalyzer.ShouldHaveCount1SyntaxVisitor))
            {
                return newExpression;
            }
            else if (properties.VisitorName == nameof(CollectionShouldContainSingleAnalyzer.WhereShouldHaveCount1SyntaxVisitor))
            {
                var remove = NodeReplacement.RemoveAndExtractArguments("Where");
                newExpression = GetNewExpression(newExpression, remove);

                return GetNewExpression(newExpression, NodeReplacement.PrependArguments("ContainSingle", remove.Arguments));
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }
    }
}
