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
    public class CollectionShouldContainItemAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldContainItem;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should()Contain() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ContainsShouldBeTrueSyntaxVisitor();
            }
        }

        public class ContainsShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ContainsShouldBeTrueSyntaxVisitor() : base(new MemberValidator("Contains"), MemberValidator.Should, new MemberValidator("BeTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldContainItemCodeFix)), Shared]
    public class CollectionShouldContainItemCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldContainItemAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("Contains");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeTrue", "Contain", remove.Arguments));
        }
    }
}
