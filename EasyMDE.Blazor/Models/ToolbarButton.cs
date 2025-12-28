namespace EasyMDE.Blazor;

/// <summary>
/// Built-in toolbar buttons. This list is not exhaustive; add as needed.
/// These serialize to EasyMDE's string tokens.
/// </summary>
public enum ToolbarButton
{
    Bold,
    Italic,
    Strikethrough,
    Heading,
    HeadingSmaller,
    HeadingBigger,
    Heading1,
    Heading2,
    Heading3,
    Code,
    Quote,
    UnorderedList,
    OrderedList,
    CleanBlock,
    Link,
    Image,
    Table,
    HorizontalRule,
    Preview,
    SideBySide,
    Fullscreen,
    Guide,
    Undo,
    Redo,
}