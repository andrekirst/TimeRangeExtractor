﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeRangeParser
{
    public class DateTimeRangeParser : IDateTimeRangerParser
    {
        private readonly List<DateTimeRangeCalculatorBase> _calculators = new List<DateTimeRangeCalculatorBase>();
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Dictionary<string, DateTimeRange> _cachedValues = new Dictionary<string, DateTimeRange>();

        public event EventHandler<RaisedCalculationEventArgs> RaisedCalculation;

        public DateTimeRangeParser(
            IDateTimeProvider dateTimeProvider,
            List<DateTimeRangeCalculatorBase> calculators)
        {
            _dateTimeProvider = dateTimeProvider;
            AddCalculators(calculators: calculators);
        }

        private void AddCalculators(List<DateTimeRangeCalculatorBase> calculators)
        {
            foreach (DateTimeRangeCalculatorBase calculator in calculators)
            {
                calculator.DateTimeProvider = _dateTimeProvider;

                if (calculator.NeedsOtherCalculations)
                {
                    calculator.OtherCalculations = calculators.Where(predicate: item => item != calculator).ToList();
                }
            }
            _calculators.AddRange(collection: calculators);
        }

        public DateTimeRange Parse(string input)
        {
            if (_cachedValues.ContainsKey(key: input))
            {
                return _cachedValues[key: input];
            }

            DateTimeRangeCalculatorBase calculation = GetImplementationByInput(input: input);

            if (calculation == null)
            {
                throw new NotSupportedException(message: $"Input \"{input}\" is not supported");
            }

            DateTimeRange calculatedValue = calculation.CalculateFromInput(input: input);

            AddCalculatedValueToCache(
                input: input,
                calculatedValue: calculatedValue);

            OnRaisedCalculation(
                raisedCalculationEventArgs: new RaisedCalculationEventArgs(
                    dateTimeRangeCalculatorBase: calculation));

            return calculatedValue;
        }

        private DateTimeRangeCalculatorBase GetImplementationByInput(string input)
        {
            return _calculators
                .FirstOrDefault(predicate: c => c.DoesMatchInput(input: input));
        }

        private void AddCalculatedValueToCache(string input, DateTimeRange calculatedValue)
        {
            _cachedValues.Add(
                key: input,
                value: calculatedValue);
        }

        private void OnRaisedCalculation(RaisedCalculationEventArgs raisedCalculationEventArgs)
        {
            RaisedCalculation?.Invoke(
                sender: this,
                e: raisedCalculationEventArgs);
        }

        public IReadOnlyCollection<string> ImplementedCalculatorNames
            => _calculators
                .Select(selector: s => s.Name)
                .ToList()
                .AsReadOnly();
    }
}