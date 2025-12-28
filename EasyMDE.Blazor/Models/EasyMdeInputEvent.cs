namespace EasyMDE.Blazor;

public sealed class EasyMdeInputEvent
{
    public string Kind { get; set; } = "beforeChange";
    public string Origin { get; set; } = "";

    public string InsertedText { get; set; } = "";
    public string RemovedText { get; set; } = "";

    public CodeMirrorPosition From { get; set; }
    public CodeMirrorPosition To { get; set; }
}