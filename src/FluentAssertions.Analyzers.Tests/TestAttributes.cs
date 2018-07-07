using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Analyzers.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotImplementedAttribute : TestCategoryBaseAttribute
    {
        public string Reason { get; set; }
        
        public override IList<string> TestCategories => new[] { "NotImplemented" };
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ImplementedAttribute : TestCategoryBaseAttribute
    {
        public string Reason { get; set; }

        public override IList<string> TestCategories => new[] { "Completed" };
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AssertionDiagnosticAttribute : Attribute
    {
        public string Assertion { get; }

        public bool Ignore { get; }

        public AssertionDiagnosticAttribute(string assertion, bool ignore = false)
        {
            Assertion = assertion;
            Ignore = ignore;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AssertionCodeFixAttribute : Attribute
    {
        public string OldAssertion { get; }
        public string NewAssertion { get; }

        public bool Ignore { get; }

        public AssertionCodeFixAttribute(string oldAssertion, string newAssertion, bool ignore = false)
        {
            OldAssertion = oldAssertion;
            NewAssertion = newAssertion;
            Ignore = ignore;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AssertionDataTestMethod : TestMethodAttribute
    {
        private static readonly string Empty = string.Empty;
        private static readonly string Because = "\"because it's possible\"";
        private static readonly string FormattedBecause = "\"because message with {0} placeholders {1} at {2}\", 3, \"is awesome\", DateTime.Now.Add(2.Seconds())";

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var diagnosticAttributes = testMethod.GetAttributes<AssertionDiagnosticAttribute>(false);
            var codeFixAttributes = testMethod.GetAttributes<AssertionCodeFixAttribute>(false);

            var results = new List<TestResult>();
            foreach (var diagnosticAttribute in diagnosticAttributes.Where(attribute => !attribute.Ignore))
            {
                foreach (var assertion in GetTestCases(diagnosticAttribute))
                {
                    var result = testMethod.Invoke(new[] { assertion });
                    result.DisplayName = $"{testMethod.TestMethodName} (\"{assertion}\")";

                    results.Add(result);
                }
            }
            foreach (var codeFixAttribute in codeFixAttributes.Where(attribute => !attribute.Ignore))
            {
                foreach (var (oldAssertion, newAssertion) in GetTestCases(codeFixAttribute))
                {
                    var result = testMethod.Invoke(new[] { oldAssertion, newAssertion });
                    result.DisplayName = $"{testMethod.TestMethodName} ({Environment.NewLine}old: \"{oldAssertion}\" {Environment.NewLine}new: \"{newAssertion}\")";

                    results.Add(result);
                }
            }

            return results.ToArray();
        }

        private IEnumerable<string> GetTestCases(AssertionDiagnosticAttribute attribute)
        {
            yield return SafeFormat(attribute.Assertion, Empty);
            yield return SafeFormat(attribute.Assertion, Because);
            yield return SafeFormat(attribute.Assertion, FormattedBecause);
        }
        private IEnumerable<(string oldAssertion, string newAssertion)> GetTestCases(AssertionCodeFixAttribute attribute)
        {
            yield return (SafeFormat(attribute.OldAssertion, Empty), SafeFormat(attribute.NewAssertion, Empty));
            yield return (SafeFormat(attribute.OldAssertion, Because), SafeFormat(attribute.NewAssertion, Because));
            yield return (SafeFormat(attribute.OldAssertion, FormattedBecause), SafeFormat(attribute.NewAssertion, FormattedBecause));
        }

        /// <summary>
        /// Adds an comma before the additional argument if needed.
        /// </summary>
        private string SafeFormat(string assertion, string arg)
        {
            var index = assertion.IndexOf("{0}");
            if (!string.IsNullOrEmpty(arg) && assertion[index - 1] != '(')
            {
                return string.Format(assertion, ", " + arg);
            }
            return string.Format(assertion, arg);
        }
    }
}
