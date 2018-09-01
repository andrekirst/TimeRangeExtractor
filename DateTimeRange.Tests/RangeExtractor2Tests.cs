using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using Xunit;

namespace DateTimeRange.Tests
{
    public class RangeExtractor2Tests
    {
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;

        private class MockImplementationTodayCalculator : DateTimeRangeCalculatorBase
        {
            public override string Name => "Today";

            public override DateTimeRange CalculateFromInput(string input = "")
            {
                return new DateTimeRange
                {
                    Start = new DateTime(year: 1986, month: 4, day: 11),
                    End = new DateTime(year: 1986, month: 4, day: 11)
                };
            }

            public override bool DoesMatchInput(string input)
            {
                return true;
            }
        }

        public RangeExtractor2Tests()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        }

        [Fact]
        public void Test_Execution_Expect_SelectorNames()
        {
            List<DateTimeRangeCalculatorBase> calculators = new List<DateTimeRangeCalculatorBase>
            {
                new MockImplementationTodayCalculator
                {
                    DateTimeProvider = _mockDateTimeProvider.Object
                }
            };

            _mockDateTimeProvider
                .SetupGet(expression: m => m.Today)
                .Returns(value: new DateTime(year: 1986, month: 4, day: 11));

            RangeExtractor rangeExtractor2 = new RangeExtractor(
                dateTimeProvider: _mockDateTimeProvider.Object,
                calculators: calculators);

            ReadOnlyCollection<string> expectedSelectorNames = new List<string> { "Today" }.AsReadOnly();
            Assert.Equal(
                expected: expectedSelectorNames,
                actual: rangeExtractor2.ImplementedCalculatorNames);
        }

        [Fact]
        public void Test_Execution_Expect_Calculation_For_Input()
        {
            List<DateTimeRangeCalculatorBase> calculators = new List<DateTimeRangeCalculatorBase>
            {
                new MockImplementationTodayCalculator
                {
                    DateTimeProvider = _mockDateTimeProvider.Object
                }
            };

            _mockDateTimeProvider
                .SetupGet(expression: m => m.Today)
                .Returns(value: new DateTime(year: 1986, month: 4, day: 11));

            RangeExtractor rangeExtractor2 = new RangeExtractor(
                dateTimeProvider: _mockDateTimeProvider.Object,
                calculators: calculators);

            DateTimeRange actual = rangeExtractor2.GenerateDateTimeRangeFromInput(input: "Today");

            DateTimeRange expected = new DateTimeRange
            {
                Start = new DateTime(year: 1986, month: 4, day: 11),
                End = new DateTime(year: 1986, month: 4, day: 11)
            };

            Assert.Equal(
                expected: expected,
                actual: actual);
        }
    }
}
