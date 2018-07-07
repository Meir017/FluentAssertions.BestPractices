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
    public class CollectionShouldNotIntersectWithAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotIntersectWith;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotIntersectWith() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new IntersectShouldBeEmptySyntaxVisitor();
            }
        }
        public class IntersectShouldBeEmptySyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public IntersectShouldBeEmptySyntaxVisitor() : base(MemberValidator.HasArguments("Intersect"), MemberValidator.Should, new MemberValidator("BeEmpty"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotIntersectWithCodeFix)), Shared]
    public class CollectionShouldNotIntersectWithCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotIntersectWithAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Intersect");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeEmpty", "NotIntersectWith", remove.Arguments));
        }
    }
}
