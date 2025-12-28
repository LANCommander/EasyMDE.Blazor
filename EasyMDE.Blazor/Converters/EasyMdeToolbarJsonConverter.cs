using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

/// <summary>
/// Converter that writes the toolbar option as:
/// - omitted (Default) => handled by parent options converter
/// - false (Disabled)
/// - array (Custom)
/// </summary>
public sealed class EasyMdeToolbarJsonConverter : JsonConverter<EasyMdeToolbar>
{
    public override EasyMdeToolbar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Deserialization is not required for EasyMDE options.");

    public override void Write(Utf8JsonWriter writer, EasyMdeToolbar value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case EasyMdeToolbar.KindType.Disabled:
                writer.WriteBooleanValue(false);
                return;

            case EasyMdeToolbar.KindType.Custom:
                writer.WriteStartArray();
                foreach (var item in value.Items ?? Array.Empty<ToolbarItem>())
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
                writer.WriteEndArray();
                return;

            case EasyMdeToolbar.KindType.Default:
            default:
                // Parent options converter should omit the property entirely.
                // If this gets called directly, we’ll write null to be explicit.
                writer.WriteNullValue();
                return;
        }
    }
}