using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace FluentAssertions.Analyzers
{
    public static class Expressions
    {
        public static InvocationExpressionSyntax SubjectShould(ExpressionSyntax subject)
        {
            return SF.InvocationExpression(
                SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, subject, SF.IdentifierName("Should")),
                SF.ArgumentList()
            );
        }
    }
}
