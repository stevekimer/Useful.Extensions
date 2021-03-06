﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Useful.Extensions.Tests
{
    /// <summary>
    /// string extension tests
    /// </summary>
    public class StringExtensionTests
    {
        #region Has Value Tests

        [Theory]
        [InlineData("RGS", "rGs")]
        [InlineData("XYZ", "x")]
        [InlineData("This test string is quite a long one", "This teST strIng is QuitE a lOng onE")]
        public void test_string_has_value_returns_true_ignoring_case(string testString, string comparisonString)
        {
            // Arrange
            // Act
            var valid = testString.HasValue(comparisonString);

            // Assert
            valid.Should().BeTrue();
        }

        [Theory]
        [InlineData("RGS", "RGS")]
        [InlineData("XYZ", "X")]
        [InlineData("This test string is quite a long one", "long")]
        [InlineData("This test string is quite a long one", "This test string is quite a long one")]
        public void test_string_has_value_returns_true_depending_on_case(string testString, string comparisonString)
        {
            // Arrange
            // Act
            var valid = testString.HasValue(comparisonString, StringComparison.Ordinal);

            // Assert
            valid.Should().BeTrue();
        }

        [Theory]
        [InlineData("RGS", "F")]
        [InlineData("XYZ", "g")]
        [InlineData("This test string is quite a long one", "This tEst stRing IS quiTe a Long one2")]
        public void test_string_has_value_contains_returns_false_ignoring_case(string testString, string comparisonString)
        {
            // Arrange
            // Act
            var valid = testString.HasValue(comparisonString);

            // Assert
            valid.Should().BeFalse();
        }

        [Theory]
        [InlineData("RGS", "rGs")]
        [InlineData("XYZ", "x")]
        [InlineData("This test string is quite a long one", "This teST strIng is QuitE a lOng onE")]
        public void test_string_has_value_returns_false_depending_on_case(string testString, string comparisonString)
        {
            // Arrange
            // Act
            var valid = testString.HasValue(comparisonString, StringComparison.Ordinal);

            // Assert
            valid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void test_string_has_value_returns_false_if_source_is_null_or_empty(string sometext)
        {
            // Arrange
            // Act
            var valid = sometext.HasValue("sometext");

            // Assert
            valid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void test_string_has_value_returns_false_if_find_is_null_or_empty(string findtext)
        {
            // Arrange
            var sometext = "sometext";

            // Act
            var valid = sometext.HasValue(findtext);

            // Assert
            valid.Should().BeFalse();
        }

        #endregion Has Value Tests

        #region EqualTo

        public static IEnumerable<object[]> StringCaseTestData
        {
            get
            {
                yield return new object[] { "sometext", "SOMETEXT" };
                yield return new object[] { "sometext", "sometext" };
                yield return new object[] { "sometext", "SometexT" };
                yield return new object[] { "sometext", "sOmetexT" };
            }
        }

        [Theory]
        [MemberData(nameof(StringCaseTestData))]
        public void test_string_is_equal_regardless_of_case(string value, string compare)
        {
            // Arrange
            // Act
            // Assert
            value.EqualsIgnoreCase(compare).Should().BeTrue();
        }

        #endregion EqualTo

        #region SubstringOrEmpty

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void test_try_substring_retuns_empty_string_when_null_empty(string value)
        {
            // Arrange
            // Act
            // Assert
            value.SubstringOrEmpty(1, 10).Should().Be(string.Empty);
        }

        [Theory]
        [InlineData(6, 10)]
        [InlineData(-1, -5)]
        [InlineData(0, -1)]
        public void test_try_substring_returns_empty_string_when_index_and_length_are_out_of_bounds(int start, int length)
        {
            // Arrange
            var value = "Test";

            // Act
            // Assert
            value.SubstringOrEmpty(start, length).Should().Be(string.Empty);
        }

        [Theory]
        [InlineData(0, "Some text to create a test")]
        [InlineData(1, "ome text to create a test")]
        [InlineData(6, "ext to create a test")]
        [InlineData(10, "to create a test")]
        public void test_if_only_the_start_given_return_the_requested_remaining_part_of_the_value(int start, string expected)
        {
            // Arrange
            var value = "Some text to create a test";

            // Act
            // Assert
            value.SubstringOrEmpty(start).Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 1, "S")]
        [InlineData(6, 5, "ext t")]
        [InlineData(10, 9, "to create")]
        [InlineData(0, 26, "Some text to create a test")]
        public void test_if_the_start_and_length_is_given_return_the_requested_part_of_the_value(int start, int length, string expected)
        {
            // Arrange
            var value = "Some text to create a test";

            // Act
            // Assert
            value.SubstringOrEmpty(start, length).Should().Be(expected);
        }

        #endregion SubstringOrEmpty

        #region Safe Trim

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("  ", "")]
        [InlineData("Some text    ", "Some text")]
        [InlineData("   Some text    ", "Some text")]
        public void test_if_trim_correctly_trims_or_returns_if_null_or_empty(string value, string expected)
        {
            // Arrange
            // Act
            // Assert
            value.SafeTrim().Should().Be(expected);
        }

        #endregion Safe Trim

        #region SubstringAfterValue

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void test_substring_after_value_with_null_or_empty_returns_empty_string(string value)
        {
            // Arrange
            // Act
            var result = value.SubstringAfterValue("something");

            // Assert
            result.Should().Be(string.Empty);
        }

        [Theory]
        [InlineData("string", " value to find from")]
        [InlineData("from", "")]
        [InlineData("", "some string value to find from")]
        [InlineData(null, "some string value to find from")]
        [InlineData("another", "some string value to find from")]
        [InlineData("m", "e string value to find from")]
        [InlineData("s", "ome string value to find from")]
        public void test_substring_after_value_with_string_find_returns_the_expected_string(string find, string expected)
        {
            // Arrange
            var text = "some string value to find from";

            // Act
            var result = text.SubstringAfterValue(find);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData('v', "alue to find from you")]
        [InlineData('V', "alue to find from you")]
        [InlineData('z', "some string value to find from you")]
        [InlineData('Z', "some string value to find from you")]
        [InlineData('l', "ue to find from you")]
        [InlineData('L', "ue to find from you")]
        [InlineData('s', "ome string value to find from you")]
        [InlineData('S', "ome string value to find from you")]
        [InlineData('y', "ou")]
        [InlineData('Y', "ou")]
        public void test_substring_after_value_with_character_find_returns_the_expected_string(char find, string expected)
        {
            // Arrange
            var text = "some string value to find from you";

            // Act
            var result = text.SubstringAfterValue(find);

            // Assert
            result.Should().Be(expected);
        }

        #endregion SubstringAfterValue

        #region SubstringBeforeValue

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void test_substring_before_value_with_null_or_empty_returns_empty_string(string value)
        {
            // Arrange
            // Act
            var result = value.SubstringBeforeValue("something");

            // Assert
            result.Should().Be(string.Empty);
        }

        [Theory]
        [InlineData("string", "some ")]
        [InlineData("from", "some string value to find ")]
        [InlineData("", "some string value to find from")]
        [InlineData(null, "some string value to find from")]
        [InlineData("another", "some string value to find from")]
        [InlineData("m", "so")]
        [InlineData("s", "")]
        public void test_substring_before_value_with_string_find_returns_the_expected_string(string find, string expected)
        {
            // Arrange
            var text = "some string value to find from";

            // Act
            var result = text.SubstringBeforeValue(find);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData('v', "some string ")]
        [InlineData('V', "some string ")]
        [InlineData('z', "some string value to find from you")]
        [InlineData('Z', "some string value to find from you")]
        [InlineData('l', "some string va")]
        [InlineData('L', "some string va")]
        [InlineData('s', "")]
        [InlineData('S', "")]
        [InlineData('y', "some string value to find from ")]
        [InlineData('Y', "some string value to find from ")]
        public void test_substring_before_value_with_character_find_returns_the_expected_string(char find, string expected)
        {
            // Arrange
            var text = "some string value to find from you";

            // Act
            var result = text.SubstringBeforeValue(find);

            // Assert
            result.Should().Be(expected);
        }

        #endregion SubstringBeforeValue

        #region Is base 64 string

        public static IEnumerable<object[]> InvalidBase64TestData
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { "" };
                yield return new object[] { "    " };
                yield return new object[] { "some string" };
            }
        }

        [Theory]
        [MemberData(nameof(InvalidBase64TestData))]
        public void test_is_base_64_returns_false_for_invalid_entries(string value)
        {
            // Arrange
            // Act
            var result = value.IsBase64();

            // Assert
            result.Should().BeFalse();
        }

#if !NETCOREAPP1_1
        public static IEnumerable<object[]> ValidBase64TestData
        {
            get
            {
                yield return new object[] { Convert.ToBase64String(Encoding.UTF8.GetBytes("some value"), Base64FormattingOptions.None) };
                yield return new object[] { "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" };
            }
        }

        [Theory]
        [MemberData(nameof(ValidBase64TestData))]
        public void test_is_base_64_with_a_valid_base64_string_returns_true(string value)
        {
            // Arrange
            // Act
            var result = value.IsBase64();

            // Assert
            result.Should().BeTrue();
        }
#endif

        #endregion Is base 64 string
    }
}