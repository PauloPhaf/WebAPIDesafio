using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAPIProject.Models;
using System.Web;
using Newtonsoft.Json;

namespace WebAPIProject.Controllers
{
    [Produces("application/json")]
    [Route("api/Sonda")]
    public class SondaController : Controller
    {
        private IMongoDatabase _dataBase;
        public SondaController()
        {
            string connectionString = "mongodb://localhost:27017";

            MongoClient client = new MongoClient(connectionString);
            _dataBase = client.GetDatabase("SondaProject");
        }

        // GET: api/Sonda/
        [HttpGet]
        public IActionResult Get()
        {
            var Sonda = _dataBase.GetCollection<SondaModel>("SondaModel").Find(x => true).ToList();

            List<dynamic> lSondas = new List<dynamic>();
            Sonda.ForEach(x => lSondas.Add(new { sonda = String.Format("{0} {1} {2}", x.Posicao_X, x.Posicao_Y, x.Direcao) }));

            return Ok(lSondas);
        }

        [HttpPost]
        public IActionResult Post([FromBody]SondasModel pSondas)
        {
            var sondasDB = _dataBase.GetCollection<SondaModel>("SondaModel");

            List<dynamic> lSondas = new List<dynamic>();
            pSondas.sondas.ForEach(x => sondasDB.InsertOne(x));

            return Ok(lSondas);
        }

        [HttpPut]
        public IActionResult Put([FromBody]EntradasModel pEntradas)
        {
            var lCoordenadaPlanalto = !String.IsNullOrEmpty(pEntradas.planalto) ? pEntradas.planalto.Split(" ").Where(x => !String.IsNullOrEmpty(x)).ToArray() : null;

            if (lCoordenadaPlanalto != null)
            {
                List<dynamic> lSondas = new List<dynamic>();
                int lXp = Convert.ToInt32(lCoordenadaPlanalto[0]); int lYp = Convert.ToInt32(lCoordenadaPlanalto[1]);

                foreach (var lEntrada in pEntradas.entradas)
                {
                    var lCoordenadaSonda = !String.IsNullOrEmpty(lEntrada.sonda) ? lEntrada.sonda.Split(" ").Where(x => !String.IsNullOrEmpty(x)).ToArray() : null;
                    var lControleSonda = !String.IsNullOrEmpty(lEntrada.controle) ? lEntrada.controle.Trim() : null;

                    if (lCoordenadaSonda != null && lControleSonda != null)
                    {
                        int lXSonda1 = Convert.ToInt32(lCoordenadaSonda[0]);
                        int lYSonda1 = Convert.ToInt32(lCoordenadaSonda[1]);
                        char lDirecaoSonda1 = Convert.ToChar(lCoordenadaSonda[2].Trim());

                        SondaModel lSonda = _dataBase.GetCollection<SondaModel>("SondaModel").Find(x => x.Posicao_X == lXSonda1 && x.Posicao_Y == lYSonda1 && x.Direcao == lDirecaoSonda1).FirstOrDefault();

                        if (lSonda != null)
                        {
                            foreach (var lControle in lControleSonda)
                                switch (lControle)
                                {
                                    case 'L':
                                        if (lSonda.Direcao == 'N')
                                            lSonda.Direcao = 'W';
                                        else if (lSonda.Direcao == 'S')
                                            lSonda.Direcao = 'E';
                                        else if (lSonda.Direcao == 'E')
                                            lSonda.Direcao = 'N';
                                        else if (lSonda.Direcao == 'W')
                                            lSonda.Direcao = 'S';
                                        break;
                                    case 'R':
                                        if (lSonda.Direcao == 'N')
                                            lSonda.Direcao = 'E';
                                        else if (lSonda.Direcao == 'S')
                                            lSonda.Direcao = 'W';
                                        else if (lSonda.Direcao == 'E')
                                            lSonda.Direcao = 'S';
                                        else if (lSonda.Direcao == 'W')
                                            lSonda.Direcao = 'N';
                                        break;
                                    case 'M':
                                        if (lSonda.Direcao == 'N')
                                            lSonda.Posicao_Y = lSonda.Posicao_Y + 1;
                                        else if (lSonda.Direcao == 'S')
                                            lSonda.Posicao_Y = lSonda.Posicao_Y - 1;
                                        else if (lSonda.Direcao == 'E')
                                            lSonda.Posicao_X = lSonda.Posicao_X + 1;
                                        else if (lSonda.Direcao == 'W')
                                            lSonda.Posicao_X = lSonda.Posicao_X - 1;
                                        break;
                                }

                            if (lSonda.Posicao_X < 0 || lSonda.Posicao_X > lXp || lSonda.Posicao_Y < 0 || lSonda.Posicao_Y > lYp)
                                throw new Exception("Sonda 1 saiu da área do planalto");

                            _dataBase.GetCollection<SondaModel>("SondaModel").ReplaceOne(c => c.Id == lSonda.Id, lSonda);

                            lSondas.Add(new { sonda = String.Format("{0} {1} {2}", lSonda.Posicao_X, lSonda.Posicao_Y, lSonda.Direcao) });
                        }
                    }
                }

                return Ok(lSondas);
            }

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            var sondasDB = _dataBase.GetCollection<SondaModel>("SondaModel");

            sondasDB.DeleteMany(x => true);

            return Ok();
        }
    }
}
