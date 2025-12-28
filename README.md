# EasyMDE.Blazor

A Blazor wrapper around [EasyMDE](https://github.com/Ionaru/easy-markdown-editor) with:

- Two-way binding (`@bind-Value`)
- Strongly-typed EasyMDE options (no `object`-typed option surface)
- Strongly-typed `Toolbar`, `Status`, and `ReadOnly` unions
- Optional key interception (fast-path in JavaScript; only calls .NET for registered keys)
- Optional ability to block specific keys (e.g., prevent `Enter` from inserting a newline)

---

## Package goals

1. Keep the **C# API strongly typed** (simple primitives/enums/unions)
2. Keep the **JavaScript surface minimal**
3. Support **Blazor Server** and **Blazor WebAssembly**
4. Avoid editor churn and event loops (programmatic `Value` updates don’t re-trigger input handlers)

---

## Installation

In your Blazor application, load the bundled EasyMDE stylesheet and script files:
```html
<link rel="stylesheet" href="_content/EasyMDE.Blazor/lib/easymde/easymde.min.css" />
<script src="_content/EasyMDE.Blazor/lib/easymde/easymde.min.js"></script>
```

---

## Basic usage

```razor
@using EasyMDE.Blazor

<EasyMdeEditor @bind-Value="Markdown" />

@code {
    private string? Markdown = "# Hello";
}
```

---

## Strongly-typed options

```razor
<EasyMdeEditor @bind-Value="Markdown" Options="Options" />

@code {
    private string? Markdown;

    private readonly EasyMdeOptions Options = new()
    {
        Placeholder = "Write markdown...",
        MinHeight = "300px",

        Toolbar = EasyMdeToolbar.Custom(new ToolbarItem[]
        {
            ToolbarItem.BuiltIn(ToolbarButton.Bold),
            ToolbarItem.BuiltIn(ToolbarButton.Italic),
            ToolbarItem.Separator,
            ToolbarItem.BuiltIn(ToolbarButton.Preview),
            ToolbarItem.BuiltIn(ToolbarButton.Fullscreen),
        }),

        Status = EasyMdeStatus.Custom(new StatusItem[]
        {
            StatusItem.BuiltIn(StatusElement.Lines),
            StatusItem.BuiltIn(StatusElement.Words),
            StatusItem.BuiltIn(StatusElement.Cursor),
        }),

        ReadOnly = EasyMdeReadOnly.Disabled
    };
}
```

### ReadOnly modes

```csharp
ReadOnly = EasyMdeReadOnly.Disabled; // editable (readOnly: false)
ReadOnly = EasyMdeReadOnly.Enabled;  // readOnly: true
ReadOnly = EasyMdeReadOnly.NoCursor; // readOnly: "nocursor"
```

---

## Key interception and blocking (fast-path)

To avoid sending every keystroke to .NET (especially on Blazor Server), the wrapper supports **pre-registered key filters**.

- JS listens to `keydown`
- If the key is **not registered**, nothing happens (fast-path)
- If the key is registered:
  - `BlockInJs = true` blocks synchronously with no .NET call
  - `NotifyDotNet = true` calls `.NET` (no blocking guarantee, just notification)
  - `AskDotNet = true` calls `.NET` for an allow/deny decision (see notes below)

### Block `Enter` from inserting a newline (no .NET round-trip)

```razor
<EasyMdeEditor @bind-Value="Markdown"
               InterceptKeys="Keys" />

@code {
    private string? Markdown;

    private readonly EasyMdeKeyRegistration[] Keys =
    [
        new()
        {
            Key = "Enter",
            BlockInJs = true
        }
    ];
}
```

### Notify .NET on Ctrl+S (save)

```razor
<EasyMdeEditor @bind-Value="Markdown"
               InterceptKeys="Keys"
               OnKey="HandleKey" />

@code {
    private string? Markdown;

    private readonly EasyMdeKeyRegistration[] Keys =
    [
        new() { Key = "s", Ctrl = true, NotifyDotNet = true }
    ];

    private ValueTask<bool> HandleKey(EasyMdeKeyEvent e)
    {
        if (e.Ctrl && (e.Key is "s" or "S"))
        {
            // trigger save
        }

        return ValueTask.FromResult(true);
    }
}
```

---

## License

MIT
