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
    public class DictionaryShouldNotContainKeyAnalyzer : DictionaryAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Dictionaries.DictionaryShouldNotContainKey;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotContainKey() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ContainsKeyShouldBeFalseSyntaxVisitor();
            }
        }

		public class ContainsKeyShouldBeFalseSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public ContainsKeyShouldBeFalseSyntaxVisitor() : base(new MemberValidator("ContainsKey"), MemberValidator.Should, new MemberValidator("BeFalse"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DictionaryShouldNotContainKeyCodeFix)), Shared]
    public class DictionaryShouldNotContainKeyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DictionaryShouldNotContainKeyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("ContainsKey");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeFalse", "NotContainKey", remove.Arguments));
        }
    }
}