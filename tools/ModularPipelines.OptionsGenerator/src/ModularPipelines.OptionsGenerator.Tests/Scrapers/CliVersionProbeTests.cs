using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CliVersionProbeTests
{
    [Test]
    [Arguments(false, "windows_amd64")]
    [Arguments(true, "windows_amd64")]
    [Arguments(false, "linux_arm64")]
    [Arguments(true, "linux_arm64")]
    public async Task Terraform_Uses_Stable_Installed_Version_From_Json(bool outdated, string platform)
    {
        var output = JsonSerializer.Serialize(new
        {
            terraform_version = "1.16.4",
            platform,
            terraform_outdated = outdated,
        });
        var executor = new RecordingExecutor("terraform", "version -json", output);
        var scraper = new TerraformCliScraper(executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<TerraformCliScraper>.Instance);

        await Assert.That(await scraper.IsAvailableAsync()).IsTrue();
        await Assert.That(await scraper.GetVersionAsync()).IsEqualTo("Terraform v1.16.4");
    }

    [Test]
    [Arguments("invalid json")]
    [Arguments("{}")]
    [Arguments("{\"terraform_version\":null}")]
    [Arguments("{\"terraform_version\":\" \"}")]
    public async Task Terraform_Rejects_Missing_Or_Invalid_Version_Metadata(string output)
    {
        var scraper = new TerraformCliScraper(new RecordingExecutor("terraform", "version -json", output),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<TerraformCliScraper>.Instance);

        await Assert.That(await scraper.GetVersionAsync()).IsNull();
    }

    [Test]
    public async Task ArgoCd_Uses_Client_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "argocd",
            "version --client",
            (executor, cache) => new ArgoCdCliScraper(
                executor,
                cache,
                NullLogger<ArgoCdCliScraper>.Instance));

    [Test]
    public async Task Eksctl_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "eksctl",
            "version",
            (executor, cache) => new EksctlCliScraper(
                executor,
                cache,
                NullLogger<EksctlCliScraper>.Instance));

    [Test]
    public async Task Cosign_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "cosign",
            "version",
            (executor, cache) => new CosignCliScraper(
                executor,
                cache,
                NullLogger<CosignCliScraper>.Instance));

    [Test]
    public async Task Kustomize_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "kustomize",
            "version",
            (executor, cache) => new KustomizeCliScraper(
                executor,
                cache,
                NullLogger<KustomizeCliScraper>.Instance));

    [Test]
    public async Task Go_Uses_Version_Subcommand() =>
        await AssertVersionProbeAsync(
            "go",
            "version",
            (executor, cache) => new GoCliScraper(
                executor,
                cache,
                NullLogger<GoCliScraper>.Instance));

    private static async Task AssertVersionProbeAsync(
        string expectedCommand,
        string expectedArguments,
        Func<ICliCommandExecutor, IHelpTextCache, ICliScraper> createScraper)
    {
        var executor = new RecordingExecutor(expectedCommand, expectedArguments);
        var scraper = createScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance));

        var isAvailable = await scraper.IsAvailableAsync();

        using (Assert.Multiple())
        {
            await Assert.That(isAvailable).IsTrue();
            await Assert.That(executor.InvocationCount).IsEqualTo(1);
        }
    }

    private sealed class RecordingExecutor(string expectedCommand, string expectedArguments, string output = "version")
        : ICliCommandExecutor
    {
        public int InvocationCount { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            InvocationCount++;
            var success = command.Equals(expectedCommand, StringComparison.Ordinal)
                          && arguments.Equals(expectedArguments, StringComparison.Ordinal);
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? output : string.Empty,
                StandardError = success ? string.Empty : "unexpected probe",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public async Task<bool> IsAvailableAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default) =>
            (await ExecuteAsync(command, arguments, cancellationToken)).Success;
    }
}
