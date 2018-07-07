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
    public class CollectionShouldHaveCountGreaterOrEqualToAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveCountGreaterOrEqualTo;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveCountGreaterOrEqualTo() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CountShouldBeGreaterOrEqualToSyntaxVisitor();
            }
        }

        public class CountShouldBeGreaterOrEqualToSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public CountShouldBeGreaterOrEqualToSyntaxVisitor() : base(new MemberValidator("Count"), MemberValidator.Should, new MemberValidator("BeGreaterOrEqualTo"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveCountGreaterOrEqualToCodeFix)), Shared]
    public class CollectionShouldHaveCountGreaterOrEqualToCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveCountGreaterOrEqualToAnalyzer.DiagnosticId);
        
        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return GetNewExpression(expression, NodeReplacement.Remove("Count"), NodeReplacement.Rename("BeGreaterOrEqualTo", "HaveCountGreaterOrEqualTo"));
        }
    }
}
