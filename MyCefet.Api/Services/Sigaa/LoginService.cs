using MyCefet.Api.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sigaa
{
    public class LoginService : ILoginService
    {
        public async Task<string> GetJsession(string username, string password)
        {
            try
            {
                var client = new RestClient("https://sig.cefetmg.br/sigaa/logar.do?dispatch=logOn");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Referer", "https://sig.cefetmg.br/sigaa/verPortalDiscente.do");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Host", "sig.cefetmg.br");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined", "width=800&height=600&urlRedirect=&subsistemaRedirect=&acao=&acessibilidade=&user.login=" + username.Trim() + "&user.senha=" + password.Trim(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                Console.WriteLine(response.Content);
                if (response.Content.Contains("Entrar no Sistema"))
                {
                    throw new LoginFailException();
                }

                return response.Cookies[0].Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
