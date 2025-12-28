namespace EasyMDE.Blazor;

public sealed class EasyMdeAutosave
{
    public bool Enabled { get; set; }
    public string? UniqueId { get; set; }
    public int? Delay { get; set; }
}