using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Novolis.Scripting.Completion;

/// <summary>Syntax diagnostics for an editor or script host.</summary>
public static class CSharpScriptDiagnostics
{
    /// <summary>Parses source and returns diagnostics with source spans.</summary>
    public static IReadOnlyList<ScriptDiagnostic> Analyze(string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var tree = CSharpSyntaxTree.ParseText(source);
        return tree.GetDiagnostics()
            .Select(diagnostic => new ScriptDiagnostic(
                diagnostic.Id,
                diagnostic.GetMessage(),
                diagnostic.Location.SourceSpan.Start,
                diagnostic.Location.SourceSpan.Length,
                diagnostic.Severity.ToString()))
            .ToArray();
    }
}
