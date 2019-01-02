﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace DateTimeRangeParser
{
    public abstract class DateTimeRangeCalculatorBase
    {
        public IDateTimeProvider DateTimeProvider { protected get; set; }

        protected DateTime Today => DateTimeProvider.Today;

        public abstract string Name { get; }

        public abstract bool DoesMatchInput(string input);

        public abstract DateTimeRange CalculateFromInput(string input = "");

        public List<DateTimeRangeCalculatorBase> OtherCalculations { protected get; set; } = new List<DateTimeRangeCalculatorBase>();

        public virtual bool NeedsOtherCalculations { get; } = false;

        public abstract List<CultureInfo> SupportedCultures { get; }

        protected static bool EqualsLowerMatch(string input, string match)
            => input?.ToLower() == match?.ToLower();

        public override string ToString()
        {
            return $"{Name} - ({string.Join(separator: ", ", values: SupportedCultures.Select(selector: s => s.EnglishName))})";
        }

        public override bool Equals(object obj)
        {
            DateTimeRangeCalculatorBase other = obj as DateTimeRangeCalculatorBase;
            return Name == other?.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}