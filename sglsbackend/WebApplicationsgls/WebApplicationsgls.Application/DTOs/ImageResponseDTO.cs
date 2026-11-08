namespace WebApplicationsgls.Application.DTOs;

public class ImageResponseDTO
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ImageTypeDTO ImageType { get; set; } = new();
    public Guid OwnerId { get; set; }
}
