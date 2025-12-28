using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

[JsonConverter(typeof(StatusItemJsonConverter))]
public abstract record StatusItem
{
    public static StatusItem BuiltIn(StatusElement element) => new BuiltInStatusItem(element);
    public static StatusItem Custom(string key) => new CustomStatusItem(key);

    internal sealed record BuiltInStatusItem(StatusElement Element) : StatusItem;
    internal sealed record CustomStatusItem(string Key) : StatusItem;
}