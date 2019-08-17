using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;

namespace MyCefet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SigaaController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IInfoService _infoService;
        private readonly IReportGradesService _gradesService;

        public SigaaController(ILoginService loginService, IInfoService infoService, IReportGradesService gradesService)
        {
            _loginService = loginService;
            _infoService = infoService;
            _gradesService = gradesService;
        }

        /// <summary>
        /// This endpoint provides a jsession id throug sigaa username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("Login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK), ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync([FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                string jsession = _loginService.GetJsession(username, password).Result;
                return Ok(jsession);
            } catch (LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }

        /// <summary>
        /// This endpoint provides informations about a student through sigaa username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("UserInfo")]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.OK), ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserInfoAsync([FromHeader(Name = "JSessionID")] string jsessionid, [FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                var student = _infoService.GetUserInfo(jsessionid, username, password);
                return Ok(student.Result);

            } catch (LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }

        /// <summary>
        /// This endpoint provides all semesters' grades through sigaa username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("GradesReport")]
        [ProducesResponseType( (int)HttpStatusCode.OK)]
        public IActionResult GetGradesReportAsync([FromHeader(Name = "JSessionID")] string jsessionid, [FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                var gradesReport = _gradesService.GetAllGrades(jsessionid, username, password);
                return Ok(gradesReport.Semester);

            } catch (LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }
    }
}
