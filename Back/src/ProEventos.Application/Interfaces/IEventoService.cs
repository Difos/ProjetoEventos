using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId,EventoDto model);
        Task<EventoDto> Update(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEventos(int userId,int eventoId);

        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes=false);
        Task<EventoDto[]> GetEventosByTemaAsync(int userId, string tema, bool includePalestrantes= false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int eventoId, bool includePalestrantes=false);
    }
}