using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;
using System.Text.Json;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AwsCliScraperTests
{
    [Test]
    public async Task Extracts_Services_From_Aws_2_36_29_Help_Fixture()
    {
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "AwsCli",
            "aws-2.36.29-root-help.json");
        using var fixture = JsonDocument.Parse(await File.ReadAllTextAsync(fixturePath));
        var helpText = fixture.RootElement.GetProperty("help").GetString()!;

        var scraper = new AwsCliScraper(
            new AwsFixtureExecutor(helpText),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["aws accessanalyzer", "aws cloudformation", "aws ec2"]);
    }

    [Test]
    public async Task Scrape_Normalizes_Ansi_Formatted_Section_Headers()
    {
        var scraper = new AwsCliScraper(
            new AwsHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["aws ec2 describe-instances"]);
    }

    [Test]
    public async Task Enum_Detection_Deduplicates_Case_Variant_Values()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "TrafficRoutingConfig",
            "AwsDeployCreateDeploymentConfigOptions",
            "Possible values: TimeBasedCanary TimeBasedLinear AllAtOnce timeBasedCanary timeBasedLinear");
        var values = definition!.Values;

        using (Assert.Multiple())
        {
            await Assert.That(values.Select(value => value.CliValue))
                .IsEquivalentTo(["TimeBasedCanary", "TimeBasedLinear", "AllAtOnce"]);
            await Assert.That(values.Select(value => value.MemberName).Distinct().Count())
                .IsEqualTo(values.Count);
        }
    }

    private sealed class AwsHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "\u001b[1mAVAILABLE SERVICES\u001b[0m\n       o ec2",
                "ec2 help" => "\u001b[1mAVAILABLE COMMANDS\u001b[0m\n       o describe-instances",
                "ec2 describe-instances help" => """
                    DESCRIPTION
                           Describes EC2 instances.

                    OPTIONS
                           --instance-ids (list)
                           Instance identifiers.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsFixtureExecutor(string rootHelp) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "help"
                    ? rootHelp
                    : "OPTIONS\n       --enabled (boolean)\n\n       Enable the command.\n",
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
