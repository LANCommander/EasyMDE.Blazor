namespace EasyMDE.Blazor;

public sealed class EasyMdeImageUploadOptions
{
    public bool? UploadImage { get; set; }
    public long? ImageMaxSize { get; set; }          // bytes
    public string? ImageAccept { get; set; }         // "image/png, image/jpeg"
    public string? ImageUploadEndpoint { get; set; }
    public bool? ImagePathAbsolute { get; set; }

    public string? ImageCSRFToken { get; set; }
    public string? ImageCSRFName { get; set; }
    public bool? ImageCSRFHeader { get; set; }
}