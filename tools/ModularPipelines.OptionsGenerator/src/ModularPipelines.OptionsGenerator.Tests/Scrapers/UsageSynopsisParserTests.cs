using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class UsageSynopsisParserTests
{
    [Test]
    public async Task Retains_Compound_Endpoint_Placeholders_As_Single_Operands()
    {
        var copy = UsageSynopsisParser.Parse(
            "Usage: minikube cp <source node name>:<source file path> <target node name>:<target absolute file path>",
            ["minikube", "cp"]);
        var mount = UsageSynopsisParser.Parse(
            "Usage: minikube mount <source directory>:<target directory> [flags]",
            ["minikube", "mount"]);

        using (Assert.Multiple())
        {
            await Assert.That(copy.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(copy.PositionalArguments[0].PlaceholderName)
                .IsEqualTo("source node name>:<source file path");
            await Assert.That(copy.PositionalArguments[1].PlaceholderName)
                .IsEqualTo("target node name>:<target absolute file path");
            await Assert.That(mount.PositionalArguments).Count().IsEqualTo(1);
            await Assert.That(mount.PositionalArguments[0].PlaceholderName)
                .IsEqualTo("source directory>:<target directory");
        }
    }

    [Test]
    public async Task Command_Group_Placeholders_Are_Not_Operands()
    {
        var parsed = UsageSynopsisParser.Parse(
            "Usage: docker buildx [OPTIONS] COMMAND",
            ["docker", "buildx"]);

        var result = UsageSynopsisParser.RemoveCommandGroupPlaceholders(parsed);

        await Assert.That(result.PositionalArguments).IsEmpty();
        await Assert.That(result.HasOperandTokens).IsFalse();
    }

    [Test]
    public async Task Parses_Regression_Operand_Syntax_Through_One_Model()
    {
        var fixtures = new[]
        {
            Fixture(
                "vault",
                "Usage: vault delete [options] PATH",
                ["vault", "delete"],
                Required("Path", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "terraform",
                "Usage: terraform [global options] import [options] ADDRESS ID",
                ["terraform", "import"],
                Required("Address", placement: PositionalArgumentPosition.AfterOptions),
                Required("Id", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "cargo",
                "Usage: cargo run [OPTIONS] [ARGS]...",
                ["cargo", "run"],
                Optional("Args", isVariadic: true, placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "packer",
                "Usage: packer build [options] TEMPLATE",
                ["packer", "build"],
                Required("Template", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "gh",
                "USAGE\n  gh api <endpoint> [flags]",
                ["gh", "api"],
                Required("Endpoint")),
            Fixture(
                "podman",
                "Usage:\n  podman attach [options] CONTAINER",
                ["podman", "attach"],
                Required("Container", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "buildah",
                "Usage: buildah add [options] container source [source ...] destination",
                ["buildah", "add"],
                Required("Container", placement: PositionalArgumentPosition.AfterOptions),
                Required("Source", isVariadic: true, placement: PositionalArgumentPosition.AfterOptions),
                Required("Destination", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "newman",
                "Usage: newman run [options] <collection|URL>",
                ["newman", "run"],
                Required("Collection", placement: PositionalArgumentPosition.AfterOptions)),
            Fixture(
                "docker",
                "Usage: docker exec [OPTIONS] CONTAINER COMMAND [ARG...]",
                ["docker", "exec"],
                Required("Container", placement: PositionalArgumentPosition.AfterOptions),
                Required("Command", placement: PositionalArgumentPosition.AfterOptions),
                Optional("Arg", isVariadic: true, placement: PositionalArgumentPosition.AfterOptions)),
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
                await Assert.That(actual.Placement).IsEqualTo(expected.Placement).Because(fixture.Tool);
                await Assert.That(actual.PositionIndex).IsEqualTo(index).Because(fixture.Tool);
            }
        }
    }

    [Test]
    public async Task Preserves_Operand_Order_Around_Options()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool copy <SOURCE> [OPTIONS] <DESTINATION>",
            ["tool", "copy"]);

        var source = result.PositionalArguments.Single(argument => argument.PropertyName == "Source");
        var destination = result.PositionalArguments.Single(argument => argument.PropertyName == "Destination");

        await Assert.That(source.Placement).IsEqualTo(PositionalArgumentPosition.BeforeOptions);
        await Assert.That(source.PositionIndex).IsEqualTo(0);
        await Assert.That(destination.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
        await Assert.That(destination.PositionIndex).IsEqualTo(0);

        var globalOptions = UsageSynopsisParser.Parse(
            "Usage: tool [GLOBAL OPTIONS] copy <SOURCE>",
            ["tool", "copy"]);

        await Assert.That(globalOptions.PositionalArguments.Single().Placement)
            .IsEqualTo(PositionalArgumentPosition.AfterOptions);
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
    public async Task Parses_Forwarded_Option_Tail_As_Multiple_Arguments()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: docker top CONTAINER [ps OPTIONS]",
            ["docker", "top"]);

        var psOptions = result.PositionalArguments.Single(argument =>
            argument.PropertyName == "PsOptions");
        using (Assert.Multiple())
        {
            await Assert.That(psOptions.IsRequired).IsFalse();
            await Assert.That(psOptions.IsVariadic).IsTrue();
            await Assert.That(psOptions.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Splits_Nested_Command_And_Argument_Operands()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: podman run [options] IMAGE [COMMAND [ARG...]]",
            ["podman", "run"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(3);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("Image");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("Command");
            await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
            await Assert.That(result.PositionalArguments[2].PropertyName).IsEqualTo("Arg");
            await Assert.That(result.PositionalArguments[2].CSharpType)
                .IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Requires_Suffixes_Outside_Optional_Qualifiers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: podman cp [options] [CONTAINER:]SRC_PATH [CONTAINER:]DEST_PATH",
            ["podman", "cp"]);

        await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(result.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
        await Assert.That(result.PositionalArguments.All(argument => argument.CSharpType == "string"))
            .IsTrue();
    }

    [Test]
    public async Task Preserves_Variadic_Marker_After_Alternative()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: docker inspect [OPTIONS] NAME|ID [NAME|ID...]",
            ["docker", "inspect"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Name");
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>");
        }
    }

    [Test]
    public async Task Applies_Bracketed_Standalone_Repeat_To_Preceding_Operand()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: podman inspect [options] {CONTAINER|IMAGE} [...]",
            ["podman", "inspect"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>");
        }
    }

    [Test]
    public async Task Preserves_Required_Core_Inside_Optional_Operand_Qualifiers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: pulumi env get [<org-name>/][<project-name>/]<environment-name>[@<version>] [property-path]",
            ["pulumi", "env", "get"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("EnvironmentName");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[0].CSharpType).IsEqualTo("string");
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("PropertyPath");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsFalse();
            await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Selects_Final_Required_Placeholder_From_Qualified_Compound_Operand()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: pulumi env clone [<org-name>/]<src-project-name>/<src-environment-name> [<dest-project-name>/]<dest-environment-name>",
            ["pulumi", "env", "clone"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("SrcEnvironmentName");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("DestEnvironmentName");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsTrue();
        }
    }

    [Test]
    [Arguments("client-secret", "ClientSecret")]
    [Arguments("secret-access-key", "SecretAccessKey")]
    [Arguments("access-token", "AccessToken")]
    public async Task Marks_Secret_Positional_Operands(string operandName, string propertyName)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool login <{operandName}>",
            ["tool", "login"]);

        var argument = result.PositionalArguments.Single();

        await Assert.That(argument.PropertyName).IsEqualTo(propertyName);
        await Assert.That(argument.IsSecret).IsTrue();
    }

    [Test]
    public async Task Preserves_Operands_Grouped_Behind_Option_Terminator()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: cargo login [OPTIONS] [-- [args]...]",
            ["cargo", "login"]);

        var argument = result.PositionalArguments.Single();

        await Assert.That(result.HasOperandTokens).IsTrue();
        await Assert.That(argument.PropertyName).IsEqualTo("Args");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsVariadic).IsTrue();
        await Assert.That(argument.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
        await Assert.That(argument.PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Parses_Inline_Usage_Continuation_Lines()
    {
        const string helpText = """
            Usage: tool upload [OPTIONS]
                   <SOURCE> <DESTINATION>

            Options:
              --force
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "upload"]);

        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Source", "Destination"]);
        await Assert.That(result.PositionalArguments.All(argument =>
                argument.Placement == PositionalArgumentPosition.AfterOptions))
            .IsTrue();
    }

    [Test]
    public async Task Ignores_Explanation_After_Terminated_Wrapped_Operand()
    {
        const string helpText = """
            Usage:
              minikube profile [MINIKUBE_PROFILE_NAME]. You can return to the default profile [flags]
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["minikube", "profile"]);

        var argument = result.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("MinikubeProfileName");
        await Assert.That(argument.IsRequired).IsFalse();
    }

    [Test]
    public async Task Carries_Standalone_Option_Terminator_To_Following_Operand()
    {
        var required = UsageSynopsisParser.Parse(
            "Usage: tool run [OPTIONS] -- <ARGS>",
            ["tool", "run"]);
        var optional = UsageSynopsisParser.Parse(
            "Usage: tool run [OPTIONS] [--] [ARGS...]",
            ["tool", "run"]);

        await Assert.That(required.PositionalArguments.Single().PrependOptionTerminator).IsTrue();
        await Assert.That(optional.PositionalArguments.Single().PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Relaxes_Operands_Absent_From_Alternate_Invocation_Forms()
    {
        const string helpText = """
            Usage:
              cargo add <DEP>[@<VERSION>]
              cargo add --path <PATH>
              cargo add --git <URL>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["cargo", "add"]);
        var dependency = result.PositionalArguments.Single();

        await Assert.That(result.MatchedSynopsisCount).IsEqualTo(3);
        await Assert.That(dependency.IsRequired).IsFalse();
        await Assert.That(dependency.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Relaxes_Operands_When_An_Alternate_Form_Is_Operandless()
    {
        const string helpText = """
            Usage:
              tool run <FILE>
              tool run
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);
        var file = result.PositionalArguments.Single();

        await Assert.That(file.IsRequired).IsFalse();
        await Assert.That(file.CSharpType).IsEqualTo("string?");
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
    public async Task Generator_Emits_Option_Terminator_Metadata()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: cargo test [OPTIONS] [-- [ARGS]...]",
            ["cargo", "test"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "cargo test",
            CommandParts = ["test"],
            ClassName = "CargoTestOptions",
            ParentClassName = "CargoOptions",
            ToolNamespacePrefix = "Cargo",
            Options = [],
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "cargo",
            NamespacePrefix = "Cargo",
            TargetNamespace = "ModularPipelines.Cargo",
            OutputDirectory = "src/ModularPipelines.Cargo",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            "PrependOptionTerminator = true");
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

    private static ExpectedArgument Required(
        string propertyName,
        bool isVariadic = false,
        PositionalArgumentPosition placement = PositionalArgumentPosition.BeforeOptions) =>
        new(propertyName, IsRequired: true, IsVariadic: isVariadic, Placement: placement);

    private static ExpectedArgument Optional(
        string propertyName,
        bool isVariadic = false,
        PositionalArgumentPosition placement = PositionalArgumentPosition.BeforeOptions) =>
        new(propertyName, IsRequired: false, IsVariadic: isVariadic, Placement: placement);

    private sealed record OperandFixture(
        string Tool,
        string HelpText,
        string[] CommandPath,
        IReadOnlyList<ExpectedArgument> ExpectedArguments);

    private sealed record ExpectedArgument(
        string PropertyName,
        bool IsRequired,
        bool IsVariadic,
        PositionalArgumentPosition Placement);

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
