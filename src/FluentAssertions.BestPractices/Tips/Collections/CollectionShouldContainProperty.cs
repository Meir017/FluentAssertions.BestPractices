﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.BestPractices
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldContainPropertyAnalyzer : FluentAssertionsAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldContainProperty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use {0} .Should() followed by .Contain() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<(FluentAssertionsCSharpSyntaxVisitor, BecauseArgumentsSyntaxVisitor)> Visitors
        {
            get
            {
                yield return (new AnyShouldBeTrueSyntaxVisitor(), new BecauseArgumentsSyntaxVisitor("BeTrue", 0));
                yield return (new WhereShouldNotBeEmptySyntaxVisitor(), new BecauseArgumentsSyntaxVisitor("NotBeEmpty", 0));
            }
        }

        public class AnyShouldBeTrueSyntaxVisitor : FluentAssertionsWithLambdaArgumentCSharpSyntaxVisitor
        {
            protected override string MethodContainingLambda => "Any";
            public AnyShouldBeTrueSyntaxVisitor() : base("Any", "Should", "BeTrue")
            {
            }
        }
        public class WhereShouldNotBeEmptySyntaxVisitor : FluentAssertionsWithLambdaArgumentCSharpSyntaxVisitor
        {
            protected override string MethodContainingLambda => "Where";
            public WhereShouldNotBeEmptySyntaxVisitor() : base("Where", "Should", "NotBeEmpty")
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldContainPropertyCodeFix)), Shared]
    public class CollectionShouldContainPropertyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldContainPropertyAnalyzer.DiagnosticId);
        
        protected override StatementSyntax GetNewStatement(ExpressionStatementSyntax statement, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(CollectionShouldContainPropertyAnalyzer.AnyShouldBeTrueSyntaxVisitor))
            {
                var remove = new NodeReplacement.RemoveAndExtractArgumentsNodeReplacement("Any");
                var newStatement = GetNewStatement(statement, remove);

                return GetNewStatement(newStatement, new NodeReplacement.RenameAndPrependArgumentsNodeReplacement("BeTrue", "Contain", remove.Arguments));
            }
            else if (properties.VisitorName == nameof(CollectionShouldContainPropertyAnalyzer.WhereShouldNotBeEmptySyntaxVisitor))
            {
                var remove = new NodeReplacement.RemoveAndExtractArgumentsNodeReplacement("Where");
                var newStatement = GetNewStatement(statement, remove);

                return GetNewStatement(newStatement, new NodeReplacement.RenameAndPrependArgumentsNodeReplacement("NotBeEmpty", "Contain", remove.Arguments));
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }
    }
}
