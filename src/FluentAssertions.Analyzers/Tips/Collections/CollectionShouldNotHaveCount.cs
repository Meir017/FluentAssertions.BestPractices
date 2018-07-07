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
    public class CollectionShouldNotHaveCountAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotHaveCount;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotHaveCount() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldNotBeSyntaxVisitor();
            }
        }

        public class CountShouldNotBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldNotBeSyntaxVisitor() : base(MemberValidator.HasNoArguments("Count"), MemberValidator.Should, MemberValidator.ArgumentIsIdentifierOrLiteral("NotBe"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotHaveCountCodeFix)), Shared]
    public class CollectionShouldNotHaveCountCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotHaveCountAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("NotBe", "NotHaveCount"));
        }
    }
}
