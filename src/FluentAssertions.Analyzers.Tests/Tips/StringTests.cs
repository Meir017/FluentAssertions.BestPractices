using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class StringTests
    {
        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.StartsWith(expected).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.ToString().StartsWith(expected).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void StringShouldStartWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldStartWithAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.StartsWith(expected).Should().BeTrue({0});",
            newAssertion: "actual.Should().StartWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().StartsWith(expected).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().StartWith(expected{0}).And.ToString();")]
        [Implemented]
        public void StringShouldStartWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldStartWithCodeFix, StringShouldStartWithAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.EndsWith(expected).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.ToString().EndsWith(expected).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void StringShouldEndWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldEndWithAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.EndsWith(expected).Should().BeTrue({0});",
            newAssertion: "actual.Should().EndWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().EndsWith(expected).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().EndWith(expected{0}).And.ToString();")]
        [Implemented]
        public void StringShouldEndWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldEndWithCodeFix, StringShouldEndWithAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().NotBeNull().And.NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.Should().NotBeNull({0}).And.NotBeEmpty();")]
        [AssertionDiagnostic("string.IsNullOrEmpty(actual).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.ToString().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.ToString().Should().NotBeNull({0}).And.NotBeEmpty().And.ToString();")]
        [AssertionDiagnostic("actual.ToString().Should().NotBeEmpty({0}).And.NotBeNull({0}).And.ToString();")]
        [AssertionDiagnostic("string.IsNullOrEmpty(actual.ToString()).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void StringShouldNotBeNullOrEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldNotBeNullOrEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull({0}).And.NotBeEmpty();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull().And.NotBeEmpty({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty({0}).And.NotBeNull();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty().And.NotBeNull({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrEmpty(actual).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().Should().NotBeNull({0}).And.NotBeEmpty().And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().Should().NotBeEmpty({0}).And.NotBeNull().And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrEmpty(actual.ToString()).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrEmpty({0}).And.ToString();")]
        [Implemented]
        public void StringShouldNotBeNullOrEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldNotBeNullOrEmptyCodeFix, StringShouldNotBeNullOrEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("string.IsNullOrEmpty(actual).Should().BeTrue({0});")]
        [AssertionDiagnostic("string.IsNullOrEmpty(actual.ToString()).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void StringShouldBeNullOrEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldBeNullOrEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrEmpty(actual).Should().BeTrue({0});",
            newAssertion: "actual.Should().BeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrEmpty(actual.ToString()).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().BeNullOrEmpty({0}).And.ToString();")]
        [Implemented]
        public void StringShouldBeNullOrEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldBeNullOrEmptyCodeFix, StringShouldBeNullOrEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual).Should().BeTrue({0});")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual.ToString()).Should().BeTrue({0});")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual.ToString()).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void StringShouldBeNullOrWhiteSpace_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldBeNullOrWhiteSpaceAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual).Should().BeTrue({0});",
            newAssertion: "actual.Should().BeNullOrWhiteSpace({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.Should().BeNullOrWhiteSpace({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual.ToString()).Should().BeTrue({0});",
            newAssertion: "actual.ToString().Should().BeNullOrWhiteSpace({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual.ToString()).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().BeNullOrWhiteSpace({0}).And.ToString();")]
        [Implemented]
        public void StringShouldBeNullOrWhiteSpace_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldBeNullOrWhiteSpaceCodeFix, StringShouldBeNullOrWhiteSpaceAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual).Should().BeFalse({0});")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual.ToString()).Should().BeFalse({0});")]
        [AssertionDiagnostic("string.IsNullOrWhiteSpace(actual.ToString()).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void StringShouldNotBeNullOrWhiteSpace_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldNotBeNullOrWhiteSpaceAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotBeNullOrWhiteSpace({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.Should().NotBeNullOrWhiteSpace({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual.ToString()).Should().BeFalse({0});",
            newAssertion: "actual.ToString().Should().NotBeNullOrWhiteSpace({0});")]
        [AssertionCodeFix(
            oldAssertion: "string.IsNullOrWhiteSpace(actual.ToString()).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.ToString().Should().NotBeNullOrWhiteSpace({0}).And.ToString();")]
        [Implemented]
        public void StringShouldNotBeNullOrWhiteSpace_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldNotBeNullOrWhiteSpaceCodeFix, StringShouldNotBeNullOrWhiteSpaceAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Length.Should().Be(k{0});")]
        [AssertionDiagnostic("actual.ToString().Length.Should().Be(k{0}).And.ToString();")]
        [Implemented]
        public void StringShouldHaveLength_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<StringShouldHaveLengthAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Length.Should().Be(k{0});",
            newAssertion: "actual.Should().HaveLength(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToString().Length.Should().Be(k{0}).And.ToString();",
            newAssertion: "actual.ToString().Should().HaveLength(k{0}).And.ToString();")]
        [Implemented]
        public void StringShouldHaveLength_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<StringShouldHaveLengthCodeFix, StringShouldHaveLengthAnalyzer>(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic<TDiagnosticAnalyzer>(string sourceAssertion) where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var source = GenerateCode.StringAssertion(sourceAssertion);

            var type = typeof(TDiagnosticAnalyzer);
            var diagnosticId = (string)type.GetField("DiagnosticId").GetValue(null);
            var message = (string)type.GetField("Message").GetValue(null);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = diagnosticId,
                Message = message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9, 13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSourceAssertion, string newSourceAssertion)
            where TCodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider, new()
            where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var oldSource = GenerateCode.StringAssertion(oldSourceAssertion);
            var newSource = GenerateCode.StringAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(oldSource, newSource);
        }
    }
}
