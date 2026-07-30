using System.Collections.Specialized;
using ModularPipelines.Constants;
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
        var output = Render(table);

        await Assert.That(output).Contains("Environment variables");
        await Assert.That(output).Contains("***");
        await Assert.That(output).Contains("two");
        await Assert.That(output.IndexOf("FIRST", StringComparison.Ordinal))
            .IsLessThan(output.IndexOf("SECOND", StringComparison.Ordinal));
    }

    [Test]
    public async Task EnvironmentVariables_DoNotWrapLongValues()
    {
        var variables = new OrderedDictionary
        {
            ["LONG_VALUE"] = new string('x', 200),
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table, width: 40);
        var valueLineCount = output
            .Split(Environment.NewLine)
            .Count(line => line.Contains('x'));

        await Assert.That(valueLineCount).IsLessThanOrEqualTo(1);
        await Assert.That(output).Contains("…");
        await Assert.That(output).DoesNotContain(new string('x', 200));
    }

    [Test]
    [Arguments("GITHUB_TOKEN")]
    [Arguments("client_secret")]
    [Arguments("DatabasePassword")]
    [Arguments("API_KEY")]
    [Arguments("SERVICE_PWD")]
    [Arguments("credential")]
    public async Task EnvironmentVariables_MaskSensitiveNamesWithoutRegisteredSecret(
        string variableName)
    {
        const string unregisteredSecret = "unregistered-secret-value";
        var variables = new OrderedDictionary
        {
            [variableName] = unregisteredSecret,
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(LoggingConstants.SecretMask);
        await Assert.That(output).DoesNotContain(unregisteredSecret);
    }

    [Test]
    public async Task EnvironmentVariables_RenderEmbeddedNewlinesAsText()
    {
        var variables = new OrderedDictionary
        {
            ["PUBLIC_VALUE"] = "first\r\nsecond",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);
        var output = Render(table);

        await Assert.That(output).Contains(@"first\r\nsecond");
    }

    private static string Render(Table table, int width = 120)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
        });
        console.Profile.Width = width;

        console.Write(table);
        return writer.ToString();
    }
}
