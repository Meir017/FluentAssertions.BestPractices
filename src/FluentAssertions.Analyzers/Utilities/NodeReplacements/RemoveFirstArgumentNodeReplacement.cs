using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RemoveFirstArgument(name: \"{_name}\")")]
    public class RemoveFirstArgumentNodeReplacement : EditNodeReplacement
    {
        public ArgumentSyntax Argument { get; private set; }

        public RemoveFirstArgumentNodeReplacement(string name) : base(name)
        {
        }

        public override InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node)
        {
            Argument = node.ArgumentList.Arguments[0];
            var exceptFirstArgument = node.ArgumentList.Arguments.Remove(Argument);

            return node.WithArgumentList(node.ArgumentList.WithArguments(exceptFirstArgument));
        }
    }
}
