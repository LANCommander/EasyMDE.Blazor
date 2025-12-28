using System.Text.Json;
using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

public sealed class EasyMdeOptionsJsonConverter : JsonConverter<EasyMdeOptions>
{
    public override EasyMdeOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Deserialization not required.");

    public override void Write(Utf8JsonWriter writer, EasyMdeOptions value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Primitives
        WriteBool(writer, "autoDownloadFontAwesome", value.AutoDownloadFontAwesome);
        WriteBool(writer, "autofocus", value.AutoFocus);
        WriteBool(writer, "forceSync", value.ForceSync);
        WriteBool(writer, "indentWithTabs", value.IndentWithTabs);

        WriteBool(writer, "lineNumbers", value.LineNumbers);
        WriteBool(writer, "lineWrapping", value.LineWrapping);

        WriteString(writer, "minHeight", value.MinHeight);
        WriteString(writer, "maxHeight", value.MaxHeight);

        WriteString(writer, "placeholder", value.Placeholder);
        WriteBool(writer, "promptURLs", value.PromptUrls);

        WriteBool(writer, "previewImagesInEditor", value.PreviewImagesInEditor);

        WriteBool(writer, "sideBySideFullscreen", value.SideBySideFullscreen);
        WriteBool(writer, "styleSelectedText", value.StyleSelectedText);
        WriteBool(writer, "syncSideBySidePreviewScroll", value.SyncSideBySidePreviewScroll);

        if (value.TabSize is not null) writer.WriteNumber("tabSize", value.TabSize.Value);
        WriteString(writer, "theme", value.Theme);
        WriteBool(writer, "toolbarTips", value.ToolbarTips);
        WriteString(writer, "toolbarButtonClassPrefix", value.ToolbarButtonClassPrefix);

        if (value.Direction is not null) writer.WriteString("direction", EasyMdeToken.Direction(value.Direction.Value));
        if (value.ScrollbarStyle is not null) writer.WriteString("scrollbarStyle", EasyMdeToken.Scrollbar(value.ScrollbarStyle.Value));
        if (value.UnorderedListStyle is not null) writer.WriteString("unorderedListStyle", EasyMdeToken.UnorderedList(value.UnorderedListStyle.Value));

        WriteString(writer, "initialValue", value.InitialValue);

        // Unions: omit when Default
        if (value.Toolbar.Kind != EasyMdeToolbar.KindType.Default)
        {
            writer.WritePropertyName("toolbar");
            JsonSerializer.Serialize(writer, value.Toolbar, options);
        }

        if (value.Status.Kind != EasyMdeStatus.KindType.Default)
        {
            writer.WritePropertyName("status");
            JsonSerializer.Serialize(writer, value.Status, options);
        }

        if (value.ReadOnly.Kind != EasyMdeReadOnly.KindType.Default)
        {
            writer.WritePropertyName("readOnly");
            JsonSerializer.Serialize(writer, value.ReadOnly, options);
        }

        if (value.PreviewClass.Kind != EasyMdePreviewClass.KindType.Default)
        {
            writer.WritePropertyName("previewClass");
            JsonSerializer.Serialize(writer, value.PreviewClass, options);
        }

        // Typed nested config
        if (value.BlockStyles is not null)
        {
            writer.WritePropertyName("blockStyles");
            writer.WriteStartObject();

            if (value.BlockStyles.Bold is not null)
                writer.WriteString("bold", value.BlockStyles.Bold == BoldStyle.Underscores ? "__" : "**");

            if (value.BlockStyles.Italic is not null)
                writer.WriteString("italic", value.BlockStyles.Italic == ItalicStyle.Underscore ? "_" : "*");

            if (value.BlockStyles.Code is not null)
                writer.WriteString("code", value.BlockStyles.Code == CodeFenceStyle.Tildes ? "~~~" : "```");

            writer.WriteEndObject();
        }

        if (value.InsertTexts is not null)
        {
            writer.WritePropertyName("insertTexts");
            writer.WriteStartObject();

            WriteInsertPair(writer, "horizontalRule", value.InsertTexts.HorizontalRule);
            WriteInsertPair(writer, "image", value.InsertTexts.Image);
            WriteInsertPair(writer, "link", value.InsertTexts.Link);
            WriteInsertPair(writer, "table", value.InsertTexts.Table);

            writer.WriteEndObject();
        }

        if (value.ParsingConfig is not null)
        {
            writer.WritePropertyName("parsingConfig");
            writer.WriteStartObject();

            WriteBool(writer, "allowAtxHeaderWithoutSpace", value.ParsingConfig.AllowAtxHeaderWithoutSpace);
            WriteBool(writer, "strikethrough", value.ParsingConfig.Strikethrough);
            WriteBool(writer, "underscoresBreakWords", value.ParsingConfig.UnderscoresBreakWords);

            writer.WriteEndObject();
        }

        if (value.RenderingConfig is not null)
        {
            writer.WritePropertyName("renderingConfig");
            JsonSerializer.Serialize(writer, value.RenderingConfig, options);
        }

        // Icons lists: arrays of strings
        WriteIconArray(writer, "hideIcons", value.HideIcons);
        WriteIconArray(writer, "showIcons", value.ShowIcons);

        // Icon class map: object { token: "class" }
        if (value.IconClassMap is not null && value.IconClassMap.Count > 0)
        {
            writer.WritePropertyName("iconClassMap");
            writer.WriteStartObject();
            foreach (var (key, className) in value.IconClassMap)
            {
                writer.WriteString(ToolbarIconToken.ToToken(key), className);
            }
            writer.WriteEndObject();
        }

        // Image upload: flatten into EasyMDE keys
        if (value.ImageUpload is not null)
        {
            WriteBool(writer, "uploadImage", value.ImageUpload.UploadImage);

            if (value.ImageUpload.ImageMaxSize is not null)
                writer.WriteNumber("imageMaxSize", value.ImageUpload.ImageMaxSize.Value);

            WriteString(writer, "imageAccept", value.ImageUpload.ImageAccept);
            WriteString(writer, "imageUploadEndpoint", value.ImageUpload.ImageUploadEndpoint);
            WriteBool(writer, "imagePathAbsolute", value.ImageUpload.ImagePathAbsolute);

            WriteString(writer, "imageCSRFToken", value.ImageUpload.ImageCSRFToken);
            WriteString(writer, "imageCSRFName", value.ImageUpload.ImageCSRFName);
            WriteBool(writer, "imageCSRFHeader", value.ImageUpload.ImageCSRFHeader);
        }

        writer.WriteEndObject();
    }

    private static void WriteString(Utf8JsonWriter writer, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            writer.WriteString(name, value);
    }

    private static void WriteBool(Utf8JsonWriter writer, string name, bool? value)
    {
        if (value is not null)
            writer.WriteBoolean(name, value.Value);
    }

    private static void WriteInsertPair(Utf8JsonWriter writer, string name, InsertTextPair? pair)
    {
        if (pair is null) return;
        writer.WritePropertyName(name);
        writer.WriteStartArray();
        writer.WriteStringValue(pair.Value.Before);
        writer.WriteStringValue(pair.Value.After);
        writer.WriteEndArray();
    }

    private static void WriteIconArray(Utf8JsonWriter writer, string name, IReadOnlyList<ToolbarIcon>? icons)
    {
        if (icons is null || icons.Count == 0) return;

        writer.WritePropertyName(name);
        writer.WriteStartArray();
        foreach (var icon in icons)
            writer.WriteStringValue(ToolbarIconToken.ToToken(icon));
        writer.WriteEndArray();
    }
}