﻿using DateTimeRangeParser.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DateTimeRangeParser
{
    public class DateTimeRange :
        IEquatable<DateTimeRange>,
        IEnumerable<DateTime>
    {
        public DateTimeRange()
        {
        }

        public DateTimeRange(
            DateTime start,
            DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTimeRange SpreadByDays(int days)
        {
            return SpreadByDays(
                daysStart: days,
                daysEnd: days);
        }

        public bool IsDateTimeBetween(DateTime dateTime)
        {
            return dateTime.Between(this);
        }

        public DateTimeRange SpreadByWeeks(int weeks)
        {
            return SpreadByDays(days: weeks * 7);
        }

        public DateTimeRange SpreadByMonths(int months)
        {
            return SpreadByMonths(
                monthsStart: months,
                monthsEnd: months);
        }

        public DateTimeRange SpreadByDays(int daysStart, int daysEnd)
        {
            return new DateTimeRange(
                start: Start.AddDays(-daysStart),
                end: End.AddDays(daysEnd));
        }

        public DateTimeRange SpreadByWeeks(int weeksStart, int weeksEnd)
        {
            return SpreadByDays(
                daysStart: weeksStart * 7,
                daysEnd: weeksEnd * 7);
        }

        public DateTimeRange SpreadByMonths(int monthsStart, int monthsEnd)
        {
            return new DateTimeRange(
                   start: Start.AddMonths(-monthsStart),
                   end: End.AddMonths(monthsEnd));
        }

        public DateTimeRange SpreadByYears(int years)
        {
            return SpreadByYears(
                yearsStart: years,
                yearsEnd: years);
        }

        public DateTimeRange SpreadByYears(int yearsStart, int yearsEnd)
        {
            return new DateTimeRange(
                start: Start.AddYears(-yearsStart),
                end: End.AddYears(yearsEnd));
        }

        public static DateTimeRange Empty =>
            new DateTimeRange(
                start: DateTime.MinValue,
                end: DateTime.MaxValue)
            {
                IsValid = false
            };

        public bool IsValid { get; protected set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeSpan TimeSpan =>
            Start >= End
            ? End - Start
            : TimeSpan.MinValue;

        public override string ToString()
        {
            return $"Start: {Start} - End: {End}";
        }

        public override bool Equals(object obj)
        {
            DateTimeRange other = obj as DateTimeRange;

            return Start == other?.Start &&
                   End == other.End;
        }

        public bool Equals(DateTimeRange other)
        {
            return other != null &&
                   IsValid == other.IsValid &&
                   Start == other.Start &&
                   End == other.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsValid, Start, End);
        }

        protected IEnumerable<DateTime> EnumerateFromStartToEnd()
        {
            if (End < Start)
            {
                yield break;
            }
            DateTime current = Start;
            while (current <= End)
            {
                yield return current;
                current = current.AddDays(value: 1);
            }
            yield break;
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return EnumerateFromStartToEnd().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(DateTimeRange range1, DateTimeRange range2)
        {
            return EqualityComparer<DateTimeRange>.Default.Equals(range1, range2);
        }

        public static bool operator !=(DateTimeRange range1, DateTimeRange range2)
        {
            return !(range1 == range2);
        }

        public static DateTimeRange operator ++(DateTimeRange dateTimeRange)
        {
            return dateTimeRange.SpreadByDays(days: 1);
        }

        public static DateTimeRange operator --(DateTimeRange dateTimeRange)
        {
            return dateTimeRange.SpreadByDays(days: -1);
        }
    }
}