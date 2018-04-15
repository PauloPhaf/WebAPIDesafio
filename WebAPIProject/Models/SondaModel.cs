using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIProject.Models
{
    public class SondaModel
    {
        public ObjectId Id { get; set; }

        public int Posicao_X { get; set; }

        public int Posicao_Y { get; set; }

        public char Direcao { get; set; }
    }

    public class SondasModel
    {
        public List<SondaModel> sondas { get; set; }
    }
}
