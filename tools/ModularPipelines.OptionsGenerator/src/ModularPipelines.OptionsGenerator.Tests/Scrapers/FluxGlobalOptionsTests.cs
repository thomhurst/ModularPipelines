using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class FluxGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--as", "--as-group", "--as-uid", "--as-user-extra", "--cache-dir", "--certificate-authority",
        "--client-certificate", "--client-key", "--cluster", "--context", "--disable-compression",
        "--insecure-skip-tls-verify", "--kube-api-burst", "--kube-api-qps", "--kubeconfig", "--namespace",
        "--ns-follows-kube-context", "--server", "--timeout", "--tls-server-name", "--token", "--user", "--verbose",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Help_Preserves_Verified_Persistent_Shapes(string newline)
    {
        var globals = new TestScraper().Globals(Fixture("root").ReplaceLineEndings(newline));
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var option in globals)
        {
            var expectedType = option.SwitchName switch
            {
                "--as-group" or "--as-user-extra" => "IEnumerable<string>?",
                "--kube-api-burst" => "int?",
                "--kube-api-qps" => "double?",
                "--disable-compression" or "--insecure-skip-tls-verify" or "--ns-follows-kube-context" or "--verbose" => "bool?",
                _ => "string?",
            };
            await Assert.That(option.CSharpType).IsEqualTo(expectedType);
            await Assert.That(option.IsFlag).IsEqualTo(expectedType == "bool?");
            await Assert.That(option.IsSecret).IsEqualTo(option.SwitchName == "--token");
            await Assert.That(option.ValueSeparator).IsEqualTo(option.IsFlag ? " " : "=");
        }

        await Assert.That(globals.Single(option => option.SwitchName == "--namespace").ShortForm).IsEqualTo("-n");
    }

    [Test]
    public async Task Traversal_Preserves_Group_Settings_And_Receiver_Token_Replacements()
    {
        var scraper = new TestScraper(new HelpExecutor());
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(commands.Select(command => string.Join(' ', command.CommandParts))).IsEquivalentTo(
            new[] { "get", "get sources", "get sources git", "create", "create secret", "create secret receiver", "trigger", "trigger receiver", "bootstrap", "bootstrap github", "envsubst" });
        foreach (var command in commands)
        {
            var locals = command.Options.Where(option => GlobalSwitches.Contains(option.SwitchName)).ToArray();
            if (command.CommandParts is ["create", "secret", "receiver"] or ["trigger", "receiver"])
            {
                await Assert.That(locals.Select(option => option.SwitchName)).IsEquivalentTo(new[] { "--token" });
                await Assert.That(locals.Single().IsSecret).IsTrue();
                await Assert.That(locals.Single().Description).Contains("token");
            }
            else
            {
                await Assert.That(locals).IsEmpty();
            }
        }

        var getGit = commands.Single(command => command.CommandParts.SequenceEqual(["get", "sources", "git"]));
        await Assert.That(getGit.Options.Select(option => option.SwitchName)).IsEquivalentTo(
            new[] { "--all-namespaces", "--label-selector", "--no-header", "--status-selector", "--watch" });
        var bootstrap = commands.Single(command => command.CommandParts.SequenceEqual(["bootstrap", "github"]));
        await Assert.That(bootstrap.Options.Any(option => option.SwitchName == "--branch")).IsTrue();
        await Assert.That(bootstrap.Options.Any(option => option.SwitchName == "--token-auth")).IsTrue();
        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        foreach (var command in resolved.Commands.Where(command => command.CommandParts.Last() == "receiver"))
        {
            var token = command.Options.Single(option => option.SwitchName == "--token");
            await Assert.That(token.PropertyName).IsEqualTo("Token");
            await Assert.That(token.ShadowsGlobalOption).IsTrue();
            await Assert.That(token.GlobalOptionPropertyType).IsEqualTo("string?");
        }

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public virtual string? Token");
        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
    }

    [Test]
    public async Task Inherited_Shape_Conflicts_Are_Not_Discarded()
    {
        var scraper = new TestScraper(new HelpExecutor());
        await scraper.ScrapeAsync().ToListAsync();
        var help = Fixture("get-sources-git").Replace("--timeout duration", "--timeout int", StringComparison.Ordinal);
        await Assert.That(async () => await scraper.Parse(["flux", "get", "sources", "git"], help))
            .Throws<InvalidOperationException>().WithMessageContaining("--timeout");
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Flux", "2.9.6", name + ".txt"));

    private sealed class TestScraper(ICliCommandExecutor? executor = null) : FluxCliScraper(
        executor ?? new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<FluxCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);
        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, ParseUsageSynopsis(path, help), CancellationToken.None);
    }

    private sealed class HelpExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var path = arguments.Replace(" --help", "", StringComparison.Ordinal);
            var fixture = arguments == "--help" ? "root" : path.Replace(' ', '-');
            var help = arguments == "--version" ? "flux version 2.9.6" : Fixture(fixture);
            string[]? children = fixture switch
            {
                "root" => ["get", "create", "trigger", "bootstrap", "envsubst"],
                "get" => ["sources"],
                "get-sources" => ["git"],
                "create" => ["secret"],
                "create-secret" => ["receiver"],
                "bootstrap" => ["github"],
                _ => null,
            };
            if (children is not null)
            {
                var start = help.IndexOf("Available Commands:", StringComparison.Ordinal);
                var end = help.IndexOf("Flags:", start, StringComparison.Ordinal);
                help = help[..start] + "Available Commands:\n" + string.Join('\n', children.Select(child => $"  {child}   Fixture command")) + "\n\n" + help[end..];
            }

            return Task.FromResult(new CliCommandResult { StandardOutput = help, StandardError = string.Empty, ExitCode = 0 });
        }
    }
}
