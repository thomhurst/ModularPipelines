using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class UsageSynopsisParserTests
{
    [Test]
    public async Task Ignores_Azure_Usage_Examples()
    {
        const string helpText = """
            Command
                az redis force-reboot : Reboot specified Redis node(s).
                    Usage example - az redis force-reboot --name testCacheName --resource-group
                    testResourceGroup --reboot-type {AllNodes, PrimaryNode, SecondaryNode} [--shard-id].
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["az", "redis", "force-reboot"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.HasExtractedSynopses).IsFalse();
            await Assert.That(result.HasOperandTokens).IsFalse();
            await Assert.That(result.PositionalArguments).IsEmpty();
        }
    }

    [Test]
    public async Task Ignores_Comma_Separated_Command_Alias()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: brew update, up [options]",
            ["brew", "update"]);
        var resultWithOperand = UsageSynopsisParser.Parse(
            "Usage: tool remove, rm <TARGET>",
            ["tool", "remove"]);
        var resultWithMultipleAliases = UsageSynopsisParser.Parse(
            "Usage: brew uninstall, remove, rm [options] formula|cask [...]",
            ["brew", "uninstall"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).IsEmpty();
            await Assert.That(result.HasOperandTokens).IsFalse();
            await Assert.That(resultWithOperand.PositionalArguments).HasSingleItem();
            await Assert.That(resultWithOperand.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Target");
            await Assert.That(resultWithMultipleAliases.PositionalArguments).HasSingleItem();
            await Assert.That(resultWithMultipleAliases.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Formula");
            await Assert.That(resultWithMultipleAliases.PositionalArguments.Single().IsVariadic)
                .IsTrue();
        }
    }

    [Test]
    public async Task Matches_Command_In_Wrapped_Alternative()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: brew bundle [install|upgrade]:",
            ["brew", "bundle", "install"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.CommandMatched).IsTrue();
            await Assert.That(result.MatchedCommandPartCount).IsEqualTo(3);
            await Assert.That(result.HasOperandTokens).IsFalse();
        }
    }

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
            await Assert.That(copy.PositionalArguments[0].PropertyName)
                .IsEqualTo("SourceNodeNameSourceFilePath");
            await Assert.That(copy.PositionalArguments[1].PropertyName)
                .IsEqualTo("TargetNodeNameTargetAbsoluteFilePath");
            await Assert.That(mount.PositionalArguments).Count().IsEqualTo(1);
            await Assert.That(mount.PositionalArguments[0].PropertyName)
                .IsEqualTo("SourceDirectoryTargetDirectory");
        }
    }

    [Test]
    public async Task Command_Group_Placeholders_Remain_Executable_Operands()
    {
        var parsed = UsageSynopsisParser.Parse(
            "Usage: docker buildx [OPTIONS] COMMAND",
            ["docker", "buildx"]);
        var nested = UsageSynopsisParser.Parse(
            "Usage: winget source [<command>] [<options>]",
            ["winget", "source"]);

        using (Assert.Multiple())
        {
            await Assert.That(parsed.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
            await Assert.That(parsed.HasOperandTokens).IsTrue();
            await Assert.That(nested.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
            await Assert.That(nested.HasOperandTokens).IsTrue();
        }
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
                Required("Path", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "terraform",
                "Usage: terraform [global options] import [options] ADDRESS ID",
                ["terraform", "import"],
                Required("Address", phase: CommandLinePhase.Passthrough),
                Required("Id", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "cargo",
                "Usage: cargo run [OPTIONS] [ARGS]...",
                ["cargo", "run"],
                Optional("Args", isVariadic: true, phase: CommandLinePhase.Passthrough)),
            Fixture(
                "packer",
                "Usage: packer build [options] TEMPLATE",
                ["packer", "build"],
                Required("Template", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "gh",
                "USAGE\n  gh api <endpoint> [flags]",
                ["gh", "api"],
                Required("Endpoint")),
            Fixture(
                "podman",
                "Usage:\n  podman attach [options] CONTAINER",
                ["podman", "attach"],
                Required("Container", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "buildah",
                "Usage: buildah add [options] container source [source ...] destination",
                ["buildah", "add"],
                Required("Container", phase: CommandLinePhase.Passthrough),
                Required("Source", isVariadic: true, phase: CommandLinePhase.Passthrough),
                Required("Destination", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "newman",
                "Usage: newman run [options] <collection|URL>",
                ["newman", "run"],
                Required("Collection", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "docker",
                "Usage: docker exec [OPTIONS] CONTAINER COMMAND [ARG...]",
                ["docker", "exec"],
                Required("Container", phase: CommandLinePhase.Passthrough),
                Required("Command", phase: CommandLinePhase.Passthrough),
                Optional("Arg", isVariadic: true, phase: CommandLinePhase.Passthrough)),
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
                await Assert.That(actual.Phase).IsEqualTo(expected.Phase).Because(fixture.Tool);
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

        await Assert.That(source.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(source.PositionIndex).IsEqualTo(0);
        await Assert.That(destination.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(destination.PositionIndex).IsEqualTo(0);

        var globalOptions = UsageSynopsisParser.Parse(
            "Usage: tool [GLOBAL OPTIONS] copy <SOURCE>",
            ["tool", "copy"]);

        await Assert.That(globalOptions.PositionalArguments.Single().Phase)
            .IsEqualTo(CommandLinePhase.Passthrough);
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
    [Arguments("[--no-secrets]:")]
    [Arguments("[--json]:")]
    [Arguments("[--debug]:")]
    [Arguments("[--file=]:")]
    public async Task Ignores_Punctuated_Option_Tokens(string optionToken)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run [target] {optionToken}",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("Target");
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
    [Arguments("Usage: brew services stop (formula | --all)")]
    [Arguments("Usage: brew services stop (--all | formula)")]
    public async Task Models_Operand_Or_Option_Alternatives_As_Optional(string helpText)
    {
        var result = UsageSynopsisParser.Parse(
            helpText,
            ["brew", "services", "stop"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Formula");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    [Arguments("SRC_PATH|-")]
    [Arguments("-|SRC_PATH")]
    public async Task Treats_Lone_Dash_Alternative_As_A_Required_Operand(string sourceAlternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: docker cp {sourceAlternative} CONTAINER:DEST_PATH",
            ["docker", "cp"]);

        var source = result.PositionalArguments[0];
        using (Assert.Multiple())
        {
            await Assert.That(source.PropertyName).IsEqualTo("SrcPath");
            await Assert.That(source.IsRequired).IsTrue();
            await Assert.That(source.CSharpType).IsEqualTo("string");
        }
    }

    [Test]
    [Arguments("--format=<json|yaml>")]
    [Arguments("[--format <json|yaml>]")]
    [Arguments("[--format {json|yaml}]")]
    [Arguments("[--format [json|yaml]]")]
    [Arguments("[--format JSON|YAML]")]
    [Arguments("[--format JSON | YAML]")]
    [Arguments("[-f|--format <json|yaml>]")]
    [Arguments("[-f|--format JSON|YAML]")]
    [Arguments("[-f|--format=<json|yaml>]")]
    [Arguments("[-f|--format=JSON|YAML]")]
    [Arguments("[-f | --format <json|yaml>]")]
    [Arguments("[-f | --format JSON | YAML]")]
    public async Task Does_Not_Model_Option_Value_Alternatives_As_Operands(string option)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run {option}",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments).IsEmpty();
    }

    [Test]
    [Arguments("--config|KEY=VALUE")]
    [Arguments("KEY=VALUE|--config")]
    public async Task Models_Assignment_Operand_Alternatives_As_Optional(string alternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run {alternative}",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("KeyValue");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Spaced_Mixed_Alternatives_Are_Not_Option_Values()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run (--config | KEY=VALUE | FILE)",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("KeyValue");
            await Assert.That(argument.IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Does_Not_Model_Option_Control_Alternatives_As_Operands()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run (options | --all)",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Precommand_Mixed_Alternatives_Select_Passthrough_Phase()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool (--config | CONFIG) run <SOURCE>",
            ["tool", "run"]);

        var source = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(source.PropertyName).IsEqualTo("Source");
            await Assert.That(source.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    [Test]
    [Arguments("--fast|DEST")]
    [Arguments("DEST|--fast")]
    public async Task Mixed_Alternatives_Transition_Following_Operands(string alternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool copy SOURCE ({alternative}) TARGET",
            ["tool", "copy"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("Source");
            await Assert.That(result.PositionalArguments[0].Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("Dest");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsFalse();
            await Assert.That(result.PositionalArguments[1].Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(result.PositionalArguments[2].PropertyName).IsEqualTo("Target");
            await Assert.That(result.PositionalArguments[2].Phase).IsEqualTo(CommandLinePhase.Passthrough);
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
        await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
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
                argument.Phase == CommandLinePhase.Passthrough))
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
            "[property: CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)] string Source");
        await Assert.That(generated).Contains(
            "[CliArgument(1, Phase = CommandLinePhase.EarlyOperand)]");
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

    [Test]
    public async Task Model_Accepts_Usage_Operand_Covered_By_Named_Option()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: winget search [[-q] <query>] [<options>]",
            ["winget", "search"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "winget search",
            CommandParts = ["search"],
            ClassName = "WingetSearchOptions",
            ParentClassName = "WingetOptions",
            ToolNamespacePrefix = "Winget",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--query",
                    ShortForm = "-q",
                    PropertyName = "Query",
                    CSharpType = "string?",
                },
            ],
        };

        command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        await Assert.That(usage.PositionalArguments.Single().PropertyName)
            .IsEqualTo("Query");
        await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
            .IsEqualTo("-q");
    }

    [Test]
    public async Task Model_Accepts_Multiword_Usage_Operand_Covered_By_Named_Option()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: grype explain --id [VULNERABILITY ID] [flags]",
            ["grype", "explain"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "grype explain",
            CommandParts = ["explain"],
            ClassName = "GrypeExplainOptions",
            ParentClassName = "GrypeOptions",
            ToolNamespacePrefix = "Grype",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--id",
                    PropertyName = "Id",
                    CSharpType = "string?",
                },
            ],
        };

        command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().PropertyName)
                .IsEqualTo("VulnerabilityId");
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("--id");
        }
    }

    [Test]
    public async Task Associates_Standalone_Option_Value_With_Its_Switch()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: kubectl diff -f FILENAME",
            ["kubectl", "diff"]);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Filename");
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("-f");
        }
    }

    [Test]
    public async Task Model_Rejects_Standalone_Operand_Sharing_Named_Option_Property()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool run [--output OUTPUT] OUTPUT",
            ["tool", "run"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "string?",
                },
            ],
        };

        void Validate() => command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        await Assert.That(Validate)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no CliPositionalArgument");
    }

    [Test]
    public async Task Model_Rejects_Operand_After_SameNamed_Boolean_Option()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool run [--output] OUTPUT",
            ["tool", "run"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
            ],
        };

        void Validate() => command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("--output");
            await Assert.That(Validate)
                .Throws<InvalidOperationException>()
                .And.HasMessageContaining("no CliPositionalArgument");
        }
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
        CommandLinePhase phase = CommandLinePhase.EarlyOperand) =>
        new(propertyName, IsRequired: true, IsVariadic: isVariadic, Phase: phase);

    private static ExpectedArgument Optional(
        string propertyName,
        bool isVariadic = false,
        CommandLinePhase phase = CommandLinePhase.EarlyOperand) =>
        new(propertyName, IsRequired: false, IsVariadic: isVariadic, Phase: phase);

    private sealed record OperandFixture(
        string Tool,
        string HelpText,
        string[] CommandPath,
        IReadOnlyList<ExpectedArgument> ExpectedArguments);

    private sealed record ExpectedArgument(
        string PropertyName,
        bool IsRequired,
        bool IsVariadic,
        CommandLinePhase Phase);

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
