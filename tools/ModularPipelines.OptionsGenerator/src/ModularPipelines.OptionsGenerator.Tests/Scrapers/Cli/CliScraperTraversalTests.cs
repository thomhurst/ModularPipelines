using System.Text.Json;
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
    public async Task CobraParentDoesNotUseChildDescription()
    {
        const string helpText = """
            Usage:
              fake parent [command]

            Available Commands:
              child  Execute a child command

            Flags:
              --help  Show help
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "parent"],
            helpText);

        await Assert.That(command!.Description).IsNull();
    }

    [Test]
    public async Task CobraParentheticalCommandSectionDoesNotUseChildDescription()
    {
        const string helpText = """
            Usage:
              kubectl root [command]

            Basic Commands (Beginner):
              create  Create a resource from a file or stdin
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["kubectl", "root"],
            helpText);

        await Assert.That(command!.Description).IsNull();
    }

    [Test]
    public async Task CobraQualifiedExamplesHeaderIsNotUsedAsDescription()
    {
        const string helpText = """
            Usage:
              fake parent [flags]

            Examples: (see below)
              fake parent --all
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "parent"],
            helpText);

        await Assert.That(command!.Description).IsNull();
    }

    [Test]
    public async Task CobraDescriptionEndingInCommandsIsPreserved()
    {
        const string helpText = """
            Manage build Commands

            Usage:
              fake build [flags]
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "build"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Manage build Commands");
    }

    [Test]
    public async Task CobraDescriptionAfterMetadataHeaderIsPreserved()
    {
        const string helpText = """
            Usage:
              fake parent [flags]

            Metadata:
            Execute or manage parent resources.

            Flags:
              --help  Show help
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "parent"],
            helpText);

        await Assert.That(command!.Description)
            .IsEqualTo("Execute or manage parent resources.");
    }

    [Test]
    [Arguments("Aliases:\n  un, del, delete")]
    [Arguments("Additional help topics:\n  fake parent advanced  Advanced help")]
    [Arguments("Available Commands: (see below)\nchild  Execute a child command")]
    public async Task CobraListSectionIsNotUsedAsDescription(string section)
    {
        var helpText = $"""
            Usage:
              fake parent [flags]

            {section}
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "parent"],
            helpText);

        await Assert.That(command!.Description).IsNull();
    }

    [Test]
    public async Task CobraLabeledDescriptionWithIndentedContinuationIsPreserved()
    {
        const string helpText = """
            Note:
              Requires administrator privileges.

            Usage:
              fake parent [flags]
            """;

        var command = await new TestCobraScraper(new StubExecutor(new Dictionary<string, string>())).Parse(
            ["fake", "parent"],
            helpText);

        await Assert.That(command!.Description)
            .IsEqualTo("Requires administrator privileges.");
    }

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
    public async Task SharedTraversal_Discards_Required_Alternatives_With_Command_Placeholders()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake <command>

                Available Commands:
                  parent:  Manage parent resources
                """,
            ["parent --help"] = """
                Usage:
                  fake parent <command>
                  fake parent --all

                Available Commands:
                  child:  Execute a child command

                Flags:
                  --all   Select all resources
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
        var parent = commands.Single(command => command.FullCommand == "fake parent");

        await Assert.That(parent.Options).Contains(option => option.SwitchName == "--all");
        await Assert.That(parent.PositionalArguments).IsEmpty();
        await Assert.That(parent.RequiredAlternativeGroups).IsEmpty();
        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake parent", "fake parent child"]);
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

        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-cli-traversal-tests",
            Guid.NewGuid().ToString("N"));
        try
        {
            var diagnosticsPath = await scraper.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                CoverageFailure("fake parent child"),
                CancellationToken.None);
            var diagnostics = await File.ReadAllTextAsync(diagnosticsPath!);

            await Assert.That(diagnostics).Contains("Manage parent resources.");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task SharedTraversal_Preserves_Mismatched_Parent_Help_For_Diagnostics()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage:
                  fake <command>

                Available Commands:
                  parent: Manage parent resources
                """,
            ["parent --help"] = """
                MISMATCHED PARENT RESPONSE

                Usage:
                  fake sibling [flags]
                """,
        });
        var scraper = new TestCobraScraper(executor);

        _ = await ScrapeAsync(scraper);

        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-cli-traversal-tests",
            Guid.NewGuid().ToString("N"));
        try
        {
            var diagnosticsPath = await scraper.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                CoverageFailure("fake parent child"),
                CancellationToken.None);
            var diagnostics = await File.ReadAllTextAsync(diagnosticsPath!);

            await Assert.That(diagnostics).Contains("MISMATCHED PARENT RESPONSE");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task SharedTraversal_Preserves_Known_Group_That_Became_A_Leaf()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage: fake <command>

                Available Commands:
                  parent: Manage parent resources
                """,
            ["parent --help"] = """
                CURRENT PARENT LEAF HELP

                Usage: fake parent [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);
        scraper.PreserveRawHelpForCommandGroups(["fake parent"]);

        _ = await ScrapeAsync(scraper);

        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-cli-traversal-tests",
            Guid.NewGuid().ToString("N"));
        try
        {
            var diagnosticsPath = await scraper.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                CoverageFailure("fake parent child"),
                CancellationToken.None);
            var diagnostics = await File.ReadAllTextAsync(diagnosticsPath!);

            await Assert.That(diagnostics).Contains("CURRENT PARENT LEAF HELP");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Cached_Help_Is_Recorded_For_Coverage_Diagnostics()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        cache.Set("fake parent", "CACHED PARENT HELP");
        var scraper = new TestCobraScraper(
            new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)),
            cache: cache);

        _ = await scraper.GetHelp(["fake", "parent"]);

        var outputDirectory = Path.Combine(
            Path.GetTempPath(),
            "mp-cli-traversal-tests",
            Guid.NewGuid().ToString("N"));
        try
        {
            var diagnosticsPath = await scraper.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                CoverageFailure("fake parent child"),
                CancellationToken.None);
            var diagnostics = await File.ReadAllTextAsync(diagnosticsPath!);
            using var document = JsonDocument.Parse(diagnostics);
            var missingHelpPaths = document.RootElement
                .GetProperty("missingHelpPaths")
                .EnumerateArray()
                .Select(static element => element.GetString())
                .ToArray();

            await Assert.That(diagnostics).Contains("CACHED PARENT HELP");
            await Assert.That(missingHelpPaths).DoesNotContain("fake parent");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
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
        // pnpm prints the parent group's help for its leaf commands.
        const string stageHelp = """
            Stage packages for publishing

            Usage: pnpm stage [OPTIONS] <COMMAND>

            Commands:
              download    Download a staged package
              publish     Stage a package for publishing

            Options:
                  --dry-run
                      Do everything except upload

                  --json
                      Show information in JSON format
            """;
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Experimental package manager for node.js

                Usage: pnpm [OPTIONS] <COMMAND>

                Commands:
                  stage    Stage packages for publishing

                Options:
                  -r, --recursive
                          Run recursively
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
    public async Task PnpmTraversal_Classifies_Options_By_Their_Value_Placeholder()
    {
        // clap prints a placeholder for every value-taking option, so a bare switch is a flag.
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Experimental package manager for node.js

                Usage: pnpm [OPTIONS] <COMMAND>

                Commands:
                  audit    Checks for known security issues
                """,
            ["audit --help"] = """
                Checks for known security issues with the installed packages

                Usage: pnpm audit [OPTIONS] [PARAMS]...

                Options:
                  -D, --dev
                          Only inspect development dependencies

                  -P, --prod
                          Only inspect production dependencies

                      --ignore-registry-errors
                          Continue when a registry is unavailable

                      --audit-level <AUDIT_LEVEL>
                          Only print advisories at or above this level

                      --registry <REGISTRY>
                          Use the specified registry

                  -F, --filter <FILTER>...
                          Restrict packages by selector

                  -h, --help
                          Print help
                """,
        });
        var scraper = new TestPnpmCliScraper(executor);

        var commands = await ScrapeAsync(scraper);
        var options = commands.Single(command => command.FullCommand == "pnpm audit")
            .Options.ToDictionary(option => option.SwitchName, StringComparer.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(options.Keys).IsEquivalentTo(
                ["--dev", "--prod", "--ignore-registry-errors", "--audit-level", "--registry", "--filter"]);
            foreach (var switchName in new[] { "--dev", "--prod", "--ignore-registry-errors" })
            {
                await Assert.That(options[switchName].IsFlag).IsTrue();
                await Assert.That(options[switchName].CSharpType).IsEqualTo("bool?");
            }

            foreach (var switchName in new[] { "--audit-level", "--registry" })
            {
                await Assert.That(options[switchName].IsFlag).IsFalse();
                await Assert.That(options[switchName].CSharpType).IsEqualTo("string?");
            }

            await Assert.That(options["--filter"].CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(options["--filter"].ShortForm).IsEqualTo("-F");
        }
    }

    [Test]
    public async Task Shared_Skip_Filter_Preserves_Uppercase_Subcommands()
    {
        var scraper = new OptionShapeScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        await Assert.That(scraper.Skips("SSH")).IsFalse();
    }

    [Test]
    public async Task Cargo_Add_Models_Required_Dependency_Source_Alternatives()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Rust's package manager

                Usage: cargo [OPTIONS] <COMMAND>

                Commands:
                  add  Add dependencies to a Cargo.toml manifest file

                Options:
                  -h, --help  Print help
                """,
            ["add --help"] = """
                Add dependencies to a Cargo.toml manifest file

                Usage: cargo add [OPTIONS] <DEP_ID|--path <PATH>|--git <URI>>...

                Arguments:
                  <DEP_ID|--path <PATH>|--git <URI>>...  Reference to a package to add

                Options:
                  -F, --features <FEATURES>  Features to activate

                Source options:
                      --path <PATH>  Filesystem path to local crate to add
                      --git <URI>    Git repository location
                      --registry <REGISTRY>  Package registry to use
                      --rename <NAME>
                          Rename the dependency

                          Example uses:
                          - Depending on multiple versions of a crate

                Package Selection:
                      --workspace  Add dependencies to every workspace package

                Examples:
                      --pretend <VALUE>  This is documentation, not an option
                """,
        });

        var command = (await ScrapeAsync(new TestCargoCliScraper(executor))).Single();

        using (Assert.Multiple())
        {
            await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("DepId");
            await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse();
            await Assert.That(command.Options.Select(option => option.PropertyName))
                .Contains("Path")
                .And.Contains("Git")
                .And.Contains("Registry")
                .And.Contains("Workspace");
            await Assert.That(command.Options.Select(option => option.PropertyName))
                .DoesNotContain("Pretend");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Path").Description)
                .IsEqualTo("Filesystem path to local crate to add");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Git").Description)
                .IsEqualTo("Git repository location");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Registry").Description)
                .IsEqualTo("Package registry to use");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Rename").Description)
                .IsEqualTo("Rename the dependency");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Workspace").Description)
                .IsEqualTo("Add dependencies to every workspace package");
            await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
                .IsEquivalentTo(["DepId", "Path", "Git"]);
        }
    }

    [Test]
    public async Task Unresolved_Inferred_Alternative_Does_Not_Drop_Command()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage: fake <command>

                Available Commands:
                  create  Create a target
                """,
            ["create --help"] = """
                Usage: fake create (<TARGET>|--global)

                Arguments:
                  <TARGET>  Target name

                Options:
                  --known string  A command-local option
                """,
        });

        var command = (await ScrapeAsync(new TestCobraScraper(executor))).Single();

        await Assert.That(command.FullCommand).IsEqualTo("fake create");
        await Assert.That(command.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Required_Alternatives_Keep_Colliding_Option_And_Operand_Members()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage: fake <command>

                Available Commands:
                  create  Create a file
                """,
            ["create --help"] = """
                Usage: fake create (<FILENAME>|--filename <FILENAME>)

                Arguments:
                  <FILENAME>  Input file

                Options:
                  --filename string  Input file option
                """,
        });
        var command = (await ScrapeAsync(new TestCobraScraper(executor))).Single();

        var resolved = InheritedPropertyCollisionResolver.Resolve(new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [command],
        }).Commands.Single();

        await Assert.That(resolved.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(["Filename", "FilenameArgument"]);
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

    private static CommandCoverageEvaluation CoverageFailure(string removedCommand) => new()
    {
        ManifestPath = "unused",
        Manifest = new CommandCoverageManifest
        {
            FormatVersion = 1,
            ToolName = "fake",
            ToolVersion = "2.0",
            CommandCount = 1,
            CommandTreeSha256 = "unused",
            Commands = ["fake sibling"],
            CommandGroups = ["fake"],
            Exclusions = [],
        },
        HasPreviousBaseline = true,
        PreviousCommandCount = 2,
        PreviousToolVersion = "1.0",
        AddedCommands = [],
        RemovedCommands = [removedCommand],
        KnownGroupsWithoutChildren = ["fake parent"],
        Violations = ["Known command groups lost all children: fake parent."],
        ChangesApproved = false,
    };

    private sealed class TestCobraScraper : CobraCliScraper
    {
        public TestCobraScraper(
            ICliCommandExecutor executor,
            ILogger? logger = null,
            IHelpTextCache? cache = null)
            : base(
                executor,
                cache ?? new HelpTextCache(NullLogger<HelpTextCache>.Instance),
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

        public Task<string?> GetHelp(string[] commandPath) =>
            GetHelpTextAsync(commandPath, CancellationToken.None);
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

    private sealed class TestCargoCliScraper(ICliCommandExecutor executor)
        : CargoCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<CargoCliScraper>.Instance);

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
