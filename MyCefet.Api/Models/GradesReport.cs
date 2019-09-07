using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Models
{
    [Serializable, JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class GradesReport
    {
        public GradesReport()
        {
            Semester = new Dictionary<string, Semester>();
        }

        public Dictionary<string, Semester> Semester { get; set; }
    }
}
