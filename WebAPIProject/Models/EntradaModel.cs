using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIProject.Models
{
    public class EntradaModel
    {
        public string sonda { get; set; }

        public string controle { get; set; }
    }

    public class EntradasModel
    {
        public string planalto { get; set; }

        public List<EntradaModel> entradas { get; set; }
    }
}
