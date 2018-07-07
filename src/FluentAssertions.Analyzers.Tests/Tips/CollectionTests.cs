using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests
{
    [TestClass]
    public class CollectionTests
    {
        public TestContext TestContext { get; set; }
        
        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void ExpressionBodyAssertion_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticExpressionBody<CollectionShouldNotBeEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeTrue({0});",
            newAssertion: "actual.Should().NotBeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void ExpressionBodyAssertion_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixExpressionBody<CollectionShouldNotBeEmptyCodeFix, CollectionShouldNotBeEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotBeEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeTrue({0});",
            newAssertion: "actual.Should().NotBeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldNotBeEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotBeEmptyCodeFix, CollectionShouldNotBeEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Any().Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.Should().HaveCount(0{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any().Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(0{0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldBeEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldBeEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any().Should().BeFalse({0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(0{0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any().Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(0{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionsShouldBeEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldBeEmptyCodeFix, CollectionShouldBeEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Any(x => x.BooleanProperty).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainProperty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldContainPropertyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any(x => x.BooleanProperty).Should().BeTrue({0});",
            newAssertion: "actual.Should().Contain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().NotBeEmpty({0});",
            newAssertion: "actual.Should().Contain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldContainPropertyCodeFix, CollectionShouldContainPropertyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Any(x => x.BooleanProperty).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().BeEmpty({0});")]
        [AssertionDiagnostic("actual.Should().OnlyContain(x => !x.BooleanProperty{0});", ignore: true)]
        [AssertionDiagnostic("actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();", ignore: true)]
        [Implemented]
        public void CollectionShouldNotContainProperty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotContainPropertyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Any(x => x.BooleanProperty).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().BeEmpty({0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().OnlyContain(x => !x.BooleanProperty{0});",
            newAssertion: "actual.Should().NotContain(x => x.BooleanProperty{0});",
            ignore: true)]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Any(x => x.BooleanProperty).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().OnlyContain(x => !x.BooleanProperty{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(x => x.BooleanProperty{0}).And.ToString();",
            ignore: true)]
        [Implemented]
        public void CollectionShouldNotContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotContainPropertyCodeFix, CollectionShouldNotContainPropertyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.All(x => x.BooleanProperty).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyContainProperty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldOnlyContainPropertyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.All(x => x.BooleanProperty).Should().BeTrue({0});",
            newAssertion: "actual.Should().OnlyContain(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().All(x => x.BooleanProperty).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyContain(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyContainProperty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldOnlyContainPropertyCodeFix, CollectionShouldOnlyContainPropertyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Contains(expectedItem).Should().BeTrue({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Contains(expectedItem).Should().BeTrue({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainItem_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldContainItemAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Contains(expectedItem).Should().BeTrue({0});",
            newAssertion: "actual.Should().Contain(expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Contains(expectedItem).Should().BeTrue({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Contain(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainItem_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldContainItemCodeFix, CollectionShouldContainItemAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Contains(unexpectedItem).Should().BeFalse({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainItem_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotContainItemAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Contains(unexpectedItem).Should().BeFalse({0});",
            newAssertion: "actual.Should().NotContain(unexpectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Contains(unexpectedItem).Should().BeFalse({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContain(unexpectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainItem_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotContainItemCodeFix, CollectionShouldNotContainItemAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().Be(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().Be(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().Be(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveCountAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(k{0});",
            newAssertion: "actual.Should().HaveCount(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(0{0});",
            newAssertion: "actual.Should().BeEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(1{0});",
            newAssertion: "actual.Should().ContainSingle({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().Be(6{0});",
            newAssertion: "actual.Should().HaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(0{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeEmpty({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle({0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().Be(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCount(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveCountCodeFix, CollectionShouldHaveCountAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterThan(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterThan(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterThan_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveCountGreaterThanAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterThan(k{0});",
            newAssertion: "actual.Should().HaveCountGreaterThan(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterThan(6{0});",
            newAssertion: "actual.Should().HaveCountGreaterThan(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterThan(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterThan(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterThan_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveCountGreaterThanCodeFix, CollectionShouldHaveCountGreaterThanAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeGreaterOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterOrEqualTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveCountGreaterOrEqualToAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterOrEqualTo(k{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeGreaterOrEqualTo(6{0});",
            newAssertion: "actual.Should().HaveCountGreaterOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeGreaterOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountGreaterOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountGreaterOrEqualTo_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveCountGreaterOrEqualToCodeFix, CollectionShouldHaveCountGreaterOrEqualToAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeLessThan(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeLessThan(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessThan(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessThan_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveCountLessThanAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessThan(k{0});",
            newAssertion: "actual.Should().HaveCountLessThan(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessThan(6{0});",
            newAssertion: "actual.Should().HaveCountLessThan(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessThan(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessThan(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessThan(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessThan(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessThan_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveCountLessThanCodeFix, CollectionShouldHaveCountLessThanAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().BeLessOrEqualTo(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().BeLessOrEqualTo(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessOrEqualTo_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveCountLessOrEqualToAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessOrEqualTo(k{0});",
            newAssertion: "actual.Should().HaveCountLessOrEqualTo(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().BeLessOrEqualTo(6{0});",
            newAssertion: "actual.Should().HaveCountLessOrEqualTo(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessOrEqualTo(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessOrEqualTo(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().BeLessOrEqualTo(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveCountLessOrEqualTo(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveCountLessOrEqualTo_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveCountLessOrEqualToCodeFix, CollectionShouldHaveCountLessOrEqualToAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().NotBe(k{0});")]
        [AssertionDiagnostic("actual.Count().Should().NotBe(6{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(k{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotHaveCountAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(k{0});",
            newAssertion: "actual.Should().NotHaveCount(k{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(6{0});",
            newAssertion: "actual.Should().NotHaveCount(6{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(k{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveCount(k{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(6{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveCount(6{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotHaveCountCodeFix, CollectionShouldNotHaveCountAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(expected.Count(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(expected.Count(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveSameCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveSameCountAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(expected.Count(){0});",
            newAssertion: "actual.Should().HaveSameCount(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(expected.Count(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveSameCount(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveSameCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveSameCountCodeFix, CollectionShouldHaveSameCountAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Count().Should().NotBe(unexpected.Count(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveSameCount_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotHaveSameCountAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Count().Should().NotBe(unexpected.Count(){0});",
            newAssertion: "actual.Should().NotHaveSameCount(unexpected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Count().Should().NotBe(unexpected.Count(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotHaveSameCount(unexpected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotHaveSameCount_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotHaveSameCountCodeFix, CollectionShouldNotHaveSameCountAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveCount(1{0});")]
        [AssertionDiagnostic("actual.Where(x => x.BooleanProperty).Should().HaveCount(1{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveCount(1{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldContainSingleAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Where(x => x.BooleanProperty).Should().HaveCount(1{0});",
            newAssertion: "actual.Should().ContainSingle(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveCount(1{0});",
            newAssertion: "actual.Should().ContainSingle({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Where(x => x.BooleanProperty).Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle(x => x.BooleanProperty{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveCount(1{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().ContainSingle({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldContainSingle_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldContainSingleCodeFix, CollectionShouldContainSingleAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().NotBeNull().And.NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.Should().NotBeEmpty().And.NotBeNull({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().NotBeNull().And.NotBeEmpty({0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().NotBeEmpty().And.NotBeNull({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotBeNullOrEmpty_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotBeNullOrEmptyAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull().And.NotBeEmpty({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty().And.NotBeNull({0});",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeNull({0}).And.NotBeEmpty();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().NotBeEmpty({0}).And.NotBeNull();",
            newAssertion: "actual.Should().NotBeNullOrEmpty({0});")]
        [Implemented]
        public void CollectionShouldNotBeNullOrEmpty_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotBeNullOrEmptyCodeFix, CollectionShouldNotBeNullOrEmptyAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.ElementAt(k).Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.ElementAt(6).Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual[k].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual[6].Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.Skip(k).First().Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.Skip(6).First().Should().Be(expectedItem{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(k).Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().ElementAt(6).Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual[k].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual[6].Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Skip(k).First().Should().Be(expectedItem{0}).And.ToString();")]
        [AssertionDiagnostic("actual.AsEnumerable().Skip(6).First().Should().Be(expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveElementAtAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.ElementAt(k).Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.ElementAt(6).Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual[k].Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual[6].Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Skip(k).First().Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(k, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.Skip(6).First().Should().Be(expectedItem{0});",
            newAssertion: "actual.Should().HaveElementAt(6, expectedItem{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().ElementAt(k).Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().ElementAt(6).Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray()[k].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.ToArray()[6].Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.ToArray().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Skip(k).First().Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(k, expectedItem{0}).And.ToString();")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Skip(6).First().Should().Be(expectedItem{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(6, expectedItem{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldHaveElementAt_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveElementAtCodeFix, CollectionShouldHaveElementAtAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.OrderBy(x => x.BooleanProperty).Should().Equal(actual{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInAscendingOrder_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldBeInAscendingOrderAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.OrderBy(x => x.BooleanProperty).Should().Equal(actual{0});",
            newAssertion: "actual.Should().BeInAscendingOrder(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().OrderBy(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeInAscendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInAscendingOrder_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldBeInAscendingOrderCodeFix, CollectionShouldBeInAscendingOrderAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0});")]
        [AssertionDiagnostic("actual.AsEnumerable().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInDescendingOrder_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldBeInDescendingOrderAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0});",
            newAssertion: "actual.Should().BeInDescendingOrder(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().OrderByDescending(x => x.BooleanProperty).Should().Equal(actual{0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().BeInDescendingOrder(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldBeInDescendingOrder_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldBeInDescendingOrderCodeFix, CollectionShouldBeInDescendingOrderAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldEqualOtherCollectionByComparer_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldEqualOtherCollectionByComparerAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0});",
            newAssertion: "actual.Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(e1 => e1.BooleanProperty).Should().Equal(expected.Select(e2 => e2.BooleanProperty){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().Equal(expected, (e1, e2) => e1.BooleanProperty == e2.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldEqualOtherCollectionByComparer_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldEqualOtherCollectionByComparerCodeFix, CollectionShouldEqualOtherCollectionByComparerAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Intersect(expected).Should().BeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Intersect(expected).Should().BeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotIntersectWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotIntersectWithAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Intersect(expected).Should().BeEmpty({0});",
            newAssertion: "actual.Should().NotIntersectWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Intersect(expected).Should().BeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotIntersectWith(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotIntersectWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotIntersectWithCodeFix, CollectionShouldNotIntersectWithAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Intersect(expected).Should().NotBeEmpty({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldIntersectWith_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldIntersectWithAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Intersect(expected).Should().NotBeEmpty({0});",
            newAssertion: "actual.Should().IntersectWith(expected{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Intersect(expected).Should().NotBeEmpty({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().IntersectWith(expected{0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldIntersectWith_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldIntersectWithCodeFix, CollectionShouldIntersectWithAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Select(x => x.BooleanProperty).Should().NotContainNulls({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(x => x.BooleanProperty).Should().NotContainNulls({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldNotContainNulls_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldNotContainNullsAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(x => x.BooleanProperty).Should().NotContainNulls({0});",
            newAssertion: "actual.Should().NotContainNulls(x => x.BooleanProperty{0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(x => x.BooleanProperty).Should().NotContainNulls({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().NotContainNulls(x => x.BooleanProperty{0}).And.ToString();")]
        [Implemented]
        [Ignore("What Should Happen?")]
        public void CollectionShouldNotContainNulls_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldNotContainNullsCodeFix, CollectionShouldNotContainNullsAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Should().HaveSameCount(actual.Distinct(){0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Should().HaveSameCount(actual.AsEnumerable().Distinct(){0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItems_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldOnlyHaveUniqueItemsAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Should().HaveSameCount(actual.Distinct(){0});",
            newAssertion: "actual.Should().OnlyHaveUniqueItems({0});")]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Should().HaveSameCount(actual.AsEnumerable().Distinct(){0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItems_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldOnlyHaveUniqueItemsCodeFix, CollectionShouldOnlyHaveUniqueItemsAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0});")]
        [AssertionDiagnostic("actual.AsEnumerable().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();")]
        [Implemented]
        public void CollectionShouldOnlyHaveUniqueItemsByComparer_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldOnlyHaveUniqueItemsByComparerAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0});",
            newAssertion: "actual.Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0});")]
        [Implemented]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().Select(x => x.BooleanProperty).Should().OnlyHaveUniqueItems({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().OnlyHaveUniqueItems(x => x.BooleanProperty{0}).And.ToString();")]
        public void CollectionShouldOnlyHaveUniqueItemsByComparer_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldOnlyHaveUniqueItemsByComparerCodeFix, CollectionShouldOnlyHaveUniqueItemsByComparerAnalyzer>(oldAssertion, newAssertion);

        [AssertionDataTestMethod]
        [AssertionDiagnostic("actual.FirstOrDefault().Should().BeNull({0});")]
        [NotImplemented]
        [AssertionDiagnostic("actual.AsEnumerable().FirstOrDefault().Should().BeNull({0}).And.ToString();")]
        [Ignore("What Should Happen?")]
        public void CollectionShouldHaveElementAt0Null_TestAnalyzer(string assertion) => VerifyCSharpDiagnosticCodeBlock<CollectionShouldHaveElementAt0NullAnalyzer>(assertion);

        [AssertionDataTestMethod]
        [AssertionCodeFix(
            oldAssertion: "actual.FirstOrDefault().Should().BeNull({0});",
            newAssertion: "actual.Should().HaveElementAt(0, null{0});")]
        [NotImplemented]
        [AssertionCodeFix(
            oldAssertion: "actual.AsEnumerable().FirstOrDefault().Should().BeNull({0}).And.ToString();",
            newAssertion: "actual.AsEnumerable().Should().HaveElementAt(0, null{0}).And.ToString();")]
        [Ignore("What Should Happen?")]
        public void CollectionShouldHaveElementAt0Null_TestCodeFix(string oldAssertion, string newAssertion) => VerifyCSharpFixCodeBlock<CollectionShouldHaveElementAt0NullCodeFix, CollectionShouldHaveElementAt0NullAnalyzer>(oldAssertion, newAssertion);

        private void VerifyCSharpDiagnosticCodeBlock<TDiagnosticAnalyzer>(string sourceAssertion) where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var source = GenerateCode.EnumerableCodeBlockAssertion(sourceAssertion);

            var type = typeof(TDiagnosticAnalyzer);
            var diagnosticId = (string)type.GetField("DiagnosticId").GetValue(null);
            var message = (string)type.GetField("Message").GetValue(null);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = diagnosticId,
                Message = message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 11,13)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpDiagnosticExpressionBody<TDiagnosticAnalyzer>(string sourceAssertion) where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var source = GenerateCode.EnumerableExpressionBodyAssertion(sourceAssertion);

            var type = typeof(TDiagnosticAnalyzer);
            var diagnosticId = (string)type.GetField("DiagnosticId").GetValue(null);
            var message = (string)type.GetField("Message").GetValue(null);

            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = diagnosticId,
                Message = message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10,16)
                },
                Severity = DiagnosticSeverity.Info
            });
        }

        private void VerifyCSharpFixCodeBlock<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSourceAssertion, string newSourceAssertion)
            where TCodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider, new()
            where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var oldSource = GenerateCode.EnumerableCodeBlockAssertion(oldSourceAssertion);
            var newSource = GenerateCode.EnumerableCodeBlockAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(oldSource, newSource);
        }

        private void VerifyCSharpFixExpressionBody<TCodeFixProvider, TDiagnosticAnalyzer>(string oldSourceAssertion, string newSourceAssertion)
            where TCodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider, new()
            where TDiagnosticAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var oldSource = GenerateCode.EnumerableExpressionBodyAssertion(oldSourceAssertion);
            var newSource = GenerateCode.EnumerableExpressionBodyAssertion(newSourceAssertion);

            DiagnosticVerifier.VerifyCSharpFix<TCodeFixProvider, TDiagnosticAnalyzer>(oldSource, newSource);
        }

        private void VerifyCSharpNoDiagnosticsCodeBlock(string assertion)
        {
            var source = GenerateCode.EnumerableCodeBlockAssertion(assertion);
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source);
        }
    }
}
