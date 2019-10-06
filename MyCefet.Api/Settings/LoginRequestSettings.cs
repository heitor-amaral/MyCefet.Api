﻿using System;
using System.Collections.Generic;

namespace MyCefet.Api.Settings
{
    public class LoginRequestSettings : BaseRequestSettings
    {

        public string AcceptEncodeHeaderKey { get; set; }
        public string AcceptEncodeHeaderValue { get; set; }
        public string CacheHeaderKey { get; set; }
        public string CacheHeaderValue { get; set; }
        public string RefererHeaderKey { get; set; }
        public string RefererHeaderValue { get; set; }
        public string CookieHeaderKey { get; set; }
        public string CookieHeaderValue { get; set; }
        public string HostHeaderKey { get; set; }
        public string HostHeaderValue { get; set; }
        public string ConnectionHeaderKey { get; set; }
        public string ConnectionHeaderValue { get; set; }
        public string ContentHeaderKey { get; set; }
        public string ContentHeaderValue { get; set; }
        public string AcceptHeaderKey { get; set; }
        public string AcceptHeaderValue { get; set; }
        public string SigaaLogonUrl { get; set; }
        public string SigaaBaseUrl { get; set; }
        public string LoginParameterInit { get; set; }

        private readonly RequestSettings _requestSettings;


        public LoginRequestSettings(RequestSettings requestSettings)
        {
            _requestSettings = requestSettings;

            AcceptEncodeHeaderKey = _requestSettings.AcceptEncodeHeaderKey;
            AcceptEncodeHeaderValue = _requestSettings.AcceptEncodeHeaderValue;
            CacheHeaderKey = _requestSettings.CacheHeaderKey;
            CacheHeaderValue = _requestSettings.LoginCacheHeaderValue;
            RefererHeaderKey = _requestSettings.RefererHeaderKey;
            RefererHeaderValue = _requestSettings.RefererHeaderValue;
            CookieHeaderKey = _requestSettings.CookieHeaderKey;
            CookieHeaderValue = _requestSettings.CookieHeaderValue;
            HostHeaderKey = _requestSettings.HostHeaderKey;
            HostHeaderValue = _requestSettings.HostHeaderValue;
            ConnectionHeaderKey = _requestSettings.ConnectionHeaderKey;
            ConnectionHeaderValue = _requestSettings.ConnectionHeaderValue;
            ContentHeaderKey = _requestSettings.ContentHeaderKey;
            ContentHeaderValue = _requestSettings.ContentHeaderValue;
            AcceptHeaderKey = _requestSettings.AcceptHeaderKey;
            AcceptHeaderValue = _requestSettings.AcceptHeaderValue;
            SigaaLogonUrl = _requestSettings.SigaaLogonUrl;
            SigaaBaseUrl = _requestSettings.SigaaBaseUrl;
            LoginParameterInit = _requestSettings.LoginParameterInit;

        }

        public override Dictionary<string, string> GetHeaderDict()
        {
            return new Dictionary<string, string>
            {
                { AcceptEncodeHeaderKey, AcceptEncodeHeaderValue },
                { CacheHeaderKey, CacheHeaderValue },
                { RefererHeaderKey, RefererHeaderValue },
                { CookieHeaderKey, CookieHeaderValue },
                { HostHeaderKey, HostHeaderValue },
                { ConnectionHeaderKey,ConnectionHeaderValue },
                { ContentHeaderKey, ContentHeaderValue },
                { AcceptHeaderKey, AcceptHeaderValue }
            };
        }
    }
}
