using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    public abstract class MsTestAnalyzer : FluentAssertionsAnalyzer
    {
        private static readonly NameSyntax MsTestNamespace = SyntaxFactory.ParseName("Microsoft.VisualStudio.TestTools.UnitTesting");

        protected override bool ShouldAnalyzeMethod(MethodDeclarationSyntax method)
        {
            var compilation = method.FirstAncestorOrSelf<CompilationUnitSyntax>();

            if (compilation == null) return false;

            return compilation.Usings.Any(usingDirective => usingDirective.Name.IsEquivalentTo(MsTestNamespace));
        }
    }

    public abstract class MsTestAssertAnalyzer : MsTestAnalyzer
    {
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type) => type.Name == "Assert";
    }
    public abstract class MsTestStringAssertAnalyzer : MsTestAnalyzer
    {
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type) => type.Name == "StringAssert";
    }
    public abstract class MsTestCollectionAssertAnalyzer : MsTestAnalyzer
    {
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type) => type.Name == "CollectionAssert";
    }

    public abstract class MsTestCodeFixProvider : FluentAssertionsCodeFixProvider
    {
        protected ExpressionSyntax RenameMethodAndReplaceWithSubjectShould(ExpressionSyntax expression, string oldName, string newName, string assert)
        {
            var rename = NodeReplacement.RenameAndRemoveFirstArgument(oldName, newName);
            var newExpression = GetNewExpression(expression, rename);

            return ReplaceIdentifier(newExpression, assert, Expressions.SubjectShould(rename.Argument.Expression));
        }
        protected ExpressionSyntax RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(ExpressionSyntax expression, string oldName, string newName, string assert)
        {
            var rename = NodeReplacement.RenameAndExtractArguments(oldName, newName);
            var newExpression = GetNewExpression(expression, rename);

            var actual = rename.Arguments[1];

            newExpression = ReplaceIdentifier(newExpression, assert, Expressions.SubjectShould(actual.Expression));

            return GetNewExpression(newExpression, NodeReplacement.WithArguments(newName, rename.Arguments.RemoveAt(1)));
        }
    }
}