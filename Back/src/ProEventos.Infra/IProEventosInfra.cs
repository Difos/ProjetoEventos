using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra
{
    public interface IProEventosInfra
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Insert<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entity) where T : class;
        Task<bool> SaveChangesAsync();


        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes);
        Task<Evento> GetAllEventosByIdAsync(int EventoId, bool includePalestrantes);


        Task<Palestrante[]> GetAllPalestranteByNameAsync(string name, bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteAsync(string tema, bool includeEventos);
        Task<Palestrante> GetAllPalestranteByIdAsync(int PalestranteId, bool includeEventos);
    }
}