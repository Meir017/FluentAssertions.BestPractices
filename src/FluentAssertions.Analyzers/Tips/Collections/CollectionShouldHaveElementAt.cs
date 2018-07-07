using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldHaveElementAtAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveElementAt;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveElementAt() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ElementAtIndexShouldBeSyntaxVisitor();
                yield return new IndexerShouldBeSyntaxVisitor();
                yield return new SkipFirstShouldBeSyntaxVisitor();
            }
        }

        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type)
        {
            if (type.AllInterfaces.Any(@interface => @interface.Name == "IReadOnlyDictionary" || @interface.Name == "IDictionary"))
            {
                return false;
            }

            return base.ShouldAnalyzeVariableType(type);
        }

        public class ElementAtIndexShouldBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ElementAtIndexShouldBeSyntaxVisitor() : base(new MemberValidator("ElementAt"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }

        public class IndexerShouldBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public IndexerShouldBeSyntaxVisitor() : base(new MemberValidator("[]"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }

        public class SkipFirstShouldBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public SkipFirstShouldBeSyntaxVisitor() : base(new MemberValidator("Skip"), MemberValidator.MathodNotContainingLambda("First"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveElementAtCodeFix)), Shared]
    public class CollectionShouldHaveElementAtCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveElementAtAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(CollectionShouldHaveElementAtAnalyzer.ElementAtIndexShouldBeSyntaxVisitor))
            {
                var remove = NodeReplacement.RemoveAndExtractArguments("ElementAt");
                var newExpression = GetNewExpression(expression, remove);

                return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("Be", "HaveElementAt", remove.Arguments));
            }
            else if (properties.VisitorName == nameof(CollectionShouldHaveElementAtAnalyzer.IndexerShouldBeSyntaxVisitor))
            {
                var remove = NodeReplacement.RemoveAndRetrieveIndexerArguments("Should");
                var newExpression = GetNewExpression(expression, remove);

                return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("Be", "HaveElementAt", remove.Arguments));
            }
            else if (properties.VisitorName == nameof(CollectionShouldHaveElementAtAnalyzer.SkipFirstShouldBeSyntaxVisitor))
            {
                var remove = NodeReplacement.RemoveAndExtractArguments("Skip");
                var newExpression = GetNewExpression(expression, remove, NodeReplacement.Remove("First"));

                return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("Be", "HaveElementAt", remove.Arguments));
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }
    }
}
