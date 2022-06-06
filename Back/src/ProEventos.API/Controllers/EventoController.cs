using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {

        }

        public IEnumerable<Evento> _evento = new Evento[]{
                new Evento(){
                    EventoId=1,
                    Tema ="Teste",
                    Local = "São Paulo",
                    Lote = "1º",
                    QtdPessoas = 200,
                    DataEvento = DateTime.Now.AddDays(1).ToShortDateString(),
                    ImagemURL = "foto.png"
                },
                new Evento(){
                    EventoId=2,
                    Tema ="Teste",
                    Local = "Curitiba",
                    Lote = "3º",
                    QtdPessoas = 150,
                    DataEvento = DateTime.Now.AddDays(20).ToShortDateString(),
                    ImagemURL = "foto.png"
                }
            };

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _evento; 
        }

         [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _evento.Where(e => e.EventoId ==id); 
        }

        

        [HttpPost]
        public string Post()
        {
            return "";
        }

    }
}
