using Newtonsoft.Json;
using System.Collections.Generic;

namespace Receitas_API.Models
{
    public partial class Receita
    {
        [JsonProperty("_id")]
        public Identificador _Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("secao")]
        public List<Secao> Secao { get; set; }
    }

    public partial class Identificador
    {
        [JsonProperty("$oid")]
        public string Oid { get; set; }
    }

    public partial class Secao
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("conteudo")]
        public List<string> Conteudo { get; set; }
    }

}


public class Rootobject
{
    public Id[] Property1 { get; set; }
}

public class Id
{
    public _Id _id { get; set; }
    public string nome { get; set; }
    public Secao[] secao { get; set; }
}

public class _Id
{
    public string oid { get; set; }
}

public class Secao
{
    public string nome { get; set; }
    public string[] conteudo { get; set; }
}




