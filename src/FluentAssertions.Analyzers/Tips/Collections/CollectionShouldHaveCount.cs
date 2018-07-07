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
    public class CollectionShouldHaveCountAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveCount;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveCount() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldBe0SyntaxVisitor();
                yield return new CountShouldBe1SyntaxVisitor();
                yield return new CountShouldBeSyntaxVisitor();
            }
        }

        public class CountShouldBe0SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBe0SyntaxVisitor() : base(MemberValidator.MathodNotContainingLambda("Count"), MemberValidator.Should, MemberValidator.ArgumentIsLiteral("Be", 0))
            {
            }
        }

        public class CountShouldBe1SyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBe1SyntaxVisitor() : base(MemberValidator.MathodNotContainingLambda("Count"), MemberValidator.Should, MemberValidator.ArgumentIsLiteral("Be", 1))
            {
            }
        }

        public class CountShouldBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBeSyntaxVisitor() : base(MemberValidator.MathodNotContainingLambda("Count"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveCountCodeFix)), Shared]
    public class CollectionShouldHaveCountCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveCountAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(CollectionShouldHaveCountAnalyzer.CountShouldBe0SyntaxVisitor))
            {
                return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.RenameAndRemoveFirstArgument("Be", "BeEmpty"));
            }
            else if (properties.VisitorName == nameof(CollectionShouldHaveCountAnalyzer.CountShouldBe1SyntaxVisitor))
            {
                return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.RenameAndRemoveFirstArgument("Be", "ContainSingle"));
            }
            else if (properties.VisitorName == nameof(CollectionShouldHaveCountAnalyzer.CountShouldBeSyntaxVisitor))
            {
                return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("Be", "HaveCount"));
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }
    }
}
