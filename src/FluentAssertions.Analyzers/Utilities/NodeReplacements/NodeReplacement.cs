using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace FluentAssertions.Analyzers
{
    public abstract class NodeReplacement
    {
        public abstract bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode);
        public abstract SyntaxNode ComputeOld(LinkedListNode<MemberAccessExpressionSyntax> listNode);
        public abstract SyntaxNode ComputeNew(LinkedListNode<MemberAccessExpressionSyntax> listNode);
        public virtual void ExtractValues(MemberAccessExpressionSyntax node) { }
        
        
        public static NodeReplacement Rename(string oldName, string newName) => new RenameNodeReplacement(oldName, newName);
        public static NodeReplacement Remove(string name) => new RemoveNodeReplacement(name);
        public static NodeReplacement RemoveOccurrence(string name, int occurrence) => new RemoveOccurrenceNodeReplacement(name, occurrence);
        public static NodeReplacement RemoveMethodBefore(string name) => new RemoveMethodBeforeNodeReplacement(name);
        public static RemoveAndExtractArgumentsNodeReplacement RemoveAndExtractArguments(string name) => new RemoveAndExtractArgumentsNodeReplacement(name);
        public static NodeReplacement RenameAndPrependArguments(string oldName, string newName, SeparatedSyntaxList<ArgumentSyntax> arguments) => new RenameAndPrependArgumentsNodeReplacement(oldName, newName, arguments);
        public static NodeReplacement RenameAndRemoveInvocationOfMethodOnFirstArgument(string oldName, string newName) => new RenameAndRemoveInvocationOfMethodOnFirstArgumentNodeReplacement(oldName, newName);
        public static RenameAndRemoveFirstArgumentNodeReplacement RenameAndRemoveFirstArgument(string oldName, string newName) => new RenameAndRemoveFirstArgumentNodeReplacement(oldName, newName);
        public static RenameAndExtractArgumentsNodeReplacement RenameAndExtractArguments(string oldName, string newName) => new RenameAndExtractArgumentsNodeReplacement(oldName, newName);
        public static RemoveFirstArgumentNodeReplacement RemoveFirstArgument(string name) => new RemoveFirstArgumentNodeReplacement(name);
        public static NodeReplacement PrependArguments(string name, SeparatedSyntaxList<ArgumentSyntax> arguments) => new PrependArgumentsNodeReplacement(name, arguments);
        public static RemoveAndRetrieveIndexerArgumentsNodeReplacement RemoveAndRetrieveIndexerArguments(string methodAfterIndexer) => new RemoveAndRetrieveIndexerArgumentsNodeReplacement(methodAfterIndexer);
        public static RenameAndNegateLambdaNodeReplacement RenameAndNegateLambda(string oldName, string newName) => new RenameAndNegateLambdaNodeReplacement(oldName, newName);
        public static WithArgumentsNodeReplacement WithArguments(string name, SeparatedSyntaxList<ArgumentSyntax> arguments) => new WithArgumentsNodeReplacement(name, arguments);
    }
}
