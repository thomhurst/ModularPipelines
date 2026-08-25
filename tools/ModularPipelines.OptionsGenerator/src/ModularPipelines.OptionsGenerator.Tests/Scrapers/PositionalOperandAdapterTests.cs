using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PositionalOperandAdapterTests
{
    private static ICliCommandExecutor Executor { get; } =
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

    private static IHelpTextCache Cache { get; } =
        new HelpTextCache(NullLogger<HelpTextCache>.Instance);

    [Test]
    public async Task Winget_Hash_Preserves_File_Operand()
    {
        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "hash"],
            "usage: winget hash [-f] <file> [<options>]");

        await AssertArgument(command, "File", isRequired: true, isVariadic: false);
    }

    [Test]
    public async Task DotNet_Store_Preserves_Variadic_Argument_Operand()
    {
        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "store"],
            "Usage: dotnet store [<argument>...] [options]");

        await AssertArgument(command, "Argument", isRequired: false, isVariadic: true);
    }

    [Test]
    public async Task Go_Fix_Preserves_Packages_But_Not_Option_Value()
    {
        const string helpText = "usage: go fix [build flags] [-fixtool prog] [fix flags] [packages]";
        var command = await new TestGoCliScraper().Parse(["go", "fix"], helpText);

        await AssertArgument(command, "Packages", isRequired: false, isVariadic: true);
        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Packages"]);
    }

    [Test]
    public async Task Go_Telemetry_Models_Choice_As_Mode()
    {
        var command = await new TestGoCliScraper().Parse(
            ["go", "telemetry"],
            "usage: go telemetry [off|local|on]");

        await AssertArgument(command, "Mode", isRequired: false, isVariadic: false);
    }

    [Test]
    public async Task Go_Generate_Models_File_Or_Package_Targets()
    {
        var command = await new TestGoCliScraper().Parse(
            ["go", "generate"],
            "usage: go generate [build flags] [file.go... | packages]");

        await AssertArgument(command, "Targets", isRequired: false, isVariadic: true);
    }

    [Test]
    public async Task Liquibase_Command_Group_Preserves_Executable_Command_Operand()
    {
        const string helpText = "Usage: liquibase init [OPTIONS] [COMMAND]\n  -h, --help   Show this help message and exit";

        var usage = UsageSynopsisParser.Parse(helpText, ["liquibase", "init"]);

        using (Assert.Multiple())
        {
            await Assert.That(usage.HasOperandTokens).IsTrue();
            await Assert.That(usage.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
        }
    }

    [Test]
    public async Task Pip_Show_Preserves_Variadic_Package_Operand()
    {
        var command = await new TestPipCliScraper().Parse(
            ["pip", "show"],
            "Usage: pip show [options] <package> ...");

        await AssertArgument(command, "Package", isRequired: true, isVariadic: true);
    }

    [Test]
    public async Task Pnpm_Add_Preserves_Name_Operand()
    {
        var command = await new TestPnpmCliScraper().Parse(
            ["pnpm", "add"],
            "Usage: pnpm add <name>");

        await AssertArgument(command, "Name", isRequired: true, isVariadic: false);
    }

    private static async Task AssertArgument(
        CliCommandDefinition? command,
        string propertyName,
        bool isRequired,
        bool isVariadic)
    {
        var argument = command!.PositionalArguments.Single(item => item.PropertyName == propertyName);
        using (Assert.Multiple())
        {
            await Assert.That(argument.IsRequired).IsEqualTo(isRequired);
            await Assert.That(argument.IsVariadic).IsEqualTo(isVariadic);
        }
    }

    private sealed class TestWinGetCliScraper()
        : WinGetCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<WinGetCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestDotNetCliScraper()
        : DotNetCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<DotNetCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestGoCliScraper()
        : GoCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<GoCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestPipCliScraper()
        : PipCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<PipCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestPnpmCliScraper()
        : PnpmCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<PnpmCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
