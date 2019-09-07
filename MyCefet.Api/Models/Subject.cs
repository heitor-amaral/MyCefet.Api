using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCefet.Api.Models
{
    [Serializable, JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Subject
    {
        public Subject(string codigo, string nome, string resultado, string faltas, string situacao)
        {
            Codigo = codigo;
            Nome = nome;
            Resultado = resultado;
            Faltas = faltas;
            Situacao = situacao;
        }

        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Resultado { get; set; }
        public string Faltas { get; set; }
        public string Situacao { get; set; }
    }
}
