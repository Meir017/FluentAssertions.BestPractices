﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.BestPractices
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionsShouldBeEmptyAnalyzer : FluentAssertionsAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionsShouldBeEmpty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use {0} .Should() followed by .BeEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override Diagnostic AnalyzeExpressionStatement(ExpressionStatementSyntax statement)
        {
            var visitor = new CollectionsShouldBeEmptySyntaxVisitor();
            statement.Accept(visitor);

            if (visitor.IsValid)
            {
                var properties = new Dictionary<string, string>
                {
                    [Constants.DiagnosticProperties.VariableName] = visitor.VariableName,
                    [Constants.DiagnosticProperties.Title] = Title
                }.ToImmutableDictionary();

                return Diagnostic.Create(
                    descriptor: Rule,
                    location: statement.Expression.GetLocation(),
                    properties: properties,
                    messageArgs: visitor.VariableName);
            }
            return null;
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionsShouldBeEmptyCodeFix)), Shared]
    public class CollectionsShouldBeEmptyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionsShouldBeEmptyAnalyzer.DiagnosticId);

        protected override StatementSyntax GetNewStatement(ImmutableDictionary<string, string> properties)
            => SyntaxFactory.ParseStatement($"{properties[Constants.DiagnosticProperties.VariableName]}.Should().BeEmpty();");
    }

    public class CollectionsShouldBeEmptySyntaxVisitor : FluentAssertionsWithoutArgumentsCSharpSyntaxVisitor
    {
        public CollectionsShouldBeEmptySyntaxVisitor() : base("Any", "Should", "BeFalse")
        {
        }
    }
}
