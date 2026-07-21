using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class EksctlCliScraperTests
{
    [Test]
    public async Task Extracts_Final_Segment_From_Fully_Qualified_Command_Lines()
    {
        const string helpText = """
            Create resource(s)

            Usage: eksctl create [flags]

            Commands:
              eksctl create accessentry                     Create access entries
              eksctl create addon                           Create an Addon
              eksctl create podidentityassociation          Create a pod identity association

            Common flags:
              -h, --help   help for this command
            """;

        var subcommands = new TestEksctlCliScraper().Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(
            ["accessentry", "addon", "podidentityassociation"]);
    }

    [Test]
    public async Task Preserves_Collection_Type_For_Enum_String_Slices()
    {
        const string helpText = """
            Update cluster logging configuration

            Usage: eksctl utils update-cluster-logging [flags]

            Flags:
                  --enable-types strings   Log types to enable (all, none, api, audit)
            """;

        var command = await new TestEksctlCliScraper().Parse(
            ["eksctl", "utils", "update-cluster-logging"],
            helpText);
        var option = command!.Options.Single(x => x.SwitchName == "--enable-types");

        await Assert.That(option.CSharpType)
            .IsEqualTo("IEnumerable<EksctlUtilsUpdateClusterLoggingEnableTypes>?");
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
    }

    [Test]
    public async Task Models_Default_True_Booleans_As_Value_Options()
    {
        const string helpText = """
            Create a cluster

            Usage: eksctl create cluster [flags]

            Flags:
                  --managed   Create EKS-managed nodegroup (default true)
            """;

        var command = await new TestEksctlCliScraper().Parse(
            ["eksctl", "create", "cluster"],
            helpText);
        var option = command!.Options.Single(x => x.SwitchName == "--managed");

        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueSeparator).IsEqualTo("=");
    }

    private sealed class TestEksctlCliScraper : EksctlCliScraper
    {
        public TestEksctlCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<EksctlCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
