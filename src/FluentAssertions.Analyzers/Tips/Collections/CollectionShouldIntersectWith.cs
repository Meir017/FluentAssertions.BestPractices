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
    public class CollectionShouldIntersectWithAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldIntersectWith;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().IntersectWith() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new IntersectShouldNotBeEmptySyntaxVisitor();
            }
        }

        public class IntersectShouldNotBeEmptySyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public IntersectShouldNotBeEmptySyntaxVisitor() : base(MemberValidator.HasArguments("Intersect"), MemberValidator.Should, new MemberValidator("NotBeEmpty"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldIntersectWithCodeFix)), Shared]
    public class CollectionShouldIntersectWithCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldIntersectWithAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Intersect");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("NotBeEmpty", "IntersectWith", remove.Arguments));
        }
    }
}
