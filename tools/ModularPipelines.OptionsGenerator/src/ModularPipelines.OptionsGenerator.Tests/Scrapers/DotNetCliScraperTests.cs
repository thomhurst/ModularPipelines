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
    public async Task NoLogo_Options_Are_Stable_Across_Sdk_Help_Formats(
        string scrapedSwitch,
        string scrapedPropertyName)
    {
        var options = new List<CliOptionDefinition>
        {
            Flag(scrapedSwitch, scrapedPropertyName),
            Flag("--debug", "Debug"),
        };

        DotNetCliCompatibility.NormalizeOptions(["build"], options);

        await Assert.That(options).Count().IsEqualTo(1);
        await Assert.That(options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(options[0].ShortForm).IsNull();
        await Assert.That(options[0].PropertyName).IsEqualTo("NoLogo");
    }

    [Test]
    [Arguments("clean")]
    [Arguments("pack")]
    [Arguments("publish")]
    public async Task NoLogo_Compatibility_Preserves_Renamed_Public_Properties(string command)
    {
        var properties = DotNetCliCompatibility.GetProperties([command]);

        await Assert.That(properties).Count().IsEqualTo(1);
        await Assert.That(properties[0].PropertyName).IsEqualTo("Nologo");
        await Assert.That(properties[0].ForwardToPropertyName).IsEqualTo("NoLogo");
    }

    [Test]
    public async Task Build_Compatibility_Preserves_Removed_Public_Properties()
    {
        var properties = DotNetCliCompatibility.GetProperties(["build"]);

        await Assert.That(properties.Select(property => property.PropertyName))
            .IsEquivalentTo(["Nologo", "Debug"]);
        await Assert.That(properties.Single(property => property.PropertyName == "Nologo").ForwardToPropertyName)
            .IsEqualTo("NoLogo");
        await Assert.That(properties.Single(property => property.PropertyName == "Debug").ForwardToPropertyName)
            .IsNull();
    }

    [Test]
    public async Task Documentation_Fallback_Applies_Build_Compatibility()
    {
        const string html = """
                            <html><body><article>
                            <dl>
                              <dt>--no-logo</dt><dd>Do not display the startup banner.</dd>
                              <dt>--debug</dt><dd>Enable debug output.</dd>
                            </dl>
                            </article></body></html>
                            """;
        using var httpClient = new HttpClient(new StaticHtmlHandler(html));
        var scraper = new DotNetCliDocumentationScraper(
            httpClient,
            NullLogger<DotNetCliDocumentationScraper>.Instance);

        var tool = await scraper.ScrapeAsync();
        var build = tool.Commands.Single(command => command.CommandParts.SequenceEqual(["build"]));

        await Assert.That(build.Options).Count().IsEqualTo(1);
        await Assert.That(build.Options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(build.Options[0].PropertyName).IsEqualTo("NoLogo");
        await Assert.That(build.CompatibilityProperties.Select(property => property.PropertyName))
            .IsEquivalentTo(["Nologo", "Debug"]);
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

    private static CliOptionDefinition Flag(string switchName, string propertyName) => new()
    {
        SwitchName = switchName,
        ShortForm = "-nologo",
        PropertyName = propertyName,
        CSharpType = "bool?",
        IsFlag = true,
    };

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
