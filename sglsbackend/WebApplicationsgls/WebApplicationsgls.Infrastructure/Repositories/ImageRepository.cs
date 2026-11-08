using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Enums;
using WebApplicationsgls.Infrastructure.Data;

namespace WebApplicationsgls.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Image>> ListarAsync()
    {
        return await _context.Images
            .OrderByDescending(image => image.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Image>> ListarPorVinculoAsync(int imageTypeId, Guid ownerId)
    {
        return await _context.Images
            .Where(image => image.tipoImagemEnum == (TipoImagemEnum)imageTypeId && image.OwnerId == ownerId)
            .OrderByDescending(image => image.CreatedAt)
            .ToListAsync();
    }

    public async Task<Image?> ObterPorIdAsync(Guid id)
    {
        return await _context.Images.FirstOrDefaultAsync(image => image.Id == id);
    }

    public async Task AdicionarAsync(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Image image)
    {
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
    }
}
