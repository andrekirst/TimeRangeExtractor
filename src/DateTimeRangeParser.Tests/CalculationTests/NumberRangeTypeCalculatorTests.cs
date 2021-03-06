﻿using DateTimeRangeParser.Calculations;
using Moq;
using System;
using Xunit;
using Shouldly;
using System.Collections.Generic;

namespace DateTimeRangeParser.Tests.CalculationTests
{
    public class NumberRangeTypeCalculatorTests
    {
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;

        public NumberRangeTypeCalculatorTests()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        }

        [InlineData("1d", true)]
        [InlineData("1w", true)]
        [InlineData("1m", true)]
        [InlineData("1y", true)]
        [InlineData("+1d", true)]
        [InlineData("+1w", true)]
        [InlineData("+1m", true)]
        [InlineData("+1y", true)]
        [InlineData("-1d", true)]
        [InlineData("-1w", true)]
        [InlineData("-1m", true)]
        [InlineData("-1y", true)]
        [InlineData("0d", false)]
        [InlineData("0w", false)]
        [InlineData("0m", false)]
        [InlineData("0y", false)]
        [InlineData("d", false)]
        [InlineData("w", false)]
        [InlineData("m", false)]
        [InlineData("y", false)]
        [InlineData("-d", false)]
        [InlineData("-w", false)]
        [InlineData("-m", false)]
        [InlineData("-y", false)]
        [InlineData("+d", false)]
        [InlineData("+w", false)]
        [InlineData("+m", false)]
        [InlineData("+y", false)]
        [Theory]
        public void DoesMatchInput_Parameterized(string input, bool expectedValue)
        {
            _mockDateTimeProvider
                .SetupGet(m => m.Today)
                .Returns(new DateTime(2018, 9, 16));

            NumberRangeTypeCalculator systemUnderTest = new NumberRangeTypeCalculator()
            {
                DateTimeProvider = _mockDateTimeProvider.Object
            };

            systemUnderTest.DoesMatchInput(input: input).ShouldBe(expectedValue);
        }

        [MemberData(nameof(Calculate_Parameterized_Data_MockedDate_20180916))]
        [Theory]
        public void Calculate_Parameterized(string input, DateTimeRange expectedValue)
        {
            _mockDateTimeProvider
                .SetupGet(m => m.Today)
                .Returns(new DateTime(2018, 9, 16));

            NumberRangeTypeCalculator systemUnderTest = new NumberRangeTypeCalculator()
            {
                DateTimeProvider = _mockDateTimeProvider.Object
            };

            systemUnderTest.CalculateFromInput(input: input).ShouldBe(expectedValue);
        }

        public static IEnumerable<object[]> Calculate_Parameterized_Data_MockedDate_20180916 =>
            new List<object[]>
            {
                new object[] { "1d", new DateTimeRange(new DateTime(2018, 9, 17), new DateTime(2018, 9, 17)) },
                new object[] { "1w", new DateTimeRange(new DateTime(2018, 9, 23), new DateTime(2018, 9, 23)) },
                new object[] { "1m", new DateTimeRange(new DateTime(2018, 10, 16), new DateTime(2018, 10, 16)) },
                new object[] { "1y", new DateTimeRange(new DateTime(2019, 9, 16), new DateTime(2019, 9, 16)) },
                new object[] { "+1d", new DateTimeRange(new DateTime(2018, 9, 17), new DateTime(2018, 9, 17)) },
                new object[] { "+1w", new DateTimeRange(new DateTime(2018, 9, 23), new DateTime(2018, 9, 23)) },
                new object[] { "+1m", new DateTimeRange(new DateTime(2018, 10, 16), new DateTime(2018, 10, 16)) },
                new object[] { "+1y", new DateTimeRange(new DateTime(2019, 9, 16), new DateTime(2019, 9, 16)) },
                new object[] { "-1d", new DateTimeRange(new DateTime(2018, 9, 15), new DateTime(2018, 9, 15)) },
                new object[] { "-1w", new DateTimeRange(new DateTime(2018, 9, 9), new DateTime(2018, 9, 9)) },
                new object[] { "-1m", new DateTimeRange(new DateTime(2018, 8, 16), new DateTime(2018, 8, 16)) },
                new object[] { "-1y", new DateTimeRange(new DateTime(2017, 9, 16), new DateTime(2017, 9, 16)) },
            };
    }
}
