using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

[JsonConverter(typeof(EasyMdeStatusJsonConverter))]
public readonly record struct EasyMdeStatus(
    EasyMdeStatus.KindType Kind,
    IReadOnlyList<StatusItem>? Items)
{
    public enum KindType
    {
        Default = 0,
        Disabled = 1,
        Custom = 2
    }

    public static EasyMdeStatus Default => new(KindType.Default, null);
    public static EasyMdeStatus Disabled => new(KindType.Disabled, null);

    public static EasyMdeStatus Custom(IReadOnlyList<StatusItem> items)
        => new(KindType.Custom, items ?? throw new ArgumentNullException(nameof(items)));
}