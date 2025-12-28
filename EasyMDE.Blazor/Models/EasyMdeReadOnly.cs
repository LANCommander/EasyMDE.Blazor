using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

[JsonConverter(typeof(EasyMdeReadOnlyJsonConverter))]
public readonly record struct EasyMdeReadOnly(EasyMdeReadOnly.KindType Kind)
{
    public enum KindType
    {
        Default = 0,   // omit property
        Disabled = 1,  // readOnly: false
        Enabled = 2,   // readOnly: true
        NoCursor = 3   // readOnly: "nocursor"
    }

    public static EasyMdeReadOnly Default => new(KindType.Default);
    public static EasyMdeReadOnly Disabled => new(KindType.Disabled);
    public static EasyMdeReadOnly Enabled => new(KindType.Enabled);
    public static EasyMdeReadOnly NoCursor => new(KindType.NoCursor);
}