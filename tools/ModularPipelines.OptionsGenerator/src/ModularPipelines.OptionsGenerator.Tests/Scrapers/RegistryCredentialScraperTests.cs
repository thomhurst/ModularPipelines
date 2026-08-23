using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class RegistryCredentialScraperTests
{
    [Test]
    public async Task Buildah_Masks_Creds()
    {
        var command = await new TestBuildahCliScraper().Parse(
            ["buildah", "build"],
            "Usage: buildah build [flags]\n\nFlags:\n  --creds string   Registry credentials");

        await AssertSecretOptions(command, "Creds");
    }

    [Test]
    public async Task Flux_Masks_Registry_Creds_But_Not_Helper_Name()
    {
        var command = await new TestFluxCliScraper().Parse(
            ["flux", "bootstrap", "git"],
            "Usage: flux bootstrap git [flags]\n\nFlags:\n  --registry-creds string   Registry credentials\n  --creds-helper string   Credential helper name");

        await AssertSecretOptions(command, "RegistryCreds");
    }

    [Test]
    public async Task Skopeo_Masks_Source_And_Destination_Creds()
    {
        var command = await new TestSkopeoCliScraper().Parse(
            ["skopeo", "copy"],
            "Usage: skopeo copy [flags]\n\nFlags:\n  --dest-creds string   Destination credentials\n  --src-creds string   Source credentials");

        await AssertSecretOptions(command, "DestCreds", "SrcCreds");
    }

    private static async Task AssertSecretOptions(
        CliCommandDefinition? command,
        params string[] expectedSecretProperties)
    {
        await Assert.That(command!.Options.Where(option => option.IsSecret).Select(option => option.PropertyName))
            .IsEquivalentTo(expectedSecretProperties);
    }

    private sealed class TestBuildahCliScraper()
        : BuildahCliScraper(
            RegistryCredentialScraperTests.Executor,
            RegistryCredentialScraperTests.Cache,
            NullLogger<BuildahCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string helpText) =>
            ParseCommandAsync(path, helpText, ParseUsageSynopsis(path, helpText), CancellationToken.None);
    }

    private sealed class TestFluxCliScraper()
        : FluxCliScraper(
            RegistryCredentialScraperTests.Executor,
            RegistryCredentialScraperTests.Cache,
            NullLogger<FluxCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string helpText) =>
            ParseCommandAsync(path, helpText, ParseUsageSynopsis(path, helpText), CancellationToken.None);
    }

    private sealed class TestSkopeoCliScraper()
        : SkopeoCliScraper(
            RegistryCredentialScraperTests.Executor,
            RegistryCredentialScraperTests.Cache,
            NullLogger<SkopeoCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string helpText) =>
            ParseCommandAsync(path, helpText, ParseUsageSynopsis(path, helpText), CancellationToken.None);
    }

    private static ICliCommandExecutor Executor { get; } =
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

    private static IHelpTextCache Cache { get; } =
        new HelpTextCache(NullLogger<HelpTextCache>.Instance);
}
