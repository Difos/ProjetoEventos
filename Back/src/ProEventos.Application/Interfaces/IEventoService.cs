using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<Evento> AddEventos(Evento model);
        Task<Evento> Update(int eventoId, Evento model);
        Task<bool> DeleteEventos(int eventoId);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes=false);
        Task<Evento[]> GetEventosByTemaAsync(string tema, bool includePalestrantes= false);
        Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes=false);
    }
}