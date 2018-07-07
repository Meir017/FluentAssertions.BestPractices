using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldHaveElementAt0NullAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldHaveElementAt0Null;
        public const string Category = Constants.Tips.Category;

        public const string Message = "TODO";

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors { get; } = Enumerable.Empty<FluentAssertionsCSharpSyntaxVisitor>();

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override Diagnostic AnalyzeExpression(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            return null;
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldHaveElementAt0NullCodeFix)), Shared]
    public class CollectionShouldHaveElementAt0NullCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldHaveElementAt0NullAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            throw new System.NotImplementedException();
        }
    }
}
