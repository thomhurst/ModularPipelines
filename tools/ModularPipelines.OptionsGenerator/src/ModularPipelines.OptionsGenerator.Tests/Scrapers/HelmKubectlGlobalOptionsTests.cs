using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class HelmKubectlGlobalOptionsTests
{
    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Helm_Persistent_Flags_Are_Inherited_Once(string newline)
    {
        var scraper = new TestHelm(new FixtureExecutor("Helm", "3.22.0", newline));
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(tool.GlobalOptions).Count().IsEqualTo(16);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(commands.SelectMany(command => command.Options)
            .Any(option => tool.GlobalOptions.Any(global => global.SwitchName == option.SwitchName))).IsFalse();
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--namespace").ShortForm).IsEqualTo("-n");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--kube-as-group").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--qps").CSharpType).IsEqualTo("double?");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--kube-token").IsSecret).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "helm install").Options
            .Any(option => option.SwitchName == "--create-namespace")).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "helm version").Options
            .Any(option => option.SwitchName == "--short")).IsTrue();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public virtual string? Namespace");
        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
    }

    [Test]
    public async Task Other_Cobra_Tools_Do_Not_Implicitly_Promote_Root_Flags()
    {
        var scraper = new OrdinaryCobra(new FixtureExecutor("Helm", "3.22.0"));
        var commands = await scraper.ScrapeAsync().ToListAsync();
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions).IsEmpty();
        await Assert.That(commands.Single().Options.Any(option => option.SwitchName == "--namespace")).IsTrue();
    }

    [Test]
    public async Task Helm_Local_Replacements_And_Intermediate_Inheritance_Are_Preserved()
    {
        var scraper = new TestHelm(new FixtureExecutor("Helm", "3.22.0"));
        await scraper.ScrapeAsync().ToListAsync();
        var command = await scraper.Parse("""
            Usage: helm version [flags]

            Flags:
                --namespace int   Command-specific namespace identifier.

            Global Flags:
              -n, --namespace string   namespace scope for this request
                --intermediate string   Setting inherited from a parent command.
            """);
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--namespace", "--intermediate"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--namespace").CSharpType).IsEqualTo("int?");
        var tool = scraper.CreateToolDefinition() with { Commands = [command] };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public new int? Namespace");
    }

    [Test]
    public async Task Helm_Conflicting_Inherited_Metadata_Is_Not_Silently_Removed()
    {
        var scraper = new TestHelm(new FixtureExecutor("Helm", "3.22.0"));
        await scraper.ScrapeAsync().ToListAsync();
        await Assert.That(async () => await scraper.Parse("""
            Usage: helm version [flags]

            Global Flags:
              -n, --namespace int   Unexpected inherited type.
            """))
            .Throws<InvalidOperationException>().WithMessageContaining("conflicting");
    }

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Kubectl_Options_Help_Is_Loaded_Once_With_Typed_Globals(string newline)
    {
        var executor = new FixtureExecutor("Kubectl", "1.37.1", newline);
        var scraper = new TestKubectl(executor);
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(executor.OptionsCalls).IsEqualTo(1);
        await Assert.That(tool.GlobalOptions).Count().IsEqualTo(30);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName))
            .Contains("--namespace").And.Contains("--context").And.Contains("--kubeconfig");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--namespace").ShortForm).IsEqualTo("-n");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--v").CSharpType).IsEqualTo("int?");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--as-group").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--token").IsSecret).IsTrue();
        var compression = tool.GlobalOptions.Single(option => option.SwitchName == "--disable-compression");
        await Assert.That(compression.IsFlag).IsTrue();
        await Assert.That(compression.NegatedSwitchName).IsEqualTo("--disable-compression=false");
        await Assert.That(commands.Single(command => command.FullCommand == "kubectl version").Options
            .Any(option => option.SwitchName == "--client")).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "kubectl annotate").Options
            .Any(option => option.SwitchName == "--overwrite")).IsTrue();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public virtual string? Context");
        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(0, true)]
    public async Task Kubectl_Missing_Global_Help_Fails_Generation(int exitCode, bool incomplete)
    {
        var scraper = new TestKubectl(new FixtureExecutor("Kubectl", "1.37.1", optionsExitCode: exitCode, incomplete: incomplete));
        await Assert.That(async () => await scraper.ScrapeAsync().ToListAsync())
            .Throws<InvalidOperationException>().WithMessageContaining("global option help is unavailable or incomplete");
    }

    [Test]
    public async Task Kubectl_Local_Global_Switch_Replacement_Retains_Its_Type()
    {
        var scraper = new TestKubectl(new FixtureExecutor("Kubectl", "1.37.1"));
        await scraper.ScrapeAsync().ToListAsync();
        var command = await scraper.Parse("""
            Usage: kubectl version [options]

            Options:
                --namespace=1: Command-specific identifier.
            """);
        await Assert.That(command!.Options.Single().CSharpType).IsEqualTo("int?");
        var tool = scraper.CreateToolDefinition() with { Commands = [command] };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public new int? Namespace");
    }

    private sealed class OrdinaryCobra(ICliCommandExecutor executor) : CobraCliScraper(
        executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger.Instance)
    {
        public override string ToolName => "helm";
        public override string NamespacePrefix => "Helm";
        public override string TargetNamespace => "ModularPipelines.Helm";
        public override string OutputDirectory => "src/ModularPipelines.Helm";

        protected override IEnumerable<string> ExtractSubcommands(string help) =>
            base.ExtractSubcommands(help).Where(command => command == "version");
    }

    private sealed class TestHelm(ICliCommandExecutor executor) : HelmCliScraper(
        executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<HelmCliScraper>.Instance)
    {
        protected override IEnumerable<string> ExtractSubcommands(string help) =>
            base.ExtractSubcommands(help).Where(command => command is "install" or "version" or "template");

        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(
            ["helm", "version"], help, ParseUsageSynopsis(["helm", "version"], help), CancellationToken.None);
    }

    private sealed class TestKubectl(ICliCommandExecutor executor) : KubectlCliScraper(
        executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance)
    {
        protected override IEnumerable<string> ExtractSubcommands(string help) =>
            base.ExtractSubcommands(help).Where(command => command is "annotate" or "version");

        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(
            ["kubectl", "version"], help, ParseUsageSynopsis(["kubectl", "version"], help), CancellationToken.None);
    }

    private sealed class FixtureExecutor(string tool, string version, string newline = "\n", int optionsExitCode = 0, bool incomplete = false)
        : ICliCommandExecutor
    {
        private int _optionsCalls;
        public int OptionsCalls => _optionsCalls;

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            var fixture = arguments == "--help" ? "root" : arguments.Replace(" --help", "", StringComparison.Ordinal);
            if (fixture == "options")
            {
                Interlocked.Increment(ref _optionsCalls);
            }

            var help = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", tool, version, fixture + ".txt"));
            if (fixture == "options" && incomplete)
            {
                help = "The following options can be passed to any command:\n";
            }

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = help.ReplaceLineEndings(newline),
                StandardError = string.Empty,
                ExitCode = fixture == "options" ? optionsExitCode : 0,
            });
        }
    }
}
