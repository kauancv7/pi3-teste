using Microsoft.AspNetCore.Http;

namespace WebApplicationsgls.API.DTOs;

public class UploadImageRequest
{
    public IFormFile File { get; set; } = default!;
    public int ImageTypeId { get; set; }
    public Guid OwnerId { get; set; }
}
