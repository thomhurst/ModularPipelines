using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class UsageSynopsisParserTests
{
    [Test]
    public async Task Parses_Regression_Operand_Syntax_Through_One_Model()
    {
        var fixtures = new[]
        {
            Fixture(
                "vault",
                "Usage: vault delete [options] PATH",
                ["vault", "delete"],
                Required("Path")),
            Fixture(
                "terraform",
                "Usage: terraform [global options] import [options] ADDRESS ID",
                ["terraform", "import"],
                Required("Address"),
                Required("Id")),
            Fixture(
                "cargo",
                "Usage: cargo new [OPTIONS] <PATH>",
                ["cargo", "new"],
                Required("Path")),
            Fixture(
                "packer",
                "Usage: packer build [options] TEMPLATE",
                ["packer", "build"],
                Required("Template")),
            Fixture(
                "gh",
                "USAGE\n  gh api <endpoint> [flags]",
                ["gh", "api"],
                Required("Endpoint")),
            Fixture(
                "podman",
                "Usage:\n  podman attach [options] CONTAINER",
                ["podman", "attach"],
                Required("Container")),
            Fixture(
                "buildah",
                "Usage: buildah add [options] container source [source ...] destination",
                ["buildah", "add"],
                Required("Container"),
                Required("Source", isVariadic: true),
                Required("Destination")),
            Fixture(
                "newman",
                "Usage: newman run [options] <collection|URL>",
                ["newman", "run"],
                Required("Collection")),
        };

        foreach (var fixture in fixtures)
        {
            var result = UsageSynopsisParser.Parse(fixture.HelpText, fixture.CommandPath);

            await Assert.That(result.HasOperandTokens)
                .IsTrue()
                .Because(fixture.Tool);
            await Assert.That(result.PositionalArguments.Count)
                .IsEqualTo(fixture.ExpectedArguments.Count)
                .Because(fixture.Tool);

            for (var index = 0; index < fixture.ExpectedArguments.Count; index++)
            {
                var actual = result.PositionalArguments[index];
                var expected = fixture.ExpectedArguments[index];
                await Assert.That(actual.PropertyName).IsEqualTo(expected.PropertyName).Because(fixture.Tool);
                await Assert.That(actual.IsRequired).IsEqualTo(expected.IsRequired).Because(fixture.Tool);
                await Assert.That(actual.IsVariadic).IsEqualTo(expected.IsVariadic).Because(fixture.Tool);
                await Assert.That(actual.PositionIndex).IsEqualTo(index).Because(fixture.Tool);
            }
        }
    }

    [Test]
    public async Task Parses_Required_Optional_And_Repeat_Markers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool deploy TARGET [environment] [FILE...]",
            ["tool", "deploy"]);

        await Assert.That(result.PositionalArguments.Count).IsEqualTo(3);
        await Assert.That(result.PositionalArguments[0].CSharpType).IsEqualTo("string");
        await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
        await Assert.That(result.PositionalArguments[2].CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(result.PositionalArguments[2].IsVariadic).IsTrue();
    }

    [Test]
    public async Task Prefers_Full_Command_Path_Over_Suffix_With_More_Operands()
    {
        const string helpText = """
            Usage:
              tool config get <TARGET>
              get <WRONG> <EXTRA>
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["tool", "config", "get"]);

        await Assert.That(result.Synopsis).IsEqualTo("tool config get <TARGET>");
        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Target"]);
    }

    [Test]
    public async Task Reports_Equally_Ranked_Synopses_As_Ambiguous()
    {
        const string helpText = """
            Usage:
              tool run <FILE>
              tool run <TARGET>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);

        await Assert.That(result.MatchedSynopsisCount).IsEqualTo(2);
        await Assert.That(result.HasAmbiguousMatch).IsTrue();
    }

    [Test]
    public async Task Shared_Traversal_Parses_Usage_Once()
    {
        var scraper = new CountingUsageScraper();
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(scraper.UsageParseCount).IsEqualTo(1);
        await Assert.That(commands.Single().PositionalArguments.Single().PropertyName)
            .IsEqualTo("Target");
    }

    [Test]
    public async Task Kustomize_Adapter_Supplies_Omitted_Buildmetadata_Operand()
    {
        const string helpText = """
            Adds build metadata.

            Usage:
              kustomize edit add buildmetadata [flags]

            Flags:
              -h, --help   help for buildmetadata
            """;

        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "edit", "add", "buildmetadata"],
            helpText);

        var metadata = command!.PositionalArguments.Single();
        await Assert.That(metadata.PropertyName).IsEqualTo("Metadata");
        await Assert.That(metadata.IsRequired).IsTrue();
    }

    [Test]
    public async Task Migrated_Scraper_Rejects_Missing_Shared_Usage_Result()
    {
        var scraper = new TestKustomizeCliScraper();

        await Assert.That(() => scraper.ParseWithoutUsage(["kustomize", "build"], "Usage: kustomize build"))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Shared traversal must pass its parsed synopsis");
    }

    [Test]
    public async Task Newman_Does_Not_Promote_Wrapped_Alternate_To_Subcommand()
    {
        const string helpText = """
            Commands:
              run [options] <collection|URL>  Run a collection
                  URL                       Wrapped description continuation
            """;

        var commands = new TestNewmanCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["run"]);
    }

    [Test]
    public async Task Generator_Renders_Required_Operands_As_Constructor_Parameters_And_Optional_As_Properties()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool upload <SOURCE> [DESTINATION...]",
            ["tool", "upload"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool upload",
            CommandParts = ["upload"],
            ClassName = "ToolUploadOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            "[property: CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)] string Source");
        await Assert.That(generated).Contains(
            "[CliArgument(1, Placement = ArgumentPlacement.BeforeOptions)]");
        await Assert.That(generated).Contains(
            "public IEnumerable<string>? Destination { get; set; }");
    }

    [Test]
    public async Task Model_Rejects_OperandTaking_Usage_With_No_Positionals()
    {
        var command = new CliCommandDefinition
        {
            FullCommand = "tool broken",
            CommandParts = ["broken"],
            ClassName = "ToolBrokenOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            UsageSynopsis = "tool broken <TARGET>",
            HasOperandTakingUsage = true,
        };

        await Assert.That(command.ValidateOperandCoverage)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no CliPositionalArgument");
    }

    private static OperandFixture Fixture(
        string tool,
        string helpText,
        string[] commandPath,
        params ExpectedArgument[] expectedArguments) =>
        new(tool, helpText, commandPath, expectedArguments);

    private static ExpectedArgument Required(string propertyName, bool isVariadic = false) =>
        new(propertyName, IsRequired: true, IsVariadic: isVariadic);

    private sealed record OperandFixture(
        string Tool,
        string HelpText,
        string[] CommandPath,
        IReadOnlyList<ExpectedArgument> ExpectedArguments);

    private sealed record ExpectedArgument(
        string PropertyName,
        bool IsRequired,
        bool IsVariadic);

    private sealed class TestKustomizeCliScraper : KustomizeCliScraper
    {
        public TestKustomizeCliScraper()
            : base(
                Executor(),
                Cache(),
                NullLogger<KustomizeCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public Task<CliCommandDefinition?> ParseWithoutUsage(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }

    private sealed class TestNewmanCliScraper : NewmanCliScraper
    {
        public TestNewmanCliScraper()
            : base(
                Executor(),
                Cache(),
                NullLogger<NewmanCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
    }

    private sealed class CountingUsageScraper : CliScraperBase
    {
        public CountingUsageScraper()
            : base(
                new StubExecutor(),
                Cache(),
                NullLogger<CountingUsageScraper>.Instance)
        {
        }

        public int UsageParseCount { get; private set; }

        public override string ToolName => "tool";

        public override string NamespacePrefix => "Tool";

        public override string TargetNamespace => "ModularPipelines.Tool";

        public override string OutputDirectory => "src/ModularPipelines.Tool";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override IEnumerable<string> GetAdditionalUsageSynopses(
            string[] commandPath,
            string helpText)
        {
            UsageParseCount++;
            return [];
        }

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Shared traversal should pass its parsed synopsis.");

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            UsageSynopsisParseResult usage,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "tool",
                CommandParts = [],
                ClassName = "ToolOptions",
                ParentClassName = "ToolOptions",
                ToolNamespacePrefix = "Tool",
                Options = [],
                PositionalArguments = usage.PositionalArguments,
                UsageSynopsis = usage.Synopsis,
                HasOperandTakingUsage = usage.HasOperandTokens,
            });
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = "Usage: tool <TARGET>",
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private static ProcessCliCommandExecutor Executor() =>
        new(NullLogger<ProcessCliCommandExecutor>.Instance);

    private static HelpTextCache Cache() =>
        new(NullLogger<HelpTextCache>.Instance);
}
