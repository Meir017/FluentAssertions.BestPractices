namespace FluentAssertions.Analyzers
{
    public static class Constants
    {
        public static class DiagnosticProperties
        {
            public const string Title = nameof(Title);
            public const string VisitorName = nameof(VisitorName);
            public const string HelpLink = nameof(HelpLink);
        }

        public static class Tips
        {
            public const string Category = "FluentAssertionTips";
            public static class Collections
            {
                public const string CollectionsShouldBeEmpty = nameof(CollectionsShouldBeEmpty);
                public const string CollectionsShouldNotBeEmpty = nameof(CollectionsShouldNotBeEmpty);
                public const string CollectionShouldContainProperty = nameof(CollectionShouldContainProperty);
                public const string CollectionShouldNotContainProperty = nameof(CollectionShouldNotContainProperty);
                public const string CollectionShouldContainItem = nameof(CollectionShouldContainItem);
                public const string CollectionShouldNotContainItem = nameof(CollectionShouldNotContainItem);
                public const string CollectionShouldHaveCount = nameof(CollectionShouldHaveCount);
                public const string CollectionShouldHaveCountGreaterThan = nameof(CollectionShouldHaveCountGreaterThan);
                public const string CollectionShouldHaveCountGreaterOrEqualTo = nameof(CollectionShouldHaveCountGreaterOrEqualTo);
                public const string CollectionShouldHaveCountLessThan = nameof(CollectionShouldHaveCountLessThan);
                public const string CollectionShouldHaveCountLessOrEqualTo = nameof(CollectionShouldHaveCountLessOrEqualTo);
                public const string CollectionShouldNotHaveCount = nameof(CollectionShouldNotHaveCount);
                public const string CollectionShouldContainSingle = nameof(CollectionShouldContainSingle);
                public const string CollectionShouldOnlyContainProperty = nameof(CollectionShouldOnlyContainProperty);
                public const string CollectionShouldHaveSameCount = nameof(CollectionShouldHaveSameCount);
                public const string CollectionShouldNotHaveSameCount = nameof(CollectionShouldNotHaveSameCount);
                public const string CollectionShouldContainSingleProperty = nameof(CollectionShouldContainSingleProperty);
                public const string CollectionShouldNotBeNullOrEmpty = nameof(CollectionShouldNotBeNullOrEmpty);
                public const string CollectionShouldHaveElementAt = nameof(CollectionShouldHaveElementAt);
                public const string CollectionShouldBeInAscendingOrder = nameof(CollectionShouldBeInAscendingOrder);
                public const string CollectionShouldBeInDescendingOrder = nameof(CollectionShouldBeInDescendingOrder);
                public const string CollectionShouldEqualOtherCollectionByComparer = nameof(CollectionShouldEqualOtherCollectionByComparer);
                public const string CollectionShouldNotIntersectWith = nameof(CollectionShouldNotIntersectWith);
                public const string CollectionShouldIntersectWith = nameof(CollectionShouldIntersectWith);
                public const string CollectionShouldNotContainNulls = nameof(CollectionShouldNotContainNulls);
                public const string CollectionShouldOnlyHaveUniqueItems = nameof(CollectionShouldOnlyHaveUniqueItems);
                public const string CollectionShouldOnlyHaveUniqueItemsByComparer = nameof(CollectionShouldOnlyHaveUniqueItemsByComparer);
                public const string CollectionShouldHaveElementAt0Null = nameof(CollectionShouldHaveElementAt0Null);
            }

            public static class Dictionaries
            {
                public const string DictionaryShouldContainKey = nameof(DictionaryShouldContainKey);
                public const string DictionaryShouldContainValue = nameof(DictionaryShouldContainValue);
                public const string DictionaryShouldContainKeyAndValue = nameof(DictionaryShouldContainKeyAndValue);
                public const string DictionaryShouldContainPair = nameof(DictionaryShouldContainPair);
                public const string DictionaryShouldNotContainKey = nameof(DictionaryShouldNotContainKey);
                public const string DictionaryShouldNotContainValue = nameof(DictionaryShouldNotContainValue);
            }

            public static class Strings
            {
                public const string StringShouldStartWith = nameof(StringShouldStartWith);
                public const string StringShouldEndWith = nameof(StringShouldEndWith);
                public const string StringShouldNotBeNullOrEmpty = nameof(StringShouldNotBeNullOrEmpty);
                public const string StringShouldBeNullOrEmpty = nameof(StringShouldBeNullOrEmpty);
                public const string StringShouldBeNullOrWhiteSpace = nameof(StringShouldBeNullOrWhiteSpace);
                public const string StringShouldNotBeNullOrWhiteSpace = nameof(StringShouldNotBeNullOrWhiteSpace);
                public const string StringShouldHaveLength = nameof(StringShouldHaveLength);
            }

            public static class Comparable
            {
                public const string ComparableShouldBePositive = nameof(ComparableShouldBePositive);
            }

            public static class Numeric
            {
                public const string NumericShouldBePositive = nameof(NumericShouldBePositive);
                public const string NumericShouldBeNegative = nameof(NumericShouldBeNegative);
                public const string NumericShouldBeInRange = nameof(NumericShouldBeInRange);
                public const string NumericShouldBeApproximately = nameof(NumericShouldBeApproximately);
            }

            public static class Exceptions
            {
                public const string ExceptionShouldThrowWithMessage = nameof(ExceptionShouldThrowWithMessage);
                public const string ExceptionShouldThrowWithInnerException = nameof(ExceptionShouldThrowWithInnerException);
            }

            public static class MsTest
            {
                public const string AssertIsTrue = nameof(AssertIsTrue);
                public const string AssertIsFalse = nameof(AssertIsFalse);
                public const string AssertIsNotNull = nameof(AssertIsNotNull);
                public const string AssertIsNull = nameof(AssertIsNull);
                public const string AssertIsInstanceOfType = nameof(AssertIsInstanceOfType);
                public const string AssertIsNotInstanceOfType = nameof(AssertIsNotInstanceOfType);
                public const string AssertAreEqual = nameof(AssertAreEqual);
                public const string AssertAreNotEqual = nameof(AssertAreNotEqual);
                public const string AssertAreSame = nameof(AssertAreSame);
                public const string AssertAreNotSame = nameof(AssertAreNotSame);
                public const string AssertThrowsException = nameof(AssertThrowsException);
                public const string AssertThrowsExceptionAsync = nameof(AssertThrowsExceptionAsync);
                public const string StringAssertContains = nameof(StringAssertContains);
                public const string StringAssertStartsWith = nameof(StringAssertStartsWith);
                public const string StringAssertEndsWith = nameof(StringAssertEndsWith);
                public const string StringAssertMatches = nameof(StringAssertMatches);
                public const string StringAssertDoesNotMatch = nameof(StringAssertDoesNotMatch);
                public const string CollectionAssertAllItemsAreInstancesOfType = nameof(CollectionAssertAllItemsAreInstancesOfType);
                public const string CollectionAssertAreEqual = nameof(CollectionAssertAreEqual);
                public const string CollectionAssertAreNotEqual = nameof(CollectionAssertAreNotEqual);
                public const string CollectionAssertAreEquivalent = nameof(CollectionAssertAreEquivalent);
                public const string CollectionAssertAreNotEquivalent = nameof(CollectionAssertAreNotEquivalent);
                public const string CollectionAssertAllItemsAreNotNull = nameof(CollectionAssertAllItemsAreNotNull);
                public const string CollectionAssertAllItemsAreUnique = nameof(CollectionAssertAllItemsAreUnique);
                public const string CollectionAssertContains = nameof(CollectionAssertContains);
                public const string CollectionAssertDoesNotContain = nameof(CollectionAssertDoesNotContain);
                public const string CollectionAssertIsSubsetOf = nameof(CollectionAssertIsSubsetOf);
                public const string CollectionAssertIsNotSubsetOf = nameof(CollectionAssertIsNotSubsetOf);
            }
        }

        public static class CodeSmell
        {
            public const string Category = "FluentAssertionCodeSmell";

            public const string NullConditionalAssertion = nameof(NullConditionalAssertion);
        }
    }
}
