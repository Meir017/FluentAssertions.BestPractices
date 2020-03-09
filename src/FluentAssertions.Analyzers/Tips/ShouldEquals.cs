using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAssertions.Analyzers.Tips
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ShouldEqualsAnalyzer : FluentAssertionsAnalyzer
    {
        public const string DiagnosticId = Constants.CodeSmell.ShouldEquals;
        public const string Category = Constants.CodeSmell.Category;
        public const string Message = "Use .Should().Equal() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldEqualsSyntaxVisitor();
            }
        }

        public class ShouldEqualsSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldEqualsSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("Equals"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ShouldEqualsCodeFix)), Shared]
    public class ShouldEqualsCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ShouldEqualsAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties) 
            => GetNewExpression(expression, NodeReplacement.Rename("Equals", "Be"));
    }
}
