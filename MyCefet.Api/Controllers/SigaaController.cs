using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCefet.Api.Interfaces;
using MyCefet.Api.Models;
using MyCefet.Api.Services.Sigaa.Interfaces;

namespace MyCefet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SigaaController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IInfoService _infoService;
        private readonly IReportGradesService _gradesService;
        private readonly IVirtualClassService _virtualClassService;

        public SigaaController(ILoginService loginService, IInfoService infoService, IReportGradesService gradesService, IVirtualClassService virtualClassService)
        {
            _loginService = loginService;
            _infoService = infoService;
            _gradesService = gradesService;
            _virtualClassService = virtualClassService;
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
                string jsession = await _loginService.Login(username, password);
                return Ok(jsession);
            } catch (LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }

        /// <summary>
        /// This endpoint provides informations about a student through sigaa username and password
        /// </summary>
        /// <param name="jsessionid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("UserInfo")]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.OK), ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserInfoAsync([FromHeader(Name = "JSessionID")] string jsessionid, [FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                var student = await _infoService.GetUserInfo(jsessionid, username, password);
                return Ok(student);

            } catch (LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }

        /// <summary>
        /// This endpoint provides all semesters' grades through sigaa username and password
        /// </summary>
        /// <param name="jsessionid"></param>
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

        ///// <summary>
        ///// This endpoint provides all semesters' grades through sigaa username and password
        ///// </summary>
        ///// <param name="jsessionid"></param>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //[HttpGet, Produces("application/json"), Route("VirtualClassGrades")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public IActionResult GetVirtualClassGrades([FromHeader(Name = "JSessionID")] string jsessionid, [FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        //{
        //    try
        //    {
        //        var gradesReport = _gradesService.GetAllGrades(jsessionid, username, password);
        //        return Ok(gradesReport.Semester);

        //    }
        //    catch (LoginFailException)
        //    {
        //        return BadRequest("Login failed");
        //    }
        //}
    }
}
