namespace EasyMDE.Blazor;

public sealed class EasyMdeKeyEvent
{
    public required string Key { get; init; }

    public bool Ctrl { get; init; }
    public bool Alt { get; init; }
    public bool Shift { get; init; }
    public bool Meta { get; init; }

    // Useful context (not huge)
    public string? Origin { get; init; } // if known
}