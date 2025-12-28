using System.Text.Json.Serialization;
using EasyMDE.Blazor.Converters;

namespace EasyMDE.Blazor;

[JsonConverter(typeof(EasyMdeOptionsJsonConverter))]
public sealed class EasyMdeOptions
{
    public bool? AutoDownloadFontAwesome { get; set; }
    public bool? AutoFocus { get; set; }
    public bool? ForceSync { get; set; }
    public bool? IndentWithTabs { get; set; }
    
    public bool? LineNumbers { get; set; }
    public bool? LineWrapping { get; set; }
    
    public string? MinHeight { get; set; }
    public string? MaxHeight { get; set; }
    
    public string? Placeholder { get; set; }
    public bool? PromptUrls { get; set; }
    public bool? SpellChecker { get; set; }
    
    public bool? PreviewImagesInEditor { get; set; }

    public bool? SideBySideFullscreen { get; set; }
    public bool? StyleSelectedText { get; set; }
    public bool? SyncSideBySidePreviewScroll { get; set; }
    
    public int? TabSize { get; set; }
    public string? Theme { get; set; }
    public bool? ToolbarTips { get; set; }
    public string? ToolbarButtonClassPrefix { get; set; }
    
    public TextDirection? Direction { get; set; }
    public ScrollbarStyle? ScrollbarStyle { get; set; }
    public UnorderedListStyle? UnorderedListStyle { get; set; }
    
    public string? InitialValue { get; set; }

    public EasyMdeToolbar Toolbar { get; set; } = EasyMdeToolbar.Default;
    public EasyMdeStatus Status { get; set; } = EasyMdeStatus.Default;
    public EasyMdeReadOnly ReadOnly { get; set; } = EasyMdeReadOnly.Default;
    public EasyMdePreviewClass PreviewClass { get; set; } = EasyMdePreviewClass.Default;

    public EasyMdeBlockStyles? BlockStyles { get; set; }
    public EasyMdeInsertTexts? InsertTexts { get; set; }
    public EasyMdeParsingConfig? ParsingConfig { get; set; }
    public EasyMdeRenderingConfig? RenderingConfig { get; set; } // or your existing type name
    public EasyMdeAutosave? Autosave { get; set; }
    
    public IReadOnlyList<ToolbarIcon>? HideIcons { get; set; }
    public IReadOnlyList<ToolbarIcon>? ShowIcons { get; set; }
    public Dictionary<ToolbarIcon, string>? IconClassMap { get; set; }
    
    public EasyMdeImageUploadOptions? ImageUpload { get; set; }
}