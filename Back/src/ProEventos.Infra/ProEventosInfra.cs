using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;

namespace ProEventos.Infra
{
    public class ProEventosInfra : IProEventosInfra
    {
        public readonly InfraContext _context;

        public ProEventosInfra(InfraContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0 ? true : false;
        }
        public void Insert<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }

        public async Task<Evento[]> GetAllEventosAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);
                if(includePalestrantes)
                {
                    query
                        .Include(e => e.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.AsNoTracking().OrderBy(e => e.Id);
                return await query.ToArrayAsync();
            }
            catch (System.Exception)
            {
                
                return default;
            }
        }

        public async Task<Evento> GetAllEventosByIdAsync(int EventoId, bool includePalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);

                if(includePalestrantes)
                {
                    query
                        .Include(e => e.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.AsNoTracking().OrderBy(e => e.Id)
                    .Where(e => e.Id == EventoId);

                return await query.FirstOrDefaultAsync();
            }
            catch (System.Exception)
            {
                
                return default;
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false) 
        {
             try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);
                if(includePalestrantes)
                {
                    query
                        .Include(e => e.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.AsNoTracking().OrderBy(e => e.Id).Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
                return await query.ToArrayAsync();
            }
            catch (System.Exception)
            {
                
                return default;
            }
        }

        public Task<Palestrante[]> GetAllPalestranteAsync(string tema, bool includeEventos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Palestrante> GetAllPalestranteByIdAsync(int PalestranteId, bool includeEventos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Palestrante[]> GetAllPalestranteByNameAsync(string name, bool includeEventos)
        {
            throw new System.NotImplementedException();
        }

    }
}