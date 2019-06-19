using System;
using System.Collections.Generic;
using System.Linq;

namespace Chatison.Utilities
{
    public class Utility
    {
        public static object GetJsonResponse(Constants.ResponseStatus status, string message, string url = null)
        {
            return new { status = status.ToString(), message, url };
        }

        public static object GetSuccessResponse()
        {
            return new { status = Constants.ResponseStatus.Success.ToString().ToLowerInvariant() };
        }

        public static object GetErrorResponse(string error)
        {
            return new { status = Constants.ResponseStatus.Error.ToString().ToLowerInvariant(), error };
        }

        public static object GetErrorResponse(IEnumerable<string> errors)
        {
            return new { status = Constants.ResponseStatus.Error.ToString().ToLowerInvariant(), errors };
        }

        public static TimeZoneInfo GetTimeZone(string timeZoneName)
        {
            return TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(x => x.StandardName.Equals(timeZoneName));
        }

        public static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetDateTime(DateTime dateTime, string timeZoneName)
        {
            //return if timezone name is empty
            if (string.IsNullOrEmpty(timeZoneName))
            {
                return dateTime.ToLocalTime();
            }

            //return converted date time
            var timeZone = GetTimeZone(timeZoneName);
            return timeZone != null
                ? TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, timeZone)
                : dateTime.ToLocalTime();
        }

        public static string GetFormattedDate(DateTime? dateTime)
        {
            return dateTime?.ToString(Constants.DateFormat);
        }

        public static string DefaultErrorMessage()
        {
            return "Something went wrong. Please try again after some time";
        }
    }
}
