using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ArgoCdGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--argocd-context", "--auth-token", "--client-crt", "--client-crt-key", "--config",
        "--controller-name", "--core", "--grpc-web", "--grpc-web-root-path", "--header",
        "--http-retry-max", "--insecure", "--kube-context", "--logformat", "--loglevel",
        "--plaintext", "--port-forward", "--port-forward-namespace", "--prompts-enabled",
        "--redis-compress", "--redis-haproxy-name", "--redis-name", "--repo-server-name",
        "--server", "--server-crt", "--server-name",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Help_Preserves_Persistent_Metadata(string newline)
    {
        var globals = new TestScraper().Globals(Fixture("root-help").ReplaceLineEndings(newline));
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var header = globals.Single(option => option.SwitchName == "--header");
        await Assert.That(header.ShortForm).IsEqualTo("-H");
        await Assert.That(header.AcceptsMultipleValues).IsTrue();
        await Assert.That(header.IsSecret).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--auth-token").IsSecret).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--client-crt-key").IsSecret).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "--http-retry-max").CSharpType).IsEqualTo("int?");
        foreach (var name in new[] { "--core", "--grpc-web", "--insecure", "--plaintext", "--port-forward", "--prompts-enabled" })
        {
            var option = globals.Single(option => option.SwitchName == name);
            await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        }
    }

    [Test]
    public async Task Full_Traversal_Preserves_Command_And_Group_Overrides()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition();
        await Assert.That(commands).Count().IsEqualTo(165);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var app = commands.Single(command => command.FullCommand == "argocd app get");
        await Assert.That(app.Options.Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        await Assert.That(app.Options.Any(option => option.SwitchName == "--app-namespace")).IsTrue();
        foreach (var name in new[] { "argocd admin import", "argocd admin settings rbac can" })
        {
            var server = commands.Single(command => command.FullCommand == name)
                .Options.Single(option => option.SwitchName == "--server");
            await Assert.That(server.Description).Contains("Kubernetes API server");
        }
        var configure = commands.Single(command => command.FullCommand == "argocd configure");
        await Assert.That(configure.Options.Single(option => option.SwitchName == "--prompts-enabled").Description)
            .IsEqualTo("Enable (or disable) optional interactive prompts");
        await Assert.That(commands.Single(command => command.FullCommand == "argocd admin cluster shards")
            .Options.Single(option => option.SwitchName == "--redis-compress").Description)
            .Contains("compression for data sent to Redis");
        await Assert.That(commands.Single(command => command.FullCommand == "argocd login")
            .PositionalArguments.Single().PropertyName).IsEqualTo("Server");
    }

    [Test]
    public async Task Conflicting_Inherited_Metadata_Fails_Instead_Of_Dropping_Command()
    {
        var scraper = new TestScraper(new HelpExecutor(conflictingRetryType: true));
        await Assert.That(async () => await scraper.ScrapeAsync().ToListAsync())
            .Throws<InvalidOperationException>().WithMessageContaining("conflicting scraped and supplemental definitions");
    }

    private static string Fixture(string name) => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "ArgoCd", "3.5.3", name + ".txt"));

    private sealed class TestScraper(ICliCommandExecutor? executor = null) : ArgoCdCliScraper(
        executor ?? new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<ArgoCdCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);
    }

    private sealed class HelpExecutor(bool conflictingRetryType = false) : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var fixture = arguments switch
            {
                "--help" => "root-help",
                "version --client" => "version",
                _ when arguments.EndsWith(" --help", StringComparison.Ordinal) =>
                    arguments[..^7].Replace(' ', '-') + "-help",
                _ => throw new InvalidOperationException($"Unexpected help command: {arguments}"),
            };
            var output = Fixture(fixture);
            if (conflictingRetryType && fixture == "app-get-help")
            {
                output = output.Replace("--http-retry-max int", "--http-retry-max string", StringComparison.Ordinal);
            }
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }
    }
}
