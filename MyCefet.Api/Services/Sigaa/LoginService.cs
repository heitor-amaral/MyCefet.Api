using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Services.Sigaa.Interfaces;
using MyCefet.Api.Settings;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sigaa
{
    public class LoginService : ILoginService
    {

        private readonly LoginRequestSettings _loginRequestSettings;

        public LoginService(LoginRequestSettings loginRequestSettings)
        {
            _loginRequestSettings = loginRequestSettings;
        }

        private async Task<string> GetJSession()
        {
            try
            {
                var client = new RestClient(_loginRequestSettings.SigaaBaseUrl);
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

                var client = new RestClient(_loginRequestSettings.SigaaLogonUrl);
                var request = new RestRequest(Method.POST);

                request.SetRequestHeaders(_loginRequestSettings.GetHeaderDict());

                request.AddParameter("undefined", (_loginRequestSettings.LoginParameterInit + username + "&user.senha=" + password).Trim(), ParameterType.RequestBody);
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
