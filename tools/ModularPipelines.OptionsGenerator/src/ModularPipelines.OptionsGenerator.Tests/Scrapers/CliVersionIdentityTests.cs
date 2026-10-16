using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CliVersionIdentityTests
{
    [Test]
    [Arguments("ansible", "ansible [core 2.21.3]\n  executable location = /opt/pipx/bin/ansible\n  python version = 3.12.3 (main, Aug 31 2026)", "2.21.3")]
    [Arguments("pip", "pip 26.2.1 from /home/runner/work/_temp/pip/lib/python3.12/site-packages/pip (python 3.12)", "26.2.1")]
    [Arguments("aws", "aws-cli/2.37.7 Python/3.14.6 Linux/6.17.0-1022-azure exe/x86_64.ubuntu.24", "2.37.7")]
    [Arguments("go", "go version go1.27.2 linux/amd64", "1.27.2")]
    [Arguments("git", "git version 2.55.0", "2.55.0")]
    [Arguments("docker", "Docker version 28.0.4, build b8034c0", "28.0.4")]
    [Arguments("jq", "jq-1.8.2", "1.8.2")]
    [Arguments("yq", "yq (https://github.com/mikefarah/yq/) version v4.54.1", "4.54.1")]
    [Arguments("kubectl", "Client Version: v1.37.1\nKustomize Version: v5.8.1", "1.37.1")]
    [Arguments("shellcheck", "ShellCheck - shell script analysis tool\nversion: 0.11.0\nlicense: GNU General Public License, version 3", "0.11.0")]
    [Arguments("trivy", "Version: 0.75.0\nVulnerability DB: 2", "0.75.0")]
    [Arguments("dotnet", "11.0.100-rc.1.26425.128", "11.0.100-rc.1.26425.128")]
    [Arguments("nbgv", "3.10.94+dea9a6c17c", "3.10.94+dea9a6c17c")]
    [Arguments("fake", "\u001b[32mfake version v1.2.3-rc.1+build.42\u001b[0m", "1.2.3-rc.1+build.42")]
    public async Task Default_Parser_Extracts_Only_Stable_Version(string tool, string output, string expected)
    {
        await Assert.That(await ProbeAsync(tool, output)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("pip 26.2.1 from C:\\agent\\pip\\lib\\site-packages\\pip (python 3.13)")]
    [Arguments("pip 26.2.1 from /another/runner/python3.14/site-packages/pip (python 3.14)")]
    public async Task Installation_Path_Does_Not_Change_Identity(string output)
    {
        await Assert.That(await ProbeAsync("pip", output)).IsEqualTo("26.2.1");
    }

    [Test]
    [Arguments("pip 26.2.1 from /opt/pip", "WARNING: Python 3.12 is deprecated")]
    [Arguments("pip 26.2.1 from /opt/pip", "pip 99.0.0")]
    [Arguments("WARNING: Python 3.12 is deprecated\npip 26.2.1 from /opt/pip", "")]
    [Arguments("", "WARNING: Python 3.12 is deprecated\npip 26.2.1 from /opt/pip")]
    [Arguments("startup banner", "pip 26.2.1 from /opt/pip")]
    public async Task Prefers_Stdout_Identity_And_Supports_Stderr_Only(string output, string error)
    {
        await Assert.That(await ProbeAsync("pip", output, error)).IsEqualTo("26.2.1");
    }

    [Test]
    [Arguments("WARNING: Python 3.12 is deprecated")]
    [Arguments("pip: warning: Python 3.12 is deprecated")]
    [Arguments("The latest version of pip is 27.0.0")]
    [Arguments("Python 3.12.3\n/opt/pip-26.2.1/bin/pip")]
    [Arguments("pip version unknown")]
    [Arguments("")]
    public async Task Does_Not_Use_Warnings_Paths_Or_Component_Versions(string output)
    {
        await Assert.That(await ProbeAsync("pip", output, output)).IsNull();
    }

    [Test]
    [Arguments("gcloud", "Google Cloud SDK 589.0.0\nalpha 2026.10.09\nbq 2.1.40\ncore 2026.10.09", "589.0.0")]
    [Arguments("gcloud", "Google Cloud SDK 589.0.0\nalpha 2026.11.01\nbq 2.2.0\ncore 2026.11.01", "589.0.0")]
    [Arguments("brew", "Homebrew 7.0.7\nHomebrew/homebrew-core (git revision abc; last commit 2026-10-16)", "7.0.7")]
    [Arguments("hadolint", "Haskell Dockerfile Linter 2.15.1", "2.15.1")]
    [Arguments("helm", "version.BuildInfo{Version:\"v3.22.0\", GitCommit:\"abc\", GoVersion:\"go1.26.8\"}", "3.22.0")]
    [Arguments("helm", "version.BuildInfo{Version:\"v4.0.0-rc.1+build.42\", GitCommit:\"def\", GoVersion:\"go1.27.2\"}", "4.0.0-rc.1+build.42")]
    public async Task Custom_Display_Formats_Exclude_Component_Metadata(string tool, string output, string expected)
    {
        var executor = new VersionExecutor(output, "WARNING: runtime version 99.0.0");
        var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        ICliScraper scraper = tool switch
        {
            "gcloud" => new GcloudCliScraper(executor, cache, NullLogger<GcloudCliScraper>.Instance),
            "brew" => new BrewCliScraper(executor, cache, NullLogger<BrewCliScraper>.Instance),
            "hadolint" => new HadolintCliScraper(executor, cache, NullLogger<HadolintCliScraper>.Instance),
            "helm" => new HelmCliScraper(executor, cache, NullLogger<HelmCliScraper>.Instance),
            _ => throw new ArgumentOutOfRangeException(nameof(tool)),
        };

        await Assert.That(await scraper.GetVersionAsync()).IsEqualTo(expected);
    }

    [Test]
    public async Task Same_Version_Drift_Guard_Remains_Active_Across_Environment_Changes()
    {
        var directory = Path.Combine(Path.GetTempPath(), "mp-version-coverage-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(await ProbeAsync("pip", "pip 26.2.1 from /runner/one/python3.12"), "pip install"),
                directory, approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(await ProbeAsync("pip", "pip 26.2.1 from C:\\runner\\two\\python3.14", "WARNING: Python 3.14"),
                    "pip install", "pip unexpected"),
                directory, approveShrinkage: false);

            await Assert.That(current.Violations).Contains(message => message.Contains("version remained '26.2.1'"));
            await Assert.That(current.AddedCommands).IsEquivalentTo(["pip unexpected"]);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static CliToolDefinition Tool(string? version, params string[] commands) => new()
    {
        ToolName = "pip",
        NamespacePrefix = "Pip",
        TargetNamespace = "ModularPipelines.Python",
        OutputDirectory = "src/ModularPipelines.Python",
        ToolVersion = version,
        Commands = [.. commands.Select(command => new CliCommandDefinition
        {
            FullCommand = command,
            CommandParts = [.. command.Split(' ').Skip(1)],
            ClassName = command.Replace(" ", string.Empty, StringComparison.Ordinal) + "Options",
            ParentClassName = "PipOptions",
            ToolNamespacePrefix = "Pip",
            Options = [],
        })],
    };

    private static Task<string?> ProbeAsync(string tool, string output, string error = "") =>
        new TestScraper(tool, new VersionExecutor(output, error)).GetVersionAsync();

    private sealed class TestScraper(string tool, ICliCommandExecutor executor) : CliScraperBase(
        executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger.Instance)
    {
        public override string ToolName => tool;
        public override string NamespacePrefix => tool;
        public override string TargetNamespace => "ModularPipelines.Test";
        public override string OutputDirectory => "src/ModularPipelines.Test";
        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];
        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath, string helpText, CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(null);
    }

    private sealed class VersionExecutor(string output, string error) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, string? workingDirectory = null,
            CancellationToken cancellationToken = default) => Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = error,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
