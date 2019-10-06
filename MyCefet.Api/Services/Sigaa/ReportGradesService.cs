using HtmlAgilityPack;
using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;
using MyCefet.Api.Settings;
using RestSharp;
using System.Web;

namespace MyCefet.Api.Services.Sigaa
{
    public class ReportGradesService : IReportGradesService
    {
        private readonly ILoginService _loginService;
        private readonly GradesRequestSettings _gradesRequestSettings;

        public ReportGradesService(ILoginService loginService, GradesRequestSettings gradesRequestSettings)
        {
            _loginService = loginService;
            _gradesRequestSettings = gradesRequestSettings;
        }

        public GradesReport GetAllGrades(string jsessionid, string username, string password)
        {
            try
            {
                if (jsessionid is null)
                    jsessionid = _loginService.Login(username, password).Result;

                var gradesReport = ScraperGrades(GetGrades(jsessionid));

                return gradesReport;
            }
            catch (LoginFailException e)
            {
                throw e;
            }
        }

        private string GetGrades(string jsessionid)
        {
            var client = new RestClient(_gradesRequestSettings.SigaaStudentPortalUrl);
            var request = new RestRequest(Method.POST);
            
            var gradesRequestHeaders = _gradesRequestSettings.GetHeaderDict();
            gradesRequestHeaders.AddValueToHeader("Cookie", jsessionid);

            request.SetRequestHeaders(gradesRequestHeaders);
            request.AddParameter("undefined", _gradesRequestSettings.GradesUndefinedParameter, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            
            return response.Content;
        }

        private GradesReport ScraperGrades(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var tables = htmlDocument.DocumentNode.SelectNodes(".//div[@class = 'notas']/table");

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
