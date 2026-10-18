using Novolis.Scripting.CSharp;
using Novolis.Scripting.Completion;

namespace Novolis.Scripting.Hosting;

/// <summary>Combined script execution and editor diagnostics service.</summary>
public sealed class CSharpScriptService
{
    private readonly CSharpScriptHost _host = new();

    /// <summary>Analyzes source before execution.</summary>
    public IReadOnlyList<ScriptDiagnostic> Analyze(string source) =>
        CSharpScriptDiagnostics.Analyze(source);

    /// <summary>Executes source with controlled host options.</summary>
    public Task<ScriptExecutionResult> ExecuteAsync(
        string source,
        ScriptHostOptions? options = null,
        CancellationToken cancellationToken = default) =>
        _host.ExecuteAsync(source, options, cancellationToken);
}
