using Microsoft.CodeAnalysis;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    public abstract class CollectionAnalyzer : FluentAssertionsAnalyzer
    {
        protected override bool ShouldAnalyzeVariableType(ITypeSymbol type)
        {
            return type.Name != "String"
                && type.AllInterfaces.Any(@interface => @interface.Name == "IEnumerable");
        }
    }
}
