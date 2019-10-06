using HtmlAgilityPack;
using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;
using MyCefet.Api.Settings;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace MyCefet.Api.Services.Sigaa
{
    public class InfoService : IInfoService
    {
        private readonly ILoginService _loginService;

        private const string ACCEPT_ENC_HEADER_KEY = "Accept-Encoding";
        private const string ACCEPT_ENC_HEADER_VALUE = "gzip, deflate, br";
        private const string CACHE_HEADER_KEY = "Cache-Control";
        private const string CACHE_HEADER_VALUE = "max-age=0";
        private const string COOKIE_HEADER_KEY = "Cookie";
        private const string COOKIE_HEADER_VALUE = "JSESSIONID=";
        private const string HOST_HEADER_KEY = "Host";
        private const string HOST_HEADER_VALUE = "sig.cefetmg.br";
        private const string CONTENT_HEADER_KEY = "Content-Type";
        private const string CONTENT_HEADER_VALUE = "application/x-www-form-urlencoded";
        private const string CONNECTION_HEADER_KEY = "Connection";
        private const string CONNECTION_HEADER_VALUE = "keep-alive";
        private const string ACCEPT_HEADER_KEY = "Accept";
        private const string ACCEPT_HEADER_VALUE = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
        private const string SIGAA_STUDENT_PORTAL_URL = "https://sig.cefetmg.br/sigaa/portais/discente/discente.jsf";

        private readonly StudentInfoRequestSettings _studentInforRequestSettings;

        public InfoService(ILoginService loginService, StudentInfoRequestSettings studentInforRequestSettings)
        {
            _loginService = loginService;
            _studentInforRequestSettings = studentInforRequestSettings;
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
            var client = new RestClient(_studentInforRequestSettings.SigaaStudentPortalUrl);

            var studentInfoHeaders = _studentInforRequestSettings.GetHeaderDict();
            studentInfoHeaders.AddValueToHeader("Cookie", jsessionid);

            var request = new RestRequest(Method.GET);
            request.SetRequestHeaders(studentInfoHeaders);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        private Student ScraperInfo(String html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var name = FixName(htmlDocument.DocumentNode.SelectSingleNode(".//p[@class = 'info-docente']").InnerText.Replace("\n", "").Replace("\t", "").Trim());
            var table = htmlDocument.DocumentNode.SelectNodes(".//div[@id = 'agenda-docente']/table/tr");

            Dictionary<string, string> infos = new Dictionary<string, string>();

            var matricula = HttpUtility.HtmlDecode(table[0].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var curso = HttpUtility.HtmlDecode(table[1].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var nivel = HttpUtility.HtmlDecode(table[2].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var status = HttpUtility.HtmlDecode(table[3].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            var entrada = HttpUtility.HtmlDecode(table[5].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
            //var rg = HttpUtility.HtmlDecode(table[8].SelectNodes(".//table/tr")[1].SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();

            return new Student(name, matricula, curso, nivel, status, entrada, "FATAL ERROR ON GETTING STUDENT'S RG NUMBER");
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
