using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Services.Sigaa.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sigaa
{
    public class LoginService : ILoginService
    {
        private const string ACCEPT_ENC_HEADER_KEY = "Accept-Encoding";
        private const string ACCEPT_ENC_HEADER_VALUE = "gzip, deflate, br";
        private const string CACHE_HEADER_KEY = "Cache-Control";
        private const string CACHE_HEADER_VALUE = "no-cache";
        private const string REFERER_HEADER_KEY = "Referer";
        private const string REFERER_HEADER_VALUE = "https://sig.cefetmg.br/sigaa/verPortalDiscente.do";
        private const string COOKIE_HEADER_KEY = "Cookie";
        private const string COOKIE_HEADER_VALUE = "JSESSIONID=";
        private const string HOST_HEADER_KEY = "Host";
        private const string HOST_HEADER_VALUE = "sig.cefetmg.br";
        private const string CONNECTION_HEADER_KEY = "Connection";
        private const string CONNECTION_HEADER_VALUE = "keep-alive";
        private const string CONTENT_HEADER_KEY = "Content-Type";
        private const string CONTENT_HEADER_VALUE = "application/x-www-form-urlencoded";
        private const string ACCEPT_HEADER_KEY = "Accept";
        private const string ACCEPT_HEADER_VALUE = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
        private const string SIGAA_LOGON_URL = "https://sig.cefetmg.br/sigaa/logar.do?dispatch=logOn";
        private const string SIGAA_BASE_URL = "https://sig.cefetmg.br/sigaa";
        private const string LOGIN_PARAMETER_INIT = "width=800&height=600&urlRedirect=&subsistemaRedirect=&acao=&acessibilidade=&user.login=";

        private async Task<string> GetJSession()
        {
            try
            {
                var client = new RestClient(SIGAA_BASE_URL);
                var request = new RestRequest(Method.GET);
                IRestResponse response = await client.ExecuteGetTaskAsync(request);

                return response.Cookies[0]?.Value;
            }
            catch
            {
                return string.Empty;
            }
        }
        public async Task<string> Login(string username, string password)
        {
            try
            {
                string jsession = await GetJSession();

                var client = new RestClient(SIGAA_LOGON_URL);
                var request = new RestRequest(Method.POST);

                var logonHeaders = new Dictionary<string, string>  
                {
                    {ACCEPT_ENC_HEADER_KEY, ACCEPT_ENC_HEADER_VALUE },
                    {CACHE_HEADER_KEY, CACHE_HEADER_VALUE },
                    {COOKIE_HEADER_KEY, COOKIE_HEADER_VALUE + jsession },
                    { HOST_HEADER_KEY, HOST_HEADER_VALUE},
                    {CONTENT_HEADER_KEY, CONTENT_HEADER_VALUE },
                    {CONNECTION_HEADER_KEY, CONNECTION_HEADER_VALUE },
                    {ACCEPT_HEADER_KEY, ACCEPT_HEADER_VALUE },
                    { REFERER_HEADER_KEY,REFERER_HEADER_VALUE }
                };
                request.SetRequestHeaders(logonHeaders);

                request.AddParameter("undefined", (LOGIN_PARAMETER_INIT + username + "&user.senha=" + password).Trim(), ParameterType.RequestBody);
                IRestResponse response = await client.ExecutePostTaskAsync(request);

                Console.WriteLine(response.Content);
                if (!response.Content.Contains("sair-sistema"))
                {
                    throw new LoginFailException();
                }

                return jsession;
            }
            catch
            {
                return null;
            }
        }
    }
}
