using HtmlAgilityPack;
using MyCefet.Api.Extensions;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyCefet.Api.Services.Sigaa
{
    public class ReportGradesService : IReportGradesService
    {
        private readonly ILoginService _loginService;

        public ReportGradesService(ILoginService loginService)
        {
            _loginService = loginService;
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

        private string GetGrades(String jsessionid)
        {
            var client = new RestClient("https://sig.cefetmg.br/sigaa/portais/discente/discente.jsf");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Cookie", "JSESSIONID=" + jsessionid);
            request.AddHeader("Referer", "https://sig.cefetmg.br/sigaa/portais/discente/discente.jsf");
            request.AddHeader("Host", "sig.cefetmg.br");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            request.AddParameter("undefined", "menu%3Aform_menu_discente=menu%3Aform_menu_discente&id=167226&jscook_action=menu_form_menu_discente_j_id_jsp_161879646_98_menu%3AA%5D%23%7B%20relatorioNotasAluno.gerarRelatorio%20%7D&javax.faces.ViewState=j_id1", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        private GradesReport ScraperGrades(String html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var tables = htmlDocument.DocumentNode.SelectNodes(".//div[@class = 'notas']/table");

            GradesReport report = null;

            if (tables != null)
            {
                report = new GradesReport();
                var labelsToRemove = new List<string> { "\n", "\t" };

                foreach (var table in tables)
                {
                    var currentSemester = table.SelectNodeByXPathAndIndex(".//caption").RemoveSubStrings(labelsToRemove).GetDecoded();
                    var tbody = table.SelectNodes(".//tbody/tr");

                    Semester semester = new Semester();
                    foreach (var tr in tbody)
                    {
                        var codigo = tr.SelectNodeByXPathAndIndex(".//td", 0).RemoveSubStrings(labelsToRemove).GetDecoded();
                        var nome = tr.SelectNodeByXPathAndIndex(".//td", 1).RemoveSubStrings(labelsToRemove).GetDecoded();
                        var resultado = tr.SelectNodeByXPathAndIndex(".//td", 4).RemoveSubStrings(labelsToRemove).GetDecoded();
                        var faltas = tr.SelectNodeByXPathAndIndex(".//td", 5).RemoveSubStrings(labelsToRemove).GetDecoded();
                        var situacao = tr.SelectNodeByXPathAndIndex(".//td", 6).RemoveSubStrings(labelsToRemove).GetDecoded();

                        semester.Subjects.Add(new Subject(codigo, nome, resultado, faltas, situacao));
                    }
                    report.Semester.Add(currentSemester, semester);
                }
            }
            return report;
        }
    }
}
