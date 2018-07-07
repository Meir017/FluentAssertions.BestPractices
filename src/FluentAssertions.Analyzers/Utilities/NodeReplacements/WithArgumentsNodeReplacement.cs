using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("WithArguments(name: \"{_name}\", arguments: \"{_arguments}\")")]
    public class WithArgumentsNodeReplacement : EditNodeReplacement
    {
        private readonly SeparatedSyntaxList<ArgumentSyntax> _arguments;

        public WithArgumentsNodeReplacement(string name, SeparatedSyntaxList<ArgumentSyntax> arguments) : base(name)
        {
            _arguments = arguments;
        }

        public override InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node) => node.WithArgumentList(node.ArgumentList.WithArguments(_arguments));
    }
}