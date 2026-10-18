namespace Novolis.Scripting.Completion;

/// <summary>Completion item exposed by the C# script language service.</summary>
public sealed record ScriptCompletionItem(
    string DisplayText,
    string FilterText,
    string SortText,
    IReadOnlyList<string> Tags);
