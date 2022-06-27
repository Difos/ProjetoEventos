using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Infra.Context;
using ProEventos.Infra.Interfaces;

namespace ProEventos.Infra
{
    public class EventoRepository : IEventoRepository
    {
        public readonly InfraContext _context;
        public EventoRepository(InfraContext context)
        {
            _context = context;
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);
                if (includePalestrantes)
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

        public async Task<Evento> GetEventosByIdAsync(int EventoId, bool includePalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);

                if (includePalestrantes)
                {
                    query
                        .Include(e => e.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.AsNoTracking().OrderBy(e => e.Id);

                return await query.FirstOrDefaultAsync(e => e.Id == EventoId);
            }
            catch (System.Exception)
            {

                return default;
            }
        }

        public async Task<Evento[]> GetEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                IQueryable<Evento> query = _context.Eventos
                    .Include(e => e.Lotes)
                    .Include(e => e.RedesSociais);
                if (includePalestrantes)
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

    }
}