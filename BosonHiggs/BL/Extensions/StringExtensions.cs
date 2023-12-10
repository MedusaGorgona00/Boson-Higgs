using System.Text.RegularExpressions;

namespace BosonHiggsApi.BL.Extensions
{
    public static class StringExtensions
    {
        public static string ToLoggingDbConnStr(this string str)
        {
            return Regex.Replace(str, @"Database=(.*?);", $@"Database=$1_log;");
        }
    }
}
