using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GoCliScraperTests
{
    [Test]
    public async Task Traverses_Root_And_Nested_Commands_Including_Optionless_Leaves()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    bug         start a bug report
                    mod         module maintenance
                    version     print Go version

                Use "go help <command>" for more information about a command.
                """,
            ["help bug"] = "usage: go bug\n\nBug starts a bug report.",
            ["help version"] = "usage: go version [-json] [file ...]\n\nVersion reports build information.",
            ["help mod"] = """
                Usage:
                    go mod <command> [arguments]

                The commands are:
                    graph       print module requirement graph
                    verify      verify dependencies have expected content

                Use "go help mod <command>" for more information about a command.
                """,
            ["help mod graph"] = "usage: go mod graph [-go=version] [-x]\n\nGraph prints the module graph.",
            ["help mod verify"] = "usage: go mod verify\n\nVerify checks cached dependencies.",
        });

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo([
                "go bug",
                "go mod",
                "go mod graph",
                "go mod verify",
                "go version",
            ]);
    }

    [Test]
    public async Task Expands_Shared_Build_Flags_For_Affected_Commands()
    {
        const string buildHelp = """
            usage: go build [-o output] [build flags] [packages]

            The build flags are shared by the build, clean, get, install, list, run,
            and test commands:

                -race
                    enable data race detection.
                -tags tag,list
                    a comma-separated list of build tags.
                -ldflags '[pattern=]arg list'
                    arguments to pass to the linker.
                -mod mode
                    module download mode.
                -trimpath
                    remove file system paths.
            """;
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages and dependencies
                    clean       remove object files
                    fix         apply fixes

                Use "go help <command>" for more information about a command.
                """,
            ["help build"] = buildHelp,
            ["help clean"] = "usage: go clean [-cache]\n\nClean removes cached files.",
            ["help fix"] = "usage: go fix [build flags] [-fixtool prog] [packages]\n\nFix applies fixes.",
        });

        var commands = await ScrapeAsync(scraper);
        var fix = commands.Single(command => command.FullCommand == "go fix");
        var clean = commands.Single(command => command.FullCommand == "go clean");

        using (Assert.Multiple())
        {
            var switchNames = fix.Options.Select(option => option.SwitchName).ToHashSet();
            await Assert.That(new[] { "-race", "-tags", "-ldflags", "-mod", "-trimpath" }
                    .All(switchNames.Contains))
                .IsTrue();
            await Assert.That(fix.Options.Single(option => option.SwitchName == "-race").IsFlag)
                .IsTrue();
            await Assert.That(fix.Options.Single(option => option.SwitchName == "-tags").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(clean.Options.Any(option => option.SwitchName == "-race"))
                .IsTrue();
        }
    }

    [Test]
    public async Task Parses_Repeatable_Edit_Operations_In_Documented_Order()
    {
        const string helpText = """
            usage: go mod edit [editing flags] [-fmt|-print|-json] [go.mod]

            The -module flag changes the module path.

            The -require=path@version and -droprequire=path flags
            add and drop a requirement.

            The -replace=old[@v]=new[@v] and -dropreplace=old[@v] flags
            add and drop a replacement.

            The -require, -droprequire, -replace, and -dropreplace editing flags may be repeated,
            and the changes are applied in the order given.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "mod", "edit"], helpText);

        using (Assert.Multiple())
        {
            var switchNames = command!.Options.Select(option => option.SwitchName).ToHashSet();
            await Assert.That(new[] { "-module", "-require", "-droprequire", "-replace", "-dropreplace" }
                    .All(switchNames.Contains))
                .IsTrue();
            await Assert.That(command.Options
                    .Where(option => option.SwitchName is "-require" or "-droprequire" or "-replace" or "-dropreplace")
                    .All(option => option.AcceptsMultipleValues && option.CSharpType == "IEnumerable<string>?"))
                .IsTrue();
        }
    }

    [Test]
    public async Task Parses_Fmt_Mod_Flag_From_Prose()
    {
        const string helpText = """
            usage: go fmt [-n] [-x] [packages]

            The -n flag prints commands that would be executed.
            The -x flag prints commands as they are executed.

            The -mod flag's value sets which module download mode to use.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "fmt"], helpText);

        var option = command!.Options.Single(item => item.SwitchName == "-mod");
        await Assert.That(option.IsFlag).IsFalse();
    }

    private static TestGoCliScraper CreateScraper(IReadOnlyDictionary<string, string> helpByArguments) =>
        new(new StubExecutor(helpByArguments));

    private static async Task<IReadOnlyList<CliCommandDefinition>> ScrapeAsync(ICliScraper scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class TestGoCliScraper(ICliCommandExecutor executor)
        : GoCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<GoCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class StubExecutor(IReadOnlyDictionary<string, string> helpByArguments)
        : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            if (!helpByArguments.TryGetValue(arguments, out var helpText))
            {
                throw new InvalidOperationException($"Unexpected arguments: {arguments}");
            }

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
