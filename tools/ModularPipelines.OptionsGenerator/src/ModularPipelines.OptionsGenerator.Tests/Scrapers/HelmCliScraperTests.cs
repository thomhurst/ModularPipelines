using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class HelmCliScraperTests
{
    [Test]
    public async Task Uses_Version_Subcommand_For_Availability_And_Version()
    {
        var executor = new RecordingExecutor();
        var scraper = new HelmCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<HelmCliScraper>.Instance);

        var isAvailable = await scraper.IsAvailableAsync();
        var version = await scraper.GetVersionAsync();

        await Assert.That(isAvailable).IsTrue();
        await Assert.That(version).IsEqualTo("version.BuildInfo{Version:\"v4.0.0\"}");
        await Assert.That(executor.Arguments).IsEquivalentTo(["version", "version"]);
    }

    [Test]
    public async Task Marks_Install_Chart_Required_While_Name_Remains_Conditional()
    {
        var scraper = new HelmCliScraper(
            new HelmHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<HelmCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var arguments = commands.Single(command => command.FullCommand == "helm install")
            .PositionalArguments;
        var name = arguments.Single(argument => argument.PropertyName == "Name");
        var chart = arguments.Single(argument => argument.PropertyName == "Chart");

        using (Assert.Multiple())
        {
            await Assert.That(name.IsRequired).IsFalse();
            await Assert.That(name.CSharpType).IsEqualTo("string?");
            await Assert.That(chart.IsRequired).IsTrue();
            await Assert.That(chart.CSharpType).IsEqualTo("string");
        }
    }

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments.Add(arguments);
            var success = arguments == "version";
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? "version.BuildInfo{Version:\"v4.0.0\"}" : string.Empty,
                StandardError = success ? string.Empty : "Error: unknown flag: --version",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);
    }

    private sealed class HelmHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "version" => "version.BuildInfo{Version:\"v4.0.0\"}",
                "--help" => """
                    The Kubernetes package manager.

                    Usage:
                      helm [command]

                    Available Commands:
                      install     install a chart

                    Flags:
                      -h, --help   help for helm
                    """,
                "install --help" => """
                    This command installs a chart archive.

                    Usage:
                      helm install [NAME] [CHART] [flags]

                    Flags:
                      -g, --generate-name   generate the name
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
}
