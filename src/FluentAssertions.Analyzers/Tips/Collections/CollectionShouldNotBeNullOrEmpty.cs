using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionShouldNotBeNullOrEmptyAnalyzer : CollectionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Collections.CollectionShouldNotBeNullOrEmpty;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().NotBeNullOrEmpty() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldNotBeNullAndNotBeEmptySyntaxVisitor();
                yield return new ShouldNotBeEmptyAndNotBeNullSyntaxVisitor();
            }
        }

        public class ShouldNotBeNullAndNotBeEmptySyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldNotBeNullAndNotBeEmptySyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("NotBeNull"), MemberValidator.And, new MemberValidator("NotBeEmpty"))
            {
            }
        }
        public class ShouldNotBeEmptyAndNotBeNullSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldNotBeEmptyAndNotBeNullSyntaxVisitor() : base(MemberValidator.Should, new MemberValidator("NotBeEmpty"), MemberValidator.And, new MemberValidator("NotBeNull"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionShouldNotBeNullOrEmptyCodeFix)), Shared]
    public class CollectionShouldNotBeNullOrEmptyCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionShouldNotBeNullOrEmptyAnalyzer.DiagnosticId);

        protected override bool CanRewriteAssertion(ExpressionSyntax expression)
        {
            var visitor = new MemberAccessExpressionsCSharpSyntaxVisitor();
            expression.Accept(visitor);

            var notBeEmpty = visitor.Members.Find(member => member.Name.Identifier.Text == "NotBeEmpty");
            var notBeNull = visitor.Members.Find(member => member.Name.Identifier.Text == "NotBeNull");

            return !(notBeEmpty.Parent is InvocationExpressionSyntax notBeEmptyInvocation && notBeEmptyInvocation.ArgumentList.Arguments.Any()
                && notBeNull.Parent is InvocationExpressionSyntax notBeNullInvocation && notBeNullInvocation.ArgumentList.Arguments.Any());
        }

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            if (properties.VisitorName == nameof(CollectionShouldNotBeNullOrEmptyAnalyzer.ShouldNotBeNullAndNotBeEmptySyntaxVisitor))
            {
                return GetCombinedAssertions(expression, "NotBeEmpty", "NotBeNull");
            }
            else if (properties.VisitorName == nameof(CollectionShouldNotBeNullOrEmptyAnalyzer.ShouldNotBeEmptyAndNotBeNullSyntaxVisitor))
            {
                return GetCombinedAssertions(expression, "NotBeNull", "NotBeEmpty");
            }
            throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
        }

        private ExpressionSyntax GetCombinedAssertions(ExpressionSyntax expression, string removeMethod, string renameMethod)
        {
            var remove = NodeReplacement.RemoveAndExtractArguments(removeMethod);
            var newExpression = GetNewExpression(expression, NodeReplacement.RemoveMethodBefore(removeMethod), remove);

            return GetNewExpression(newExpression, NodeReplacement.RenameAndPrependArguments(renameMethod, "NotBeNullOrEmpty", remove.Arguments));
        }
    }
}
