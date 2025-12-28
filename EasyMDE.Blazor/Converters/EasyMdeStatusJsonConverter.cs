using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

public sealed class EasyMdeStatusJsonConverter : JsonConverter<EasyMdeStatus>
{
    public override EasyMdeStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, EasyMdeStatus value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case EasyMdeStatus.KindType.Disabled:
                writer.WriteBooleanValue(false);
                return;

            case EasyMdeStatus.KindType.Custom:
                writer.WriteStartArray();
                foreach (var item in value.Items ?? Array.Empty<StatusItem>())
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
                writer.WriteEndArray();
                return;

            case EasyMdeStatus.KindType.Default:
            default:
                writer.WriteNullValue();
                return;
        }
    }
}