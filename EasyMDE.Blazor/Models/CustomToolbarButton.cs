namespace EasyMDE.Blazor;

/// <summary>
/// Custom button config (EasyMDE supports object buttons).
/// Your shape here is typed; the converter writes the object EasyMDE expects.
/// </summary>
public sealed record CustomToolbarButton
{
    public required string Name { get; init; }          // e.g. "myButton"
    public required string Action { get; init; }        // JS action name (see note below)
    public string? ClassName { get; init; }             // e.g. "fa fa-star"
    public string? Title { get; init; }                 // tooltip
    public bool? NoDisable { get; init; }               // keep enabled in preview?
}