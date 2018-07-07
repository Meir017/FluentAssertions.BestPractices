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
    public class StringShouldStartWithAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldStartWith;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().StartWith() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new StartWithShouldBeTrueSyntaxVisitor();
            }
        }

		public class StartWithShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public StartWithShouldBeTrueSyntaxVisitor() : base(new MemberValidator("StartsWith"), MemberValidator.Should, new MemberValidator("BeTrue"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldStartWithCodeFix)), Shared]
    public class StringShouldStartWithCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldStartWithAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("StartsWith");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeTrue", "StartWith", remove.Arguments));
        }
    }
}