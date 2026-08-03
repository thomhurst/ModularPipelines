using ModularPipelines.Context;
using ModularPipelines.Jq.Options;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Jq.UnitTests.Attributes;

public class JqOptionsTests : TestBase
{
    [Test]
    public async Task Renders_Aliases_Numeric_Options_Pairs_And_Positionals()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            NullInput = true,
            RawOutput = true,
            Indent = 2,
            Arg =
            [
                new CliValuePair("name", "Ada"),
                new CliValuePair("environment", "ci"),
            ],
            Filter = ".user",
            InputFiles = ["input.json"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--null-input",
            "--raw-output",
            "--indent", "2",
            "--arg", "name", "Ada",
            "--arg", "environment", "ci",
            "--",
            ".user",
            "input.json",
        ]);
    }

    [Test]
    public async Task Renders_Filter_File_And_Named_File_Options()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            FromFile = "filter.jq",
            SlurpFile = [new CliValuePair("documents", "documents.json")],
            RawFile = [new CliValuePair("template", "template.txt")],
            InputFiles = ["input.json"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--from-file", "filter.jq",
            "--slurpfile", "documents", "documents.json",
            "--rawfile", "template", "template.txt",
            "input.json",
        ]);
    }

    [Test]
    public async Task Rejects_RunTests_With_OptionTerminated_Filter()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new JqExecuteOptions
        {
            LibraryPath = ["modules"],
            Binary = true,
            RunTests = "tests.jq",
            Filter = "-1",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Renders_OptionTerminator_Before_DashPrefixed_Filter()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            Filter = "-1",
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--", "-1"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Renders_Bare_RunTests_For_Standard_Input()
    {
        var arguments = BuildArguments(new JqExecuteOptions { RunTests = string.Empty });

        await Assert.That(arguments).IsEquivalentTo(["--run-tests"]);
    }
}
