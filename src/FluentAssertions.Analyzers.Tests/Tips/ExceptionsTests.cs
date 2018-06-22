﻿using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class ExceptionsTests
    {
        [AssertionDataTestMethod]        
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Contain(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().Be(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().Be(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().Throw<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowWithMessage_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<ExceptionShouldThrowWithMessageAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string*\"{0});")]        
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"a constant string*\"{0});")]  
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().Throw<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().Throw<Exception>().WithMessage(\"*a constant string\"{0});")]  
        [Implemented]
        public void ExceptionShouldThrowWithMessage_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<ExceptionShouldThrowWithMessageCodeFix, ExceptionShouldThrowWithMessageAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Be(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().Be(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(expectedMessage{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});")]
        [AssertionDiagnostic("action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});")]
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_TestAnalyzer(string assertion) => VerifyCSharpDiagnostic<ExceptionShouldThrowWithMessageAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Contain(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Be(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(expectedMessage{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().Be(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"{{expectedMessage}}*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string*\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().StartWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"a constant string*\"{0});")]  
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(expectedMessage{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage($\"*{{expectedMessage}}\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().Which.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string\"{0});")]
        [AssertionCodeFix(
            oldAssertion: "action.Should().ThrowExactly<Exception>().And.Message.Should().EndWith(\"a constant string\"{0});",
            newAssertion: "action.Should().ThrowExactly<Exception>().WithMessage(\"*a constant string\"{0});")]  
        [Implemented]
        public void ExceptionShouldThrowExactlyWithMessage_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFix<ExceptionShouldThrowWithMessageCodeFix, ExceptionShouldThrowWithMessageAnalyzer>(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnostic<TDiagnosticAnalyzer>(string sourceAssersion) where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            System.Console.WriteLine(sourceAssersion);
            var source = GenerateCode.ExceptionAssertion(sourceAssersion);

            var type = typeof(TDiagnosticAnalyzer);
            var diagnosticId = (string)type.GetField("DiagnosticId").GetValue(null);
            var message = (string)type.GetField("Message").GetValue(null);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = diagnosticId,
                Message = message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 9,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSourceAssertion, string newSourceAssertion)
            where TCodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider, new()
            where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var oldSource = GenerateCode.ExceptionAssertion(oldSourceAssertion);
            var newSource = GenerateCode.ExceptionAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(oldSource, newSource);
        }
    }
}
