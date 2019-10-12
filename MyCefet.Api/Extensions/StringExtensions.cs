using System.Collections.Generic;
using System.Web;

namespace MyCefet.Api.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSubStrings(this string mainString, List<string> subsStringsToRemove)
        {
            subsStringsToRemove.ForEach(p => mainString = mainString.Replace(p, string.Empty));
            return mainString.Trim();
        }

        public static string GetDecoded(this string mainString)
        {
            return HttpUtility.HtmlDecode(mainString);
        }
    }
}
