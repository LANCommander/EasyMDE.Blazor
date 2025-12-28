using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

public sealed class EasyMdePreviewClassJsonConverter : JsonConverter<EasyMdePreviewClass>
{
    public override EasyMdePreviewClass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, EasyMdePreviewClass value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case EasyMdePreviewClass.KindType.Single:
                writer.WriteStringValue(value.Single);
                return;

            case EasyMdePreviewClass.KindType.Many:
                writer.WriteStartArray();
                foreach (var item in value.Many ?? Array.Empty<string>())
                    writer.WriteStringValue(item);
                writer.WriteEndArray();
                return;

            case EasyMdePreviewClass.KindType.Default:
            default:
                writer.WriteNullValue();
                return;
        }
    }
}