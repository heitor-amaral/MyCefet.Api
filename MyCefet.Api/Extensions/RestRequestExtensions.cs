using RestSharp;
using System.Collections.Generic;

namespace MyCefet.Api.Extensions
{
    public static class RestRequestExtensions
    {
        public static void SetRequestHeaders(this RestRequest restRequest, Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                restRequest.AddHeader(header.Key, header.Value);
            }
        }
    }
}
