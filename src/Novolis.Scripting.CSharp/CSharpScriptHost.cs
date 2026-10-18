using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Novolis.Scripting.CSharp;

/// <summary>Executes C# snippets with explicit host inputs.</summary>
public sealed class CSharpScriptHost
{
    /// <summary>Runs a script and returns its value or structured failure.</summary>
    public async Task<ScriptExecutionResult> ExecuteAsync(
        string code,
        ScriptHostOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        options ??= new ScriptHostOptions();

        var scriptOptions = ScriptOptions.Default
            .AddReferences(options.References)
            .AddImports(options.Imports);
        try
        {
            var state = await CSharpScript.RunAsync(
                code,
                scriptOptions,
                options.Globals,
                cancellationToken: cancellationToken);
            return new ScriptExecutionResult(state.ReturnValue, []);
        }
        catch (CompilationErrorException exception)
        {
            return new ScriptExecutionResult(
                null,
                exception.Diagnostics.Select(diagnostic => diagnostic.ToString()).ToArray());
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return new ScriptExecutionResult(null, [], exception);
        }
    }
}
