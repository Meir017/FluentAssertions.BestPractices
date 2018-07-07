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
    public class StringShouldBeNullOrEmptyAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldBeNullOrEmpty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should() followed by .BeNullOrEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new StringIsNullOrEmptyShouldBeTrueSyntaxVisitor();
            }
        }

        public class StringIsNullOrEmptyShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public StringIsNullOrEmptyShouldBeTrueSyntaxVisitor() : base(new MemberValidator("IsNullOrEmpty"), MemberValidator.Should, new MemberValidator("BeTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldBeNullOrEmptyCodeFix)), Shared]
    public class StringShouldBeNullOrEmptyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldBeNullOrEmptyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("IsNullOrEmpty");
            var newExpression = GetNewExpression(expression, remove);

            var rename = NodeReplacement.Rename("BeTrue", "BeNullOrEmpty");
            newExpression = GetNewExpression(newExpression, rename);

            var stringKeyword = newExpression.DescendantNodes().OfType<PredefinedTypeSyntax>().Single();
            var subject = remove.Arguments.First().Expression;

            return newExpression.ReplaceNode(stringKeyword, subject.WithTriviaFrom(stringKeyword));
        }
    }
}