using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DictionaryShouldContainKeyAndValueAnalyzer : DictionaryAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Dictionaries.DictionaryShouldContainKeyAndValue;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().Contain() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldContainKeyAndContainValueSyntaxVisitor();
                yield return new ShouldContainValueAndContainKeySyntaxVisitor();
            }
        }

        public class ShouldContainKeyAndContainValueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {

            public ShouldContainKeyAndContainValueSyntaxVisitor() : base(MemberValidator.Should, MemberValidator.ArgumentIsIdentifierOrLiteral("ContainKey"), MemberValidator.And, MemberValidator.ArgumentIsIdentifierOrLiteral("ContainValue"))
            {
            }
        }

        public class ShouldContainValueAndContainKeySyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldContainValueAndContainKeySyntaxVisitor() : base(MemberValidator.Should, MemberValidator.ArgumentIsIdentifierOrLiteral("ContainValue"), MemberValidator.And, MemberValidator.ArgumentIsIdentifierOrLiteral("ContainKey"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DictionaryShouldContainKeyAndValueCodeFix)), Shared]
    public class DictionaryShouldContainKeyAndValueCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DictionaryShouldContainKeyAndValueAnalyzer.DiagnosticId);

        protected override bool CanRewriteAssertion(ExpressionSyntax expression)
        {
            var visitor = new MemberAccessExpressionsCSharpSyntaxVisitor();
            expression.Accept(visitor);

            var containKey = visitor.Members.Find(member => member.Name.Identifier.Text == "ContainKey");
            var containValue = visitor.Members.Find(member => member.Name.Identifier.Text == "ContainValue");

            return !(containKey.Parent is InvocationExpressionSyntax containKeyInvocation && containKeyInvocation.ArgumentList.Arguments.Count > 1
                && containValue.Parent is InvocationExpressionSyntax containValueInvocation && containValueInvocation.ArgumentList.Arguments.Count > 1);
        }

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(DictionaryShouldContainPairAnalyzer.ShouldContainKeyAndContainValueSyntaxVisitor))
            {
                var renameKeyArguments = NodeReplacement.RenameAndExtractArguments("ContainKey", "Contain");
                var removeValueArguments = NodeReplacement.RemoveAndExtractArguments("ContainValue");
                var newExpression = GetNewExpression(expression, NodeReplacement.RemoveMethodBefore("ContainValue"), renameKeyArguments, removeValueArguments);

                var newArguments = MergeContainKeyAndContainValueArguments(renameKeyArguments.Arguments, removeValueArguments.Arguments);

                return GetNewExpression(newExpression, NodeReplacement.WithArguments("Contain", newArguments));
            }
            else if (properties.VisitorName == nameof(DictionaryShouldContainPairAnalyzer.ShouldContainValueAndContainKeySyntaxVisitor))
            {
                var removeKeyArguments = NodeReplacement.RemoveAndExtractArguments("ContainKey");
                var renameValueArguments = NodeReplacement.RenameAndExtractArguments("ContainValue", "Contain");
                var newExpression = GetNewExpression(expression, NodeReplacement.RemoveMethodBefore("ContainKey"), removeKeyArguments, renameValueArguments);

                var newArguments = MergeContainKeyAndContainValueArguments(removeKeyArguments.Arguments, renameValueArguments.Arguments);

                return GetNewExpression(newExpression, NodeReplacement.WithArguments("Contain", newArguments));
            }
            else
            {
                throw new InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
            }
        }

        private SeparatedSyntaxList<ArgumentSyntax> MergeContainKeyAndContainValueArguments(SeparatedSyntaxList<ArgumentSyntax> keyArguments, SeparatedSyntaxList<ArgumentSyntax> valueArguments)
        {
            return new SeparatedSyntaxList<ArgumentSyntax>()
                .Add(keyArguments[0])
                .Add(valueArguments[0])
                .AddRange(keyArguments.RemoveAt(0))
                .AddRange(valueArguments.RemoveAt(0));
        }
    }
}