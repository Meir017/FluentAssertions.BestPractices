using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RemoveMethodBefore(name: \"{_name}\")")]
    public class RemoveMethodBeforeNodeReplacement : RemoveNodeReplacement
    {
        public RemoveMethodBeforeNodeReplacement(string name) : base(name)
        {
        }

        public override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => base.IsValidNode(listNode.Previous);
    }
}
