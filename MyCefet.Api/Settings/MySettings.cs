using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyCefet.Api.Settings
{

    public partial class MySettings
    {
        [JsonProperty("Logging")]
        public Logging Logging { get; set; }

        [JsonProperty("AllowedHosts")]
        public string AllowedHosts { get; set; }

        [JsonProperty("Settings")]
        public Settings Settings { get; set; }
    }

    public partial class ApiSettings
    {
        [JsonProperty("Identifier")]
        public string Identifier { get; set; }

        [JsonProperty("AccessKey")]
        public string AccessKey { get; set; }

        [JsonProperty("Authorization")]
        public string Authorization { get; set; }
    }

    public partial class Logging
    {
        [JsonProperty("LogLevel")]
        public LogLevel LogLevel { get; set; }
    }

    public partial class LogLevel
    {
        [JsonProperty("Default")]
        public string Default { get; set; }
    }

    public partial class Settings
    {
        [JsonProperty("RequestSettings")]
        public RequestSettings RequestSettings { get; set; }

        [JsonProperty("ApiSettings")]
        public ApiSettings ApiSettings { get; set; }
    }

    public partial class RequestSettings
    {
        [JsonProperty("AcceptEncodeHeaderKey")]
        public string AcceptEncodeHeaderKey { get; set; }

        [JsonProperty("AcceptEncodeHeaderValue")]
        public string AcceptEncodeHeaderValue { get; set; }

        [JsonProperty("CacheHeaderKey")]
        public string CacheHeaderKey { get; set; }

        [JsonProperty("CacheHeaderValue")]
        public string CacheHeaderValue { get; set; }

        [JsonProperty("LoginCacheHeaderValue")]
        public string LoginCacheHeaderValue { get; set; }

        [JsonProperty("CookieHeaderKey")]
        public string CookieHeaderKey { get; set; }

        [JsonProperty("CookieHeaderValue")]
        public string CookieHeaderValue { get; set; }

        [JsonProperty("HostHeaderKey")]
        public string HostHeaderKey { get; set; }

        [JsonProperty("HostHeaderValue")]
        public string HostHeaderValue { get; set; }

        [JsonProperty("ContentHeaderKey")]
        public string ContentHeaderKey { get; set; }

        [JsonProperty("ContentHeaderValue")]
        public string ContentHeaderValue { get; set; }

        [JsonProperty("ConnectionHeaderKey")]
        public string ConnectionHeaderKey { get; set; }

        [JsonProperty("ConnectionHeaderValue")]
        public string ConnectionHeaderValue { get; set; }

        [JsonProperty("RefererHeaderKey")]
        public string RefererHeaderKey { get; set; }

        [JsonProperty("RefererHeaderValue")]
        public string RefererHeaderValue { get; set; }

        [JsonProperty("AcceptHeaderKey")]
        public string AcceptHeaderKey { get; set; }

        [JsonProperty("AcceptHeaderValue")]
        public string AcceptHeaderValue { get; set; }

        [JsonProperty("SigaaStudentPortalUrl")]
        public string SigaaStudentPortalUrl { get; set; }

        [JsonProperty("SigaaLogonUrl")]
        public string SigaaLogonUrl { get; set; }

        [JsonProperty("SigaaBaseUrl")]
        public string SigaaBaseUrl { get; set; }

        [JsonProperty("LoginParameterInit")]
        public string LoginParameterInit { get; set; }

        [JsonProperty("GradesUndefinedParameter")]
        public string GradesUndefinedParameter { get; set; }
    }

    public abstract class BaseRequestSettings
    {
        public abstract Dictionary<string, string> GetHeaderDict();
    }


}
