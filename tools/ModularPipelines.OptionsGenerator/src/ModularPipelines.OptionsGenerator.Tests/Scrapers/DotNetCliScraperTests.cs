using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DotNetCliScraperTests
{
    [Test]
    public async Task Sdk_Root_Inherits_Only_Diagnostics()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "dotnet-10.0.401-root-help.txt"));
        var options = new TestDotNetCliScraper().ParseGlobals(help);
        await Assert.That(options).Count().IsEqualTo(1);
        var diagnostics = options.Single();
        await Assert.That(diagnostics.SwitchName).IsEqualTo("--diagnostics");
        await Assert.That(diagnostics.ShortForm).IsEqualTo("-d");
        await Assert.That(diagnostics.PropertyName).IsEqualTo("Diagnostics");
        await Assert.That(diagnostics.CSharpType).IsEqualTo("bool?");
        await Assert.That(diagnostics.IsFlag).IsTrue();
    }

    [Test]
    public async Task Command_Verbosity_Is_Not_A_Root_Global()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "dotnet-10.0.401-build-help.txt"));
        var scraper = new TestDotNetCliScraper();
        await Assert.That(scraper.ParseGlobals(help)).IsEmpty();
    }

    [Test]
    [Arguments("--ucr, --use-current-runtime", "--use-current-runtime", null, "UseCurrentRuntime", true)]
    [Arguments("--use-current-runtime, --ucr", "--use-current-runtime", null, "UseCurrentRuntime", true)]
    [Arguments("--sc, --self-contained", "--self-contained", null, "SelfContained", true)]
    [Arguments("-v, -verbosity <LEVEL>", "-verbosity", "-v", "Verbosity", false)]
    [Arguments("-v, --verbosity <LEVEL>", "--verbosity", "-v", "Verbosity", false)]
    [Arguments("-ss|--symbol-source <SOURCE>", "--symbol-source", "-ss", "SymbolSource", false)]
    public async Task Option_Aliases_Preserve_Descriptive_Switch(string declaration, string expectedSwitch,
        string? shortForm, string propertyName, bool isFlag)
    {
        var command = await new TestDotNetCliScraper().Parse(["dotnet", "build"],
            $"Options:\n  {declaration}  Configure this option.\n");
        var option = command!.Options.Single(option => option.PropertyName == propertyName);
        await Assert.That(option.SwitchName).IsEqualTo(expectedSwitch);
        await Assert.That(option.ShortForm).IsEqualTo(shortForm);
        await Assert.That(option.IsFlag).IsEqualTo(isFlag);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Version_Is_Excluded_Only_At_Root(bool root)
    {
        var command = await new TestDotNetCliScraper().Parse(root ? ["dotnet"] : ["dotnet", "pack"],
            "Options:\n  --version <VERSION>  The version of the package to create.\n");

        await Assert.That(command?.Options.Any(option => option.SwitchName == "--version") ?? false).IsEqualTo(!root);
    }

    [Test]
    [Arguments("pack", "--version <VERSION>  The version of the package to create.", "string?", false)]
    [Arguments("nuget", "--version  Show version information", "bool?", true)]
    [Arguments("workload", "--version  Display the currently installed workload version. [default: False]", "bool?", true)]
    public async Task Version_Type_Reflects_Command_After_Manual_Overrides(
        string subcommand, string declaration, string expectedType, bool isFlag)
    {
        var command = await new TestDotNetCliScraper().Parse(["dotnet", subcommand],
            $"Options:\n  {declaration}\n");
        var tool = new CliToolDefinition
        {
            ToolName = "dotnet",
            NamespacePrefix = "DotNet",
            TargetNamespace = "ModularPipelines.DotNet",
            OutputDirectory = "src/ModularPipelines.DotNet",
            Commands = [command!],
        };
        var enhancer = OptionTypeEnhancer.CreateDefault(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance), NullLoggerFactory.Instance);
        var enhanced = await enhancer.EnhanceManualOverridesAsync(tool);
        var version = enhanced.Commands.Single().Options.Single(option => option.SwitchName == "--version");
        await Assert.That(version.CSharpType).IsEqualTo(expectedType);
        await Assert.That(version.IsFlag).IsEqualTo(isFlag);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Test_Help_Isolates_Runner_Configuration_And_Preserves_Sdk(bool failHelp)
    {
        var executor = new IsolatedTestHelpExecutor(failHelp);
        var scraper = new TestDotNetCliScraper(executor);
        if (failHelp)
        {
            await Assert.ThrowsAsync<IOException>(() => scraper.Help(["dotnet", "test"]));
        }
        else
        {
            await Assert.That(await scraper.Help(["dotnet", "test"])).Contains("--filter");
        }

        await Assert.That(executor.WorkingDirectory).IsNotNull();
        await Assert.That(Directory.Exists(executor.WorkingDirectory)).IsFalse();
        using var settings = JsonDocument.Parse(executor.Settings!);
        await Assert.That(settings.RootElement.GetProperty("sdk").GetProperty("version").GetString()).IsEqualTo("10.0.401");
        await Assert.That(settings.RootElement.GetProperty("sdk").GetProperty("rollForward").GetString()).IsEqualTo("disable");
        await Assert.That(settings.RootElement.TryGetProperty("test", out _)).IsFalse();
    }

    [Test]
    public async Task Subcommands_With_Positional_Signatures_Are_Discovered()
    {
        const string helpText = """
                                Commands:
                                  add                                      Add a package or reference.
                                  delete <PackageId> <PackageVersion>      Delete a package.
                                  source <PackageSourcePath>               Add a package source.
                                  why <PROJECT | SOLUTION | FILE> <PACKAGE>    Show dependency paths.

                                Options:
                                  -h, --help  Show command line help.
                                """;

        var subcommands = new TestDotNetCliScraper().Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["add", "delete", "source", "why"]);
    }

    [Test]
    [Arguments("--nologo", "Nologo")]
    [Arguments("--no-logo", "NoLogo")]
    public async Task NoLogo_Options_Are_Normalized_Across_Sdk_Help_Formats(
        string scrapedSwitch,
        string scrapedPropertyName)
    {
        var options = new List<CliOptionDefinition>
        {
            Flag(scrapedSwitch, scrapedPropertyName),
            Flag("--debug", "Debug"),
        };

        DotNetCliNormalizer.NormalizeOptions(["build"], options);

        await Assert.That(options).Count().IsEqualTo(1);
        await Assert.That(options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(options[0].ShortForm).IsNull();
        await Assert.That(options[0].PropertyName).IsEqualTo("NoLogo");
    }

    [Test]
    public async Task Preserves_Current_Option_And_Operand_Arity()
    {
        const string helpText = """
            Usage: dotnet tool run <COMMAND_NAME> [<toolArguments>...]

            Arguments:
              <COMMAND_NAME>       The command to run.
              <toolArguments>      Arguments passed to the tool.

            Options:
              --project [<PROJECT>]  The project file to operate on.
              --exact-match          Require an exact package match. [default: False]
            """;

        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "tool", "run"],
            helpText);

        var project = command!.Options.Single(option => option.PropertyName == "Project");
        var exactMatch = command.Options.Single(option => option.PropertyName == "ExactMatch");
        var commandName = command.PositionalArguments.Single(argument =>
            argument.PropertyName == "CommandName");
        var toolArguments = command.PositionalArguments.Single(argument =>
            argument.PropertyName == "ToolArguments");
        using (Assert.Multiple())
        {
            await Assert.That(project.IsFlag).IsFalse();
            await Assert.That(project.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(exactMatch.IsFlag).IsTrue();
            await Assert.That(commandName.IsRequired).IsTrue();
            await Assert.That(commandName.CSharpType).IsEqualTo("string");
            await Assert.That(toolArguments.IsRequired).IsFalse();
            await Assert.That(toolArguments.IsVariadic).IsTrue();
            await Assert.That(toolArguments.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(toolArguments.Phase).IsEqualTo(CommandLinePhase.Passthrough);
            await Assert.That(toolArguments.PrependOptionTerminator).IsTrue();
        }
    }

    [Test]
    public async Task Build_Preserves_Optional_Project_Operand_Metadata()
    {
        const string helpText = """
            Usage: dotnet build [<PROJECT | SOLUTION | FILE>]

            Arguments:
              <PROJECT | SOLUTION | FILE>  The project or solution file to operate on.

            Options:
              -h, --help  Show command line help.
            """;

        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "build"],
            helpText);

        var projectSolution = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "ProjectSolution");
        using (Assert.Multiple())
        {
            await Assert.That(projectSolution.IsRequired).IsFalse();
            await Assert.That(projectSolution.IsVariadic).IsFalse();
            await Assert.That(projectSolution.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Test_Preserves_Both_Option_Terminators()
    {
        const string helpText = """
            Usage: dotnet test [options] [[--] <platformOptions>... -- [<extensionOptions>...]]

            Arguments:
              <platformOptions>   Arguments passed to the test platform.
              <extensionOptions>  Arguments passed to test extensions.

            Options:
              --no-build  Do not build before testing. [default: False]
            """;

        var command = await new TestDotNetCliScraper().Parse(
            ["dotnet", "test"],
            helpText);

        var platformOptions = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "PlatformOptions");
        var extensionOptions = command.PositionalArguments.Single(argument =>
            argument.PropertyName == "ExtensionOptions");
        using (Assert.Multiple())
        {
            await Assert.That(platformOptions.PrependOptionTerminator).IsTrue();
            await Assert.That(platformOptions.RepeatOptionTerminator).IsFalse();
            await Assert.That(extensionOptions.PrependOptionTerminator).IsTrue();
            await Assert.That(extensionOptions.RepeatOptionTerminator).IsTrue();
        }
    }

    private static CliOptionDefinition Flag(string switchName, string propertyName) => new()
    {
        SwitchName = switchName,
        ShortForm = "-nologo",
        PropertyName = propertyName,
        CSharpType = "bool?",
        IsFlag = true,
    };

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--no-restore  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage:
              dotnet build [options] <PROJECT | SOLUTION>

            Options:
              -c, --configuration <CONFIGURATION>  The configuration to use for building the project. Pair with
                                                   --no-restore  to skip restoring first.
              -o, --output <OUTPUT_DIR>            The output directory to place built artifacts in.
            """;

        var command = await new TestDotNetCliScraper().Parse(["dotnet", "build"], helpText);

        using (Assert.Multiple())
        {
            // "-p" is the synthesized MSBuild property switch every build command receives.
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--configuration", "--output", "-p"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--configuration").Description)
                .IsEqualTo("The configuration to use for building the project. Pair with --no-restore  to skip restoring first.");
        }
    }

    private sealed class TestDotNetCliScraper : DotNetCliScraper
    {
        public TestDotNetCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<DotNetCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => [.. ExtractSubcommands(helpText)];

        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string helpText) => ParseGlobalOptions(helpText);

        public Task<string?> Help(string[] commandPath) => GetHelpTextAsync(commandPath, CancellationToken.None);

        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var command = await ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
            return command is null ? null : ApplyIgnoredOptionPolicy(command);
        }
    }

    private sealed class IsolatedTestHelpExecutor(bool failHelp) : ICliCommandExecutor
    {
        public string? WorkingDirectory { get; private set; }
        public string? Settings { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            if (arguments == "--version")
            {
                return Task.FromResult(new CliCommandResult { ExitCode = 0, StandardOutput = "10.0.401", StandardError = "" });
            }

            WorkingDirectory = workingDirectory;
            Settings = workingDirectory is null ? null : File.ReadAllText(Path.Combine(workingDirectory, "global.json"));
            if (failHelp)
            {
                throw new IOException("Help execution failed.");
            }

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = "Options:\n  --filter <EXPRESSION>  Filter tests.\n",
                StandardError = "",
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class StaticHtmlHandler(string html) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(html),
                RequestMessage = request,
            });
    }
}
