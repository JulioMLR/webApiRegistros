﻿using Microsoft.Extensions.Hosting;

namespace webApiRegistros.Entidades
{
    public class Registro
    {

        public int id { get; set; }
        public string o1 { get; set; }
        public string o2 { get; set; }
        public string o3 { get; set; }
        public Objeto objeto { get; set; }

    }
}