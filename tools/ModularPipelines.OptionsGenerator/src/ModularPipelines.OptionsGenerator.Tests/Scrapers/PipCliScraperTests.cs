using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PipCliScraperTests
{
    [Test]
    public async Task Captured_Abi_Description_Preserves_The_Python_Version_Flag()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "pip-25.3-install-help.txt"));

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);
        var description = command!.Options.Single(option => option.SwitchName == "--abi").Description;

        await Assert.That(description).Contains("--python-version when using this option.");
        await Assert.That(command.Options.Any(option => option.SwitchName == "--user")).IsTrue();
    }

    [Test]
    [Arguments("Use --python-", "version for compatibility.", "Use --python-version for compatibility.")]
    [Arguments("Use `--python-", "version` for compatibility.", "Use `--python-version` for compatibility.")]
    [Arguments("Use separate", "words for clarity.", "Use separate words for clarity.")]
    [Arguments("Use platform-", "specific wheels.", "Use platform- specific wheels.")]
    [Arguments("Use platform-specific", "wheels.", "Use platform-specific wheels.")]
    [Arguments("Use pre- and", "post-processing.", "Use pre- and post-processing.")]
    [Arguments("Use --", "before operands.", "Use -- before operands.")]
    [Arguments("Use --python-", "--platform together.", "Use --python- --platform together.")]
    public async Task Joins_Only_A_Split_Long_Option_Token(string first, string second, string expected)
    {
        var helpText = $"""
            Usage:
              pip install [options] <requirement specifier> ...

            Install Options:
              --abi <abi>    {first}
                             {second}
              --user         Install for the current user.
            """;

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);

        await Assert.That(command!.Options.Single(option => option.SwitchName == "--abi").Description).IsEqualTo(expected);
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--abi", "--user"]);
    }

    [Test]
    public async Task Split_Option_Reference_Does_Not_Consume_A_New_Declaration()
    {
        const string helpText = """
            Usage:
              pip install [options] <requirement specifier> ...

            Install Options:
              --abi <abi>    See --python-
              --version      Print the version.
            """;

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);

        await Assert.That(command!.Options.Single(option => option.SwitchName == "--abi").Description).IsEqualTo("See --python-");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--version").Description).IsEqualTo("Print the version.");
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--no-deps  to ..." line deliberately keeps two spaces so it satisfies the
        // option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage:
              pip install [options] <requirement specifier> ...

            Install Options:
              -r, --requirement <file>    Install from the given requirements file. Combine with
                                          --no-deps  to skip dependency installation.
              -e, --editable <path/url>   Install a project in editable mode.
            """;

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--requirement", "--editable"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--requirement").Description)
                .IsEqualTo("Install from the given requirements file. Combine with --no-deps  to skip dependency installation.");
        }
    }

    private sealed class TestPipCliScraper()
        : PipCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PipCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
