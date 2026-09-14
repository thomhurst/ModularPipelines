using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PipCliScraperTests
{
    [Test]
    [Arguments("uninstall", "Package")]
    [Arguments("wheel", "RequirementSpecifier")]
    [Arguments("lock", "LocalProjectPath")]
    [Arguments("install", "RequirementSpecifier")]
    [Arguments("download", "RequirementSpecifier")]
    public async Task Requirement_File_Alternatives_Do_Not_Require_Positional_Operands(string verb, string positionalName)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", $"pip-25.3-{verb}-help.txt"));
        var scraper = new PipCliScraper(new RequirementHelpExecutor(verb, help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<PipCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var leaf = commands.Single(command => command.FullCommand == $"pip {verb}");
        var positional = leaf.PositionalArguments.Single();
        await Assert.That(positional.PropertyName).IsEqualTo(positionalName);
        await Assert.That(positional.IsRequired).IsFalse();
        await Assert.That(positional.IsValidationRequired == true).IsFalse();
        await Assert.That(positional.IsVariadic).IsTrue();
        var requirement = leaf.Options.Single(option => option.SwitchName == "--requirement");
        await Assert.That(requirement.ShortForm).IsEqualTo("-r");
        await Assert.That(requirement.AcceptsMultipleValues).IsTrue();
        var inputChoice = leaf.RequiredAlternativeGroups.Single();
        await Assert.That(inputChoice.PropertyNames).Contains(positionalName).And.Contains("Requirement");
        if (verb != "uninstall")
        {
            await Assert.That(inputChoice.PropertyNames).Contains("Group");
            if (verb != "download")
            {
                await Assert.That(inputChoice.PropertyNames).Contains("Editable");
            }
        }

        var tool = new CliToolDefinition
        {
            ToolName = "pip",
            NamespacePrefix = "Pip",
            TargetNamespace = "ModularPipelines.Python",
            OutputDirectory = "output",
            Commands = [leaf],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).DoesNotContain("Required = true");
    }

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

    private sealed class RequirementHelpExecutor(string verb, string help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => $"Usage: pip <command> [options]\n\nCommands:\n  {verb}    Run the pip command.\n",
                    _ when arguments == $"{verb} --help" => help,
                    _ => throw new InvalidOperationException($"Unexpected pip invocation: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
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
