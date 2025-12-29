const _instances = new Map();

function _resolveElement(elementOrId) {
    if (!elementOrId) return null;
    if (typeof elementOrId === "string") return document.getElementById(elementOrId);
    return elementOrId;
}

function _keySignature(e) {
    // Normalize: "Ctrl+Alt+Shift+Meta+Key"
    // Use event.key as-is (e.g., "Enter", "Tab", "a", "A").
    const parts = [];
    if (e.ctrlKey) parts.push("Ctrl");
    if (e.altKey) parts.push("Alt");
    if (e.shiftKey) parts.push("Shift");
    if (e.metaKey) parts.push("Meta");
    parts.push(e.key);
    return parts.join("+");
}

function _regSignature(reg) {
    const parts = [];
    if (reg.ctrl) parts.push("Ctrl");
    if (reg.alt) parts.push("Alt");
    if (reg.shift) parts.push("Shift");
    if (reg.meta) parts.push("Meta");
    parts.push(reg.key);
    return parts.join("+");
}

export function create(textareaElementOrId, dotNetRef, options, interopOptions) {
    const textarea = _resolveElement(textareaElementOrId);
    if (!textarea) throw new Error("EasyMDE textarea element not found.");

    const instanceKey = textarea.id || crypto.randomUUID();
    textarea.dataset.easymdeKey = instanceKey;

    if (_instances.has(instanceKey)) 
        return instanceKey;

    const editor = new window.EasyMDE({ ...(options || {}), element: textarea });

    const entry = {
        editor,
        dotNetRef,
        lastSent: editor.value(),
        suppress: false,

        // Key registration map: signature -> registration
        keyMap: new Map(),

        keydownHandler: null,
        beforeChangeHandler: null,

        // When we need reliable async enforcement, we arm a one-shot gate.
        pendingGate: null // { allowed: null|bool, token: string }
    };

    // Build key map from registrations
    const regs = (interopOptions && interopOptions.interceptKeys) || [];
    
    for (const reg of regs) {
        entry.keyMap.set(_regSignature(reg), reg);
    }

    // Standard change propagation
    const onChange = async () => {
        if (entry.suppress) return;

        const current = editor.value();
        if (current === entry.lastSent) return;

        entry.lastSent = current;
        await dotNetRef.invokeMethodAsync("NotifyValueChanged", current);
    };
    editor.codemirror.on("change", onChange);

    // Keydown filter
    const onKeyDown = async (cm, e) => {
        const sig = _keySignature(e);
        const reg = entry.keyMap.get(sig);
        if (!reg) return; // fast path: do nothing

        // If purely block in JS (e.g. Enter), do it here, no .NET involved.
        if (reg.blockInJs) {
            e.preventDefault();
            e.stopPropagation();
        }

        // If we don't have .NET callback, nothing else to do.
        if (!reg.notifyDotNet && !reg.askDotNet) return;

        const evt = {
            key: e.key,
            ctrl: !!e.ctrlKey,
            alt: !!e.altKey,
            shift: !!e.shiftKey,
            meta: !!e.metaKey,
            origin: "+keydown"
        };

        // Notify only (fire-and-forget decision irrelevant)
        if (reg.notifyDotNet && !reg.askDotNet) {
            // We cannot await if we don't need a decision; but we can still await safely.
            try { await entry.dotNetRef.invokeMethodAsync("HandleKey", evt); } catch { }
            return;
        }

        // Ask .NET for allow/deny:
        // IMPORTANT: keydown is synchronous, so you can't reliably wait and then prevent.
        // If enforceViaBeforeChange is true, we arm a one-shot beforeChange gate that will cancel/reapply.
        if (reg.askDotNet) {
            if (reg.enforceViaBeforeChange) {
                // Arm the gate for the very next change caused by this key.
                // Token helps avoid stale answers.
                const token = crypto.randomUUID();
                entry.pendingGate = { token, allowed: null };

                // Call .NET (async)
                let allowed = true;
                try {
                    allowed = await entry.dotNetRef.invokeMethodAsync("HandleKey", evt);
                } catch {
                    allowed = true;
                }

                // Store decision for beforeChange to consume.
                if (entry.pendingGate && entry.pendingGate.token === token) {
                    entry.pendingGate.allowed = !!allowed;
                }
                return;
            }

            // If not enforcing, we can still notify/ask, but cannot guarantee blocking.
            try { await entry.dotNetRef.invokeMethodAsync("HandleKey", evt); } catch { }
        }
    };

    editor.codemirror.on("keydown", onKeyDown);
    entry.keydownHandler = onKeyDown;

    // Optional: reliable enforcement via beforeChange when pendingGate is armed
    const onBeforeChange = (cm, change) => {
        if (!entry.pendingGate) return;

        const gate = entry.pendingGate;

        // We only want to gate the immediate next change. Consume it once.
        entry.pendingGate = null;

        // If decision not ready yet, safest behavior is to allow (avoid freezing).
        if (gate.allowed === null) return;

        if (gate.allowed === false) {
            change.cancel();
            return;
        }

        // allowed === true -> let it proceed
    };

    editor.codemirror.on("beforeChange", onBeforeChange);
    entry.beforeChangeHandler = onBeforeChange;

    _instances.set(instanceKey, entry);
    return instanceKey;
}

export function setValue(textareaElementOrId, value) {
    const textarea = _resolveElement(textareaElementOrId);
    if (!textarea) return;

    const key = textarea.dataset.easymdeKey;
    if (!key) return;

    const entry = _instances.get(key);
    if (!entry) return;

    const next = value ?? "";
    const current = entry.editor.value();
    if (current === next) return;

    entry.suppressInput = true;
    try {
        entry.editor.value(next);
        entry.lastSent = next;
    } finally {
        entry.suppressInput = false;
    }
}

export function getValue(textareaElementOrId) {
    const textarea = _resolveElement(textareaElementOrId);
    if (!textarea) return "";

    const key = textarea.dataset.easymdeKey;
    if (!key) return textarea.value ?? "";

    const entry = _instances.get(key);
    if (!entry) return textarea.value ?? "";

    return entry.editor.value() ?? "";
}

export function destroy(textareaElementOrId) {
    const textarea = _resolveElement(textareaElementOrId);
    
    if (!textarea)
        return;

    const key = textarea.dataset.easymdeKey;
    
    if (!key)
        return;

    const entry = _instances.get(key);
    
    if (!entry)
        return;

    try {
        if (entry.keydownHandler)
            entry.editor.codemirror.off("keydown", entry.keydownHandler);
        
        if (entry.beforeChangeHandler)
            entry.editor.codemirror.off("beforeChange", entry.beforeChangeHandler);

        entry.editor.toTextArea();
        entry.dotNetRef?.dispose?.();
    } finally {
        _instances.delete(key);
        delete textarea.dataset.easymdeKey;
    }
}