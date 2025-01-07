using System;
using System.Text.RegularExpressions;

namespace Fanex.BalanceBreakdown.Business.Services
{
    public static class DateExtractor
    {
        public static DateTime ExtractDateTimeFromFileName(string input)
        {
            // Define regex patterns for each case
            const string pattern1 = @"/(\d{4})(\d{2})(\d{2})"; // Matches /YYYYMMDD
            const string pattern2 = @"\/(\d{4})\/(\d{2})\/(\d{2})"; // Matches /YYYY/MM/DD
            const string pattern3 = @"\/(\d{4})\/(\d{2})"; // Matches /YYYY/MM

            var match1 = Regex.Match(input, pattern1);
            if (match1.Success)
            {
                var year = int.Parse(match1.Groups[1].Value);
                var month = int.Parse(match1.Groups[2].Value);
                var day = int.Parse(match1.Groups[3].Value);
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            }

            var match2 = Regex.Match(input, pattern2);
            if (match2.Success)
            {
                var year = int.Parse(match2.Groups[1].Value);
                var month = int.Parse(match2.Groups[2].Value);
                var day = int.Parse(match2.Groups[3].Value);
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            }

            var match3 = Regex.Match(input, pattern3);
            if (match3.Success)
            {
                var year = int.Parse(match3.Groups[1].Value);
                var month = int.Parse(match3.Groups[2].Value);
                return new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Local);
            }

            return new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
        }
    }
}