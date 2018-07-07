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
    public class CollectionShouldHaveCountLessOrEqualToAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveCountLessOrEqualTo;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveCountLessOrEqualTo() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldBeLessOrEqualToSyntaxVisitor();
            }
        }
        public class CountShouldBeLessOrEqualToSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBeLessOrEqualToSyntaxVisitor() : base(new MemberValidator("Count"), MemberValidator.Should, new MemberValidator("BeLessOrEqualTo"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveCountLessOrEqualToCodeFix)), Shared]
    public class CollectionShouldHaveCountLessOrEqualToCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveCountLessOrEqualToAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("BeLessOrEqualTo", "HaveCountLessOrEqualTo"));
        }
    }
}