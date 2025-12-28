using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

/// <summary>
/// Discriminated union for the EasyMDE "toolbar" option.
/// - Default: do not write the toolbar property (use EasyMDE defaults)
/// - Disabled: write "toolbar": false
/// - Custom: write "toolbar": [ ... ]
/// </summary>
[JsonConverter(typeof(EasyMdeToolbarJsonConverter))]
public readonly record struct EasyMdeToolbar(EasyMdeToolbar.KindType Kind, IReadOnlyList<ToolbarItem>? Items)
{
    public enum KindType
    {
        Default = 0,
        Disabled = 1,
        Custom = 2
    }

    public static EasyMdeToolbar Default => new(KindType.Default, null);
    public static EasyMdeToolbar Disabled => new(KindType.Disabled, null);

    public static EasyMdeToolbar Custom(IReadOnlyList<ToolbarItem> items)
        => new(KindType.Custom, items ?? throw new ArgumentNullException(nameof(items)));
}