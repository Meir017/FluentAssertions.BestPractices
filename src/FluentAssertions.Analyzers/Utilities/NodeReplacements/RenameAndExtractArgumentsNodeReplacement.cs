using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RenameAndExtractArguments(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameAndExtractArgumentsNodeReplacement : RenameNodeReplacement
    {
        public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; private set; }

        public RenameAndExtractArgumentsNodeReplacement(string oldName, string newName) : base(oldName, newName)
        {
        }

        public override void ExtractValues(MemberAccessExpressionSyntax node)
        {
            Arguments = ((InvocationExpressionSyntax)node.Parent).ArgumentList.Arguments;
        }
    }
}
