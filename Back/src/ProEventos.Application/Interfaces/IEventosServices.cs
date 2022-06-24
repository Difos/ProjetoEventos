using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Interfaces
{
    public interface IEventosServices
    {
        Task<Evento> AddEventos(Evento model);
        Task<Evento> Update(int eventoId, Evento model);
        Task<bool> DeleteEventos(int eventoId);

        Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes=false);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes= false);
        Task<Evento> GetAllEventosByIdAsync(int EventoId, bool includePalestrantes=false);
    }
}