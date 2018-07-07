using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RenameAndPrependArguments(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameAndPrependArgumentsNodeReplacement : RenameNodeReplacement
    {
        private readonly SeparatedSyntaxList<ArgumentSyntax> _arguments;

        public RenameAndPrependArgumentsNodeReplacement(string oldName, string newName, SeparatedSyntaxList<ArgumentSyntax> arguments) : base(oldName, newName)
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
