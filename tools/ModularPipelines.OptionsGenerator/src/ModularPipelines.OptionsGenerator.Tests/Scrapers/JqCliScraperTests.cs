using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class JqCliScraperTests
{
    private const string HelpText = """
        Usage: jq [options] <jq filter> [file...]

        Command options:
          -n, --null-input          use `null` as the single input value;
          -s, --slurp               read all inputs into an array and use it as
                                    the single input value;
              --indent n            use n spaces for indentation (max 7 spaces);
          -f, --from-file           load the filter from a file;
          -L, --library-path dir    search modules from the directory;
              --arg name value      set $name to the string value;
              --slurpfile name file set $name to an array of JSON values read
                                    from the file;
        """;

    [Test]
    public async Task Parses_Aliases_Operands_And_Multiline_Descriptions()
    {
        var command = await new TestJqCliScraper().Parse(HelpText);

        await Assert.That(command.Options).Count().IsEqualTo(9);

        var nullInput = command.Options.Single(x => x.PropertyName == "NullInput");
        await Assert.That(nullInput.ShortForm).IsEqualTo("-n");
        await Assert.That(nullInput.IsFlag).IsTrue();

        var slurp = command.Options.Single(x => x.PropertyName == "Slurp");
        await Assert.That(slurp.Description).Contains("single input value");

        var indent = command.Options.Single(x => x.PropertyName == "Indent");
        await Assert.That(indent.CSharpType).IsEqualTo("int?");
        await Assert.That(indent.IsNumeric).IsTrue();

        await Assert.That(command.Options.Single(x => x.PropertyName == "FromFile").CSharpType).IsEqualTo("string?");

        var argument = command.Options.Single(x => x.PropertyName == "Arg");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<CliOptionValuePair>?");
        await Assert.That(argument.AcceptsMultipleValues).IsTrue();

        var slurpFile = command.Options.Single(x => x.PropertyName == "SlurpFile");
        await Assert.That(slurpFile.Description).Contains("from the file");

        var libraryPath = command.Options.Single(x => x.PropertyName == "LibraryPath");
        await Assert.That(libraryPath.PreferShortForm).IsTrue();

        var runTests = command.Options.Single(x => x.PropertyName == "RunTests");
        await Assert.That(runTests.CSharpType).IsEqualTo("string?");

        var endOfOptions = command.Options.Single(x => x.PropertyName == "EndOfOptions");
        await Assert.That(endOfOptions.SwitchName).IsEqualTo("--");
        await Assert.That(endOfOptions.IsFlag).IsTrue();
    }

    [Test]
    public async Task Places_Filter_And_Inputs_After_Options()
    {
        var command = await new TestJqCliScraper().Parse(HelpText);

        await Assert.That(command.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(command.PositionalArguments[0].PropertyName).IsEqualTo("Filter");
        await Assert.That(command.PositionalArguments[0].Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
        await Assert.That(command.PositionalArguments[1].PropertyName).IsEqualTo("InputFiles");
        await Assert.That(command.PositionalArguments[1].Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
    }

    private sealed class TestJqCliScraper : JqCliScraper
    {
        public TestJqCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<JqCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition> Parse(string helpText) =>
            (await ParseCommandAsync([ToolName], helpText, CancellationToken.None))!;
    }
}
