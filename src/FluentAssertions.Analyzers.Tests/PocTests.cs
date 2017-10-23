using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class PocTests
    {
        [TestMethod]
        public void ReplaceMultipleMethodsWithOtherMethods()
        {
            string Template(string assertion) => new StringBuilder()
                .AppendLine("using System.Collections.Generic;")
                .AppendLine("using System.Linq;")
                .AppendLine("using System;")
                .AppendLine("using FluentAssertions;")
                .AppendLine("namespace PocNamespace")
                .AppendLine("{")
                .AppendLine("    public class Program")
                .AppendLine("    {")
                .AppendLine("        public static void Main()")
                .AppendLine("        {")
                .AppendLine("            var nestedList = new List<List<int>>();")
                .AppendLine($"            {assertion}")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}")
                .ToString();

            // old: .B() ... .D()
            // new: .H() ...

            var oldAssertion = "nestedList.Should().NotBeNull(\"because I said {0} so\".Substring(\"because\".Length), Environment.MachineName).And.ContainSingle().Which.Should().NotBeEmpty();";
            var newAssertion = "a.A().H(\"The Reason {0} is so {1}\", \"this\", 7).C.E.F(\"go to {0} and then {1} !.\", \"hell\", true);";

            var oldSource = Template(oldAssertion);
            var newSource = Template(newAssertion);

            var statement = ParseStatement(oldAssertion);

            var walker = new PocCSharpSyntaxWalker("Should", "NotBeNull", "And", "NotBeEmpty");
            statement.Accept(walker);

            var result = walker.IsValid;

            /*
            DiagnosticVerifier.VerifyCSharpDiagnostic<PocAnalyzer>(oldSource);
            DiagnosticVerifier.VerifyCSharpFix<PocCodeFix, PocAnalyzer>(oldSource, newSource);
            */
            var nestedList = new List<List<int>>();
            nestedList.Should().NotBeNull("because I said {0} so".Substring("because".Length + 1), Environment.MachineName).And.ContainSingle().Which.Should().NotBeEmpty();
        }

        [TestMethod]
        [Ignore]
        public void MyTestMethod()
        {
            const string testClassName = "DictionaryTests";
            string input = $@"d:\Visual Studio Projects\FluentAssertions.Analyzers\src\FluentAssertions.Analyzers.Tests\Tips\{testClassName}.cs";
            string output = $@"d:\Visual Studio Projects\FluentAssertions.Analyzers\src\FluentAssertions.Analyzers.Tests\Tips\{testClassName}.generated.cs";

            var compilation = ParseCompilationUnit(File.ReadAllText(input));

            var rewriter = new AttributesCSharpSyntaxRewriter();
            var newCompilation = compilation.Accept(rewriter);

            var newCode = newCompilation.ToFullString()
                .Replace("(            ", "(\n            ")
                .Replace(",            ", ",\n            ");

            File.WriteAllText(output, $"{newCode}");
        }
        private class AttributesCSharpSyntaxRewriter : CSharpSyntaxRewriter
        {
            const string prefix = "ToDictionary(p => p.Key, p=> p.Value)";
            const string postfix = "And.ToString()";
            static string AddToText(string text) => text
                .Replace("actual.", $"actual.{prefix}.")
                .Replace(";", $".{postfix};");


            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                if (node.AttributeLists.Count == 0) return node;

                var attributeLists = node.AttributeLists.RemoveAt(node.AttributeLists.Count - 1);

                foreach (var list in node.AttributeLists.RemoveAt(node.AttributeLists.Count - 1))
                {
                    var attribute = list.Attributes[0];
                    switch (attribute.Name)
                    {
                        case IdentifierNameSyntax identifier when identifier.Identifier.Text == "AssertionDiagnostic":
                            var argument = (LiteralExpressionSyntax)attribute.ArgumentList.Arguments[0].Expression;

                            var newText = AddToText(argument.Token.Text);

                            attributeLists = attributeLists.Add(AttributeList().AddAttributes(
                                attribute.WithArgumentList(AttributeArgumentList().AddArguments(
                                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken(newText)))
                                    ))
                                ).WithTriviaFrom(list));
                            break;
                        case IdentifierNameSyntax identifier when identifier.Identifier.Text == "AssertionCodeFix":
                            var oldAssertion = (LiteralExpressionSyntax)attribute.ArgumentList.Arguments[0].Expression;
                            var newAssertion = (LiteralExpressionSyntax)attribute.ArgumentList.Arguments[1].Expression;

                            var oldAssertionText = AddToText(oldAssertion.Token.Text);
                            var newAssertionText = AddToText(newAssertion.Token.Text);

                            attributeLists = attributeLists.Add(AttributeList().AddAttributes(
                                attribute.WithArgumentList(AttributeArgumentList().AddArguments(
                                    attribute.ArgumentList.Arguments[0].WithExpression(LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken(oldAssertionText))),
                                    attribute.ArgumentList.Arguments[1].WithExpression(LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken(newAssertionText)))
                                    ).WithTriviaFrom(attribute.ArgumentList))
                                ).WithTriviaFrom(list));
                            break;
                        default:
                            break;
                    }
                }

                attributeLists = attributeLists.Add(node.AttributeLists.Last());

                return node.WithAttributeLists(attributeLists);
            }
        }
    }
}
