﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace DateTimeRangeParser.Calculations
{
    public class ThisYearCalculator : DateTimeRangeCalculatorBase
    {
        public override string Name => "ThisYear";

        public override List<CultureInfo> SupportedCultures =>
            new List<CultureInfo>()
            {
                CultureInfo.GetCultureInfoByIetfLanguageTag(name: "en")
            };

        public sealed override DateTimeRange CalculateFromInput(string input = "")
        {
            DateTime today = DateTimeProvider.Today;

            return new DateTimeRange
            {
                Start = new DateTime(year: today.Year, month: 1, day: 1),
                End = new DateTime(year: today.Year, month: 12, day: 31)
            };
        }

        public override bool DoesMatchInput(string input)
        {
            return EqualsLowerMatch(
                input: input,
                match: "thisyear");
        }
    }
}