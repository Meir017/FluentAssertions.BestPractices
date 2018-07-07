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
    public class CollectionShouldNotContainNullsAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotContainNulls;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should()### instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new SelectShouldNotContainNullsSyntaxVisitor();
            }
        }

        public class SelectShouldNotContainNullsSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public SelectShouldNotContainNullsSyntaxVisitor() : base(MemberValidator.MathodContainingLambda("Select"), MemberValidator.Should, new MemberValidator("NotContainNulls"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotContainNullsCodeFix)), Shared]
    public class CollectionShouldNotContainNullsCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotContainNullsAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Select");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.PrependArguments("NotContainNulls", remove.Arguments));
        }
    }
}
