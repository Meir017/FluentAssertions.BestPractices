using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics;

namespace FluentAssertions.Analyzers
{
    [DebuggerDisplay("RenameAndNegateLambda(oldName: \"{_oldName}\", newName: \"{_newName}\")")]
    public class RenameAndNegateLambdaNodeReplacement : RenameNodeReplacement
    {
        public RenameAndNegateLambdaNodeReplacement(string oldName, string newName) : base(oldName, newName)
        {
        }

        public override InvocationExpressionSyntax ComputeNew(InvocationExpressionSyntax node)
        {
            var oldLambda = (LambdaExpressionSyntax)node.ArgumentList.Arguments[0].Expression;
            var newLambda = NagateLambda(oldLambda);

            return node.ReplaceNode(oldLambda, newLambda);
        }

        private LambdaExpressionSyntax NagateLambda(LambdaExpressionSyntax lambda)
        {
            throw new NotImplementedException();
        }
    }
}
