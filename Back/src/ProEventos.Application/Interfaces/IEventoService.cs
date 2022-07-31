using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> Update(int eventoId, EventoDto model);
        Task<bool> DeleteEventos(int eventoId);

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes=false);
        Task<EventoDto[]> GetEventosByTemaAsync(string tema, bool includePalestrantes= false);
        Task<EventoDto> GetEventosByIdAsync(int eventoId, bool includePalestrantes=false);
    }
}