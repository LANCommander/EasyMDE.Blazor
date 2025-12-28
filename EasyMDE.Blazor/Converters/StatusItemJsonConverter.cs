using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

public sealed class StatusItemJsonConverter : JsonConverter<StatusItem>
{
    public override StatusItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, StatusItem value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case StatusItem.BuiltInStatusItem builtIn:
                writer.WriteStringValue(ToToken(builtIn.Element));
                return;

            case StatusItem.CustomStatusItem custom:
                writer.WriteStringValue(custom.Key);
                return;

            default:
                throw new JsonException($"Unsupported StatusItem type: {value.GetType().FullName}");
        }
    }

    private static string ToToken(StatusElement element) => element switch
    {
        StatusElement.Autosave => "autosave",
        StatusElement.Lines => "lines",
        StatusElement.Words => "words",
        StatusElement.Cursor => "cursor",
        _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
    };
}