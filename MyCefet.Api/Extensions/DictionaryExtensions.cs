using System.Collections.Generic;

namespace MyCefet.Api.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddValueToHeader(this Dictionary<string, string> dict, string key, string value)
        {
            dict[key] = dict[key] + value;
        }
    }
}
