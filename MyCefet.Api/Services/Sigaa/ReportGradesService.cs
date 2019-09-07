using HtmlAgilityPack;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
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
                    jsessionid = _loginService.GetJsession(username, password).Result;

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
