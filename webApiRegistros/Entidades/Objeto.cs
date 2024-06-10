using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;

namespace webApiRegistros.Entidades
{
    public class Objeto
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }

        [JsonIgnore]
        public List<Registro> registros { get; set; }

    }
}
