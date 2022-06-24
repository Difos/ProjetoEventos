using System;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Infra;
using ProEventos.Infra.Interfaces;

namespace ProEventos.Application
{
    public class EventoService : IEventosServices
    {
        public readonly IRepository _repo;
        public readonly IEventoRepository _eventoRepository;
        public EventoService(IRepository repo, IEventoRepository eventoRepository)
        {
            _repo = repo;
            _eventoRepository = eventoRepository;
        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _repo.Add<Evento>(model);

                if (await _repo.SaveChangesAsync())
                {
                    return await _eventoRepository.GetAllEventosByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> Update(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoRepository.GetAllEventosByIdAsync(eventoId,false);

                if(evento == null) return null;

                _repo.Update(model);

                model.Id = eventoId;

                if(await _repo.SaveChangesAsync())
                {   
                    return await _eventoRepository.GetAllEventosByIdAsync(model.Id,false);
                }

                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);;
            }
        }
        public async Task<bool> DeleteEventos(int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetAllEventosByIdAsync(eventoId,false);

                if(evento == null) throw new Exception("Evento for delete not found.");

                _repo.Delete<Evento>(evento);

                return await _repo.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);;
            }
        }

        public Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<Evento> GetAllEventosByIdAsync(int EventoId, bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            throw new System.NotImplementedException();
        }
    }
}