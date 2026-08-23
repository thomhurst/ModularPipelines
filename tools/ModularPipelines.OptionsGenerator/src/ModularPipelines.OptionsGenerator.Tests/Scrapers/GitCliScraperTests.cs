using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GitCliScraperTests
{
    [Test]
    public async Task Tool_Definition_Preserves_The_Hand_Written_Grouped_Facade()
    {
        var scraper = new GitCliScraper(
            new StubExecutor(),
            NullLogger<GitCliScraper>.Instance);

        var tool = scraper.CreateToolDefinition();

        await Assert.That(tool.GenerateCommandFacade).IsFalse();
        await Assert.That(tool.GenerateCode).IsFalse();
        await Assert.That(tool.DocumentationOutputDirectory).IsNull();
    }

    [Test]
    public async Task ScrapeAsync_Uses_Stdout_Help_When_Stderr_Is_Empty()
    {
        var scraper = new GitCliScraper(
            new StdoutHelpExecutor(),
            NullLogger<GitCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        using (Assert.Multiple())
        {
            await Assert.That(commands).HasSingleItem();
            await Assert.That(commands[0].FullCommand).IsEqualTo("git switch");
            await Assert.That(commands[0].Options.Single().SwitchName).IsEqualTo("--force");
        }
    }

    [Test]
    public async Task ExtractSubcommands_Skips_Global_Options_Before_Command_Name()
    {
        const string help = """
                            usage: git remote [-v | --verbose]
                               or: git remote [-v | --verbose] show [-n] <name>
                               or: git remote get-url [--push] [--all] <name>
                               or: git remote set-url [--push] <name> <newurl>
                            """;

        var subcommands = GitCliScraper.ExtractSubcommands("remote", help);

        await Assert.That(subcommands).IsEquivalentTo(["get-url", "set-url", "show"]);
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    private sealed class StdoutHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(arguments switch
            {
                "help -a" => Result(
                    "Main Porcelain Commands\n   switch                  Switch branches"),
                "switch -h" => Result(
                    "usage: git switch [<options>] [<branch>]\n    -f, --force            force checkout",
                    exitCode: 129),
                _ => Result(string.Empty, "unexpected invocation", exitCode: 1),
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        private static CliCommandResult Result(
            string standardOutput,
            string standardError = "",
            int exitCode = 0) =>
            new()
            {
                StandardOutput = standardOutput,
                StandardError = standardError,
                ExitCode = exitCode,
            };
    }
}
