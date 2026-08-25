using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KubectlCliScraperTests
{
    [Test]
    public async Task Uses_Version_Subcommand_For_Availability_And_Version()
    {
        var executor = new RecordingExecutor();
        var scraper = new KubectlCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KubectlCliScraper>.Instance);

        var isAvailable = await scraper.IsAvailableAsync();
        var version = await scraper.GetVersionAsync();

        await Assert.That(isAvailable).IsTrue();
        await Assert.That(version).IsEqualTo("Client Version: v1.35.0");
        await Assert.That(executor.AvailabilityArguments).IsEquivalentTo(["version --client"]);
        await Assert.That(executor.Arguments).IsEquivalentTo(["version --client"]);
    }

    [Test]
    [Arguments("diff", "Usage:\n  kubectl diff -f FILENAME", "-f, --filename stringArray   Files to diff.")]
    [Arguments("attach", "Usage:\n  kubectl attach POD -c CONTAINER", "-c, --container string   Container name.")]
    public async Task Option_Values_Are_Not_Emitted_As_Positionals(
        string commandName,
        string usage,
        string option)
    {
        var helpText = $"{usage}\n\nOptions:\n  {option}";
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", commandName],
            helpText);

        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .DoesNotContain("Filename")
            .And.DoesNotContain("Container");
    }

    [Test]
    public async Task Port_Forward_Numbered_Repeat_Is_One_Collection()
    {
        const string helpText = """
            Usage:
              kubectl port-forward TYPE/NAME [options] [LOCAL_PORT:]REMOTE_PORT [...[LOCAL_PORT_N:]REMOTE_PORT_N]
            """;
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "port-forward"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["TypeOrName", "LocalPortRemotePort"]);
            await Assert.That(command.PositionalArguments.Single(argument =>
                argument.PropertyName == "LocalPortRemotePort").IsVariadic).IsTrue();
        }
    }

    private sealed class TestKubectlCliScraper()
        : KubectlCliScraper(
            new RecordingExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KubectlCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public List<string> AvailabilityArguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments.Add(arguments);
            var success = arguments == "version --client";
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? "Client Version: v1.35.0" : string.Empty,
                StandardError = success ? string.Empty : "error: unknown flag: --version",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> IsAvailableAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default)
        {
            AvailabilityArguments.Add(arguments);
            return Task.FromResult(arguments == "version --client");
        }
    }
}
