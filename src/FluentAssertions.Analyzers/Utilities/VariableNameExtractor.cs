using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace FluentAssertions.Analyzers
{
    public class VariableNameExtractor : CSharpSyntaxWalker
    {
        private readonly SemanticModel _semanticModel;

        public string VariableName { get; private set; }
        public IdentifierNameSyntax VariableIdentifierName { get; private set; }

        public VariableNameExtractor(SemanticModel semanticModel = null)
        {
            _semanticModel = semanticModel;
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (IsVariable(node))
            {
                VariableName = node.Identifier.Text;
                VariableIdentifierName = node;
            }
        }

        public override void Visit(SyntaxNode node)
        {
            // the first identifier encountered will be the one at the bottom of the syntax tree
            if (VariableName == null)
            {
                base.Visit(node);
            }
        }

        public static string ExtractVariabeName(ArgumentSyntax argument)
        {
            if (argument.Parent is ArgumentListSyntax argumentList && argumentList.Parent is InvocationExpressionSyntax invocation)
            {
                return ExtractVariabeName(invocation);
            }
            return null;
        }

        public static string ExtractVariabeName(InvocationExpressionSyntax invocation)
        {
            var variableExtractor = new VariableNameExtractor();
            invocation.Accept(variableExtractor);

            return variableExtractor.VariableName;
        }

        private bool IsVariable(IdentifierNameSyntax node)
        {
            if (_semanticModel != null)
            {
                SymbolInfo info = _semanticModel.GetSymbolInfo(node);
                if (info.Symbol == null ||
                    info.Symbol.Kind == SymbolKind.Method ||
                    info.Symbol.IsStatic)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
