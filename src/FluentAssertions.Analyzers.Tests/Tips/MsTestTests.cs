using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class MsTestTests
    {
        [AssertionDataTestMethod]
        [AssertionDiagnostic("Assert.IsTrue(actual{0});")]
        [AssertionDiagnostic("Assert.IsTrue(bool.Parse(\"true\"){0});")]
        [Implemented]
        public void AssertIsTrue_TestAnalyzer(string assertion)
        {
            var source = GenerateCode.MsTestAssertion(methodArguments: "bool actual", assertion: assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = AssertIsTrueAnalyzer.DiagnosticId,
                Message = AssertIsTrueAnalyzer.Message,
                Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Info,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11, 13)
                }
            });
        }

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "Assert.IsTrue(actual{0});",
            newAssertion: "actual.Should().BeTrue({0});")]
        [AssertionCodeFix(
            oldAssertion: "Assert.IsTrue(bool.Parse(\"true\"){0});",
            newAssertion: "bool.Parse(\"true\").Should().BeTrue({0});")]
        [Implemented]
        public void AssertIsTrue_TestCodeFix(string oldAssertion, string newAssertion)
        {
            const string methodArguments = "bool actual";

            var oldSource = GenerateCode.MsTestAssertion(methodArguments, oldAssertion);
            var newSource = GenerateCode.MsTestAssertion(methodArguments, newAssertion);

            DiagnosticVerifier.VerifyCSharpFix<AssertIsTrueCodeFix, AssertIsTrueAnalyzer>(oldSource, newSource);
        }
    }
}
