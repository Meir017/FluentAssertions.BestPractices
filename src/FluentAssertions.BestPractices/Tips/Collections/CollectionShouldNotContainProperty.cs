﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.BestPractices
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldNotContainPropertyAnalyzer : FluentAssertionsAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotContainProperty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use {0} .Should() followed by .BeEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override Diagnostic AnalyzeExpressionStatement(ExpressionStatementSyntax statement)
        {
            var visitor = new CollectionShouldNotContainPropertySyntaxVisitor();
            statement.Accept(visitor);

            if (visitor.IsValid)
            {
                var properties = new Dictionary<string, string>
                {
                    [Constants.DiagnosticProperties.VariableName] = visitor.VariableName,
                    [Constants.DiagnosticProperties.Title] = Title,
                    [Constants.DiagnosticProperties.PredicateString] = visitor.PredicateString
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

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotContainPropertyCodeFix)), Shared]
    public class CollectionShouldNotContainPropertyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotContainPropertyAnalyzer.DiagnosticId);

        protected override StatementSyntax GetNewStatement(ImmutableDictionary<string, string> properties)
            => SyntaxFactory.ParseStatement($"{properties[Constants.DiagnosticProperties.VariableName]}.Should().NotContain({properties[Constants.DiagnosticProperties.PredicateString]});");
    }

    public class CollectionShouldNotContainPropertySyntaxVisitor : FluentAssertionsWithoutArgumentsCSharpSyntaxVisitor
    {
        public string PredicateString { get; private set; }

        public override bool IsValid => base.IsValid && PredicateString != null;

        public CollectionShouldNotContainPropertySyntaxVisitor() : base("Any", "Should", "BeFalse")
        {
        }

        
        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            if (node.Arguments.Count == 1 && RequiredMethods.Count == 0)
            {
                base.Visit(node.Arguments[0]);
            }
        }
        
        public override void VisitArgument(ArgumentSyntax node)
        {
            if (node.Expression is SimpleLambdaExpressionSyntax lambda)
            {
                PredicateString = lambda.ToFullString();
            }
        }
    }
}
