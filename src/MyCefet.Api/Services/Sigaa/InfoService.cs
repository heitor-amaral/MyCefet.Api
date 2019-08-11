using HtmlAgilityPack;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
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

        public async Task<Student> GetUserInfo(string username, string password)
        {
            try
            {
                string jsession = _loginService.GetJsession(username, password).Result;

                var student = ScraperInfo(GetInfoStudant(jsession));

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

            var name = htmlDocument.DocumentNode.SelectSingleNode(".//p[@class = 'info-docente']").InnerText.Replace("\n", "").Replace("\t", "").Trim();
            var table = htmlDocument.DocumentNode.SelectNodes(".//div[@id = 'agenda-docente']/table/tr");

            Dictionary<string, string> infos = new Dictionary<string, string>();

            var matricula = HttpUtility.HtmlDecode(table[0].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var curso = HttpUtility.HtmlDecode(table[1].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var nivel = HttpUtility.HtmlDecode(table[2].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var status = HttpUtility.HtmlDecode(table[3].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var entrada = HttpUtility.HtmlDecode(table[5].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var rg = HttpUtility.HtmlDecode(table[8].SelectNodes(".//table/tr")[1].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();

            //infos.Add("Nome", name);
            //infos.Add("Matricula", matricula);
            //infos.Add("Curso", curso);
            //infos.Add("Nivel", nivel);
            //infos.Add("Status", status);
            //infos.Add("Entrada", entrada);
            //infos.Add("RG", rg);



            //var entries = infos.Select(d =>
            //    string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value))
            //);
            //string json = JsonConvert.SerializeObject(infos).Replace("\\", "");

            return new Student(name, matricula, curso, nivel, status, entrada, rg);
        }
    }
}
