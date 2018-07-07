using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace FluentAssertions.Analyzers
{
    public abstract class EditNodeReplacement : NodeReplacement
    {
        private readonly string _name;

        protected EditNodeReplacement(string name)
        {
            _name = name;
        }

        public abstract InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node);

        public sealed override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Name.Identifier.Text == _name;
        public sealed override SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Parent;
        public sealed override SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode)
        {
            return ComputeNew((InvocationExpressionSyntax)listNode.Value.Parent);
        }
    }
}
