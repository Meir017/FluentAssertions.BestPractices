using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class NullConditionalAssertionTests
    {
        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual?.Should().Be(expected{0});")]
        [AssertionDiagnostic("actual?.MyProperty.Should().Be(\"test\"{0});")]
        [AssertionDiagnostic("actual.MyProperty?.Should().Be(\"test\"{0});")]
        [Implemented]
        public void TestAnalyzer(string assertion) => VerifyCSharpDiagnostic(assertion);

        private void VerifyCSharpDiagnostic(string assertion)
        {
            var code = new StringBuilder()
                .AppendLine("using System;")
                .AppendLine("using FluentAssertions;using FluentAssertions.Extensions;")
                .AppendLine("namespace TestNamespace")
                .AppendLine("{")
                .AppendLine("    class TestClass")
                .AppendLine("    {")
                .AppendLine("        void TestMethod(MyClass actual, MyClass expected)")
                .AppendLine("        {")
                .AppendLine($"            {assertion}")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("    class MyClass")
                .AppendLine("    {")
                .AppendLine("        public string MyProperty { get; set; }")
                .AppendLine("    }")
                .AppendLine("    class Program")
                .AppendLine("    {")
                .AppendLine("        public static void Main()")
                .AppendLine("        {")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}")
                .ToString();

            DiagnosticVerifier.VerifyCSharpDiagnostic<NullConditionalAssertionAnalyzer>(code, new DiagnosticResult
            {
                Id = NullConditionalAssertionAnalyzer.DiagnosticId,
                Message = NullConditionalAssertionAnalyzer.Message,
                Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9, 13)
                }
            });
        }
    }
}
