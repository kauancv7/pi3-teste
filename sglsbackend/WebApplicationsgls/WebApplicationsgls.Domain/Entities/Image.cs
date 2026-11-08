using WebApplicationsgls.Domain.Enums;

namespace WebApplicationsgls.Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public TipoImagemEnum tipoImagemEnum { get; set; }
    public Guid OwnerId { get; set; }
}
