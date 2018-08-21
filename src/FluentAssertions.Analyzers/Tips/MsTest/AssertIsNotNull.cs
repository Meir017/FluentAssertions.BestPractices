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
    public class AssertIsNotNullAnalyzer : MsTestAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.AssertIsNotNull;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use {0} .Should().NotBeNull() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AssertIsNotNullSyntaxVisitor();
            }
        }

        public class AssertIsNotNullSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public AssertIsNotNullSyntaxVisitor() : base(new MemberValidator("IsNotNull"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AssertIsNotNullCodeFix)), Shared]
    public class AssertIsNotNullCodeFix : MsTestCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AssertIsNotNullAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
            => RenameMethodAndReplaceWithSubjectShould(expression, "IsNotNull", "NotBeNull", "Assert");
    }
}