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
    public class DictionaryShouldContainPairAnalyzer : DictionaryAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Dictionaries.DictionaryShouldContainPair;
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
        
        public abstract class ContainKeyValueSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            protected ContainKeyValueSyntaxVisitor(params MemberValidator[] members) : base(members)
            {
            }

            public override bool IsValid(ExpressionSyntax expression)
            {
                if (!base.IsValid(expression)) return false;

                var visitor = new MemberAccessExpressionsCSharpSyntaxVisitor();
                expression.Accept(visitor);

                var containKey = visitor.Members.Find(member => member.Name.Identifier.Text == "ContainKey");
                var containValue = visitor.Members.Find(member => member.Name.Identifier.Text == "ContainValue");

                return containKey.Parent is InvocationExpressionSyntax keyInvocation
                    && containValue.Parent is InvocationExpressionSyntax valueInvocation

                    && keyInvocation.ArgumentList.Arguments is SeparatedSyntaxList<ArgumentSyntax> containKeyArguments
                    && valueInvocation.ArgumentList.Arguments is SeparatedSyntaxList<ArgumentSyntax> containValueArguments

                    && containKeyArguments.First().Expression is MemberAccessExpressionSyntax keyArgument
                    && containValueArguments.First().Expression is MemberAccessExpressionSyntax valueArgument

                    && keyArgument.Expression is IdentifierNameSyntax keyIdentifier
                    && valueArgument.Expression is IdentifierNameSyntax valueIdentifier

                    && keyIdentifier.Identifier.Text == valueIdentifier.Identifier.Text;
            }

            protected static bool KeyIsProperty(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                return arguments.First().Expression is MemberAccessExpressionSyntax valueAccess
                    && valueAccess.Expression is IdentifierNameSyntax identifier
                    && valueAccess.Name.Identifier.Text == "Key";
            }
            protected static bool ValueIsProperty(SeparatedSyntaxList<ArgumentSyntax> arguments)
            {
                if (!arguments.Any()) return false;

                return arguments.First().Expression is MemberAccessExpressionSyntax valueAccess
                    && valueAccess.Expression is IdentifierNameSyntax identifier
                    && valueAccess.Name.Identifier.Text == "Value";
            }
        }

        public class ShouldContainKeyAndContainValueSyntaxVisitor : ContainKeyValueSyntaxVisitor
        {
            public ShouldContainKeyAndContainValueSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("ContainKey", KeyIsProperty), MemberValidator.And, new MemberValidator("ContainValue", ValueIsProperty))
            {
            }
        }

        public class ShouldContainValueAndContainKeySyntaxVisitor : ContainKeyValueSyntaxVisitor
        {
            public ShouldContainValueAndContainKeySyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("ContainValue", ValueIsProperty), MemberValidator.And, new MemberValidator("ContainKey", KeyIsProperty))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DictionaryShouldContainPairCodeFix)), Shared]
    public class DictionaryShouldContainPairCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DictionaryShouldContainPairAnalyzer.DiagnosticId);

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
                var remove = NodeReplacement.RemoveAndExtractArguments("ContainValue");
                var newExpression = GetNewExpression(expression, NodeReplacement.RemoveMethodBefore("ContainValue"), remove);

                var newArguments = GetArgumentsWithFirstAsPairIdentifierArgument(remove.Arguments);

                newExpression = GetNewExpression(newExpression, NodeReplacement.RenameAndRemoveFirstArgument("ContainKey", "Contain"));

                newExpression = GetNewExpression(newExpression, NodeReplacement.PrependArguments("Contain", newArguments));

                return newExpression;
            }
            else if (properties.VisitorName == nameof(DictionaryShouldContainPairAnalyzer.ShouldContainValueAndContainKeySyntaxVisitor))
            {
                var remove = NodeReplacement.RemoveAndExtractArguments("ContainKey");
                var newExpression = GetNewExpression(expression, NodeReplacement.RemoveMethodBefore("ContainKey"), remove);

                var newArguments = GetArgumentsWithFirstAsPairIdentifierArgument(remove.Arguments);

                newExpression = GetNewExpression(newExpression, NodeReplacement.RenameAndRemoveFirstArgument("ContainValue", "Contain"));

                newExpression = GetNewExpression(newExpression, NodeReplacement.PrependArguments("Contain", newArguments));

                return newExpression;
            }
            else
            {
                throw new InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
            }
        }

        private SeparatedSyntaxList<ArgumentSyntax> GetArgumentsWithFirstAsPairIdentifierArgument(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            var argument = arguments[0];
            var memberAccess = (MemberAccessExpressionSyntax)argument.Expression;
            var identifier = (IdentifierNameSyntax)memberAccess.Expression;

            return arguments.Replace(argument, argument.WithExpression(identifier));
        }
    }
}
