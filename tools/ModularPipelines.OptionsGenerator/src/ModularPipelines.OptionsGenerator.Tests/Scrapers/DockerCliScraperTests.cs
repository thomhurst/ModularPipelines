using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DockerCliScraperTests
{
    private const string RootHelp = """
        Usage: docker [OPTIONS] COMMAND

        Flags:
          --root-only string   An action that is not inherited

        Global Options:
              --config string      Location of client config files
          -c, --context string     Name of the context to use to connect to the daemon
          -D, --debug              Enable debug mode
          -H, --host string        Daemon socket to connect to
          -l, --log-level string   Set the logging level ("debug", "info", "warn", "error", "fatal")
              --tls                Use TLS; implied by --tlsverify
              --tlscacert string   Trust certs signed only by this CA
              --tlscert string     Path to TLS certificate file
              --tlskey string      Path to TLS key file
              --tlsverify          Use TLS and verify the remote
          -v, --version            Print version information and quit

        Examples:
          --not-an-option string   An example outside the global section
        """;

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Global_Options_Use_Only_The_Explicit_Section(string lineEnding)
    {
        var options = new TestDockerCliScraper().ParseGlobals(RootHelp.Replace("\n", lineEnding));

        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(
            ["--config", "--context", "--debug", "--host", "--log-level", "--tls", "--tlscacert", "--tlscert", "--tlskey", "--tlsverify"]);
        var host = options.Single(option => option.SwitchName == "--host");
        await Assert.That(host.ShortForm).IsEqualTo("-H");
        await Assert.That(host.CSharpType).IsEqualTo("string?");
        await Assert.That(host.IsFlag).IsFalse();
        await Assert.That(options.Single(option => option.SwitchName == "--debug").IsFlag).IsTrue();
    }

    [Test]
    public async Task Ordinary_Root_Flags_Are_Not_Promoted_To_Global_Options()
    {
        var options = new TestDockerCliScraper().ParseGlobals("""
            Usage: docker [OPTIONS] COMMAND

            Flags:
              --root-only string   This section is not global
            """);

        await Assert.That(options).IsEmpty();
    }

    [Test]
    public async Task Global_And_Command_Local_Switches_Remain_Independently_Configurable()
    {
        var command = await new TestDockerCliScraper().Parse(["docker", "service", "create"], """
            Usage: docker service create [OPTIONS] IMAGE

            Options:
              --host list     Set one or more custom host-to-IP mappings
              --config list   Specify configurations to expose to the service
            """);
        var tool = new CliToolDefinition
        {
            ToolName = "docker",
            NamespacePrefix = "Docker",
            TargetNamespace = "ModularPipelines.Docker",
            OutputDirectory = "src/ModularPipelines.Docker",
            Commands = [command!],
            GlobalOptions =
            [
                new CliOptionDefinition { SwitchName = "--host", PropertyName = "Host", CSharpType = "string?" },
                new CliOptionDefinition { SwitchName = "--config", PropertyName = "Config", CSharpType = "string?" },
            ],
        };

        var baseCode = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        var commandCode = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(baseCode).Contains("[CliGlobalOptions]");
        await Assert.That(baseCode).Contains("public virtual string? Host");
        await Assert.That(baseCode).Contains("public virtual string? Config");
        await Assert.That(commandCode).Contains("public IEnumerable<string>? ServiceHost");
        await Assert.That(commandCode).Contains("public IEnumerable<string>? ServiceConfig");
        await Assert.That(commandCode).DoesNotContain("public IEnumerable<string>? Host ");
        await Assert.That(commandCode).DoesNotContain("public IEnumerable<string>? Config ");
    }

    [Test]
    public async Task Global_Context_Does_Not_Hide_A_Command_Operand()
    {
        var command = await new TestDockerCliScraper().Parse(["docker", "buildx", "create"], """
            Usage: docker buildx create [OPTIONS] [CONTEXT]
            """);
        var tool = new CliToolDefinition
        {
            ToolName = "docker",
            NamespacePrefix = "Docker",
            TargetNamespace = "ModularPipelines.Docker",
            OutputDirectory = "src/ModularPipelines.Docker",
            Commands = [command!],
            GlobalOptions = [new CliOptionDefinition { SwitchName = "--context", PropertyName = "Context", CSharpType = "string?" }],
        };

        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        var repeated = InheritedPropertyCollisionResolver.Resolve(resolved);

        await Assert.That(resolved.GlobalOptions.Single().PropertyName).IsEqualTo("Context");
        await Assert.That(resolved.Commands.Single().PositionalArguments.Single().PropertyName).IsEqualTo("BuildxContext");
        await Assert.That(repeated.Commands.Single().PositionalArguments.Single().PropertyName).IsEqualTo("BuildxContext");
    }

    [Test]
    [Arguments("exec")]
    [Arguments("run")]
    public async Task ComposeCommands_Preserve_Canonical_NoTty_Switch_Casing(string subcommand)
    {
        var helpText = $$"""
            Execute a command in a running container

            Usage: docker compose {{subcommand}} [OPTIONS] SERVICE COMMAND [ARGS...]

            Options:
              -T, --no-tty   Disable pseudo-TTY allocation
            """;
        var command = await new TestDockerCliScraper().Parse(
            ["docker", "compose", subcommand],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--no-TTY");
            await Assert.That(option.ShortForm).IsEqualTo("-T");
            if (subcommand == "exec")
            {
                await Assert.That(option.IsFlag).IsFalse();
                await Assert.That(option.CSharpType).IsEqualTo("bool?");
                await Assert.That(option.ValueSeparator).IsEqualTo("=");
                await Assert.That(option.Description)
                    .IsEqualTo("Disable pseudo-TTY allocation (default: auto-detected)");
            }
        }
    }

    [Test]
    public async Task ComposeExec_Preserves_NoTty_WhenInstalledHelpOmitsIt()
    {
        const string helpText = """
            Execute a command in a running container

            Usage: docker compose exec [OPTIONS] SERVICE COMMAND [ARGS...]
            """;
        var command = await new TestDockerCliScraper().Parse(
            ["docker", "compose", "exec"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--no-TTY");
            await Assert.That(option.ShortForm).IsEqualTo("-T");
            await Assert.That(option.PropertyName).IsEqualTo("NoTty");
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.Description)
                .IsEqualTo("Disable pseudo-TTY allocation (default: auto-detected)");
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Switch_Normalization_Rejects_Distinct_Options_With_One_Canonical_Name()
    {
        const string helpText = """
            Usage: fake run [OPTIONS]

            Options:
              --current string   Current value
              --legacy string    Legacy value
            """;

        await Assert.That(() => new CollidingSwitchScraper().Parse(
                ["fake", "run"],
                helpText))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("maps both '--current' and '--legacy'");
    }

    [Test]
    public async Task Switch_Normalization_Treats_Source_Switch_Casing_As_Distinct()
    {
        const string helpText = """
            Usage: fake run [OPTIONS]

            Options:
              --current string   Current value
              --CURRENT string   Upper-case value
            """;

        await Assert.That(() => new CollidingSwitchScraper().Parse(
                ["fake", "run"],
                helpText))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("maps both '--current' and '--CURRENT'");
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Mention_Flags_Stay_Prose()
    {
        const string helpText = """
            Usage:  docker run [OPTIONS] IMAGE [COMMAND] [ARG...]

            Create and run a new container from an image

            Options:
                  --tls                Use TLS; implied by
                                       --tlsverify
                  --tlsverify          Use TLS and verify the remote
            """;

        var command = await new TestDockerCliScraper().Parse(["docker", "run"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--tls", "--tlsverify"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--tls").Description)
                .IsEqualTo("Use TLS; implied by --tlsverify");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--tlsverify").Description)
                .IsEqualTo("Use TLS and verify the remote");
        }
    }

    private sealed class TestDockerCliScraper : DockerCliScraper
    {
        public TestDockerCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<DockerCliScraper>.Instance)
        {
        }

        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string helpText) => ParseGlobalOptions(helpText);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class CollidingSwitchScraper : CobraCliScraper
    {
        public CollidingSwitchScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<CollidingSwitchScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override string NormalizeOptionSwitchName(
            string[] commandParts,
            string switchName) => "--canonical";

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
