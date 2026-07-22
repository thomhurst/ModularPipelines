using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KindCliScraperTests
{
    [Test]
    public async Task Docker_Image_Positionals_Are_One_Required_Collection()
    {
        var command = await Parse(
            ["kind", "load", "docker-image"],
            """
            Loads docker images from host into all or specified nodes by name

            Usage:
              kind load docker-image <IMAGE> [IMAGE...] [flags]
            """);

        var positional = command.PositionalArguments.Single();
        await Assert.That(positional.PropertyName).IsEqualTo("Image");
        await Assert.That(positional.CSharpType).IsEqualTo("IEnumerable<string>");
        await Assert.That(positional.IsRequired).IsTrue();
    }

    [Test]
    public async Task Image_Archive_Dotted_Positional_Is_Required()
    {
        var command = await Parse(
            ["kind", "load", "image-archive"],
            """
            Loads docker image from archive into all or specified nodes by name

            Usage:
              kind load image-archive <IMAGE.tar> [flags]
            """);

        var positional = command.PositionalArguments.Single();
        await Assert.That(positional.PropertyName).IsEqualTo("ImageTar");
        await Assert.That(positional.CSharpType).IsEqualTo("string");
        await Assert.That(positional.IsRequired).IsTrue();
    }

    [Test]
    public async Task Node_Image_Source_Enum_Excludes_Description_Suffix()
    {
        var command = await Parse(
            ["kind", "build", "node-image"],
            """
            Build the node image

            Usage:
              kind build node-image [kubernetes-source] [flags]

            Flags:
                  --type string   optionally specify one of 'url', 'file', 'release', 'ci' or 'source' as the type of build
            """);

        var values = command.Options.Single(option => option.SwitchName == "--type").EnumDefinition!.Values;
        await Assert.That(values.Select(value => value.CliValue))
            .IsEquivalentTo(["url", "file", "release", "ci", "source"]);
    }

    private static async Task<CliCommandDefinition> Parse(string[] commandPath, string helpText) =>
        (await new TestKindCliScraper().Parse(commandPath, helpText))!;

    private sealed class TestKindCliScraper : KindCliScraper
    {
        public TestKindCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<KindCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
