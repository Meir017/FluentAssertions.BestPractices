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
    public class CollectionShouldHaveCountGreaterThanAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveCountGreaterThan;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveCountGreaterThan() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldBeGreaterThanSyntaxVisitor();
            }
        }

        public class CountShouldBeGreaterThanSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBeGreaterThanSyntaxVisitor() : base(new MemberValidator("Count"), MemberValidator.Should, new MemberValidator("BeGreaterThan"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveCountGreaterThanCodeFix)), Shared]
    public class CollectionShouldHaveCountGreaterThanCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveCountGreaterThanAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("BeGreaterThan", "HaveCountGreaterThan"));
        }
    }
}
