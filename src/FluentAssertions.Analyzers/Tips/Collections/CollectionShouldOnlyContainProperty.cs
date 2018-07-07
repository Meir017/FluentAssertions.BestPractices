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
    public class CollectionShouldOnlyContainPropertyAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldOnlyContainProperty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().OnlyContain() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AllShouldBeTrueSyntaxVisitor();
            }
        }

        public class AllShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public AllShouldBeTrueSyntaxVisitor() : base(MemberValidator.MathodContainingLambda("All"), MemberValidator.Should, new MemberValidator("BeTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldOnlyContainPropertyCodeFix)), Shared]
    public class CollectionShouldOnlyContainPropertyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldOnlyContainPropertyAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("All");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeTrue", "OnlyContain", remove.Arguments));
        }
    }
}
