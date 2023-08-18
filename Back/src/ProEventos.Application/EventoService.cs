using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Infra;
using ProEventos.Infra.Interfaces;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        public readonly IRepository _repo;
        public readonly IEventoRepository _eventoRepository;
        public readonly IMapper _mapper;
        public EventoService(IRepository repo, 
                             IEventoRepository eventoRepository, 
                             IMapper mapper)
        {
            _repo = repo;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                evento.UserId = userId; 
                _repo.Add<Evento>(evento);

                if (await _repo.SaveChangesAsync())
                {
                    var result = await _eventoRepository.GetEventosByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> Update(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(userId, eventoId,false);
                if(evento == null) return null;

                model.Id = eventoId;
                model.UserId = userId;

                _mapper.Map(model,evento);

                _repo.Update<Evento>(evento);


                if(await _repo.SaveChangesAsync())
                {   
                    var eventoAlterado = await _eventoRepository.GetEventosByIdAsync(userId, evento.Id,false);
                   
                    return _mapper.Map<EventoDto>(eventoAlterado);;
                }

                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);;
            }
        }
        public async Task<bool> DeleteEventos(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(userId, eventoId,false);

                if(evento == null) throw new Exception("Evento for delete not found.");
              
                _repo.Delete<Evento>(evento);

                return await _repo.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetAllEventosAsync(userId, false);
                var result = _mapper.Map<EventoDto[]>(eventos);
                if(eventos == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex) 
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async  Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(userId, eventoId,false);

                var result = _mapper.Map<EventoDto>(evento);
                if(evento == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoRepository.GetEventosByTemaAsync(userId, tema,false);
                var result = _mapper.Map<EventoDto[]>(eventos);
                if(eventos == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex) 
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}