using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RenameAndRemoveFirstArgument(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameAndRemoveFirstArgumentNodeReplacement : RenameNodeReplacement
    {
        public ArgumentSyntax Argument { get; private set; }

        public RenameAndRemoveFirstArgumentNodeReplacement(string oldName, string newName) : base(oldName, newName)
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
