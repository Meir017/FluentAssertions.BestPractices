using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("Rename(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameNodeReplacement : NodeReplacement
    {
        private readonly string _oldName;
        private readonly string _newName;

        public RenameNodeReplacement(string oldName, string newName)
        {
            _oldName = oldName;
            _newName = newName;
        }

        public virtual InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node) => node;

        public sealed override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Name.Identifier.Text == _oldName;
        public sealed override SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Parent;
        public sealed override SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode)
        {
            var current = listNode.Value;
            var parent = (InvocationExpressionSyntax)current.Parent;

            var newNode = parent.WithExpression(current.WithName(current.Name.WithIdentifier(SyntaxFactory.ParseToken(_newName))));
            return ComputeNew(newNode);
        }
    }
}
