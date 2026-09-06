using System.Net;
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
        public TestDotNetCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<DotNetCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => [.. ExtractSubcommands(helpText)];

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
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
