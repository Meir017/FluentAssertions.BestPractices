using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("PrependArguments(name: \"{_name}\")")]
    public class PrependArgumentsNodeReplacement : EditNodeReplacement
    {
        private readonly SeparatedSyntaxList<ArgumentSyntax> _arguments;

        public PrependArgumentsNodeReplacement(string name, SeparatedSyntaxList<ArgumentSyntax> arguments) : base(name)
        {
            _arguments = arguments;
        }

        public override InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node)
        {
            var combinedArguments = node.ArgumentList.WithArguments(_arguments)
                .AddArguments(node.ArgumentList.Arguments.ToArray());

            return node.WithArgumentList(combinedArguments);
        }
    }
}
