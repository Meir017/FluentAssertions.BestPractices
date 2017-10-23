using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PocAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PocAnalyzer";
        public const string Category = "POC";
        public const string Title = "POC analyzer";

        public const string Message = "POC analyzer tree searcher";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        private readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterCodeBlockAction(AnalyzeCodeBlock);
        }

        private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        {
            var method = context.CodeBlock as MethodDeclarationSyntax;
            if (method == null) return;

            if (method.Body != null)
            {
                foreach (var statement in method.Body.Statements.OfType<ExpressionStatementSyntax>())
                {
                    var diagnostic = AnalyzeExpression(statement.Expression);
                    if (diagnostic != null)
                    {
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                return;
            }
            if (method.ExpressionBody != null)
            {
                var diagnostic = AnalyzeExpression(method.ExpressionBody.Expression);
                if (diagnostic != null)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private Diagnostic AnalyzeExpression(ExpressionSyntax expression)
        {
            var walker = new PocCSharpSyntaxWalker("Should", "NotBeNull", "And", "NotBeEmpty");
            expression.Accept(walker);

            var result = walker.IsValid;

            return null;
        }


    }

    public class PocCSharpSyntaxWalker : CSharpSyntaxWalker
    {
        public readonly string[] SpecialProperties = { "And", "Which" };

        public ImmutableStack<string> AllMembers { get; }
        public ImmutableStack<string> Members { get; private set; }

        public bool IsValid { get; private set; }

        public PocCSharpSyntaxWalker(params string[] members)
        {
            AllMembers = ImmutableStack.Create(members);
            Members = AllMembers;
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (Members.IsEmpty)
            {
                IsValid = true;
                return;
            }

            var name = node.Name.Identifier.Text;
            Console.WriteLine($"MemberAccess: {name}, Members: {string.Join(",", Members)}");

            if (SpecialProperties.Contains(Members.Peek()))
            {
                Members = Members.Pop();
            }
            else if (node.Parent.IsKind(SyntaxKind.InvocationExpression))
            {
                if (Members.Peek() == name)
                {
                    Members = Members.Pop();
                }
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
