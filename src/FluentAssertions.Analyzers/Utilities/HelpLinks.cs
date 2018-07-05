﻿using System;
using System.Collections.Generic;

namespace FluentAssertions.Analyzers
{
    public static class HelpLinks
    {
        private static readonly Dictionary<Type, string> TypesHelpLinks;
        private static string GetHelpLink(string id) => $"https://fluentassertions.com/tips/#{id}";

        static HelpLinks()
        {
            TypesHelpLinks = new Dictionary<Type, string>
            {
                [typeof(CollectionShouldNotBeEmptyAnalyzer.AnyShouldBeTrueSyntaxVisitor)] = GetHelpLink("Collections-1"),
                [typeof(CollectionShouldBeEmptyAnalyzer.AnyShouldBeFalseSyntaxVisitor)] = GetHelpLink("Collections-2"),
                [typeof(CollectionShouldContainPropertyAnalyzer.AnyShouldBeTrueSyntaxVisitor)] = GetHelpLink("Collections-3"),
                [typeof(CollectionShouldNotContainPropertyAnalyzer.AnyShouldBeFalseSyntaxVisitor)] = GetHelpLink("Collections-4"),
                [typeof(CollectionShouldOnlyContainPropertyAnalyzer.AllShouldBeTrueSyntaxVisitor)] = GetHelpLink("Collections-5"),
                [typeof(CollectionShouldContainItemAnalyzer.ContainsShouldBeTrueSyntaxVisitor)] = GetHelpLink("Collections-6"),
                [typeof(CollectionShouldNotContainItemAnalyzer.ContainsShouldBeFalseSyntaxVisitor)] = GetHelpLink("Collections-7"),
                // missing Collections-8
                [typeof(CollectionShouldHaveCountAnalyzer.CountShouldBeSyntaxVisitor)] = GetHelpLink("Collections-9"),
                [typeof(CollectionShouldHaveCountGreaterThanAnalyzer.CountShouldBeGreaterThanSyntaxVisitor)] = GetHelpLink("Collections-10"),
                [typeof(CollectionShouldHaveCountGreaterOrEqualToAnalyzer.CountShouldBeGreaterOrEqualToSyntaxVisitor)] = GetHelpLink("Collections-11"),
                [typeof(CollectionShouldHaveCountLessThanAnalyzer.CountShouldBeLessThanSyntaxVisitor)] = GetHelpLink("Collections-12"),
                [typeof(CollectionShouldHaveCountLessOrEqualToAnalyzer.CountShouldBeLessOrEqualToSyntaxVisitor)] = GetHelpLink("Collections-13"),
                [typeof(CollectionShouldNotHaveCountAnalyzer.CountShouldNotBeSyntaxVisitor)] = GetHelpLink("Collections-14"),
                [typeof(CollectionShouldContainSingleAnalyzer.ShouldHaveCount1SyntaxVisitor)] = GetHelpLink("Collections-15"),
                [typeof(CollectionShouldHaveCountAnalyzer.CountShouldBe1SyntaxVisitor)] = GetHelpLink("Collections-15"),
                [typeof(CollectionShouldBeEmptyAnalyzer.ShouldHaveCount0SyntaxVisitor)] = GetHelpLink("Collections-16"),
                [typeof(CollectionShouldHaveCountAnalyzer.CountShouldBe0SyntaxVisitor)] = GetHelpLink("Collections-16"), // hmmm
                [typeof(CollectionShouldHaveSameCountAnalyzer.ShouldHaveCountOtherCollectionCountSyntaxVisitor)] = GetHelpLink("Collections-17"),
                [typeof(CollectionShouldNotHaveSameCountAnalyzer.CountShouldNotBeOtherCollectionCountSyntaxVisitor)] = GetHelpLink("Collections-18"),
                [typeof(CollectionShouldContainPropertyAnalyzer.WhereShouldNotBeEmptySyntaxVisitor)] = GetHelpLink("Collections-19"),
                [typeof(CollectionShouldNotContainPropertyAnalyzer.WhereShouldBeEmptySyntaxVisitor)] = GetHelpLink("Collections-20"),
                [typeof(CollectionShouldContainSingleAnalyzer.WhereShouldHaveCount1SyntaxVisitor)] = GetHelpLink("Collections-21"),
                [typeof(CollectionShouldNotContainPropertyAnalyzer.ShouldOnlyContainSyntaxVisitor)] = GetHelpLink("Collections-22"),
                [typeof(CollectionShouldNotBeNullOrEmptyAnalyzer.ShouldNotBeNullAndNotBeEmptySyntaxVisitor)] = GetHelpLink("Collections-23"),
                [typeof(CollectionShouldNotBeNullOrEmptyAnalyzer.ShouldNotBeEmptyAndNotBeNullSyntaxVisitor)] = GetHelpLink("Collections-23"),
                [typeof(CollectionShouldHaveElementAtAnalyzer.ElementAtIndexShouldBeSyntaxVisitor)] = GetHelpLink("Collections-24"),
                [typeof(CollectionShouldHaveElementAtAnalyzer.IndexerShouldBeSyntaxVisitor)] = GetHelpLink("Collections-25"),
                [typeof(CollectionShouldHaveElementAtAnalyzer.SkipFirstShouldBeSyntaxVisitor)] = GetHelpLink("Collections-26"),
                [typeof(CollectionShouldBeInAscendingOrderAnalyzer.OrderByShouldEqualSyntaxVisitor)] = GetHelpLink("Collections-27"),
                [typeof(CollectionShouldBeInDescendingOrderAnalyzer.OrderByDescendingShouldEqualSyntaxVisitor)] = GetHelpLink("Collections-28"),
                [typeof(CollectionShouldEqualOtherCollectionByComparerAnalyzer.SelectShouldEqualOtherCollectionSelectSyntaxVisitor)] = GetHelpLink("Collections-29"),
                [typeof(CollectionShouldNotIntersectWithAnalyzer.IntersectShouldBeEmptySyntaxVisitor)] = GetHelpLink("Collections-30"),
                [typeof(CollectionShouldIntersectWithAnalyzer.IntersectShouldNotBeEmptySyntaxVisitor)] = GetHelpLink("Collections-31"),
                [typeof(CollectionShouldNotContainNullsAnalyzer.SelectShouldNotContainNullsSyntaxVisitor)] = GetHelpLink("Collections-32"),
                [typeof(CollectionShouldOnlyHaveUniqueItemsAnalyzer.ShouldHaveSameCountThisCollectionDistinctSyntaxVisitor)] = GetHelpLink("Collections-33"),
                [typeof(CollectionShouldOnlyHaveUniqueItemsByComparerAnalyzer.SelectShouldOnlyHaveUniqueItemsSyntaxVisitor)] = GetHelpLink("Collections-34"),

                [typeof(NumericShouldBePositiveAnalyzer.NumericShouldBeBeGreaterThan0SyntaxVisitor)] = GetHelpLink("Comparable-and-Numerics-1"),
                [typeof(NumericShouldBeNegativeAnalyzer.NumericShouldBeBeLessThan0SyntaxVisitor)] = GetHelpLink("Comparable-and-Numerics-2"),
                [typeof(NumericShouldBeApproximatelyAnalyzer.MathAbsShouldBeLessOrEqualToSyntaxVisitor)] = GetHelpLink(""), // TODO: add to docs
                [typeof(NumericShouldBeInRangeAnalyzer.BeGreaterOrEqualToAndBeLessOrEqualToSyntaxVisitor)] = GetHelpLink(""), // TODO: add to docs
                [typeof(NumericShouldBeInRangeAnalyzer.BeLessOrEqualToAndBeGreaterOrEqualToSyntaxVisitor)] = GetHelpLink(""), // TODO: add to docs

                [typeof(DictionaryShouldContainKeyAnalyzer.ContainsKeyShouldBeTrueSyntaxVisitor)] = GetHelpLink("Dictionaries-1"),
                [typeof(DictionaryShouldNotContainKeyAnalyzer.ContainsKeyShouldBeFalseSyntaxVisitor)] = GetHelpLink("Dictionaries-2"),
                [typeof(DictionaryShouldContainValueAnalyzer.ContainsValueShouldBeTrueSyntaxVisitor)] = GetHelpLink("Dictionaries-3"),
                [typeof(DictionaryShouldNotContainValueAnalyzer.ContainsValueShouldBeFalseSyntaxVisitor)] = GetHelpLink("Dictionaries-4"),
                [typeof(DictionaryShouldContainKeyAndValueAnalyzer.ShouldContainKeyAndContainValueSyntaxVisitor)] = GetHelpLink("Dictionaries-5"),
                [typeof(DictionaryShouldContainKeyAndValueAnalyzer.ShouldContainValueAndContainKeySyntaxVisitor)] = GetHelpLink("Dictionaries-5"),
                [typeof(DictionaryShouldContainPairAnalyzer.ShouldContainKeyAndContainValueSyntaxVisitor)] = GetHelpLink("Dictionaries-6"),
                [typeof(DictionaryShouldContainPairAnalyzer.ShouldContainValueAndContainKeySyntaxVisitor)] = GetHelpLink("Dictionaries-6"),

                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldContain)] = GetHelpLink("Exceptions-1"),
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldContain)] = GetHelpLink("Exceptions-1"),
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldContain)] = GetHelpLink("Exceptions-2"),
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldContain)] = GetHelpLink("Exceptions-2"),
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldBe)] = GetHelpLink(""),  // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldBe)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldBe)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldBe)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldStartWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldStartWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldStartWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldEndWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldEndWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldEndWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldEndWith)] = GetHelpLink(""), // TODO: add to docs
                [typeof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldStartWith)] = GetHelpLink(""), // TODO: add to docs

                [typeof(StringShouldStartWithAnalyzer.StartWithShouldBeTrueSyntaxVisitor)] = GetHelpLink("Strings-1"),
                [typeof(StringShouldEndWithAnalyzer.EndWithShouldBeTrueSyntaxVisitor)] = GetHelpLink("Strings-2"),
                [typeof(StringShouldNotBeNullOrEmptyAnalyzer.StringShouldNotBeNullAndNotBeEmptySyntaxVisitor)] = GetHelpLink("Strings-3"),
                [typeof(StringShouldNotBeNullOrEmptyAnalyzer.StringShouldNotBeEmptyAndNotBeNullSyntaxVisitor)] = GetHelpLink("Strings-3"),
                [typeof(StringShouldBeNullOrEmptyAnalyzer.StringIsNullOrEmptyShouldBeTrueSyntaxVisitor)] = GetHelpLink("Strings-4"),
                [typeof(StringShouldNotBeNullOrEmptyAnalyzer.StringIsNullOrEmptyShouldBeFalseSyntaxVisitor)] = GetHelpLink("Strings-5"),
                [typeof(StringShouldBeNullOrWhiteSpaceAnalyzer.StringShouldBeNullOrWhiteSpaceSyntaxVisitor)] = GetHelpLink("Strings-6"),
                [typeof(StringShouldNotBeNullOrWhiteSpaceAnalyzer.StringShouldNotBeNullOrWhiteSpaceSyntaxVisitor)] = GetHelpLink("Strings-7"),
                [typeof(StringShouldHaveLengthAnalyzer.LengthShouldBeSyntaxVisitor)] = GetHelpLink("Strings-8"),
                [typeof(StringShouldBeNullOrEmptyAnalyzer.StringShouldBeEmptyAndBeNullSyntaxVisitor)] = GetHelpLink(""), // TODO: add to docs
                [typeof(StringShouldBeNullOrEmptyAnalyzer.StringShouldBeNullAndBeEmptySyntaxVisitor)] = GetHelpLink(""), // TODO: add to docs
            };
        }

        public static string Get(Type type)
            => TypesHelpLinks.TryGetValue(type, out var value) ? value : string.Empty; 
    }
}