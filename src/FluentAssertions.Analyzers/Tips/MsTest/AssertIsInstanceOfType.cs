using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssertIsInstanceOfTypeAnalyzer : MsTestAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.AssertIsInstanceOfType;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeOfType() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new AssertIsInstanceOfTypeSyntaxVisitor();
            }
        }

        public class AssertIsInstanceOfTypeSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
        {
            public AssertIsInstanceOfTypeSyntaxVisitor() : base(new MemberValidator("IsInstanceOfType"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AssertIsInstanceOfTypeCodeFix)), Shared]
    public class AssertIsInstanceOfTypeCodeFix : MsTestCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AssertIsInstanceOfTypeAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            var newExpression = RenameMethodAndReplaceWithSubjectShould(expression, "IsInstanceOfType", "BeOfType", "Assert");

            var beOfType = newExpression.DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .First(node => node.Name.Identifier.Text == "BeOfType");

            if (beOfType.Parent is InvocationExpressionSyntax invocation)
            {
                var arguments = invocation.ArgumentList.Arguments;
                if (arguments.Any() && arguments[0].Expression is TypeOfExpressionSyntax typeOfExpression)
                {                    
                    var genericBeOfType = beOfType.WithName(SF.GenericName(beOfType.Name.Identifier.Text)
                        .AddTypeArgumentListArguments(typeOfExpression.Type)                        
                    );
                    newExpression = newExpression.ReplaceNode(beOfType, genericBeOfType);
                    return GetNewExpression(newExpression, NodeReplacement.RemoveFirstArgument("BeOfType"));
                }
            }

            return newExpression;
        }
    }
}