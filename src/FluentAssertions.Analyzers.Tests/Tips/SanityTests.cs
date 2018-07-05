﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class SanityTests
    {
        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/11")]
        public void CountWithPredicate()
        {
            const string assertion = "actual.Count(d => d.Message.Contains(\"a\")).Should().Be(2);";
            var source = GenerateCode.EnumerableCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/10")]
        public void AssertionCallMultipleMethodWithTheSameNameAndArguments()
        {
            const string assertion = "actual.Should().Contain(d => d.Message.Contains(\"a\")).And.Contain(d => d.Message.Contains(\"c\"));";
            var source = GenerateCode.EnumerableCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/44")]
        public void CollectionShouldHaveElementAt_ShouldIgnoreDictionaryTypes()
        {
            string source = GenerateCode.DictionaryAssertion("actual[\"key\"].Should().Be(expectedValue);");
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/13")]
        public void PropertyOfIndexerShouldBe_ShouldNotThrowException()
        {
            const string assertion = "actual[0].Message.Should().Be(\"test\");";
            var source = GenerateCode.EnumerableCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/13")]
        public void PropertyOfElementAtShouldBe_ShouldNotTriggerDiagnostic()
        {
            const string assertion = "actual.ElementAt(0).Message.Should().Be(\"test\");";
            var source = GenerateCode.EnumerableCodeBlockAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/10")]
        public void NestedAssertions_ShouldNotTrigger()
        {
            const string declaration = "var nestedList = new List<List<int>>();";
            const string assertion = "nestedList.Should().NotBeNull().And.ContainSingle().Which.Should().NotBeEmpty();";
            var source = GenerateCode.EnumerableCodeBlockAssertion(declaration + assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/18")]
        public void DictionaryShouldContainPair_WhenPropertiesOfDifferentVariables_ShouldNotTrigger()
        {
            const string assertion = "actual.Should().ContainValue(pair.Value).And.ContainKey(otherPair.Key);";
            var source = GenerateCode.DictionaryAssertion(assertion);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/41")]
        public void ExpressionBasedFunction_ShouldNotThrow()
        {
            const string source = @"
public class TestClass
{
    private SomeClass CreateSomeClass() => new SomeClass();
 
    public class SomeClass
    { }
    public static void Main() { }
}";

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/58")]
        public void StaticWithNameof_ShouldNotThrow()
        {
            const string source = @"public class TestClass
{
    private static string StaticResult { get; set; }

    public static void Main()
    {
        StaticResult = nameof(Main);
    }
}";
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/49")]
        public void WritingToConsole_ShouldNotThrow()
        {
            const string source = @"
public class TestClass
{
    public static void Main()
    {
        System.Console.WriteLine();
    }
}";
            
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }    

        [TestMethod]
        [Implemented(Reason = "https://github.com/fluentassertions/fluentassertions.analyzers/issues/64")]
        public void CollectionShouldNotContainProperty_WhenAssertionIsIdiomatic_ShouldNotTrigger()
        {            
            const string source = @"
using FluentAssertions;
using FluentAssertions.Extensions;

public class TestClass
{
    public static void Main()
    {
        var list = new[] { string.Empty };
        list.Should().OnlyContain(e => e.Contains(string.Empty));
    }
}"; 

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }
    }
}
