using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

/// <summary>
/// A single toolbar item, strongly typed.
/// Serializes to one of:
/// - string token (built-in command)
/// - "|" separator
/// - object (custom button)
/// </summary>
[JsonConverter(typeof(ToolbarItemJsonConverter))]
public abstract record ToolbarItem
{
    public static ToolbarItem BuiltIn(ToolbarButton button) => new BuiltInButtonItem(button);
    public static ToolbarItem Separator { get; } = new SeparatorItem();
    public static ToolbarItem Custom(CustomToolbarButton button) => new CustomButtonItem(button);

    internal sealed record BuiltInButtonItem(ToolbarButton Command) : ToolbarItem;
    internal sealed record SeparatorItem() : ToolbarItem;
    internal sealed record CustomButtonItem(CustomToolbarButton Button) : ToolbarItem;
}