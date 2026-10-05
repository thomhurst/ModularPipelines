using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PodmanGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--cdi-spec-dir", "--cgroup-manager", "--config", "--conmon", "--connection",
        "--events-backend", "--hooks-dir", "--identity", "--imagestore", "--log-level",
        "--module", "--network-config-dir", "--out", "--remote", "--root", "--runroot",
        "--runtime", "--runtime-flag", "--ssh", "--storage-driver", "--storage-opt",
        "--syslog", "--tls-ca", "--tls-cert", "--tls-details", "--tls-key", "--tmpdir",
        "--transient-store", "--url", "--volumepath",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Settings_Preserve_Types_Aliases_And_Explicit_False(string newline)
    {
        var options = new TestScraper().Globals(Fixture("root-help").ReplaceLineEndings(newline));
        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var option in options)
        {
            var boolean = option.SwitchName is "--remote" or "--syslog" or "--transient-store";
            var repeated = option.SwitchName is "--cdi-spec-dir" or "--hooks-dir" or "--module"
                or "--runtime-flag" or "--storage-opt";
            var expectedType = (option.SwitchName, boolean, repeated) switch
            {
                (_, true, _) => "bool?",
                (_, _, true) => "IEnumerable<string>?",
                ("--log-level", _, _) => "PodmanLogLevel?",
                _ => "string?",
            };
            await Assert.That(option.CSharpType).IsEqualTo(expectedType);
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.ValueArity).IsEqualTo(boolean
                ? CliOptionValueArity.Optional : CliOptionValueArity.Required);
            if (boolean)
            {
                await Assert.That(option.ValueSeparator).IsEqualTo("=");
            }

            if (repeated)
            {
                await Assert.That(option.CollectionSeparator).IsNull();
                await Assert.That(option.GroupValues).IsFalse();
            }
        }

        await Assert.That(options.Single(option => option.SwitchName == "--connection").ShortForm).IsEqualTo("-c");
        await Assert.That(options.Single(option => option.SwitchName == "--remote").ShortForm).IsEqualTo("-r");
        await Assert.That(options.Single(option => option.SwitchName == "--tmpdir").Description).Contains("TMPDIR");
    }

    [Test]
    public async Task Full_Traversal_Preserves_Local_Identity_And_Compose_Provider_Scope()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = InheritedPropertyCollisionResolver.Resolve(scraper.CreateToolDefinition() with { Commands = commands });
        await Assert.That(commands).Count().IsEqualTo(258);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var rootIdentity = tool.GlobalOptions.Single(option => option.SwitchName == "--identity");
        var localIdentity = tool.Commands.Single(command => command.FullCommand == "podman system connection add")
            .Options.Single(option => option.SwitchName == "--identity");
        await Assert.That(localIdentity.PropertyName).IsNotEqualTo(rootIdentity.PropertyName);
        await Assert.That(localIdentity.ShadowsGlobalOption).IsFalse();
        var compose = tool.Commands.Single(command => command.FullCommand == "podman compose");
        await Assert.That(compose.Options.Select(option => option.SwitchName)).Contains("--project-name");
        await Assert.That(compose.Options.Select(option => option.SwitchName)).Contains("--file");
        await Assert.That(tool.GlobalOptions.Any(option => option.SwitchName == "--project-name")).IsFalse();
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Podman", "6.1.3", name + ".txt"));

    private sealed class TestScraper() : PodmanCliScraper(
        new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<PodmanCliScraper>.Instance)
    {
        protected override string? ComposeProviderPath => "fixture-compose";

        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);
    }

    private sealed class HelpExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var fixture = arguments switch
            {
                "--help" => "root-help",
                "--version" => "version",
                _ when arguments.EndsWith(" --help", StringComparison.Ordinal) =>
                    arguments[..^7].Replace(' ', '-') + "-help",
                _ => throw new InvalidOperationException($"Unexpected help command: {command} {arguments}"),
            };
            if (command == "fixture-compose")
            {
                fixture = fixture == "root-help" ? "compose-help" : "compose-" + fixture;
            }

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = Fixture(fixture),
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }
    }
}
