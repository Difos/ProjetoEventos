using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes=false);
        Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes=false);
        Task<Evento> GetAllEventosByIdAsync(int EventoId, bool includePalestrantes=false);
    }
}