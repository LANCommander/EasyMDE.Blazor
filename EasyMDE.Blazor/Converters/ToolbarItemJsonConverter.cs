using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyMDE.Blazor.Converters;

public sealed class ToolbarItemJsonConverter : JsonConverter<ToolbarItem>
{
    public override ToolbarItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, ToolbarItem value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ToolbarItem.BuiltInButtonItem builtIn:
                writer.WriteStringValue(ToEasyMdeToken(builtIn.Command));
                return;

            case ToolbarItem.SeparatorItem:
                writer.WriteStringValue("|");
                return;

            case ToolbarItem.CustomButtonItem custom:
                writer.WriteStartObject();
                writer.WriteString("name", custom.Button.Name);
                writer.WriteString("action", custom.Button.Action);

                if (!string.IsNullOrWhiteSpace(custom.Button.ClassName))
                    writer.WriteString("className", custom.Button.ClassName);

                if (!string.IsNullOrWhiteSpace(custom.Button.Title))
                    writer.WriteString("title", custom.Button.Title);

                if (custom.Button.NoDisable is not null)
                    writer.WriteBoolean("noDisable", custom.Button.NoDisable.Value);

                writer.WriteEndObject();
                return;

            default:
                throw new JsonException($"Unsupported ToolbarItem type: {value.GetType().FullName}");
        }
    }

    private static string ToEasyMdeToken(ToolbarButton button) => button switch
    {
        ToolbarButton.Bold => "bold",
        ToolbarButton.Italic => "italic",
        ToolbarButton.Strikethrough => "strikethrough",
        ToolbarButton.Heading => "heading",
        ToolbarButton.HeadingSmaller => "heading-smaller",
        ToolbarButton.HeadingBigger => "heading-bigger",
        ToolbarButton.Heading1 => "heading-1",
        ToolbarButton.Heading2 => "heading-2",
        ToolbarButton.Heading3 => "heading-3",
        ToolbarButton.Code => "code",
        ToolbarButton.Quote => "quote",
        ToolbarButton.UnorderedList => "unordered-list",
        ToolbarButton.OrderedList => "ordered-list",
        ToolbarButton.CleanBlock => "clean-block",
        ToolbarButton.Link => "link",
        ToolbarButton.Image => "image",
        ToolbarButton.Table => "table",
        ToolbarButton.HorizontalRule => "horizontal-rule",
        ToolbarButton.Preview => "preview",
        ToolbarButton.SideBySide => "side-by-side",
        ToolbarButton.Fullscreen => "fullscreen",
        ToolbarButton.Guide => "guide",
        ToolbarButton.Undo => "undo",
        ToolbarButton.Redo => "redo",
        _ => throw new ArgumentOutOfRangeException(nameof(button), button, "Add mapping for this ToolbarButton.")
    };
}