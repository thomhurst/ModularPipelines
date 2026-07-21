using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class HadolintCliScraperTests
{
    [Test]
    public async Task Parses_Multiline_And_Repeatable_Options()
    {
        const string helpText = """
            Usage: hadolint [DOCKERFILE...]

            Available options:
              -h,--help                Show this help text
              --file-path-in-report FILEPATHINREPORT
                                       The file path referenced in the generated report.
              -f,--format ARG          The output format for the results
              --error RULECODE         Make the rule have the level error
              --warning RULECODE       Make the rule have the level warning
              --info RULECODE          Make the rule have the level info
              --style RULECODE         Make the rule have the level style
              --ignore RULECODE        A rule to ignore
              --trusted-registry REGISTRY (e.g. docker.io)
                                       A trusted registry
              --require-label LABELSCHEMA (e.g. maintainer:text)
                                       A required label schema
              -t,--failure-threshold THRESHOLD
                                       Minimum failure severity
            """;

        var command = await new TestHadolintCliScraper().Parse(helpText.Replace("\n", "\r\n"));

        await Assert.That(GetOption(command, "--file-path-in-report").CSharpType).IsEqualTo("string?");
        await Assert.That(GetOption(command, "--file-path-in-report").Description)
            .IsEqualTo("The file path referenced in the generated report.");
        await Assert.That(GetOption(command, "--failure-threshold").CSharpType)
            .IsEqualTo("HadolintFailureThreshold?");

        string[] repeatableOptions =
        [
            "--error", "--warning", "--info", "--style", "--ignore",
            "--trusted-registry", "--require-label",
        ];

        foreach (var optionName in repeatableOptions)
        {
            var option = GetOption(command, optionName);
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
        }
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private sealed class TestHadolintCliScraper : HadolintCliScraper
    {
        public TestHadolintCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<HadolintCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition> Parse(string helpText) =>
            (await ParseCommandAsync(["hadolint"], helpText, CancellationToken.None))!;
    }
}
