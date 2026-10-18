using System.Reflection;

namespace Novolis.Scripting.CSharp;

/// <summary>Controlled references, imports, and globals for a C# script.</summary>
public sealed record ScriptHostOptions
{
    /// <summary>Assemblies made available to the script.</summary>
    public IReadOnlyList<Assembly> References { get; init; } = [];

    /// <summary>Namespaces imported into the script.</summary>
    public IReadOnlyList<string> Imports { get; init; } = [];

    /// <summary>Globals object exposed to the script.</summary>
    public object? Globals { get; init; }
}
