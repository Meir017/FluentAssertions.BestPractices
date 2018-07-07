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
    public class StringShouldEndWithAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldEndWith;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().EndWith() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new EndWithShouldBeTrueSyntaxVisitor();
            }
        }

        public class EndWithShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public EndWithShouldBeTrueSyntaxVisitor() : base(new MemberValidator("EndsWith"), MemberValidator.Should, new MemberValidator("BeTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldEndWithCodeFix)), Shared]
    public class StringShouldEndWithCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldEndWithAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("EndsWith");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeTrue", "EndWith", remove.Arguments));
        }
    }
}