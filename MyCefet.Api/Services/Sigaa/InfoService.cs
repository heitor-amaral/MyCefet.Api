using HtmlAgilityPack;
using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web;

namespace MyCefet.Api.Services.Sigaa
{
    public class InfoService : IInfoService
    {
        private readonly ILoginService _loginService;

        public InfoService(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<Student> GetUserInfo(string jsessionid, string username, string password)
        {
            try
            {
                if (jsessionid is null)
                    jsessionid = await _loginService.Login(username, password);

                var student = ScraperInfo(GetInfoStudant(jsessionid));

                return student;
            }
            catch (LoginFailException e)
            {
                throw e;
            }
        }

        private string GetInfoStudant(String jsessionid)
        {
            var client = new RestClient("https://sig.cefetmg.br/sigaa/portais/discente/discente.jsf");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Cache-Control", "max-age=0");
            request.AddHeader("Cookie", "JSESSIONID=" + jsessionid);
            request.AddHeader("Host", "sig.cefetmg.br");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        private Student ScraperInfo(String html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var labelsToRemove = new List<string> { "\n", "\t" };

            var name = FixName(htmlDocument.DocumentNode.SelectNodeByXPathAndIndex(".//p[@class = 'info-docente']").RemoveSubStrings(labelsToRemove).GetDecoded());
            var table = htmlDocument.DocumentNode.SelectNodes(".//div[@id = 'agenda-docente']/table/tr");

            Dictionary<string, string> infos = new Dictionary<string, string>();

            var matricula = table[0].SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();           
            var curso = table[1].SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();
            var nivel = table[2].SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();
            var status = table[3].SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();
            var entrada = table[5].SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();
            var rg = HttpUtility.HtmlDecode(table[8].SelectNodes(".//table/tr")[1].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();

            return new Student(name, matricula, curso, nivel, status, entrada, rg);
        }

        private string FixName(string name)
        {
            name = name.ToLower();
            var array = name.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            for (int i = 1; i < array.Length; i++)  
            {  
                if (array[i - 1] == ' ')  
                {  
                    if (char.IsLower(array[i]))  
                    {  
                        array[i] = char.ToUpper(array[i]);  
                    }  
                }  
            }  
            return new string(array);  
        }
    }
}
