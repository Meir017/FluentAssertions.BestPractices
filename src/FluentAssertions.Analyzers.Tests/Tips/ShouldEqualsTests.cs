using FluentAssertions.Analyzers.Tips;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class ShouldEqualsTests
    {
        [TestMethod]
        [Implemented]
        public void ShouldEquals_TestAnalyzer()
            => VerifyCSharpDiagnosticExpressionBody("actual.Should().Equals(expected);");

        [TestMethod]
        [Implemented]
        public void ShouldEquals_TestCodeFix()
            => VerifyCSharpFixCodeBlock(
                oldSourceAssertion: "actual.Should().Equals(expected);",
                newSourceAssertion: "actual.Should().Be(expected);");

        private void VerifyCSharpDiagnosticExpressionBody(string sourceAssertion)
        {
            var source = GenerateCode.ObjectStatement(sourceAssertion);
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = ShouldEqualsAnalyzer.DiagnosticId,
                Message = ShouldEqualsAnalyzer.Message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFixCodeBlock(string oldSourceAssertion, string newSourceAssertion)
        {
            var oldSource = GenerateCode.ObjectStatement(oldSourceAssertion);
            var newSource = GenerateCode.ObjectStatement(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }
    }
}
