namespace EasyMDE.Blazor;

public sealed class EasyMdeParsingConfig
{
    public bool? AllowAtxHeaderWithoutSpace { get; set; }
    public bool? Strikethrough { get; set; }
    public bool? UnderscoresBreakWords { get; set; }
}