using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Models
{
    [Serializable, JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Student
    {
        public Student(string name, string matricula, string curso, string nivel, string status, string entrada, string rG)
        {
            Name = name;
            Matricula = matricula;
            Curso = curso;
            Nivel = nivel;
            Status = status;
            Entrada = entrada;
            RG = rG;
        }

        public string Name { get; set; }
        public string Matricula { get; set; }
        public string Curso { get; set; }
        public string Nivel { get; set; }
        public string Status { get; set; }
        public string Entrada { get; set; }
        public string RG { get; set; }
    }
}
