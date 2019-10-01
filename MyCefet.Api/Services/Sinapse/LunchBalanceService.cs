using HtmlAgilityPack;
using MyCefet.Api.Services.Sinapse.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sinapse
{
    public class LunchBalanceService : ILunchBalanceService
    {
        private readonly ILoginService _loginService;

        public LunchBalanceService(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<string> GetCurrentBalance(string jsessionid, string username, string password)
        {
            try
            {
                if (jsessionid is null)
                    jsessionid = await _loginService.Login(username, password);

                var gradesReport = ScraperBalance(await GetBalance(jsessionid));

                return gradesReport;
            }
            catch (Api.Interfaces.LoginFailException e)
            {
                throw e;
            }
        }

        private async Task<string> GetBalance(String jsessionid)
        {
            var client = new RestClient("https://sinapse.cefetmg.br/sinapse-web/jsp/comum/pagina/base/infosistema.jsf");
            var request = new RestRequest();
            request.AddHeader("Cookie", "JSESSIONID=" + jsessionid);

            var response = await client.ExecuteGetTaskAsync(request);
            return response.Content;
        }

        private string ScraperBalance(String html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var balance = htmlDocument.DocumentNode.SelectSingleNode(".//span[@id = 'modulosAcesso:saldoConta']").InnerText;


            return balance;
        }
    }
}
