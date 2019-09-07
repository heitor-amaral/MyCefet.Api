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
    public class Semester
    {
        public Semester()
        {
            Subjects = new ArrayList();
        }

        public ArrayList Subjects { get; set; }
    }
}
