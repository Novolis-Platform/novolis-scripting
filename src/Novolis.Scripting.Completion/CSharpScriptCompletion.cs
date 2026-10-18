using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;

namespace Novolis.Scripting.Completion;

/// <summary>Provides Roslyn completion for C# script source.</summary>
public sealed class CSharpScriptCompletion
{
    /// <summary>Gets completion items at a source position.</summary>
    public async Task<IReadOnlyList<ScriptCompletionItem>> CompleteAsync(
        string source,
        int position,
        IEnumerable<Assembly>? references = null,
        IEnumerable<string>? imports = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(position);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(position, source.Length);

        var projectId = ProjectId.CreateNewId();
        var documentId = DocumentId.CreateNewId(projectId);
        var metadataReferences = (references ?? [])
            .Where(assembly => !string.IsNullOrWhiteSpace(assembly.Location))
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
            .ToArray();
        var parseOptions = new CSharpParseOptions(kind: SourceCodeKind.Script);
        var compilationOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            usings: imports ?? []);
        var projectInfo = ProjectInfo.Create(
            projectId,
            VersionStamp.Create(),
            "C# Script",
            "CSharpScript",
            LanguageNames.CSharp,
            parseOptions: parseOptions,
            compilationOptions: compilationOptions,
            metadataReferences: metadataReferences);
        var assemblies = MefHostServices.DefaultAssemblies
            .Add(typeof(CompletionService).Assembly)
            .Add(Assembly.Load("Microsoft.CodeAnalysis.CSharp.Features"));
        var host = MefHostServices.Create(assemblies);
        using var workspace = new AdhocWorkspace(host);
        var solution = workspace.CurrentSolution
            .AddProject(projectInfo)
            .AddDocument(documentId, "Script.csx", SourceText.From(source));
        var document = solution.GetDocument(documentId)
            ?? throw new InvalidOperationException("The script document could not be created.");
        var service = CompletionService.GetService(document);
        if (service is null)
        {
            return KeywordCompletions(source, position);
        }

        var completionList = await service.GetCompletionsAsync(
            document,
            position,
            cancellationToken: cancellationToken);
        var items = completionList?.ItemsList
            .Select(item => new ScriptCompletionItem(
                item.DisplayText,
                item.FilterText,
                item.SortText,
                item.Tags))
            .ToArray() ?? [];
        return items.Length == 0 ? KeywordCompletions(source, position) : items;
    }

    private static IReadOnlyList<ScriptCompletionItem> KeywordCompletions(string source, int position)
    {
        var start = position;
        while (start > 0 && (char.IsLetterOrDigit(source[start - 1]) || source[start - 1] == '_'))
        {
            start--;
        }

        var prefix = source[start..position];
        var keywords = new[]
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default",
            "delegate", "do", "double", "else", "enum", "event", "explicit",
            "extern", "false", "finally", "fixed", "float", "for", "foreach",
            "goto", "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object",
            "operator", "out", "override", "params", "private", "protected",
            "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
            "async", "await", "get", "init", "record", "required", "set", "var",
            "when", "with", "yield",
        };
        return keywords
        .Where(keyword => keyword.StartsWith(prefix, StringComparison.Ordinal))
        .Select(keyword => new ScriptCompletionItem(keyword, keyword, keyword, ["Keyword"]))
        .ToArray();
    }
}
