using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    private const string GlobalOptionsHelp = """
        NAME
               aws -
        SYNOPSIS
               aws [options] <command> <subcommand> [parameters]
        GLOBAL OPTIONS
               --region (string)

               The region to use.

               --profile (string)

               Use a named credential profile.

               --output (string)

               Output format.

               o json
               o text
               o yaml-stream

               --cli-read-timeout (int)

               Socket read timeout in seconds.

               --cli-connect-timeout (int)

               Socket connect timeout in seconds.

               --color (string)

               Color output.

               o on
               o off
               o auto

               --cli-binary-format (string)

               Binary format.

               o base64
               o raw-in-base64-out

               --cli-error-format (string)

               Error format.

               o legacy
               o enhanced

               --no-sign-request (boolean)

               Do not sign requests.

               --version (string)

               Display the version.

        AVAILABLE SERVICES
               o ec2
        """;

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Scrape_Collects_Global_Options_Without_Duplicating_Command_Options(string newline)
    {
        var scraper = new AwsCliScraper(
            new AwsFixtureExecutor(GlobalOptionsHelp.Replace("\r\n", "\n").Replace("\n", newline)),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(
            ["--region", "--profile", "--output", "--cli-read-timeout", "--cli-connect-timeout",
                "--color", "--cli-binary-format", "--cli-error-format", "--no-sign-request"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--region").Description)
            .IsEqualTo("The region to use.");
        await Assert.That(globals.Single(option => option.SwitchName == "--cli-read-timeout").CSharpType)
            .IsEqualTo("int?");
        await Assert.That(globals.Single(option => option.SwitchName == "--cli-connect-timeout").CSharpType)
            .IsEqualTo("int?");
        await Assert.That(globals.Single(option => option.SwitchName == "--no-sign-request").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--output").EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["json", "text", "yaml-stream"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--color").EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["on", "off", "auto"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--cli-binary-format").EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["base64", "raw-in-base64-out"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--cli-error-format").EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["legacy", "enhanced"]);
        await Assert.That(commands.Single().Options.Select(option => option.SwitchName)).IsEquivalentTo(["--enabled"]);

        var baseFile = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single();
        await Assert.That(baseFile.Content).Contains("[CliOption(\"--region\"");
        await Assert.That(baseFile.Content).Contains("[CliGlobalOptions]");
        await Assert.That(baseFile.Content).Contains("public virtual string? Profile");
        await Assert.That(baseFile.Content).Contains("public virtual int? CliReadTimeout");
        await Assert.That(baseFile.Content).Contains("public virtual int? CliConnectTimeout");
        await Assert.That(baseFile.Content).Contains("[CliFlag(\"--no-sign-request\"");
        await Assert.That(baseFile.Content).DoesNotContain("--version");
        var commandFile = (await new OptionsClassGenerator().GenerateAsync(tool)).Single();
        await Assert.That(commandFile.Content).DoesNotContain("--region");
    }

    [Test]
    public async Task Global_Properties_Do_Not_Hide_Command_Options_Or_Operands()
    {
        var command = (await new TestAwsCliScraper().Parse(["aws", "fixture", "apply"], """
            OPTIONS
                   --region (string)
                       A service-specific region value.
            """))!;
        var tool = new CliToolDefinition
        {
            ToolName = "aws",
            NamespacePrefix = "Aws",
            TargetNamespace = "ModularPipelines.AmazonWebServices",
            OutputDirectory = "src/ModularPipelines.AmazonWebServices",
            GlobalOptions =
            [
                new() { SwitchName = "--region", PropertyName = "Region", CSharpType = "string?" },
                new() { SwitchName = "--profile", PropertyName = "Profile", CSharpType = "string?" },
            ],
            Commands = [command with
            {
                PositionalArguments =
                [
                    new() { PropertyName = "Profile", CSharpType = "string?", PositionIndex = 0 },
                ],
            }],
        };

        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        await Assert.That(resolved.GlobalOptions.Select(option => option.PropertyName))
            .IsEquivalentTo(["Region", "Profile"]);
        await Assert.That(resolved.Commands.Single().Options.Single().PropertyName).IsEqualTo("FixtureRegion");
        await Assert.That(resolved.Commands.Single().PositionalArguments.Single().PropertyName).IsEqualTo("FixtureProfile");
        var commandFile = (await new OptionsClassGenerator().GenerateAsync(resolved)).Single();
        await Assert.That(commandFile.Content).Contains("public string? FixtureRegion");
        await Assert.That(commandFile.Content).Contains("public string? FixtureProfile");
        await Assert.That(commandFile.Content).Contains("[CliOption(\"--region\"");
    }

    [Test]
    public async Task Command_Options_Are_Not_Dropped_Based_On_Global_Names()
    {
        var command = await new TestAwsCliScraper().Parse(["aws", "fixture", "apply"], """
            OPTIONS
                   --region (string)
                       A service-specific region value.
            GLOBAL OPTIONS
                   --profile (string)
                       A global profile.
            """);

        await Assert.That(command!.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--region"]);
    }
}
