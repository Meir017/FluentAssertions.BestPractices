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
    public class StringShouldHaveLengthAnalyzer : StringAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Strings.StringShouldHaveLength;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().HaveLength() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new LengthShouldBeSyntaxVisitor();
            }
        }

        public class LengthShouldBeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public LengthShouldBeSyntaxVisitor() : base(new MemberValidator("Length"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringShouldHaveLengthCodeFix)), Shared]
    public class StringShouldHaveLengthCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(StringShouldHaveLengthAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var remove = NodeReplacement.Remove("Length");
            var newExpression = GetNewExpression(expression, remove);

            return GetNewExpression(newExpression, NodeReplacement.Rename("Be", "HaveLength"));

        }
    }
}