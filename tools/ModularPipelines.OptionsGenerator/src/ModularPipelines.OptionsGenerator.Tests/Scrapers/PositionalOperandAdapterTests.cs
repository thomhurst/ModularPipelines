using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
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
    public async Task DotNet_NuGet_Add_Source_Uses_Current_Operand_Name()
    {
        const string helpText = "Usage: NuGet.CommandLine.XPlat add source <PackageSourcePath> [options]";
        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "nuget", "add", "source"],
            helpText);

        await AssertArgument(command, "PackageSourcePath", isRequired: true, isVariadic: false);
    }

    [Test]
    public async Task DotNet_NuGet_Push_Uses_Current_Variadic_Operand()
    {
        const string helpText = "Usage: NuGet.CommandLine.XPlat push <package-paths>... [options]";
        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "nuget", "push"],
            helpText);

        await AssertArgument(command, "PackagePaths", isRequired: true, isVariadic: true);
    }

    [Test]
    public async Task Go_Fix_Preserves_Packages_But_Not_Option_Value()
    {
        const string helpText = "usage: go fix [build flags] [-fixtool prog] [fix flags] [packages]";
        var command = await new TestGoCliScraper().Parse(["go", "fix"], helpText);

        await AssertArgument(command, "Packages", isRequired: false, isVariadic: true);
        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Packages"]);
        await Assert.That(command.PositionalArguments.Single().Phase)
            .IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Go_Mod_Preserves_Generic_Arguments_As_Separate_Tokens()
    {
        var command = await new TestGoCliScraper().Parse(
            ["go", "mod"],
            "usage: go mod <command> [arguments]");

        var arguments = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "Arguments");
        using (Assert.Multiple())
        {
            await Assert.That(arguments.IsVariadic).IsTrue();
            await Assert.That(arguments.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(arguments.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
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
    public async Task Pip_Install_Treats_Package_Index_Syntax_As_Option_Label()
    {
        var command = await new TestPipCliScraper().Parse(
            ["pip", "install"],
            "Usage: pip install [options] <requirement specifier> [package-index-options] ...");

        var requirement = command!.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(requirement.PropertyName).IsEqualTo("RequirementSpecifier");
            await Assert.That(requirement.IsVariadic).IsTrue();
            await Assert.That(requirement.CSharpType).IsEqualTo("IEnumerable<string>");
        }
    }

    [Test]
    public async Task Go_Clean_Extracts_Options_Without_A_Flags_Table()
    {
        const string helpText = """
            usage: go clean [-i] [-r] [-cache] [-o output] [packages]

            The -i flag removes installed archives.
            The -r flag applies clean recursively.
            The -cache flag removes the entire build cache.
            """;

        var command = await new TestGoCliScraper().Parse(["go", "clean"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["-i", "-r", "-cache", "-o"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "-o").IsFlag)
                .IsFalse();
            await Assert.That(command.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Packages");
        }
    }

    [Test]
    public async Task Go_Clean_Does_Not_Treat_Prose_Examples_As_Options()
    {
        const string helpText = """
            usage: go clean [-i] [packages]

            The -i flag removes installed archives.
            DIR.test(.exe) from go test -c
            """;

        var command = await new TestGoCliScraper().Parse(["go", "clean"], helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["-i"]);
    }

    [Test]
    public async Task Go_Usage_Parses_Inline_Option_Values()
    {
        const string helpText = "usage: go tool compile -o=FILE [packages]";
        var command = await new TestGoCliScraper().Parse(
            ["go", "tool", "compile"],
            helpText);
        var output = command!.Options.Single(option => option.SwitchName == "-o");

        using (Assert.Multiple())
        {
            await Assert.That(output.IsFlag).IsFalse();
            await Assert.That(output.CSharpType).IsEqualTo("string?");
            await Assert.That(output.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Pip_Require_Hashes_Is_A_Presence_Only_Flag()
    {
        const string helpText = """
            Usage:
              pip install [options] <requirement specifier> ...

            Install Options:
              --require-hashes      Require a hash to check each requirement against, for repeatable installs.
            """;

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);
        var requireHashes = command!.Options.Single(option => option.PropertyName == "RequireHashes");

        using (Assert.Multiple())
        {
            await Assert.That(requireHashes.IsFlag).IsTrue();
            await Assert.That(requireHashes.CSharpType).IsEqualTo("bool?");
            await Assert.That(requireHashes.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    [Arguments("cache", "Usage: pip cache list [<pattern>]\n  pip cache purge")]
    [Arguments("config", "Usage: pip config [<file-option>] list\n  pip config get command.option")]
    [Arguments("index", "Usage: pip index versions <package>")]
    public async Task Pip_Parent_Groups_Do_Not_Treat_Child_Syntax_As_Operands(
        string commandName,
        string helpText)
    {
        var command = await new TestPipCliScraper().Parse(["pip", commandName], helpText);

        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Pnpm_Add_Preserves_Name_Operand()
    {
        var command = await new TestPnpmCliScraper().Parse(
            ["pnpm", "add"],
            "Usage: pnpm add <name>");

        await AssertArgument(command, "Name", isRequired: true, isVariadic: false);
        await Assert.That(command!.PositionalArguments.Single().Phase)
            .IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Pnpm_Stage_Does_Not_Treat_Child_Syntax_As_Operands()
    {
        const string helpText = "Usage: pnpm stage publish [<tarball>|<dir>]\n"
                                + "       pnpm stage list [<package-spec>]";

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "stage"], helpText);

        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Pnpm_Audit_Extracts_Child_Without_Modeling_It_As_An_Operand()
    {
        const string helpText = """
            Usage: pnpm audit [options]
                   pnpm audit signatures [options]

            Commands:
                  signatures    Verify registry signatures
            """;
        var scraper = new TestPnpmCliScraper();

        var command = await scraper.Parse(["pnpm", "audit"], helpText);

        await Assert.That(scraper.Extract(helpText)).IsEquivalentTo(["signatures"]);
        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Pnpm_Unlink_Ignores_Parenthetical_Explanation()
    {
        const string helpText = "Usage: pnpm unlink (in package dir)\n"
                                + "       pnpm unlink <pkg>...";

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "unlink"], helpText);

        await AssertArgument(command, "Pkg", isRequired: false, isVariadic: true);
    }

    [Test]
    [Arguments("metadata")]
    [Arguments("stacks")]
    [Arguments("state")]
    public async Task Terraform_Command_Group_Args_Are_Variadic(string commandName)
    {
        var command = await new TestTerraformCliScraper().Parse(
            ["terraform", commandName],
            $"Usage: terraform {commandName} [args]");

        await AssertArgument(command, "Args", isRequired: false, isVariadic: true);
        await Assert.That(command!.PositionalArguments.Single().CSharpType)
            .IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    [Arguments("eval")]
    [Arguments("eval-all")]
    public async Task Yq_Expressions_And_Files_Render_After_Options(string commandName)
    {
        var command = await new TestYqCliScraper().Parse(
            ["yq", commandName],
            $"Usage: yq {commandName} [expression] [yaml_file1]...");

        await Assert.That(command!.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(command.PositionalArguments.All(argument =>
                argument.Phase == CommandLinePhase.Passthrough
                && argument.PrependOptionTerminatorIfValueStartsWithDash))
            .IsTrue();
    }

    [Test]
    public async Task Packer_Inspect_Template_Renders_After_Options()
    {
        var command = await new TestPackerCliScraper().Parse(
            ["packer", "inspect"],
            "Usage: packer inspect TEMPLATE");

        await Assert.That(command!.PositionalArguments.Single().PropertyName).IsEqualTo("Template");
        await Assert.That(command.PositionalArguments.Single().Phase)
            .IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Packer_Plugins_Preserves_Trailing_Arguments_As_Separate_Tokens()
    {
        var command = await new TestPackerCliScraper().Parse(
            ["packer", "plugins"],
            "Usage: packer plugins <subcommand> [options] [args]");

        var arguments = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "Args");
        using (Assert.Multiple())
        {
            await Assert.That(arguments.IsVariadic).IsTrue();
            await Assert.That(arguments.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(arguments.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
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
        public IReadOnlyList<string> Extract(string helpText) =>
            ExtractSubcommands(helpText).ToArray();

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestTerraformCliScraper()
        : TerraformCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<TerraformCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestYqCliScraper()
        : YqCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<YqCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestPackerCliScraper()
        : PackerCliScraper(
            PositionalOperandAdapterTests.Executor,
            PositionalOperandAdapterTests.Cache,
            NullLogger<PackerCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
