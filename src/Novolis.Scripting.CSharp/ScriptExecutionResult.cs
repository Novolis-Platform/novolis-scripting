namespace Novolis.Scripting.CSharp;

/// <summary>Result from a C# script execution.</summary>
public sealed record ScriptExecutionResult(
    object? ReturnValue,
    IReadOnlyList<string> Diagnostics,
    Exception? Fault = null)
{
    /// <summary>Whether the script completed without diagnostics or faults.</summary>
    public bool Succeeded => Fault is null && Diagnostics.Count == 0;
}
