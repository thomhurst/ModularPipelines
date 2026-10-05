using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class MinikubeGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--add_dir_header", "--alsologtostderr", "--alsologtostderrthreshold", "--bootstrapper",
        "--legacy_stderr_threshold_behavior", "--log_backtrace_at", "--log_dir", "--log_file",
        "--log_file_max_size", "--logtostderr", "--one_output", "--profile", "--rootless",
        "--skip-audit", "--skip_headers", "--skip_log_headers", "--stderrthreshold", "--user", "--v", "--vmodule",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Options_Help_Preserves_Persistent_Types_And_Aliases(string newline)
    {
        var options = new TestScraper().Globals(("Global Flags:\n" + Fixture("options")).ReplaceLineEndings(newline));
        await Assert.That(GlobalSwitches.Except(options.Select(option => option.SwitchName))).IsEmpty();
        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var option in options)
        {
            var expectedType = option.SwitchName switch
            {
                "--add_dir_header" or "--alsologtostderr" or "--legacy_stderr_threshold_behavior"
                    or "--logtostderr" or "--one_output" or "--rootless" or "--skip-audit"
                    or "--skip_headers" or "--skip_log_headers" => "bool?",
                "--v" => "int?",
                "--log_file_max_size" => "ulong?",
                _ => "string?",
            };
            await Assert.That(option.CSharpType).IsEqualTo(expectedType);
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
            await Assert.That(option.ValueArity).IsEqualTo(expectedType == "bool?"
                ? CliOptionValueArity.Optional : CliOptionValueArity.Required);
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }

        await Assert.That(options.Single(option => option.SwitchName == "--profile").ShortForm).IsEqualTo("-p");
        await Assert.That(options.Single(option => option.SwitchName == "--bootstrapper").ShortForm).IsEqualTo("-b");
        await Assert.That(options.Single(option => option.SwitchName == "--v").ShortForm).IsEqualTo("-v");
        await Assert.That(options.Single(option => option.SwitchName == "--log_backtrace_at").Description)
            .IsEqualTo("when logging hits line file:N, emit a stack trace");
    }

    [Test]
    public async Task Full_Help_Traversal_Loads_Globals_Once_And_Preserves_Command_Scopes()
    {
        var executor = new HelpExecutor();
        var scraper = new TestScraper(executor);
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var globals = scraper.CreateToolDefinition().GlobalOptions;

        await Assert.That(commands).Count().IsEqualTo(48);
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(executor.OptionsCalls).IsEqualTo(1);
        await Assert.That(commands.SelectMany(command => command.Options)
            .Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        await Assert.That(commands.Single(command => command.FullCommand == "minikube status")
            .Options.Any(option => option.SwitchName == "--output")).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "minikube start")
            .Options.Any(option => option.SwitchName == "--driver")).IsTrue();
        foreach (var commandName in new[] { "minikube status", "minikube config view" })
        {
            var format = commands.Single(command => command.FullCommand == commandName)
                .Options.Single(option => option.SwitchName == "--format");
            await Assert.That(format.CSharpType).IsEqualTo("string?");
            await Assert.That(format.IsFlag).IsFalse();
            await Assert.That(format.ShortForm).IsEqualTo(commandName == "minikube status" ? "-f" : null);
            await Assert.That(format.Description!.StartsWith("Go template format string", StringComparison.Ordinal)).IsTrue();
        }
        await Assert.That(commands.Any(command => command.FullCommand == "minikube version")).IsFalse();
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(0, true)]
    public async Task Missing_Or_Failed_Global_Help_Does_Not_Produce_A_Partial_Surface(int exitCode, bool missing)
    {
        var scraper = new TestScraper(new HelpExecutor(exitCode, missing));
        await Assert.That(async () => await scraper.ScrapeAsync().ToListAsync())
            .Throws<InvalidOperationException>().WithMessageContaining("global option help is unavailable");
    }

    [Test]
    [Arguments(null)]
    [Arguments("--profile")]
    [Arguments("--vmodule")]
    public async Task Incomplete_Global_Help_Does_Not_Produce_A_Partial_Surface(string? missingSwitch)
    {
        var help = missingSwitch is null
            ? "The following options can be passed to any command:\n"
            : string.Join('\n', Fixture("options").Split('\n')
                .Where(line => !line.Contains(missingSwitch + "=", StringComparison.Ordinal)));
        var scraper = new TestScraper(new HelpExecutor(optionsOutput: help));
        await Assert.That(async () => await scraper.ScrapeAsync().ToListAsync())
            .Throws<InvalidOperationException>().WithMessageContaining("global option help is incomplete");
    }

    [Test]
    public async Task Additional_Persistent_Settings_Are_Not_Blocked_By_Completeness_Check()
    {
        var scraper = new TestScraper(new HelpExecutor(optionsOutput:
            Fixture("options") + "\n    --future-setting='': Future persistent setting\n"));
        await scraper.ScrapeAsync().ToListAsync();
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Select(option => option.SwitchName))
            .Contains("--future-setting");
    }

    [Test]
    [Arguments("--profile='minikube': Profile", false)]
    [Arguments("--profile=1: Profile", true)]
    public async Task Inherited_Duplicates_Are_Validated_Before_Removal(string declaration, bool conflicting)
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var help = "Status.\n\nOptions:\n    -p, " + declaration + "\n\nUsage:\n  minikube status [flags]\n";
        if (conflicting)
        {
            await Assert.That(async () => await scraper.Parse(help)).Throws<InvalidOperationException>();
        }
        else
        {
            var command = await scraper.Parse(help);
            await Assert.That(command!.Options).IsEmpty();
        }
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Minikube", "1.39.0", name + ".txt"));

    private sealed class TestScraper(ICliCommandExecutor? executor = null) : MinikubeCliScraper(
        executor ?? new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<MinikubeCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string help) =>
            ParseCommandAsync(["minikube", "status"], help,
                ParseUsageSynopsis(["minikube", "status"], help), CancellationToken.None);
    }

    private sealed class HelpExecutor(int optionsExitCode = 0, bool missingOptions = false, string? optionsOutput = null) : ICliCommandExecutor
    {
        private int _optionsCalls;
        public int OptionsCalls => _optionsCalls;

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var fixture = arguments switch
            {
                "--help" => "root-help",
                "version --short" => "version",
                "options" or "options --help" => "options",
                _ when arguments.EndsWith(" --help", StringComparison.Ordinal) =>
                    arguments[..^7].Replace(' ', '-') + "-help",
                _ => throw new InvalidOperationException($"Unexpected help command: {arguments}"),
            };
            if (fixture == "options")
            {
                Interlocked.Increment(ref _optionsCalls);
            }

            var output = fixture == "options" ? optionsOutput ?? Fixture(fixture) : Fixture(fixture);
            if (fixture == "options" && missingOptions)
            {
                output = string.Empty;
            }

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = fixture == "options" ? optionsExitCode : 0,
            });
        }
    }
}
