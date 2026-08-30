using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class TerraformCliScraperTests
{
    private readonly TestTerraformCliScraper _scraper = new();

    [Test]
    [Arguments("artifacts", "-deployment-step-id", "The ID of the deployment step. (required)")]
    [Arguments("artifacts", "-artifact-name", "The artifact type to retrieve. (required)")]
    [Arguments("show", "-deployment-step-id", "The ID of the deployment step to show. (required)")]
    public async Task DeploymentStep_Selectors_Are_Value_Options(
        string command,
        string switchName,
        string description)
    {
        var definition = await _scraper.Parse(
            ["terraform", "stacks", "deployment-step", command],
            CreateHelpText(command, switchName, description));

        await AssertValueOption(definition, switchName);
    }

    [Test]
    public async Task Diagnostics_Id_Is_A_Value_Option()
    {
        const string helpText = """
            Usage: terraform stacks diagnostics [options]

            Options:
              -id  The ID of the stack configuration or deployment step to retrieve diagnostics for. Supported prefixes are "stc-" for configuration IDs and "sds-" for step IDs.
            """;

        var definition = await _scraper.Parse(["terraform", "stacks", "diagnostics"], helpText);

        await AssertValueOption(definition, "-id");
    }

    [Test]
    public async Task Operational_Stacks_Switch_Remains_A_Flag()
    {
        const string helpText = """
            Usage: terraform stacks fmt [options]

            Options:
              -check  Check if the input is formatted.
            """;

        var definition = await _scraper.Parse(["terraform", "stacks", "fmt"], helpText);
        var option = definition!.Options.Single(item => item.SwitchName == "-check");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.ValueSeparator).IsEqualTo(" ");
        }
    }

    private static string CreateHelpText(string command, string switchName, string description) => $$"""
        Usage: terraform stacks deployment-step {{command}} [options]

        Options:
          {{switchName}}  {{description}}
        """;

    private static async Task AssertValueOption(CliCommandDefinition? definition, string switchName)
    {
        var option = definition!.Options.Single(item => item.SwitchName == switchName);

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
        }
    }

    private sealed class TestTerraformCliScraper()
        : TerraformCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<TerraformCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
