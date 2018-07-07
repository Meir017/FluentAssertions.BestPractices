using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RenameAndRemoveInvocationOfMethodOnFirstArgument(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameAndRemoveInvocationOfMethodOnFirstArgumentNodeReplacement : RenameNodeReplacement
    {
        public RenameAndRemoveInvocationOfMethodOnFirstArgumentNodeReplacement(string oldName, string newName) : base(oldName, newName)
        {
        }

        public override InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node)
        {
            // replaces: .oldName(expected.XXX()) with .newName(expected)
            var oldArgumentExpression = (InvocationExpressionSyntax)node.ArgumentList.Arguments[0].Expression;
            var newArgumentExpression = ((MemberAccessExpressionSyntax)oldArgumentExpression.Expression).Expression;

            return node.ReplaceNode(oldArgumentExpression, newArgumentExpression);
        }
    }
}