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
    public class StringShouldBeNullOrWhiteSpaceAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldBeNullOrWhiteSpace;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .IsNullOrWhiteSpace() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new StringShouldBeNullOrWhiteSpaceSyntaxVisitor();
            }
        }

        public class StringShouldBeNullOrWhiteSpaceSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public StringShouldBeNullOrWhiteSpaceSyntaxVisitor() : base(new MemberValidator("IsNullOrWhiteSpace"), MemberValidator.Should, new MemberValidator("BeTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldBeNullOrWhiteSpaceCodeFix)), Shared]
    public class StringShouldBeNullOrWhiteSpaceCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldBeNullOrWhiteSpaceAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("IsNullOrWhiteSpace");
            var newExpression = GetNewExpression(expression, remove);

            var rename = NodeReplacement.Rename("BeTrue", "BeNullOrWhiteSpace");
            newExpression = GetNewExpression(newExpression, rename);

            var stringKeyword = newExpression.DescendantNodes().OfType<PredefinedTypeSyntax>().Single();
            var subject = remove.Arguments.First().Expression;

            return newExpression.ReplaceNode(stringKeyword, subject.WithTriviaFrom(stringKeyword));
        }
    }
}