namespace EasyMDE.Blazor;

public sealed class EasyMdeKeyRegistration
{
    // Examples: "Enter", "Tab", "Backspace", "k", "K", " "
    public required string Key { get; init; }

    // Optional modifiers
    public bool Ctrl { get; init; }
    public bool Alt { get; init; }
    public bool Shift { get; init; }
    public bool Meta { get; init; }

    // If true, JS will call into .NET when this key is pressed.
    public bool NotifyDotNet { get; init; }

    // If true, JS will preventDefault/stopPropagation immediately (no .NET call).
    public bool BlockInJs { get; init; }

    // If true, JS will round-trip to .NET for allow/deny.
    // NOTE: you cannot reliably “prevent default” based on an async response in keydown alone.
    // If you set AskDotNet=true and you want to block actual editor changes, use the beforeChange gate (see below).
    public bool AskDotNet { get; init; }

    // If true, the async decision will be enforced via beforeChange cancel/reapply (reliable blocking).
    // Use this sparingly.
    public bool EnforceViaBeforeChange { get; init; }
}