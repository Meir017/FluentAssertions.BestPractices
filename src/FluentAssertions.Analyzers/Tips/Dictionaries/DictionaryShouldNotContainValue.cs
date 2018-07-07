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
    public class DictionaryShouldNotContainValueAnalyzer : DictionaryAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Dictionaries.DictionaryShouldNotContainValue;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotContainValue() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ContainsValueShouldBeFalseSyntaxVisitor();
            }
        }

        public class ContainsValueShouldBeFalseSyntaxVisitor: FluentAssertionsCSharpSyntaxVisitor
        {
            public ContainsValueShouldBeFalseSyntaxVisitor() : base(new MemberValidator("ContainsValue"), MemberValidator.Should, new MemberValidator("BeFalse"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DictionaryShouldNotContainValueCodeFix)), Shared]
    public class DictionaryShouldNotContainValueCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DictionaryShouldNotContainValueAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments("ContainsValue");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments("BeFalse", "NotContainValue", remove.Arguments));
        }
    }
}