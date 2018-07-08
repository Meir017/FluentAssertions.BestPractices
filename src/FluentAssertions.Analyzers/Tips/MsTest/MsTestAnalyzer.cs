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

        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type) => type.Name == "Assert";
    }
}
