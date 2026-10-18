using Novolis.Scripting.Hosting;
using Novolis.Scripting.Completion;

namespace Novolis.Scripting.Unit;

public sealed class ScriptingTests
{
    [Test]
    public async Task Host_executes_expression()
    {
        var result = await new CSharpScriptService().ExecuteAsync("1 + 2");

        await Assert.That(result.Succeeded).IsTrue();
        await Assert.That(result.ReturnValue).IsEqualTo(3);
    }

    [Test]
    public async Task Diagnostics_report_invalid_source()
    {
        var diagnostics = new CSharpScriptService().Analyze("1 +");

        await Assert.That(diagnostics.Count).IsGreaterThan(0);
    }

    [Test]
    public async Task Completion_returns_keyword_items()
    {
        var items = await new CSharpScriptCompletion().CompleteAsync("pub", 3);

        await Assert.That(items.Any(item => item.DisplayText == "public")).IsTrue();
    }
}
