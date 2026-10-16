using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class MinikubeMetadataTests
{
    [Test]
    [Arguments("image", "pull")]
    [Arguments("image", "push")]
    [Arguments("cache", "add")]
    [Arguments("cache", "delete")]
    public async Task Undocumented_Image_Lists_Remain_Optional_And_Repeatable(string group, string action)
    {
        var scraper = new TestScraper();
        var command = await scraper.Parse(["minikube", group, action], Fixture($"{group}-{action}-help"));
        var argument = command!.PositionalArguments.Single();

        await Assert.That(argument.PropertyName).IsEqualTo("Images");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsVariadic).IsTrue();
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.PositionIndex).IsEqualTo(0);

        var file = (await new OptionsClassGenerator().GenerateAsync(
            scraper.CreateToolDefinition() with { Commands = [command] })).Single();
        await Assert.That(file.Content).Contains("[CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]");
        await Assert.That(file.Content).Contains("IEnumerable<string>? Images");
    }

    [Test]
    public async Task Image_Tag_Requires_Source_Then_Target()
    {
        var scraper = new TestScraper();
        var command = await scraper.Parse(["minikube", "image", "tag"], Fixture("image-tag-help"));
        var arguments = command!.PositionalArguments;

        await Assert.That(arguments.Select(argument => argument.PropertyName)).IsEquivalentTo(["Source", "Target"]);
        await Assert.That(arguments[0].PropertyName).IsEqualTo("Source");
        await Assert.That(arguments[1].PropertyName).IsEqualTo("Target");
        await Assert.That(arguments.Select(argument => argument.PositionIndex)).IsEquivalentTo([0, 1]);
        await Assert.That(arguments.All(argument => argument.IsRequired && !argument.IsVariadic
            && argument.CSharpType == "string")).IsTrue();

        var file = (await new OptionsClassGenerator().GenerateAsync(
            scraper.CreateToolDefinition() with { Commands = [command] })).Single();
        await Assert.That(file.Content).Contains("public MinikubeImageTagOptions(");
        await Assert.That(file.Content).Contains("[CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)]");
        await Assert.That(file.Content).Contains("[CliArgument(1, Phase = CommandLinePhase.EarlyOperand, Required = true)]");
    }

    [Test]
    public async Task Update_Context_Preserves_Complete_Wrapped_Description()
    {
        var command = await new TestScraper().Parse(["minikube", "update-context"], Fixture("update-context-help"));
        await Assert.That(command!.Description).IsEqualTo(
            "Retrieves the IP address of the running cluster, checks it with IP in kubeconfig, and corrects kubeconfig if incorrect.");
    }

    private static string Fixture(string name) => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Minikube", "1.39.0", name + ".txt"));

    internal static async Task<CliCommandDefinition> ParseFixture(string group, string action) =>
        (await new TestScraper().Parse(["minikube", group, action], Fixture($"{group}-{action}-help")))!;

    private sealed class TestScraper() : MinikubeCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<MinikubeCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, ParseUsageSynopsis(path, help), CancellationToken.None);
    }
}
