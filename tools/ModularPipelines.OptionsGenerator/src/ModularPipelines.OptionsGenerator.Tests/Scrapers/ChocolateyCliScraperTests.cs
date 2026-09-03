using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ChocolateyCliScraperTests
{
    [Test]
    [Arguments("[<options/switches>]")]
    [Arguments("[<options or switches>]")]
    public async Task Options_Section_Markers_Are_Not_Operands(string marker)
    {
        var helpText = $"""
            Chocolatey v2.5.1
            Info Command

            Usage

                choco info <pkg> {marker}

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "info"],
            helpText);

        command!.ValidateOperandCoverage();
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Pkg"]);
    }

    [Test]
    [Arguments("apikey")]
    [Arguments("export")]
    [Arguments("info")]
    [Arguments("outdated")]
    [Arguments("setapikey")]
    public async Task Options_Only_Usage_Does_Not_Discard_Command(string commandName)
    {
        var helpText = $"""
            Chocolatey v2.7.4
            {commandName} Command

            Usage

                choco {commandName} [<options/switches>]

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", commandName],
            helpText);

        command!.ValidateOperandCoverage();
        using (Assert.Multiple())
        {
            await Assert.That(command.HasOperandTakingUsage).IsFalse();
            await Assert.That(command.PositionalArguments).IsEmpty();
        }
    }

    [Test]
    public async Task Command_Coverage_Protects_Full_Chocolatey_Surface()
    {
        var coverage = new TestChocolateyCliScraper().CreateToolDefinition().CommandCoverage;

        using (Assert.Multiple())
        {
            await Assert.That(coverage.MinimumCommandCount).IsEqualTo(27);
            await Assert.That(coverage.SentinelCommands).IsEquivalentTo(
            [
                "choco apikey",
                "choco export",
                "choco info",
                "choco outdated",
                "choco setapikey",
            ]);
        }
    }

    [Test]
    [Arguments("install", "<pkg> [<pkg2> <pkgN>]", "Pkg2PkgN", "The <pkg2> <pkgN> operand.")]
    [Arguments("uninstall", "<pkg> [pkg2 pkgN]", "Pkg2PkgN", "The <pkg2> <pkgN> operand.")]
    [Arguments("upgrade", "<pkg> [<pkg2> <pkgN>]", "Pkg2PkgN", "The <pkg2> <pkgN> operand.")]
    [Arguments("new", "<name> [<property=value> <propertyN=valueN>]", "PropertyValuePropertyNValueN", "The <property=value> <propertyN=valueN> operand.")]
    public async Task Repeatable_Operand_Groups_Become_Collections(
        string commandName,
        string operands,
        string expectedPropertyName,
        string expectedDescription)
    {
        var helpText = $"""
            Chocolatey v2.5.1
            {commandName} Command

            Usage

                choco {commandName} {operands} [<options/switches>]

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", commandName],
            helpText);

        var argument = command!.PositionalArguments.Single(candidate =>
            candidate.PropertyName == expectedPropertyName);
        using (Assert.Multiple())
        {
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.Description).IsEqualTo(expectedDescription);
        }
    }

    [Test]
    [Arguments("list", "<filter>", "Filter")]
    [Arguments("feature", "[list]|disable|enable", "List")]
    [Arguments("pin", "[list]|add|remove", "List")]
    [Arguments("source", "[list]|add|remove|enable|disable", "List")]
    public async Task Default_Action_Operands_Remain_Optional(
        string commandName,
        string operand,
        string expectedPropertyName)
    {
        var helpText = $"""
            Chocolatey v2.5.1
            {commandName} Command

            Usage

                choco {commandName} {operand} [<options/switches>]

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", commandName],
            helpText);

        var argument = command!.PositionalArguments.Single(candidate =>
            candidate.PropertyName == expectedPropertyName);
        using (Assert.Multiple())
        {
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
            await Assert.That(argument.IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Commands_Without_Default_Actions_Keep_Required_Operands()
    {
        const string helpText = """
            Chocolatey v2.5.1
            Install Command

            Usage

                choco install <pkg> [<options/switches>]

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "install"],
            helpText);

        var argument = command!.PositionalArguments.Single(candidate =>
            candidate.PropertyName == "Pkg");
        using (Assert.Multiple())
        {
            await Assert.That(argument.CSharpType).IsEqualTo("string");
            await Assert.That(argument.IsRequired).IsTrue();
        }
    }

    [Test]
    public async Task Config_Alternatives_Become_An_Optional_Action()
    {
        const string helpText = """
            Chocolatey v2.5.1
            Config Command

            Usage

                choco config [list]|get|set|unset [<options/switches>]

            Options and Switches
            ====================

                --name=VALUE
                Name - The configuration setting name.
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "config"],
            helpText);

        command!.ValidateOperandCoverage();

        var action = command.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(action.PropertyName).IsEqualTo("Action");
            await Assert.That(action.CSharpType).IsEqualTo("string?");
            await Assert.That(action.IsRequired).IsFalse();
            await Assert.That(command.Options.Single().PropertyName).IsEqualTo("Name");
        }
    }

    [Test]
    public async Task Pack_Operands_Are_Reindexed_After_The_Options_Marker()
    {
        const string helpText = """
            Chocolatey v2.7.4
            Pack Command

            Usage

                choco pack [<path to nuspec>] [<options/switches>] [<property=value>]

            Options and Switches
            ====================
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "pack"],
            helpText);

        await Assert.That(command!.PositionalArguments
                .Select(argument => (argument.PropertyName, argument.PositionIndex)))
            .IsEquivalentTo([("PathToNuspec", 0), ("PropertyValue", 1)]);
    }

    [Test]
    public async Task Template_Name_Option_Is_Preserved()
    {
        const string helpText = """
            Chocolatey v2.7.4
            Template Command

            Usage

                choco template [list]|info [<options/switches>]

            Options and Switches
            ====================

             -n, --name=VALUE
                 The name of the template to get information about.
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "template"],
            helpText);

        var name = command!.Options.Single(option => option.PropertyName == "Name");
        using (Assert.Multiple())
        {
            await Assert.That(name.SwitchName).IsEqualTo("--name");
            await Assert.That(name.ShortForm).IsEqualTo("-n");
            await Assert.That(name.CSharpType).IsEqualTo("string?");
        }
    }

    private sealed class TestChocolateyCliScraper : ChocolateyCliScraper
    {
        public TestChocolateyCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<ChocolateyCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return await ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
