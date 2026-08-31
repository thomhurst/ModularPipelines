using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class CliScraperTraversalTests
{
    [Test]
    public async Task SharedTraversal_Discovers_ExecutableParent_And_Children_Without_Command_Placeholders()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                A tool-neutral CLI.

                Usage:
                  fake <command> [flags]

                Available Commands:
                  parent:  Execute or manage parent resources
                """,
            ["parent --help"] = """
                Execute or manage parent resources.

                Usage:
                  fake parent [TARGET] <command> [flags]

                Available Commands:
                  child:  Execute a child command

                Flags:
                  --scope string   Select a scope
                """,
            ["parent child --help"] = """
                Execute a child command.

                Usage:
                  fake parent child [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake parent", "fake parent child"]);
        await Assert.That(commands.Single(command => command.FullCommand == "fake parent").Options)
            .Contains(option => option.SwitchName == "--scope");
        await Assert.That(commands.Single(command => command.FullCommand == "fake parent")
                .PositionalArguments.Single().PropertyName)
            .IsEqualTo("Target");
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "parent --help", "parent child --help"]);
    }

    [Test]
    public async Task SharedTraversal_Preserves_Command_Operand_When_Only_Skipped_Children_Are_Extracted()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake COMMAND

                Available Commands:
                  parent:  Execute a parent command
                """,
            ["parent --help"] = """
                Usage:
                  fake parent COMMAND [flags]

                Available Commands:
                  help:  Show help

                Flags:
                  --scope string   Select a scope
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var command = (await ScrapeAsync(scraper)).Single();

        await Assert.That(command.FullCommand).IsEqualTo("fake parent");
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("Command");
        await Assert.That(executor.Arguments).IsEquivalentTo(["--help", "parent --help"]);
    }

    [Test]
    public async Task SharedTraversal_Retains_Empty_Command_Group_Definition()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake COMMAND

                Available Commands:
                  parent:  Manage parent resources
                """,
            ["parent --help"] = """
                Usage:
                  fake parent COMMAND

                Available Commands:
                  child:  Execute a child command
                """,
            ["parent child --help"] = """
                Usage:
                  fake parent child [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);
        var parent = commands.Single(command => command.FullCommand == "fake parent");

        await Assert.That(parent.Options).IsEmpty();
        await Assert.That(parent.PositionalArguments).IsEmpty();
        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake parent", "fake parent child"]);
    }

    [Test]
    public async Task SharedTraversal_Skips_Invalid_Group_And_Continues_With_Sibling()
    {
        var emptyGroupHelp = """
            Manage parent resources.

            Usage:
              fake parent <command>

            Available Commands:
            """.ReplaceLineEndings("\r\n");
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                A broken CLI.

                Usage:
                  fake <command>

                Available Commands:
                  parent:  Manage parent resources
                  sibling: Execute a sibling command
                """,
            ["parent --help"] = emptyGroupHelp,
            ["sibling --help"] = """
                Execute a sibling command.

                Usage:
                  fake sibling [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var logger = new RecordingLogger();
        var scraper = new TestCobraScraper(executor, logger);

        await Assert.That(scraper.DeclaresCommandGroup(emptyGroupHelp)).IsTrue();
        await Assert.That(scraper.GetSubcommands(emptyGroupHelp)).IsEmpty();

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake sibling"]);
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "parent --help", "sibling --help"]);
        await Assert.That(logger.Warnings).Contains(warning =>
            warning.Exception is InvalidOperationException
            && warning.Message.Contains("Failed to validate subcommand discovery: fake parent"));
    }

    [Test]
    public async Task SharedTraversal_Detects_Command_On_Second_Usage_Line()
    {
        const string helpText = """
            Usage:
              fake [flags]
              fake <command> [flags]

            Available Commands:
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        await Assert.That(scraper.DeclaresCommandGroup(helpText)).IsTrue();
    }

    [Test]
    public async Task DockerTraversal_Detects_And_Skips_Builder_Alias_Tree()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  docker <command>

                Management Commands:
                  builder     Manage builds
                  buildx      Docker Buildx
                """,
            ["builder --help"] = """
                Usage:
                  docker buildx [OPTIONS] COMMAND
                """,
            ["buildx --help"] = """
                Usage: docker buildx [OPTIONS] COMMAND

                Flags:
                  --builder string   Override the builder

                Commands:
                  build   Start a build
                  dap     Start a debugger
                """,
            ["buildx build --help"] = """
                Usage: docker buildx build [OPTIONS] PATH

                Flags:
                  --pull   Always attempt to pull
                """,
            ["buildx dap --help"] = """
                Usage: docker buildx dap [OPTIONS] COMMAND

                Commands:
                  build   Debug a build
                """,
            ["buildx dap build --help"] = """
                Usage: docker buildx dap build [OPTIONS] COMMAND [ARG...]

                Flags:
                  --debug   Enable debug logging
                """,
        });
        var scraper = new DockerCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<DockerCliScraper>.Instance);

        var commands = await ScrapeAsync(scraper);
        var alias = scraper.CreateToolDefinition().CommandGroupAliases.Single();

        await Assert.That(commands.Select(command => command.FullCommand))
            .Contains("docker buildx");
        await Assert.That(commands.Single(command => command.FullCommand == "docker buildx")
            .PositionalArguments).IsEmpty();
        await Assert.That(commands.Single(command => command.FullCommand == "docker buildx dap")
            .PositionalArguments).IsEmpty();
        await Assert.That(commands.Single(command => command.FullCommand == "docker buildx dap build")
            .PositionalArguments.Select(argument => argument.PropertyName))
            .Contains("Command");
        await Assert.That(alias.Alias).IsEqualTo("builder");
        await Assert.That(alias.CanonicalCommand).IsEqualTo("buildx");
        await Assert.That(executor.Arguments).DoesNotContain("builder build --help");
    }

    [Test]
    public async Task DockerTraversal_Removes_Placeholder_From_NonBuildx_Command_Group()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  docker COMMAND

                Management Commands:
                  compose    Docker Compose
                """,
            ["compose --help"] = """
                Usage: docker compose [OPTIONS] COMMAND

                Options:
                  --ansi string    Control ANSI output

                Commands:
                  build    Build services
                """,
            ["compose build --help"] = """
                Usage: docker compose build [OPTIONS] [SERVICE...]

                Options:
                  --pull    Always attempt to pull
                """,
        });
        var scraper = new DockerCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<DockerCliScraper>.Instance);

        var commands = await ScrapeAsync(scraper);
        var compose = commands.Single(command => command.FullCommand == "docker compose");

        await Assert.That(compose.PositionalArguments).IsEmpty();
        await Assert.That(commands.Single(command => command.FullCommand == "docker compose build")
                .PositionalArguments.Single().PropertyName)
            .IsEqualTo("Service");
    }

    [Test]
    public async Task CobraTraversal_Parses_Command_Headings_With_Lowercase_Connectors()
    {
        const string helpText = """
            Configuration and Management Commands:
              addons    Manage addons
              config    Manage configuration

            Networking and Connectivity Commands:
              service   Connect to a service
              tunnel    Connect to load balancers
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        await Assert.That(scraper.GetSubcommands(helpText))
            .IsEquivalentTo(["addons", "config", "service", "tunnel"]);
    }

    [Test]
    public async Task CobraTraversal_Does_Not_Treat_Trailing_Guidance_As_Commands()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake [command]

                Available Commands:
                  run    Run the operation

                Use "fake [command] --help" for more information.

                Learn More
                  Read the CLI reference.
                """,
            ["run --help"] = """
                Usage:
                  fake run [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake run"]);
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "run --help"]);
    }

    [Test]
    public async Task CobraTraversal_Does_Not_Treat_Title_Cased_Prose_As_Command_Section()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake [command]

                List Installed Commands
                Use "fake [command] --help" for more information.
                guidance    Read the CLI reference.
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
        await Assert.That(executor.Arguments).IsEquivalentTo(["--help"]);
    }

    [Test]
    public async Task CobraTraversal_Stops_When_Nested_Command_Reprints_Parent_Help()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake [command]

                Commands:
                  init    Initialize a project
                """,
            ["init --help"] = """
                Usage:
                  fake [command]

                Commands:
                  init    Initialize a project
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "init --help"]);
    }

    [Test]
    public async Task PodmanTraversal_Uses_ComposeProvider_Help()
    {
        var executor = new ComposeProviderExecutor();
        var scraper = new TestPodmanCliScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .Contains("podman compose build");
        await Assert.That(executor.Invocations)
            .Contains(("docker-compose-shim", "build --help"));
    }

    [Test]
    public async Task PodmanTraversal_Rejects_Recursive_ComposeProvider_Help()
    {
        var scraper = new TestPodmanCliScraper(new RecursiveComposeProviderExecutor());

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
    }

    [Test]
    [Arguments("container clone", "CONTAINER NAME IMAGE", 3)]
    [Arguments("pod clone", "POD NAME", 2)]
    public async Task PodmanClone_Keeps_Defaulted_Output_Operands_Optional(
        string command,
        string operands,
        int expectedCount)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"Usage: podman {command} [options] {operands}";

        var definition = await scraper.Parse(commandPath, helpText);

        using (Assert.Multiple())
        {
            await Assert.That(definition!.PositionalArguments).Count().IsEqualTo(expectedCount);
            await Assert.That(definition.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(definition.PositionalArguments.Skip(1).All(argument => !argument.IsRequired)).IsTrue();
            await Assert.That(definition.PositionalArguments.Skip(1).All(argument => argument.CSharpType == "string?")).IsTrue();
        }
    }

    [Test]
    [Arguments("exec")]
    [Arguments("container exec")]
    public async Task PodmanExec_Requires_Container_And_Command(string command)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"Usage: podman {command} [options] CONTAINER COMMAND [ARG...]";

        var definition = await scraper.Parse(commandPath, helpText);

        using (Assert.Multiple())
        {
            await Assert.That(definition!.PositionalArguments).Count().IsEqualTo(3);
            await Assert.That(definition.PositionalArguments.Take(2).All(argument => argument.IsRequired)).IsTrue();
            await Assert.That(definition.PositionalArguments.Take(2).All(argument => argument.CSharpType == "string")).IsTrue();
            await Assert.That(definition.PositionalArguments[2].IsRequired).IsFalse();
            await Assert.That(definition.PositionalArguments[2].CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    [Arguments("secret exists", "SECRET")]
    [Arguments("secret inspect", "SECRET [SECRET...]")]
    [Arguments("secret rm", "SECRET [SECRET...]")]
    public async Task PodmanSecret_Identifiers_Are_Not_Secret_Values(
        string command,
        string operands)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"Usage: podman {command} [options] {operands}";

        var definition = await scraper.Parse(commandPath, helpText);

        await Assert.That(definition!.PositionalArguments.All(argument => !argument.IsSecret)).IsTrue();
    }

    [Test]
    public async Task Podman_Creds_Option_Is_Secret()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        const string helpText = """
            Usage: podman pull [options] IMAGE

            Flags:
              --creds string   Credentials (USERNAME:PASSWORD) to use for authenticating to a registry
              --creds-helper string   Select a credential helper by name
            """;

        var definition = await scraper.Parse(["podman", "pull"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(definition!.Options.Single(option => option.PropertyName == "Creds").IsSecret)
                .IsTrue();
            await Assert.That(definition.Options.Single(option => option.PropertyName == "CredsHelper").IsSecret)
                .IsFalse();
        }
    }

    [Test]
    [Arguments("artifact rm", "ARTIFACT [ARTIFACT...]")]
    [Arguments("quadlet rm", "QUADLET [QUADLET...]")]
    public async Task Podman_All_Removal_Operands_Are_Optional(
        string command,
        string operands)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"Usage: podman {command} [options] {operands}";

        var definition = await scraper.Parse(commandPath, helpText);
        var argument = definition!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(GeneratorUtils.BuildOptionsParameter(definition))
                .IsEqualTo($"{definition.ClassName}? options = null");
        }
    }

    [Test]
    public async Task Podman_Artifact_Add_Accepts_Multiple_Paths()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        const string helpText = "Usage: podman artifact add [options] ARTIFACT PATH";

        var definition = await scraper.Parse(["podman", "artifact", "add"], helpText);
        var arguments = definition!.PositionalArguments;

        using (Assert.Multiple())
        {
            await Assert.That(arguments).Count().IsEqualTo(2);
            await Assert.That(arguments[0].PropertyName).IsEqualTo("Artifact");
            await Assert.That(arguments[0].CSharpType).IsEqualTo("string");
            await Assert.That(arguments[0].IsRequired).IsTrue();
            await Assert.That(arguments[1].PropertyName).IsEqualTo("Path");
            await Assert.That(arguments[1].CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(arguments[1].IsRequired).IsTrue();
            await Assert.That(arguments[1].IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Podman_Positional_Fix_Fails_When_Expected_Operand_Is_Missing()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        const string helpText = "Usage: podman artifact add [options] ARTIFACT";

        await Assert.That(async () =>
                await scraper.Parse(["podman", "artifact", "add"], helpText))
            .Throws<InvalidDataException>();
    }

    [Test]
    [Arguments("kube down")]
    [Arguments("kube play")]
    public async Task Podman_Kube_Files_Preserve_The_Required_Scalar_And_Add_Repeatability(string command)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"Usage: podman {command} [options] [KUBEFILE [KUBEFILE...]]|-";

        var definition = await scraper.Parse(commandPath, helpText);
        var arguments = definition!.PositionalArguments;

        using (Assert.Multiple())
        {
            await Assert.That(arguments).Count().IsEqualTo(2);
            await Assert.That(arguments[0].PropertyName).IsEqualTo("Kubefile");
            await Assert.That(arguments[0].IsRequired).IsTrue();
            await Assert.That(arguments[0].IsVariadic).IsFalse();
            await Assert.That(arguments[0].CSharpType).IsEqualTo("string");
            await Assert.That(arguments[1].PropertyName).IsEqualTo("AdditionalKubefiles");
            await Assert.That(arguments[1].IsRequired).IsFalse();
            await Assert.That(arguments[1].IsVariadic).IsTrue();
            await Assert.That(arguments[1].CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Podman_Variadic_Fix_Preserves_The_Parsed_Element_Type()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var fixedArguments = scraper.ApplyPositionalFixes(
            "artifact add",
            [
                new CliPositionalArgument
                {
                    PropertyName = "Artifact",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "Path",
                    CSharpType = "int?",
                },
            ]);

        await Assert.That(fixedArguments[1].CSharpType).IsEqualTo("IEnumerable<int>");
    }

    [Test]
    public async Task Podman_Kube_File_Fix_Preserves_Additional_Parsed_Operands()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        const string helpText = "Usage: podman kube play [options] [KUBEFILE...] INPUT";

        var definition = await scraper.Parse(["podman", "kube", "play"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(definition!.PositionalArguments).Count().IsEqualTo(3);
            await Assert.That(definition.PositionalArguments[0].PropertyName).IsEqualTo("Kubefile");
            await Assert.That(definition.PositionalArguments[1].PropertyName).IsEqualTo("AdditionalKubefiles");
            await Assert.That(definition.PositionalArguments[2].PropertyName).IsEqualTo("Input");
            await Assert.That(definition.PositionalArguments.Select(argument => argument.PositionIndex))
                .IsEquivalentTo([0, 1, 2]);
        }
    }

    [Test]
    [Arguments("generate kube", "NoTrunc")]
    [Arguments("kube generate", "NoTrunc")]
    [Arguments("kube play", "NoTrunc")]
    [Arguments("machine rm", "SaveKeys")]
    public async Task Podman_Removed_Flags_Are_Retained_As_Compatibility_Properties(
        string command,
        string propertyName)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var operands = command == "kube play" ? " [KUBEFILE...]" : string.Empty;
        var helpText = $"Usage: podman {command} [options]{operands}";

        var definition = await scraper.Parse(commandPath, helpText);
        var property = definition!.CompatibilityProperties.Single();

        using (Assert.Multiple())
        {
            await Assert.That(property.PropertyName).IsEqualTo(propertyName);
            await Assert.That(property.CSharpType).IsEqualTo("bool?");
            await Assert.That(property.ForwardToPropertyName).IsNull();
        }
    }

    [Test]
    public async Task Podman_Machine_Init_Retains_Removed_Compatibility_Properties()
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        const string helpText = "Usage: podman machine init [options] [NAME]";

        var definition = await scraper.Parse(["podman", "machine", "init"], helpText);
        var properties = definition!.CompatibilityProperties.ToDictionary(property => property.PropertyName);

        using (Assert.Multiple())
        {
            await Assert.That(properties.Keys).IsEquivalentTo(["ImagePath", "VolumeDriver"]);
            await Assert.That(properties["ImagePath"].CSharpType).IsEqualTo("string?");
            await Assert.That(properties["ImagePath"].ForwardToPropertyName).IsEqualTo("Image");
            await Assert.That(properties["VolumeDriver"].CSharpType).IsEqualTo("string?");
            await Assert.That(properties["VolumeDriver"].ForwardToPropertyName).IsNull();
        }
    }

    [Test]
    [Arguments("build")]
    [Arguments("image build")]
    public async Task Podman_Build_Output_Preserves_Scalar_And_Exposes_Collection(string command)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"""
            Usage: podman {command} [options] CONTEXT

            Flags:
              -o, --output strings   output destination
            """;

        var definition = await scraper.Parse(commandPath, helpText);
        var outputs = definition!.Options.Single(option => option.PropertyName == "Outputs");
        var output = definition.CompatibilityProperties.Single(property => property.PropertyName == "Output");

        using (Assert.Multiple())
        {
            await Assert.That(output.CSharpType).IsEqualTo("string?");
            await Assert.That(output.ForwardToPropertyName).IsEqualTo("Outputs");
            await Assert.That(output.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.ScalarToCollection);
            await Assert.That(outputs.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(outputs.AcceptsMultipleValues).IsTrue();
        }
    }

    [Test]
    [Arguments("build")]
    [Arguments("farm build")]
    [Arguments("image build")]
    public async Task Podman_Build_Timestamp_Preserves_Integer_And_Exposes_String(string command)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();
        var helpText = $"""
            Usage: podman {command} [options] CONTEXT

            Flags:
                  --timestamp string   set build timestamp
            """;

        var definition = await scraper.Parse(commandPath, helpText);
        var timestampValue = definition!.Options.Single(option => option.PropertyName == "TimestampValue");
        var timestamp = definition.CompatibilityProperties.Single(property => property.PropertyName == "Timestamp");

        using (Assert.Multiple())
        {
            await Assert.That(timestamp.CSharpType).IsEqualTo("int?");
            await Assert.That(timestamp.ForwardToPropertyName).IsEqualTo("TimestampValue");
            await Assert.That(timestamp.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableInt32ToString);
            await Assert.That(timestampValue.CSharpType).IsEqualTo("string?");
            await Assert.That(timestampValue.IsNumeric).IsFalse();
        }
    }

    [Test]
    [Arguments("inspect", "Usage: podman inspect [options] ARTIFACT [ARTIFACT...]", "Container")]
    [Arguments("manifest add", "Usage: podman manifest add [options] LIST IMAGEORARTIFACT [IMAGEORARTIFACT...]", "Image")]
    [Arguments("manifest annotate", "Usage: podman manifest annotate [options] LIST IMAGEORARTIFACT", "Image")]
    [Arguments("manifest remove", "Usage: podman manifest remove [options] LIST DIGEST", "Image")]
    public async Task Podman_Retains_Existing_Positional_Property_Names(
        string command,
        string helpText,
        string expectedPropertyName)
    {
        var scraper = new TestPodmanCliScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
        var commandPath = new[] { "podman" }.Concat(command.Split(' ')).ToArray();

        var definition = await scraper.Parse(commandPath, helpText);

        await Assert.That(definition!.PositionalArguments.Last().PropertyName)
            .IsEqualTo(expectedPropertyName);
    }

    [Test]
    public async Task SharedShapeInference_Models_Documented_Repeatability()
    {
        const string helpText = """
            Execute a command.

            Usage:
              fake execute [flags]

            Flags:
              --tag string   May be specified multiple times
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        var command = await scraper.Parse(["fake", "execute"], helpText);
        var tag = command!.Options.Single();

        await Assert.That(tag.AcceptsMultipleValues).IsTrue();
        await Assert.That(tag.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    [Arguments("Accepts multiple values")]
    [Arguments("One or more label selectors")]
    public async Task SharedShapeInference_Preserves_Common_Repeatability_Phrases(string description)
    {
        var helpText = $"""
            Execute a command.

            Usage:
              fake execute [flags]

            Flags:
              --tag string   {description}
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        var command = await scraper.Parse(["fake", "execute"], helpText);
        var tag = command!.Options.Single();

        await Assert.That(tag.AcceptsMultipleValues).IsTrue();
        await Assert.That(tag.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task SharedShapeInference_Models_Optional_Cobra_Option_Values()
    {
        const string helpText = """
            Create a provider.

            Usage:
              fake provider [flags]

            Flags:
              --draft string[="new"]   Set without a value to create a draft
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        var command = await scraper.Parse(["fake", "provider"], helpText);
        var draft = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(draft.CSharpType).IsEqualTo("string?");
            await Assert.That(draft.IsFlag).IsFalse();
            await Assert.That(draft.ValueSeparator).IsEqualTo("=");
            await Assert.That(draft.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        }
    }

    [Test]
    public async Task SharedTraversal_Skips_One_Invalid_Option_Shape()
    {
        const string helpText = """
            Execute a command.

            Usage:
              fake [flags]

            Flags:
              --tag string   May be specified multiple times
            """;
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = helpText,
        });
        var scraper = new OptionShapeScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
    }

    [Test]
    public async Task SharedTraversal_Preserves_Boolean_Value_Options_With_Repeatability_Prose()
    {
        const string helpText = """
            Execute a command.

            Usage:
              fake [flags]

            Flags:
              --tag=<true|false>   May be specified multiple times
            """;
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = helpText,
        });
        var scraper = new OptionShapeScraper(executor, "bool?");

        var command = (await ScrapeAsync(scraper)).Single();
        var option = command.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    public async Task SharedTraversal_Skips_Invalid_Operand_Coverage()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Execute a command.

                Usage:
                  fake <TARGET>
                """,
        });
        var logger = new RecordingLogger();
        var scraper = new OperandCoverageMismatchScraper(executor, logger);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
        await Assert.That(logger.Warnings).Contains(warning =>
            warning.Exception is InvalidOperationException
            && warning.Message.Contains("Failed to parse command: fake"));
    }

    [Test]
    public async Task PnpmTraversal_Does_Not_Recurse_Into_Repeated_Parent_Help()
    {
        const string stageHelp = """
            Usage: pnpm stage publish [<tarball>|<dir>] [options]
                   pnpm stage download <stage-id>

            Subcommands:
                  download    Download a staged package
                  publish     Stage a package for publishing

            Options:
                  --dry-run   Do everything except upload
                  --json      Show information in JSON format
            """;
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage: pnpm [command] [flags]

                Other:
                      stage    Stage packages for publishing

                Options:
                  -r, --recursive    Run recursively
                """,
            ["stage --help"] = stageHelp,
            ["stage download --help"] = stageHelp,
            ["stage publish --help"] = stageHelp,
        });
        var scraper = new TestPnpmCliScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["pnpm stage", "pnpm stage download", "pnpm stage publish"]);
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "stage --help", "stage download --help", "stage publish --help"]);
        await Assert.That(commands.Single(command => command.FullCommand == "pnpm stage publish").Options
                .Where(option => option.SwitchName is "--dry-run" or "--json")
                .Select(option => (option.SwitchName, option.CSharpType, option.IsFlag)))
            .IsEquivalentTo([
                ("--dry-run", "bool?", true),
                ("--json", "bool?", true),
            ]);
    }

    [Test]
    public async Task Shared_Skip_Filter_Preserves_Uppercase_Subcommands()
    {
        var scraper = new OptionShapeScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        await Assert.That(scraper.Skips("SSH")).IsFalse();
    }

    private static async Task<IReadOnlyList<CliCommandDefinition>> ScrapeAsync(ICliScraper scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class TestCobraScraper : CobraCliScraper
    {
        public TestCobraScraper(ICliCommandExecutor executor, ILogger? logger = null)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                logger ?? NullLogger<TestCobraScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override int MaxParallelism => 2;

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public bool DeclaresCommandGroup(string helpText) => HelpDeclaresCommandGroup(helpText);

        public IReadOnlyList<string> GetSubcommands(string helpText) => ExtractSubcommands(helpText).ToList();
    }

    private sealed class RecordingLogger : ILogger
    {
        public List<(string Message, Exception? Exception)> Warnings { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Warnings.Add((formatter(state, exception), exception));
            }
        }
    }

    private sealed class TestPodmanCliScraper(ICliCommandExecutor executor)
        : PodmanCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PodmanCliScraper>.Instance)
    {
        protected override string? ComposeProviderPath => "docker-compose-shim";

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public IReadOnlyList<CliPositionalArgument> ApplyPositionalFixes(
            string command,
            IReadOnlyList<CliPositionalArgument> positionalArguments) =>
            ApplyPositionalArgumentFixes(command.Split(' '), positionalArguments);
    }

    private sealed class TestPnpmCliScraper(ICliCommandExecutor executor)
        : PnpmCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PnpmCliScraper>.Instance);

    private sealed class ComposeProviderExecutor : ICliCommandExecutor
    {
        public List<(string Command, string Arguments)> Invocations { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Invocations.Add((command, arguments));
            var helpText = (command, arguments) switch
            {
                ("podman", "--help") => """
                    Usage: podman [OPTIONS] COMMAND

                    Commands:
                      compose    Run Compose workloads
                    """,
                ("docker-compose-shim", "--help") => """
                    Usage: docker compose [OPTIONS] COMMAND

                    Options:
                      --ansi string    Control ANSI output

                    Commands:
                      build    Build services
                    """,
                ("docker-compose-shim", "build --help") => """
                    Usage: docker compose build [OPTIONS] [SERVICE...]

                    Options:
                      --pull    Always attempt to pull
                    """,
                _ => throw new InvalidOperationException(
                    $"Unexpected invocation: {command} {arguments}"),
            };

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class RecursiveComposeProviderExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var helpText = command switch
            {
                "podman" => """
                    Usage: podman [OPTIONS] COMMAND

                    Commands:
                      compose    Run Compose workloads
                    """,
                "docker-compose-shim" => """
                    Usage: podman [OPTIONS] COMMAND

                    Commands:
                      compose    Run Compose workloads
                    """,
                _ => throw new InvalidOperationException($"Unexpected invocation: {command} {arguments}"),
            };
            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class OptionShapeScraper : CliScraperBase
    {
        private readonly string _csharpType;

        public OptionShapeScraper(ICliCommandExecutor executor, string csharpType = "string?")
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<OptionShapeScraper>.Instance)
        {
            _csharpType = csharpType;
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        public bool Skips(string subcommand) => IsSkippableSubcommand(subcommand);

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--tag",
                        PropertyName = "Tag",
                        CSharpType = _csharpType,
                        Description = "May be specified multiple times",
                    },
                ],
            });
    }

    private sealed class OperandCoverageMismatchScraper : CliScraperBase
    {
        public OperandCoverageMismatchScraper(ICliCommandExecutor executor, ILogger logger)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                logger)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options = [],
            });
    }

    private sealed class StubExecutor(IReadOnlyDictionary<string, string> helpByArguments)
        : ICliCommandExecutor
    {
        private readonly object _gate = new();
        private readonly List<string> _arguments = [];

        public IReadOnlyList<string> Arguments
        {
            get
            {
                lock (_gate)
                {
                    return _arguments.ToArray();
                }
            }
        }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            lock (_gate)
            {
                _arguments.Add(arguments);
            }

            if (!helpByArguments.TryGetValue(arguments, out var helpText))
            {
                throw new InvalidOperationException($"Unexpected arguments: {arguments}");
            }

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
