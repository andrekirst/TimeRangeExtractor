﻿using Moq;

namespace DateTimeRangeParser.Tests.CalculationTests
{
    public class ThisYearCalculatorTests
    {
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;

        public ThisYearCalculatorTests()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        }
    }
}
