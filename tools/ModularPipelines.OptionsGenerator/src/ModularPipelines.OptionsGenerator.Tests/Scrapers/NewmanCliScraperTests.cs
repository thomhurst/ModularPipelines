using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NewmanCliScraperTests
{
    [Test]
    public async Task Captured_Traversal_Keeps_All_Run_Options_Local()
    {
        var fixtureDirectory = Path.Combine(AppContext.BaseDirectory, "Fixtures");
        var root = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory, "newman-6.2.2-root-help.txt"));
        var run = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory, "newman-6.2.2-run-help.txt"));
        var scraper = new NewmanCliScraper(new FixtureExecutor(root, run),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<NewmanCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands).Count().IsEqualTo(1);
        await Assert.That(commands[0].FullCommand).IsEqualTo("newman run");
        await Assert.That(commands[0].Options).Count().IsEqualTo(33);
        await Assert.That(commands[0].PositionalArguments.Single().Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions).IsEmpty();
    }

    [Test]
    [Arguments("--environment", "string?")]
    [Arguments("--globals", "string?")]
    [Arguments("--iteration-count", "int?")]
    [Arguments("--iteration-data", "string?")]
    [Arguments("--export-environment", "string?")]
    [Arguments("--export-globals", "string?")]
    [Arguments("--export-collection", "string?")]
    [Arguments("--color", "string?")]
    [Arguments("--working-dir", "string?")]
    [Arguments("--ssl-client-cert-list", "string?")]
    [Arguments("--ssl-client-cert", "string?")]
    [Arguments("--ssl-client-key", "string?")]
    [Arguments("--ssl-extra-ca-certs", "string?")]
    [Arguments("--cookie-jar", "string?")]
    [Arguments("--export-cookie-jar", "string?")]
    public async Task Angle_Bracket_Values_Require_One_Typed_Value(string name, string type)
    {
        var option = (await ParseCapturedRun()).Options.Single(option => option.SwitchName == name);

        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        await Assert.That(option.PropertyType).IsEqualTo(type);
    }

    [Test]
    [Arguments("--reporters")]
    [Arguments("--bail")]
    [Arguments("--delay-request")]
    [Arguments("--timeout")]
    [Arguments("--timeout-request")]
    [Arguments("--timeout-script")]
    public async Task Bracketed_Values_Support_Both_Bare_And_Valued_Forms(string name)
    {
        var command = await ParseCapturedRun();
        var option = command.Options.Single(option => option.SwitchName == name);

        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        await Assert.That(option.Description!).DoesNotStartWith("[");
    }

    [Test]
    public async Task Spaced_Short_Alias_Is_Preserved()
    {
        var command = await ParseCapturedRun();
        var option = command.Options.Single(option => option.SwitchName == "--suppress-exit-code");

        await Assert.That(option.IsFlag).IsTrue();
        await Assert.That(option.ShortForm).IsEqualTo("-x");
    }

    [Test]
    public async Task Wrapped_Descriptions_Retain_Repeatability_And_Defaults()
    {
        var command = await ParseCapturedRun();
        var folder = command.Options.Single(option => option.SwitchName == "--folder");

        await Assert.That(folder.Description!).Contains("Can be specified multiple times to run multiple folders");
        await Assert.That(folder.AcceptsMultipleValues).IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--reporters").Description!)
            .Contains("(default: [\"cli\"])");
    }

    [Test]
    [Arguments("--global-var")]
    [Arguments("--env-var")]
    public async Task Variable_Options_Repeat_Required_Values(string name)
    {
        var option = (await ParseCapturedRun()).Options.Single(option => option.SwitchName == name);

        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    [Arguments("--postman-api-key")]
    [Arguments("--ssl-client-passphrase")]
    public async Task Credential_Values_Remain_Secret(string name)
    {
        var option = (await ParseCapturedRun()).Options.Single(option => option.SwitchName == name);

        await Assert.That(option.IsSecret).IsTrue();
        await Assert.That(option.IsFlag).IsFalse();
    }

    private static async Task<CliCommandDefinition> ParseCapturedRun()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "newman-6.2.2-run-help.txt"));
        return (await new TestScraper().Parse(help))!;
    }

    private sealed class TestScraper() : NewmanCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<NewmanCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(
            ["newman", "run"], help, ParseUsageSynopsis(["newman", "run"], help), CancellationToken.None);
    }

    private sealed class FixtureExecutor(string root, string run) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => root,
                    "run --help" => run,
                    _ => throw new InvalidOperationException($"Unexpected Newman invocation: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
