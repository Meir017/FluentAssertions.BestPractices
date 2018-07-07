using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("Remove(name: \"{_name}\")")]
    public class RemoveNodeReplacement : NodeReplacement
    {
        private readonly string _name;

        public RemoveNodeReplacement(string name)
        {
            _name = name;
        }

        public override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode?.Value?.Name?.Identifier.Text == _name;
        public sealed override SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode?.Previous?.Value ?? listNode.Value.Parent;
        public sealed override SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode)
        {
            if (listNode.Previous == null) return listNode.Next.Value.Parent;

            var previous = listNode.Previous.Value;
            var next = listNode.Next?.Value ?? listNode.Value.Expression;

            if (next.Parent is InvocationExpressionSyntax nextInvocation)
            {
                return previous.WithExpression(nextInvocation);
            }

            return previous.WithExpression(next);
        }
    }
}
