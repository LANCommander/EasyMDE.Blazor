namespace EasyMDE.Blazor.Converters;

internal static class EasyMdeToken
{
    public static string Direction(TextDirection value) => value switch
    {
        TextDirection.Ltr => "ltr",
        TextDirection.Rtl => "rtl",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static string Scrollbar(ScrollbarStyle value) => value switch
    {
        ScrollbarStyle.Native => "native",
        ScrollbarStyle.Null => "null",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    public static string UnorderedList(UnorderedListStyle value) => value switch
    {
        UnorderedListStyle.Asterisk => "*",
        UnorderedListStyle.Dash => "-",
        UnorderedListStyle.Plus => "+",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };
}