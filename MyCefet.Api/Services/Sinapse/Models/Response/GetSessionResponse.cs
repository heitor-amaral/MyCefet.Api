using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Services.Sinapse.Models.Response
{
    public class GetSessionResponse
    {
        public string Jsession { get; set; }
        public IRestResponse Response { get; set; }
    }
}
