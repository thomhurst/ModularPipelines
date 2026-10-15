using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NodePackageManagerGlobalOptionsTests
{
    [Test]
    public async Task Yarn_Berry_Only_Inherits_Leading_Cwd_Not_Install_Options()
    {
        var tool = await Scrape("yarn");
        var cwd = tool.GetGlobalOptions().Single();
        await Assert.That(cwd.SwitchName).IsEqualTo("--cwd");
        await Assert.That(cwd.CSharpType).IsEqualTo("string?");
        await Assert.That(cwd.IsFlag).IsFalse();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        await Assert.That(tool.Commands.Single().Options.Any(option => option.SwitchName == "--immutable")).IsTrue();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("[CliGlobalOptions]");
        await Assert.That(generated).Contains("public virtual string? Cwd");
        await Assert.That(generated).DoesNotContain("Immutable");
        await Assert.That(generated).DoesNotContain("Json");
    }

    [Test]
    public async Task Pnpm_Uses_Root_Common_Options_Without_Workspace_Operations()
    {
        var tool = await Scrape("pnpm");
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Single(option => option.SwitchName == "--dir").ShortForm).IsEqualTo("-C");
        await Assert.That(globals.Single(option => option.SwitchName == "--dir").CSharpType).IsEqualTo("string?");
        await Assert.That(globals.Single(option => option.SwitchName == "--workspace-root").ShortForm).IsEqualTo("-w");
        await Assert.That(globals.Single(option => option.SwitchName == "--ignore-workspace").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--color").ValueArity)
            .IsEqualTo(ModularPipelines.Attributes.CliOptionValueArity.Optional);
        await Assert.That(globals.Single(option => option.SwitchName == "--workspace-packages").AcceptsMultipleValues).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--npmrc-auth-file").Aliases).Contains("--userconfig");
        await Assert.That(globals.Single(option => option.SwitchName == "--https-proxy").IsSecret).IsTrue();
        await Assert.That(globals.Any(option => option.SwitchName is "--recursive" or "--filter" or "--parallel" or "--help" or "--version" or "--stream")).IsFalse();
        var command = tool.Commands.Single();
        await Assert.That(command.Options.Any(option => option.SwitchName == "--dir")).IsFalse();
        await Assert.That(command.Options.Any(option => option.SwitchName == "--reporter")).IsFalse();
        await Assert.That(command.Options.Any(option => option.SwitchName == "--frozen-lockfile")).IsTrue();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
    }

    [Test]
    public async Task Npm_Inherits_Execution_And_Transport_Config_Not_Install_Settings()
    {
        var tool = await Scrape("npm");
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Single(option => option.SwitchName == "--prefix").ShortForm).IsEqualTo("-C");
        await Assert.That(globals.Single(option => option.SwitchName == "--logs-max").CSharpType).IsEqualTo("int?");
        await Assert.That(globals.Single(option => option.SwitchName == "--timing").CSharpType).IsEqualTo("bool?");
        await Assert.That(globals.Single(option => option.SwitchName == "--timing").IsFlag).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "--color").CSharpType).IsEqualTo("string?");
        await Assert.That(globals.Single(option => option.SwitchName == "--ca").AcceptsMultipleValues).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--https-proxy").IsSecret).IsTrue();
        await Assert.That(globals.Any(option => option.SwitchName is "--save-dev" or "--global" or "--workspace" or "--otp" or "--version" or "--help")).IsFalse();
        await Assert.That(tool.Commands.Single().Options.Any(option => option.SwitchName == "--save-dev")).IsTrue();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("[SecretValue]");
        await Assert.That(generated).Contains("[CliOption(\"--timing\", Format = OptionFormat.EqualsSeparated)]");
    }

    [Test]
    public async Task Npm_Supplemental_Shapes_Match_Installed_Public_Config_Definitions()
    {
        var globals = (await Scrape("npm")).GetGlobalOptions();
        using var metadata = JsonDocument.Parse(await Fixture("Npm", "11.11.0", "npm-shared-config.json"));
        await Assert.That(globals.Select(option => option.SwitchName[2..]))
            .IsEquivalentTo(metadata.RootElement.EnumerateObject().Select(property => property.Name));
        foreach (var option in globals)
        {
            var definition = metadata.RootElement.GetProperty(option.SwitchName[2..]);
            var type = definition.GetProperty("type");
            if (type.ValueKind == JsonValueKind.String)
            {
                await Assert.That(option.CSharpType).IsEqualTo(type.GetString() switch
                {
                    "Boolean" => "bool?",
                    "Number" => option.SwitchName == "--fetch-retry-factor" ? "double?" : "int?",
                    _ => "string?",
                });
            }
            else
            {
                await Assert.That(option.AcceptsMultipleValues)
                    .IsEqualTo(type.EnumerateArray().Any(value => value.ValueKind == JsonValueKind.String && value.GetString() == "Array"));
            }

            if (definition.TryGetProperty("short", out var shortForm))
            {
                await Assert.That(option.ShortForm).IsEqualTo("-" + shortForm.GetString());
            }
        }
    }

    [Test]
    public async Task Pnpm_Command_Override_Retains_Repeatability_And_Arity()
    {
        var tool = await Scrape("pnpm", "Usage: pnpm install [OPTIONS]\n\nOptions:\n      --dir <DIR>...\n          Repeat directory values\n");
        await Assert.That(tool.GetGlobalOptions().Single(option => option.SwitchName == "--dir").AcceptsMultipleValues).IsFalse();
        await Assert.That(tool.Commands.Single().Options.Single().AcceptsMultipleValues).IsTrue();
        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        var generated = (await new OptionsClassGenerator().GenerateAsync(resolved)).Single().Content;
        await Assert.That(generated).Contains("new IEnumerable<string>? Dir");
    }

    [Test]
    public async Task Pnpm_Command_With_Different_Reporter_Choices_Keeps_Its_Enum()
    {
        var tool = await Scrape("pnpm", "Usage: pnpm install [OPTIONS]\n\nOptions:\n      --reporter <REPORTER>\n          Select reporter\n\n          [possible values: custom, debug]\n");
        var local = tool.Commands.Single().Options.Single();
        await Assert.That(local.EnumDefinition!.EnumName).IsEqualTo("PnpmInstallReporter");
        await Assert.That(local.EnumDefinition.Values.Select(value => value.CliValue)).IsEquivalentTo(["custom", "debug"]);
        await Assert.That(tool.GetGlobalOptions().Single(option => option.SwitchName == "--reporter").EnumDefinition!.EnumName)
            .IsEqualTo("PnpmReporter");
    }

    [Test]
    public async Task Npm_Command_Override_Keeps_Its_Arity_And_Local_Settings()
    {
        var tool = await Scrape("npm", "Install packages\n\nUsage:\nnpm install\n\nOptions:\n[--prefix] [--otp <otp>]\n");
        await Assert.That(tool.GetGlobalOptions().Single(option => option.SwitchName == "--prefix").IsFlag).IsFalse();
        var command = tool.Commands.Single();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--prefix").IsFlag).IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--otp").IsSecret).IsTrue();
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("new bool? Prefix");
    }

    internal static async Task<CliToolDefinition> Scrape(string tool, string? childHelp = null)
    {
        var pnpmHelp = await Fixture("Pnpm", "12.8.1", "pnpm-help.txt");
        var root = tool switch
        {
            "pnpm" => "Usage: pnpm [OPTIONS] <COMMAND>\n\nCommands:\n  install    Install packages\n\n"
                + pnpmHelp[pnpmHelp.IndexOf("Options:", StringComparison.Ordinal)..],
            "yarn" => "━━━ General commands ━━━\n  yarn install [--immutable]\n    Install packages.\n",
            _ => "All commands:\n\n    install\n\n",
        };
        childHelp ??= tool switch
        {
            "pnpm" => await Fixture("Pnpm", "12.8.1", "pnpm-install-help.txt"),
            "yarn" => await Fixture("Yarn", "4.18.1", "yarn-install.help"),
            _ => "Install packages\n\nUsage:\nnpm install [<package-spec> ...]\n\nOptions:\n[--save-dev] [--registry <registry>]\n",
        };
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var executor = new FixtureExecutor(root, childHelp);
        CliScraperBase scraper = tool switch
        {
            "pnpm" => new TestPnpm(executor, cache),
            "yarn" => new TestYarn(executor, cache),
            _ => new TestNpm(executor, cache),
        };
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return scraper.CreateToolDefinition() with { Commands = commands };
    }

    private static Task<string> Fixture(string tool, string version, string name) =>
        File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", tool, version, name));

    private sealed class TestPnpm(ICliCommandExecutor executor, IHelpTextCache cache)
        : PnpmCliScraper(executor, cache, NullLogger<PnpmCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class TestYarn(ICliCommandExecutor executor, IHelpTextCache cache)
        : YarnCliScraper(executor, cache, NullLogger<YarnCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class TestNpm(ICliCommandExecutor executor, IHelpTextCache cache)
        : NpmCliScraper(executor, cache, NullLogger<NpmCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class FixtureExecutor(string root, string child) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, string? workingDirectory = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new CliCommandResult { StandardOutput = arguments == "--help" ? root : arguments == "--version" ? "4.18.1" : child, StandardError = "", ExitCode = 0 });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
