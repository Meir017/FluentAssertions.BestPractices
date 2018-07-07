using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace FluentAssertions.Analyzers
{

    public class FluentAssertionsCSharpSyntaxVisitor : CSharpSyntaxVisitor
    {
        public string VariableName { get; private set; }
        public IdentifierNameSyntax IdentifierNameVariable { get; private set; }

        public ImmutableStack<MemberValidator> AllMembers { get; }
        public ImmutableStack<MemberValidator> Members { get; private set; }

        public virtual bool IsValid(ExpressionSyntax expression) => Members.IsEmpty;

        public virtual ImmutableDictionary<string, string> ToDiagnosticProperties() => ImmutableDictionary<string, string>.Empty
            .Add(Constants.DiagnosticProperties.VisitorName, GetType().Name)
            .Add(Constants.DiagnosticProperties.HelpLink, HelpLinks.Get(GetType()))
            .ToImmutableDictionary();

        public FluentAssertionsCSharpSyntaxVisitor(params MemberValidator[] members)
        {
            AllMembers = ImmutableStack.Create(members);
            Members = AllMembers;
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var name = node.Name.Identifier.Text;

            if (Members.IsEmpty)
            {
                // no op
            }
            else if (name == "And")
            {
                if (Members.Peek().Name == "And")
                {
                    Members = Members.Pop();
                }
            }
            else if (node.Parent is InvocationExpressionSyntax invocation)
            {
                var member = Members.Peek();
                if (member.Name == name && member.AreArgumentsValid(invocation.ArgumentList.Arguments))
                {
                    Members = Members.Pop();
                }
                else
                {
                    Members = AllMembers;
                }
            }
            else if (node.Parent is MemberAccessExpressionSyntax memberAccess && memberAccess.IsKind(SyntaxKind.SimpleMemberAccessExpression)
                && name == Members.Peek().Name)
            {
                Members = Members.Pop();
            }
            else
            {
                Members = AllMembers;
            }

            Visit(node.Expression);
        }

        public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            const string name = "[]";

            var member = Members.Peek();
            if (Members.IsEmpty)
            {
                // no op
            }
            else if (member.Name == name && member.AreArgumentsValid(node.ArgumentList.Arguments))
            {
                Members = Members.Pop();
            }
            else
            {
                Members = AllMembers;
            }

            Visit(node.Expression);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            Visit(node.Expression);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            IdentifierNameVariable = node;
            VariableName = node.Identifier.Text;
        }

        /*
        private int _indent = 0;
        public override void Visit(SyntaxNode node)
        {
            _indent++;
            var indent = new string(' ', _indent * 2);
            Console.WriteLine($"{indent}{node.GetType().Name}");
            base.Visit(node);
            --_indent;
        }
        */
    }
}
