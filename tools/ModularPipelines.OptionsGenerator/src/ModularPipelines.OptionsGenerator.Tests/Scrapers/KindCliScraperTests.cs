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

    [Test]
    public async Task Ordinary_One_Of_Prose_Does_Not_Create_An_Enum()
    {
        var command = await Parse(
            ["kind", "create", "cluster"],
            """
            Create a cluster

            Usage:
              kind create cluster [flags]

            Flags:
                  --sparse-checkout strings   list of directories; the configured path must be one of them, accepts comma-separated values
            """);

        var option = command.Options.Single(option => option.SwitchName == "--sparse-checkout");
        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    [Arguments(
        "docker",
        """Set the logging level ("debug", "info", "warn", "error", "fatal")""",
        "debug,info,warn,error,fatal")]
    [Arguments(
        "kubectl",
        """Must be "background", "orphan", or "foreground". Selects the deletion cascading strategy.""",
        "background,orphan,foreground")]
    public async Task Shared_Cobra_Parser_Preserves_Production_Enum_Phrases(
        string toolName,
        string description,
        string expectedValues)
    {
        var command = await Parse(
            [toolName, "run"],
            $"""
             {toolName} production enum fixture

             Usage:
               {toolName} run [flags]

             Flags:
                   --value string   {description}
             """);

        var option = command.Options.Single(option => option.SwitchName == "--value");
        await Assert.That(option.EnumDefinition).IsNotNull();
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(expectedValues.Split(','));
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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
