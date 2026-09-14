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
        await Assert.That(definition!.Options.Single(option => option.SwitchName == switchName).IsRequired).IsTrue();
    }

    [Test]
    [Arguments("The ID of the deployment step.", false)]
    [Arguments("The ID is required only when another option is set.", false)]
    [Arguments("The name, for example required.", false)]
    [Arguments("For example, use (required) to illustrate a marker.", false)]
    [Arguments("The name, FOR EXAMPLE use (required).", false)]
    [Arguments("For example, use 'name' (required).", false)]
    [Arguments("For example, use 'Example. (required) value'.", false)]
    [Arguments("For example, use (required). The selector (required).", true)]
    [Arguments("The selector (required). For example, use the name.", true)]
    [Arguments("For example, use a name. Use the 'name' selector (required).", true)]
    [Arguments("The name, for example \"(required)\".", false)]
    [Arguments("The name, for example '(required)'.", false)]
    [Arguments("The name, for example `(required)`.", false)]
    [Arguments("The name, for example \"Example. (required) value\".", false)]
    [Arguments("The name, for example 'Example. (required) value'.", false)]
    [Arguments("The name, for example `Example. (required) value`.", false)]
    [Arguments("The name. (required when creating a deployment)", false)]
    [Arguments("The name. (REQUIRED)", true)]
    [Arguments("(required) The name of the deployment step.", true)]
    [Arguments("The name. (required) One of: plan-description, apply-description.", true)]
    [Arguments("The name.\n                       (required)", true)]
    [Arguments("The ID of the deployment run to watch (required).", true)]
    [Arguments("The ID of the deployment run to cancel (required).", true)]
    [Arguments("The ID of the deployment run (required).", true)]
    [Arguments("A comma-separated list of deployment names to rerun within the deployment group (required).", true)]
    [Arguments("The name of the organization to target. Overrides the ENV VAR 'TF_STACKS_ORGANIZATION_NAME' if provided. (required)", true)]
    [Arguments("The name of the project to target. Overrides the ENV VAR 'TF_STACKS_PROJECT_NAME' if provided. (required)", true)]
    [Arguments("The name of the stack to target. Overrides the ENV VAR 'TF_STACKS_STACK_NAME' if provided. (required)", true)]
    [Arguments("The deployment's ID (required).", true)]
    [Arguments("Use the 'name' selector (required).", true)]
    [Arguments("The name, for example \"(required).\".", false)]
    [Arguments("The name, for example '(required).'.", false)]
    [Arguments("The name, for example `(required).`.", false)]
    public async Task Only_Explicit_Required_Markers_Make_Options_Required(string description, bool expected)
    {
        var definition = await _scraper.Parse(
            ["terraform", "stacks", "deployment-step", "show"],
            CreateHelpText("show", "-deployment-step-id", description));

        await Assert.That(definition!.Options.Single().IsRequired).IsEqualTo(expected);
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

    [Test]
    public async Task DeploymentStepParentDoesNotUseChildDescription()
    {
        const string helpText = """
            Usage: terraform stacks deployment-step <subcommand> [options]

            Subcommands:
              artifacts  Download artifacts from a deployment step.
              show       Show details of a deployment step in the current configuration.
            """;

        var definition = await _scraper.Parse(
            ["terraform", "stacks", "deployment-step"],
            helpText);

        await Assert.That(definition!.Description).IsNull();
    }

    [Test]
    public async Task DeploymentStepShowUsesItsOwnDescription()
    {
        const string helpText = """
            Usage: terraform stacks deployment-step show [options]

            Show the details of a single deployment step.

            Options:
              -deployment-step-id  The ID of the deployment step to show. (required)
            """;

        var definition = await _scraper.Parse(
            ["terraform", "stacks", "deployment-step", "show"],
            helpText);

        await Assert.That(definition!.Description)
            .IsEqualTo("Show the details of a single deployment step.");
    }

    [Test]
    public async Task DescriptionAfterAliasesIsPreserved()
    {
        const string helpText = """
            Usage: terraform stacks deployment-step show [options]

            Aliases: s

            Show the details of a single deployment step.

            Options:
              -deployment-step-id  The ID of the deployment step to show. (required)
            """;

        var definition = await _scraper.Parse(
            ["terraform", "stacks", "deployment-step", "show"],
            helpText);

        await Assert.That(definition!.Description)
            .IsEqualTo("Show the details of a single deployment step.");
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

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "-lock=false  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage: terraform plan [options]

            Options:
              -input=false        Ask for input for variables if not directly set. Combine with
                                  -lock=false  to skip state locking as well.
              -lock=false         Don't hold a state lock during the operation.
            """;

        var definition = await _scraper.Parse(["terraform", "plan"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(definition!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["-input", "-lock"]);
            await Assert.That(definition.Options.Single(option => option.SwitchName == "-input").Description)
                .IsEqualTo("Ask for input for variables if not directly set. Combine with -lock=false  to skip state locking as well.");
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
