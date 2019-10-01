using MyCefet.Api.Services.Sinapse.Interfaces;
using MyCefet.Api.Services.Sinapse.Models.Response;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sinapse
{
    public class LoginService : ILoginService
    {
        public async Task<string> Login(string username, string password)
        {
            try
            {
                string jsession = await GetJSession();

          
                var client = new RestClient("https://sinapse.cefetmg.br/sinapse-web/jsp/comum/pagina/base/login.jsf");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Referer", "http://sinapse.cefetmg.br/sinapse-web/jsp/comum/pagina/base/infosistema.jsf");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Cookie", "JSESSIONID=" + jsession);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                request.AddParameter("undefined", "formlogin1=formlogin1&formlogin1%3Alogin=" + username + "&formlogin1%3Asenha=" + password + "&formlogin1%3Abotaologar=Autenticar&javax.faces.ViewState=j_id1", ParameterType.RequestBody);

                IRestResponse response = await client.ExecutePostTaskAsync(request);

                client = new RestClient("https://sinapse.cefetmg.br/sinapse-web/jsp/comum/pagina/base/infosistema.jsf");
                request = new RestRequest();
                request.AddHeader("Cookie", "JSESSIONID=" + jsession);
                response = await client.ExecuteGetTaskAsync(request);

                Console.WriteLine(response.Content);
                if (!response.Content.Contains("Abrir chamado"))
                {
                    throw new Api.Interfaces.LoginFailException();
                }

                return jsession;
            }
            catch
            {
                return null;
            }
        }
        private async Task<string> GetJSession()
        {
            try
            {
                var client = new RestClient("https://sinapse.cefetmg.br/");
                var request = new RestRequest(Method.GET);
                IRestResponse response = await client.ExecuteGetTaskAsync(request);

                return response.Cookies[0]?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
