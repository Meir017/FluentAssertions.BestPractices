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
    public class DictionaryShouldContainKeyAnalyzer : DictionaryAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Dictionaries.DictionaryShouldContainKey;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().ContainKey() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ContainsKeyShouldBeTrueSyntaxVisitor();
            }
        }

		public class ContainsKeyShouldBeTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public ContainsKeyShouldBeTrueSyntaxVisitor() : base(new MemberValidator("ContainsKey"), MemberValidator.Should, new MemberValidator("BeTrue"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DictionaryShouldContainKeyCodeFix)), Shared]
    public class DictionaryShouldContainKeyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DictionaryShouldContainKeyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("ContainsKey");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeTrue", "ContainKey", remove.Arguments));
        }
    }
}