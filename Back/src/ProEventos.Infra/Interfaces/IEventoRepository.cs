using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetEventosByTemaAsync(int userId, string tema, bool includePalestrantes=false);
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes=false);
        Task<Evento> GetEventosByIdAsync(int userId, int EventoId, bool includePalestrantes=false);
    }
}