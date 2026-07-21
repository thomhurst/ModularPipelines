using ModularPipelines.Helpers.Internal;
using ModularPipelines.Jq.Options;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Attributes;

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
    public async Task Renders_Portable_Library_Path_And_Documented_Trailing_Options()
    {
        var arguments = BuildArguments(new JqExecuteOptions
        {
            LibraryPath = ["modules"],
            RunTests = "tests.jq",
            EndOfOptions = true,
            Filter = "-1",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "-L", "modules",
            "--run-tests", "tests.jq",
            "--",
            "-1",
        ]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
