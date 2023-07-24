using System;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Infra;
using ProEventos.Infra.Interfaces;
using ProEventos.Domain;
using AutoMapper;
using System.Linq;

public class LoteService : ILoteService
{
    private readonly IRepository _repo;
    private readonly ILoteRepository _loteRepository;
    private readonly IMapper _mapper;
    public LoteService(IRepository repo, ILoteRepository loteRepository, IMapper mapper)
    {
        _repo = repo;
        _loteRepository = loteRepository;
        _mapper = mapper;
    }
    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) throw new Exception("Lote para delete não encontrado");

            _repo.Delete<Lote>(lote);

            return await _repo.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return null;

            var resultado = _mapper.Map<LoteDto>(lote);

            return resultado;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
    {
        try
        {
            var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            var resultado = _mapper.Map<LoteDto[]>(lotes);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddLotes(eventoId, model);
                }
                else
                {
                    var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                    model.EventoId = eventoId;

                    _mapper.Map(model, lote);

                    _repo.Update<Lote>(lote);

                    await _repo.SaveChangesAsync();
                }
            }

            var loteRetorno = await _loteRepository.GetLotesByEventoIdAsync(eventoId);

            return _mapper.Map<LoteDto[]>(loteRetorno);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddLotes(int eventoId, LoteDto model)
    {
        try
        {
            var lote = _mapper.Map<Lote>(model);

            lote.EventoId = eventoId;

            _repo.Add<Lote>(lote);

            await _repo.SaveChangesAsync();

        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
}