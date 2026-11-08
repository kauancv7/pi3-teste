namespace WebApplicationsgls.Application.DTOs;

public class UploadImageDTO
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Length { get; set; }
    public Stream Content { get; set; } = Stream.Null;
    public int ImageTypeId { get; set; }
    public Guid OwnerId { get; set; }
}
