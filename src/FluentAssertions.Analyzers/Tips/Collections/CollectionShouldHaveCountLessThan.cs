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
    public class CollectionShouldHaveCountLessThanAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveCountLessThan;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveCountLessThan() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldBeLessThanSyntaxVisitor();
            }
        }

        public class CountShouldBeLessThanSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBeLessThanSyntaxVisitor() : base(new MemberValidator("Count"), MemberValidator.Should, new MemberValidator("BeLessThan"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveCountLessThanCodeFix)), Shared]
    public class CollectionShouldHaveCountLessThanCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveCountLessThanAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("BeLessThan", "HaveCountLessThan"));
        }
    }
}
