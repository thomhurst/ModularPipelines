using System.Collections.Specialized;
using ModularPipelines.Engine.Executors;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Engine.Executors;

public class PipelineInitializerTests
{
    [Test]
    public async Task EnvironmentVariables_AreRenderedAsMaskedSortedTable()
    {
        var variables = new OrderedDictionary
        {
            ["SECOND"] = "two",
            ["FIRST"] = "secret",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value == "secret" ? "***" : value);
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
        });

        console.Write(table);
        var output = writer.ToString();

        await Assert.That(output).Contains("Environment variables");
        await Assert.That(output).Contains("***");
        await Assert.That(output.IndexOf("FIRST", StringComparison.Ordinal))
            .IsLessThan(output.IndexOf("SECOND", StringComparison.Ordinal));
    }
}
