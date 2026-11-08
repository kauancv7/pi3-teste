using WebApplicationsgls.Application.DTOs;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Domain.Entities;
using WebApplicationsgls.Domain.Enums;

namespace WebApplicationsgls.Application.Services;

public class ImageService : IImageService
{
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".webp",
        ".bmp"
    };

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
        "image/bmp"
    };

    private readonly IImageRepository _imageRepository;
    private readonly IImageStorageService _imageStorageService;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProdutoRepository _produtoRepository;

    public ImageService(
        IImageRepository imageRepository,
        IImageStorageService imageStorageService,
        ICategoriaRepository categoriaRepository,
        IProdutoRepository produtoRepository)
    {
        _imageRepository = imageRepository;
        _imageStorageService = imageStorageService;
        _categoriaRepository = categoriaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<ImageResponseDTO> SalvarAsync(UploadImageDTO dto, CancellationToken cancellationToken = default)
    {
        ValidateUpload(dto);
        var imageType = await ValidateOwnerAsync(dto);

        var storageResult = await _imageStorageService.SalvarAsync(
            dto.Content,
            dto.FileName,
            dto.ContentType,
            cancellationToken);

        var image = new Image
        {
            Id = Guid.NewGuid(),
            FileName = storageResult.FileName,
            ContentType = dto.ContentType,
            FilePath = storageResult.RelativeUrl,
            CreatedAt = DateTime.UtcNow,
            tipoImagemEnum = imageType,
            OwnerId = dto.OwnerId
        };

        try
        {
            await _imageRepository.AdicionarAsync(image);
        }
        catch
        {
            // Remove o arquivo físico caso a persistência dos metadados falhe.
            await _imageStorageService.RemoverAsync(storageResult.RelativeUrl, cancellationToken);
            throw;
        }

        return ToResponse(image);
    }

    public async Task<List<ImageResponseDTO>> ListarAsync()
    {
        var images = await _imageRepository.ListarAsync();
        return images.Select(ToResponse).ToList();
    }

    public async Task<List<ImageResponseDTO>> ListarPorVinculoAsync(int imageTypeId, Guid ownerId)
    {
        ValidateImageTypeAndOwnerId(imageTypeId, ownerId);

        var images = await _imageRepository.ListarPorVinculoAsync(imageTypeId, ownerId);
        return images.Select(ToResponse).ToList();
    }

    public async Task<ImageResponseDTO?> ObterPorIdAsync(Guid id)
    {
        var image = await _imageRepository.ObterPorIdAsync(id);
        return image == null ? null : ToResponse(image);
    }

    public async Task<ImageDownloadDTO?> BaixarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var image = await _imageRepository.ObterPorIdAsync(id);
        if (image == null)
            return null;

        return new ImageDownloadDTO
        {
            FileName = image.FileName,
            ContentType = image.ContentType,
            Content = await _imageStorageService.AbrirAsync(image.FilePath, cancellationToken)
        };
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var image = await _imageRepository.ObterPorIdAsync(id);
        if (image == null)
            return false;

        await _imageRepository.RemoverAsync(image);
        await _imageStorageService.RemoverAsync(image.FilePath, cancellationToken);
        return true;
    }

    private static void ValidateUpload(UploadImageDTO dto)
    {
        if (dto.Content == Stream.Null || dto.Length <= 0)
            throw new ArgumentException("A imagem enviada está vazia.");

        if (dto.Length > MaxFileSizeInBytes)
            throw new ArgumentException("A imagem excede o tamanho máximo permitido de 5 MB.");

        var extension = Path.GetExtension(dto.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            throw new ArgumentException("Extensão de imagem não permitida.");

        if (string.IsNullOrWhiteSpace(dto.ContentType) || !AllowedContentTypes.Contains(dto.ContentType))
            throw new ArgumentException("Tipo de conteúdo da imagem não permitido.");
    }

    private static void ValidateImageTypeAndOwnerId(int imageTypeId, Guid ownerId)
    {
        if (!Enum.IsDefined(typeof(TipoImagemEnum), imageTypeId))
            throw new ArgumentException("Tipo de imagem inválido. Informe 1 para categoria ou 2 para produto.");

        if (ownerId == Guid.Empty)
            throw new ArgumentException("O identificador do vínculo da imagem é obrigatório.");
    }

    private async Task<TipoImagemEnum> ValidateOwnerAsync(UploadImageDTO dto)
    {
        ValidateImageTypeAndOwnerId(dto.ImageTypeId, dto.OwnerId);

        var imageType = (TipoImagemEnum)dto.ImageTypeId;

        var exists = imageType switch
        {
            TipoImagemEnum.Categoria => await _categoriaRepository.ObterPorIdAsync(dto.OwnerId) != null,
            TipoImagemEnum.Produto => await _produtoRepository.ObterPorIdAsync(dto.OwnerId) != null,
            _ => false
        };

        if (!exists)
            throw new ArgumentException(imageType == TipoImagemEnum.Categoria
                ? "Categoria vinculada à imagem não encontrada."
                : "Produto vinculado à imagem não encontrado.");

        return imageType;
    }

    private static ImageTypeDTO ToImageTypeDTO(TipoImagemEnum imageType)
    {
        return new ImageTypeDTO
        {
            Id = (int)imageType,
            Name = imageType == TipoImagemEnum.Categoria ? "Category" : "Product"
        };
    }

    private static ImageResponseDTO ToResponse(Image image)
    {
        return new ImageResponseDTO
        {
            Id = image.Id,
            FileName = image.FileName,
            ContentType = image.ContentType,
            Url = image.FilePath,
            CreatedAt = image.CreatedAt,
            ImageType = ToImageTypeDTO(image.tipoImagemEnum),
            OwnerId = image.OwnerId
        };
    }
}
