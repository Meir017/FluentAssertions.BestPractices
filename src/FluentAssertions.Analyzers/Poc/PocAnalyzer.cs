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
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected virtual DiagnosticDescriptor Rule => new DiagnosticDescriptor("PocAnalyzer", "POC analyzer", "POC analyzer tree searcher", "POC", DiagnosticSeverity.Info, true);

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

        protected virtual IEnumerable<PocSyntaxWalker> Visitors { get; }

        private Diagnostic AnalyzeExpression(ExpressionSyntax expression)
        {
            foreach (var visitor in Visitors)
            {
                expression.Accept(visitor);

                if (visitor.IsValid)
                {
                    return CreateDiagnostic(visitor, expression);
                }
            }
            return null;
        }

        protected virtual Diagnostic CreateDiagnostic(PocSyntaxWalker visitor, ExpressionSyntax expression)
        {
            return Diagnostic.Create(
                descriptor: Rule,
                location: expression.GetLocation());
        }
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PocCollectionShouldNotBeNullOrEmptyAnalyzer : PocAnalyzer
    {
        public const string Title = "TitlePocCollectionShouldNotBeNullOrEmptyAnalyzer";
        public const string DiagnosticId = "PocCollectionShouldNotBeNullOrEmptyAnalyzerId";
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotBeNullOrEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<PocSyntaxWalker> Visitors
        {
            get
            {
                yield return new ShouldNotBeEmptyAndNotBeNullSyntaxVisitor();
                yield return new ShouldNotBeNullAndNotBeEmptySyntaxVisitor();
            }
        }

        public class ShouldNotBeNullAndNotBeEmptySyntaxVisitor : PocSyntaxWalker
        {
            public ShouldNotBeNullAndNotBeEmptySyntaxVisitor() : base("Should", "NotBeNull", "And", "NotBeEmpty")
            {
            }
        }
        public class ShouldNotBeEmptyAndNotBeNullSyntaxVisitor : PocSyntaxWalker
        {
            public ShouldNotBeEmptyAndNotBeNullSyntaxVisitor() : base("Should", "NotBeEmpty", "And", "NotBeNull")
            {
            }
        }
    }

    public class PocSyntaxWalker : CSharpSyntaxWalker
    {
        public readonly string[] SpecialProperties = { "And", "Which" };

        public ExpressionSyntax Leaf { get; private set; }

        public ImmutableStack<string> AllMembers { get; }
        public ImmutableStack<string> Members { get; private set; }

        public bool IsValid => Members.IsEmpty;

        public PocSyntaxWalker(params string[] members)
        {
            AllMembers = ImmutableStack.Create(members);
            Members = AllMembers;
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var name = node.Name.Identifier.Text;
            Console.WriteLine($"MemberAccess: {name}, Members: {string.Join(",", Members)}");

            if (IsValid)
            {
                // no op
            }
            else if (SpecialProperties.Contains(Members.Peek()))
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

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            Console.WriteLine($"VisitIdentifierName: {node.Identifier.Text}");
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
