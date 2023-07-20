using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using ProEventos.Infra.Interfaces;
using System.Linq;

public class LoteRepository : ILoteRepository
{
    private readonly InfraContext _context;
    public LoteRepository(InfraContext context)
    {
        _context = context;
    }
    public async Task<Lote> GetLoteByIdsAsync(int eventoId, int LoteId)
    {
        IQueryable<Lote> query = _context.Lotes;

        query = query.AsNoTracking()
                     .Where(lote => lote.EventoId == eventoId 
                                 && lote.Id == LoteId);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
    {
        IQueryable<Lote> query = _context.Lotes;

        query = query.AsNoTracking()
                     .Where(lote => lote.EventoId == eventoId);
        return await query.ToArrayAsync();
    }
}