namespace Novolis.Scripting.Completion;

/// <summary>Diagnostic returned by C# script source analysis.</summary>
public sealed record ScriptDiagnostic(
    string Id,
    string Message,
    int Start,
    int Length,
    string Severity);
