
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCountBot.Extensions
{
    public static class StringExtensions
    {
        public static bool TryGetSunMoonPhase(this string timeString, int? sunriseHour, int? sunsetHour, out string output)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                output = string.Empty;
                return false;
            }

            if (!TimeOnly.TryParse(timeString, out var time))
            {
                output = string.Empty;
                return false;
            }

            var sunrise = sunriseHour ?? 6;
            var sunset = sunsetHour ?? 20;
            var hour = time.Hour;

            output = hour >= sunrise && hour < sunset ? "☀️" : "🌙";
            return !string.IsNullOrEmpty(output);
        }
    }
}
