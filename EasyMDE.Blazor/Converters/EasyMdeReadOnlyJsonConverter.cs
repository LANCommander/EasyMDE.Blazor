using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

public sealed class EasyMdeReadOnlyJsonConverter : JsonConverter<EasyMdeReadOnly>
{
    public override EasyMdeReadOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, EasyMdeReadOnly value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case EasyMdeReadOnly.KindType.Disabled:
                writer.WriteBooleanValue(false);
                return;

            case EasyMdeReadOnly.KindType.Enabled:
                writer.WriteBooleanValue(true);
                return;

            case EasyMdeReadOnly.KindType.NoCursor:
                writer.WriteStringValue("nocursor");
                return;

            case EasyMdeReadOnly.KindType.Default:
            default:
                writer.WriteNullValue();
                return;
        }
    }
}