using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace FluentAssertions.Analyzers
{
    public class MemberAccessExpressionsCSharpSyntaxVisitor : CSharpSyntaxVisitor
    {
        public List<MemberAccessExpressionSyntax> Members { get; } = new List<MemberAccessExpressionSyntax>();

        public override void VisitInvocationExpression(InvocationExpressionSyntax node) => Visit(node.Expression);

        public sealed override void VisitExpressionStatement(ExpressionStatementSyntax node) => Visit(node.Expression);


        public sealed override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            Members.Add(node);
            Visit(node.Expression);
        }
    }
}
