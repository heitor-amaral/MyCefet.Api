using HtmlAgilityPack;
using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;
using MyCefet.Api.Settings;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyCefet.Api.Services.Sigaa
{
    public class VirtualClassService : IVirtualClassService
    {
        private readonly ILoginService _loginService;
        private readonly VirtualClassRequestSettings _virtualClassRequestSettings;

        public VirtualClassService(ILoginService loginService, VirtualClassRequestSettings virtualClassRequestSettings)
        {
            _loginService = loginService;
            _virtualClassRequestSettings = virtualClassRequestSettings;
        }

        public async Task<GradesReport> GetAllGrades(string jsessionid, string username, string password)
        {
            try
            {
                if (jsessionid is null)
                    jsessionid = await _loginService.Login(username, password);

                var gradesReport = ScraperGrades(await GetGrades(jsessionid));

                return gradesReport;
            }
            catch (LoginFailException e)
            {
                throw e;
            }
        }

        private async Task<string> GetGrades(String jsessionid)
        {
            var client = new RestClient(_virtualClassRequestSettings.SigaaStudentPortalUrl);
            var request = new RestRequest(Method.GET);

            var virtualClassRequestHeaders = _virtualClassRequestSettings.GetHeaderDict();
            virtualClassRequestHeaders.AddValueToHeader("Cookie", jsessionid);
            
            IRestResponse response = await client.ExecutePostTaskAsync(request);

            return response.Content;
        }

        private GradesReport ScraperGrades(String html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var tables = htmlDocument.DocumentNode.SelectNodes(".//td[@class = 'descricao']");

            GradesReport report = null;

            if (tables != null)
            {
                report = new GradesReport();
                foreach (var table in tables)
                {
                    var currentSemester = HttpUtility.HtmlDecode(table.SelectSingleNode(".//caption").InnerText).Replace("\n", "").Replace("\t", "").Trim();
                    var tbody = table.SelectNodes(".//tbody/tr");

                    Semester semester = new Semester();
                    foreach (var tr in tbody)
                    {
                        var codigo = HttpUtility.HtmlDecode(tr.SelectNodes(".//td")[0].InnerText).Replace("\n", "").Replace("\t", "").Trim();
                        var nome = HttpUtility.HtmlDecode(tr.SelectNodes(".//td")[1].InnerText).Replace("\n", "").Replace("\t", "").Trim();
                        var resultado = HttpUtility.HtmlDecode(tr.SelectNodes(".//td")[4].InnerText).Replace("\n", "").Replace("\t", "").Trim();
                        var faltas = HttpUtility.HtmlDecode(tr.SelectNodes(".//td")[5].InnerText).Replace("\n", "").Replace("\t", "").Trim();
                        var situacao = HttpUtility.HtmlDecode(tr.SelectNodes(".//td")[6].InnerText).Replace("\n", "").Replace("\t", "").Trim();

                        semester.Subjects.Add(new Subject(codigo, nome, resultado, faltas, situacao));
                    }
                    report.Semester.Add(currentSemester, semester);
                }
            }
            return report;
        }
    }
}
