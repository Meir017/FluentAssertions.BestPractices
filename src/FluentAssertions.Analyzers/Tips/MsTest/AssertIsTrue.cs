using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssertIsTrueAnalyzer : MsTestAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.AssertIsTrue;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeTrue() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AssertIsTrueSyntaxVisitor();
            }
        }

        public class AssertIsTrueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public AssertIsTrueSyntaxVisitor() : base(new MemberValidator("IsTrue"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AssertIsTrueCodeFix)), Shared]
    public class AssertIsTrueCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldBeEmptyAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var rename = NodeReplacement.RenameAndRemoveFirstArgument("IsTrue", "BeTrue");
            var newExpression = GetNewExpression(expression, rename);

            var assertNode = newExpression.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .First(node => node.Identifier.Text == "Assert");

            newExpression = newExpression.ReplaceNode(assertNode, GetSubjectShouldExpression(rename.Argument).WithTriviaFrom(assertNode));

            return newExpression;
        }

        private static InvocationExpressionSyntax GetSubjectShouldExpression(ArgumentSyntax argument)
        {
            return SF.InvocationExpression(
                SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, argument.Expression, SF.IdentifierName("Should")),
                SF.ArgumentList()
            );
        }
    }
}
