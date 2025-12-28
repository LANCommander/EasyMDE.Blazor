namespace EasyMDE.Blazor.Converters;

internal static class ToolbarIconToken
{
    public static string ToToken(ToolbarIcon icon) => icon switch
    {
        ToolbarIcon.Bold => "bold",
        ToolbarIcon.Italic => "italic",
        ToolbarIcon.Strikethrough => "strikethrough",
        ToolbarIcon.Heading => "heading",
        ToolbarIcon.HeadingSmaller => "heading-smaller",
        ToolbarIcon.HeadingBigger => "heading-bigger",
        ToolbarIcon.Heading1 => "heading-1",
        ToolbarIcon.Heading2 => "heading-2",
        ToolbarIcon.Heading3 => "heading-3",
        ToolbarIcon.Code => "code",
        ToolbarIcon.Quote => "quote",
        ToolbarIcon.UnorderedList => "unordered-list",
        ToolbarIcon.OrderedList => "ordered-list",
        ToolbarIcon.CleanBlock => "clean-block",
        ToolbarIcon.Link => "link",
        ToolbarIcon.Image => "image",
        ToolbarIcon.Table => "table",
        ToolbarIcon.HorizontalRule => "horizontal-rule",
        ToolbarIcon.Preview => "preview",
        ToolbarIcon.SideBySide => "side-by-side",
        ToolbarIcon.Fullscreen => "fullscreen",
        ToolbarIcon.Guide => "guide",
        ToolbarIcon.Undo => "undo",
        ToolbarIcon.Redo => "redo",
        ToolbarIcon.UploadImage => "upload-image",
        _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, null)
    };
}