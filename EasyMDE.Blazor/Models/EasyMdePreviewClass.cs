using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

[JsonConverter(typeof(EasyMdePreviewClassJsonConverter))]
public readonly record struct EasyMdePreviewClass(EasyMdePreviewClass.KindType Kind, string? Single, IReadOnlyList<string>? Many)
{
    public enum KindType
    {
        Default = 0,   // omit
        Single = 1,    // "previewClass": "..."
        Many = 2       // "previewClass": ["...", "..."]
    }

    public static EasyMdePreviewClass Default => new(KindType.Default, null, null);
    public static EasyMdePreviewClass Of(string value) => new(KindType.Single, value, null);
    public static EasyMdePreviewClass Of(IReadOnlyList<string> values) => new(KindType.Many, null, values);
}