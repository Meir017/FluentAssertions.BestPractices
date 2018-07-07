using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StringShouldNotBeNullOrWhiteSpaceAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldNotBeNullOrWhiteSpace;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .NotBeNullOrWhiteSpace() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new StringShouldNotBeNullOrWhiteSpaceSyntaxVisitor();
            }
        }

        public class StringShouldNotBeNullOrWhiteSpaceSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public StringShouldNotBeNullOrWhiteSpaceSyntaxVisitor() : base(new MemberValidator("IsNullOrWhiteSpace"), MemberValidator.Should, new MemberValidator("BeFalse"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldNotBeNullOrWhiteSpaceCodeFix)), Shared]
    public class StringShouldNotBeNullOrWhiteSpaceCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldNotBeNullOrWhiteSpaceAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("IsNullOrWhiteSpace");
            var newExpression = GetNewExpression(expression, remove);

            var rename = NodeReplacement.Rename("BeFalse", "NotBeNullOrWhiteSpace");
            newExpression = GetNewExpression(newExpression, rename);

            var stringKeyword = newExpression.DescendantNodes().OfType<PredefinedTypeSyntax>().Single();
            var subject = remove.Arguments.First().Expression;

            return newExpression.ReplaceNode(stringKeyword, subject.WithTriviaFrom(stringKeyword));
        }
    }
}