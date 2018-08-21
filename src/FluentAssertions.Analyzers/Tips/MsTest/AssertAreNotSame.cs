using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssertAreNotSameAnalyzer : MsTestAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.AssertAreNotSame;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotBeSameAs() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AssertAreNotSameSyntaxVisitor();
            }
        }

		public class AssertAreNotSameSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public AssertAreNotSameSyntaxVisitor() : base(new MemberValidator("AreNotSame"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AssertAreNotSameCodeFix)), Shared]
    public class AssertAreNotSameCodeFix : MsTestCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AssertAreNotSameAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
            => RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(expression, "AreNotSame", "NotBeSameAs", "Assert");
    }
}