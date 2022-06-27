using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetEventosByTemaAsync(string tema, bool includePalestrantes=false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes=false);
        Task<Evento> GetEventosByIdAsync(int EventoId, bool includePalestrantes=false);
    }
}