using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RemoveOccurrence(name: \"{_name}\", occurrence: {_occurrence})")]
    public class RemoveOccurrenceNodeReplacement : RemoveNodeReplacement
    {
        private int _occurrence;

        public RemoveOccurrenceNodeReplacement(string name, int occurrence) : base(name)
        {
            _occurrence = occurrence;
        }

        public sealed override bool IsValidNode(LinkedListNode<MemberAccessExpressionSyntax> listNode)
        {
            if (base.IsValidNode(listNode))
            {
                --_occurrence;
                return _occurrence == 0;
            }

            return false;
        }
    }
}
