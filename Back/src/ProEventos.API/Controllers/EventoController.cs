using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Domain;
using ProEventos.Infra;
using ProEventos.Infra.Context;
using ProEventos.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService _userService;
        public EventoController(IEventoService eventoService, 
                                IWebHostEnvironment hostEnvironment,
                                IUserService userService)
        {
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), true);
                if (eventos == null) return NoContent();

               
                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to retrieve data. Erro {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(),id, false);
                if (evento == null) return NoContent();
                
                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to retrieve data. Erro {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetEventosByTemaAsync(User.GetUserId(), tema, true);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to retrieve data. Erro {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return BadRequest("Data not insert");

                var file = Request.Form.Files[0];

                if(file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }

                var EventoRetorno = await _eventoService.Update(User.GetUserId(), eventoId, evento);

                return Ok(EventoRetorno);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to insert data. Erro {ex.Message}");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.AddEventos(User.GetUserId(), model);
                if (eventos == null) return BadRequest("Data not insert");

                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to insert data. Erro {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.Update(User.GetUserId(), id, model);
                if (eventos == null) return BadRequest("Data not updated");

                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to update data. Erro {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id);
                if(evento == null) return NoContent();
                if(await _eventoService.DeleteEventos(User.GetUserId(), id)) 
                {
                    DeleteImage(evento.ImagemURL);
                    return Ok(new { message = "deleted"});
                }
                else 
                {
                    throw new Exception("Erro ao tentar deletar");
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to deleted data. Erro {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile) 
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                              .Take(10)
                                              .ToArray()
                                              ).Replace(' ','-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yyymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"resources/images",imageName);
            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
           
        }
        [NonAction]
        public void DeleteImage(string imageURL) 
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"resources/images", imageURL);
            if(System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

    }
}
