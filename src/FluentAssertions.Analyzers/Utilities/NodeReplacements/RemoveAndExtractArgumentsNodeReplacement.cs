using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RemoveAndExtractArguments(name: \"{_name}\")")]
    public class RemoveAndExtractArgumentsNodeReplacement : RemoveNodeReplacement
    {
        public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; private set; }

        public RemoveAndExtractArgumentsNodeReplacement(string name) : base(name)
        {
        }

        public override void ExtractValues(MemberAccessExpressionSyntax node)
        {
            var invocation = (InvocationExpressionSyntax)node.Parent;

            Arguments = invocation.ArgumentList.Arguments;
        }
    }
}
