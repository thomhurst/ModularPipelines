using ModularPipelines.Helpers.Internal;
using ModularPipelines.Jq.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Jq.UnitTests.Attributes;

public class JqOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

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
                new CliOptionValuePair("name", "Ada"),
                new CliOptionValuePair("environment", "ci"),
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
            SlurpFile = [new CliOptionValuePair("documents", "documents.json")],
            RawFile = [new CliOptionValuePair("template", "template.txt")],
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
    public async Task Renders_RunTests_After_Positionals()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            LibraryPath = ["modules"],
            Binary = true,
            RunTests = "tests.jq",
            Filter = "-1",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "-L", "modules",
            "-b",
            "-1",
            "--run-tests", "tests.jq",
        ],
        TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Renders_EndOfOptions_Before_DashPrefixed_Filter()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            EndOfOptions = true,
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

    private IReadOnlyList<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
