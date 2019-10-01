using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCefet.Api.Services.Sinapse.Interfaces;

namespace MyCefet.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SinapseController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILunchBalanceService _lunchBalance;

        public SinapseController(ILoginService loginService, ILunchBalanceService lunchBalance)
        {
            _loginService = loginService;
            _lunchBalance = lunchBalance;
        }

        

        /// <summary>
        /// This endpoint provides a jsession id throug sinapse username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("Login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK), ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBalanceAsync([FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                string jsession = await _loginService.Login(username, password);
                return Ok(jsession);
            }
            catch (Interfaces.LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }

        /// <summary>
        /// This endpoint provides the lunch balance through sinapse username and password
        /// </summary>
        /// <param name="jsessionid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json"), Route("LunchBalance")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK), ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBalanceAsync([FromHeader(Name = "JSessionID")] string jsessionid, [FromHeader(Name = "Username")] string username, [FromHeader(Name = "Password")] string password)
        {
            try
            {
                var balance = await _lunchBalance.GetCurrentBalance(jsessionid, username, password);
                return Ok(balance);
            }
            catch (Interfaces.LoginFailException)
            {
                return BadRequest("Login failed");
            }
        }
    }
}