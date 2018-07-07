using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RemoveAndRetrieveIndexerArguments(methodAfterIndexer: \"{_methodAfterIndexer}\")")]
    public class RemoveAndRetrieveIndexerArgumentsNodeReplacement : NodeReplacement
    {
        private readonly string _methodAfterIndexer;

        public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; private set; }

        public RemoveAndRetrieveIndexerArgumentsNodeReplacement(string methodAfterIndexer)
        {
            _methodAfterIndexer = methodAfterIndexer;
        }

        public sealed override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value.Name.Identifier.Text == _methodAfterIndexer;
        public sealed override SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode) => listNode.Value;
        public sealed override SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode)
        {
            var current = listNode.Value;

            var elementAccess = (ElementAccessExpressionSyntax)current.Expression;
            Arguments = elementAccess.ArgumentList.Arguments;

            return current.WithExpression(elementAccess.Expression);
        }
    }
}
