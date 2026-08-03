using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Context;

public class CommandLineBuilderTests : TestBase
{
    [Test]
    public async Task Build_FromGenericOptions_ReturnsCorrectCommandLine()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new GenericCommandLineToolOptions("echo")
        {
            Arguments = ["hello", "world"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("echo");
        await Assert.That(result.Arguments).IsEquivalentTo(new[] { "hello", "world" });
    }

    [Test]
    public async Task Build_FromGenericOptions_WithRunSettings_AddsDoubleDash()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new GenericCommandLineToolOptions("dotnet")
        {
            Arguments = ["test"],
            RunSettings = ["--filter", "Category=Unit"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("dotnet");
        await Assert.That(result.Arguments).IsEquivalentTo(new[] { "test", "--", "--filter", "Category=Unit" });
    }

    [Test]
    public async Task Build_Places_Terminal_Options_After_Manual_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Arguments = ["."],
            RunTests = "tests.jq",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq . --run-tests tests.jq");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_RunSettings()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            RunTests = "tests.jq",
            RunSettings = ["--filter", "Category=Unit"],
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_Property_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            RunTests = "tests.jq",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Rejects_Terminal_Options_With_Manual_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        CommandLine Build() => builder.Build(new TestTerminalOptions
        {
            Arguments = ["--", "-1"],
            RunTests = "tests.jq",
        });

        await Assert.That(Build)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("end-of-options marker");
    }

    [Test]
    public async Task Build_Accepts_Terminal_Option_When_Option_Value_Is_DoubleDash()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Argument = new CliValuePair("name", "--"),
            RunTests = "tests.jq",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq --arg name -- --run-tests tests.jq");
    }

    [Test]
    public async Task Build_Accepts_Terminal_Argument_Own_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            TerminalArgument = "-x",
        });

        await Assert.That(result.ToString()).IsEqualTo("jq -- -x");
    }

    [Test]
    public async Task Build_Empty_RunSettings_Emit_No_Terminator()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new GenericCommandLineToolOptions("dotnet")
        {
            Arguments = ["test"],
            RunSettings = [],
        });

        await Assert.That(result.ToString()).IsEqualTo("dotnet test");
    }

    [Test]
    public async Task Build_Property_Terminator_Is_Reused_For_RunSettings()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestTerminalOptions
        {
            Filter = "-1",
            RunSettings = ["extra"],
        });

        await Assert.That(result.ToString()).IsEqualTo("jq -- -1 extra");
    }

    [Test]
    public async Task Build_FromAttributeBasedOptions_ResolvesToolAndSubcommands()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new TestAttributeOptions
        {
            Force = true,
            Output = "/path/to/output"
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("mytool");
        await Assert.That(result.Arguments).Contains("sub");
        await Assert.That(result.Arguments).Contains("command");
        await Assert.That(result.Arguments).Contains("--force");
        await Assert.That(result.Arguments).Contains("--output");
        await Assert.That(result.Arguments).Contains("/path/to/output");
    }

    [Test]
    public async Task Build_RuntimeIdentity_Overrides_StaticIdentity()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestAttributeOptions
        {
            Tool = "runtime-tool",
            CommandParts = ["runtime", "command"],
        });

        await Assert.That(result.ToString()).IsEqualTo("runtime-tool runtime command");
    }

    [Test]
    public async Task Build_PreferredAlias_Overrides_Subcommand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestAliasOptions());

        await Assert.That(result.ToString()).IsEqualTo("mytool short");
    }

    [Test]
    public async Task Build_Uses_Constructor_Computed_CommandParts()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestComputedCommandPartsOptions("deploy")
        {
            Force = true,
        });

        await Assert.That(result.ToString()).IsEqualTo("mytool resource deploy --force");
    }

    [Test]
    public async Task Build_WithPositionalArguments_PlacesCorrectly()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new TestPositionalOptions
        {
            FilePath = "test.cs",
            ConfigPath = "config.json"
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("processor");

        // The file path should appear before the config path based on placement
        var args = result.Arguments.ToList();
        var fileIndex = args.IndexOf("test.cs");
        var configIndex = args.IndexOf("config.json");

        await Assert.That(fileIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(configIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(fileIndex).IsLessThan(configIndex);
    }

    [Test]
    public async Task Build_ReturnsImmutableCommandLine()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new GenericCommandLineToolOptions("echo")
        {
            Arguments = ["original"]
        };

        var result = builder.Build(options);

        // Verify the arguments are readonly
        await Assert.That(result.Arguments).IsTypeOf<System.Collections.ObjectModel.ReadOnlyCollection<string>>();
    }

    [Test]
    public async Task Build_ToString_FormatsCorrectly()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var options = new GenericCommandLineToolOptions("git")
        {
            Arguments = ["status", "-s"]
        };

        var result = builder.Build(options);

        await Assert.That(result.ToString()).IsEqualTo("git status -s");
    }

    [Test]
    public async Task Build_SkipsDuplicateToolInArguments()
    {
        var builder = await GetService<ICommandLineBuilder>();

        // Some legacy code might include the tool name in Arguments
        var options = new GenericCommandLineToolOptions("git")
        {
            Arguments = ["git", "status"]
        };

        var result = builder.Build(options);

        await Assert.That(result.Tool).IsEqualTo("git");
        // Should only have "status", not "git status"
        await Assert.That(result.Arguments.Count(a => a == "git")).IsEqualTo(0);
        await Assert.That(result.Arguments).Contains("status");
    }

    [Test]
    public async Task Build_Places_Global_Options_Before_Subcommands()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var modelProvider = await GetService<ICommandModelProvider>();

        var searchPath = modelProvider.GetCommandModel(typeof(TestGlobalCommandOptions))
            .Single(part => part.PropertyName == nameof(TestGlobalOptions.SearchPath));
        await Assert.That(searchPath.IsGlobalOption).IsTrue();

        var result = builder.Build(new TestGlobalCommandOptions
        {
            SearchPath = "changelogs",
            ChangelogFile = "main.xml"
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "liquibase --search-path=changelogs update --changelog-file=main.xml");
    }

    [Test]
    public async Task Build_Keeps_MultiLevel_Command_Chain_Atomic()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var result = builder.Build(new TestMultiLevelCommandOptions
        {
            Context = "remote",
            Reference = "build-reference",
            Follow = true,
        });

        await Assert.That(result.ToString()).IsEqualTo(
            "docker --context remote buildx history logs build-reference --follow");
    }

    [CliTool("mytool")]
    [CliSubCommand("sub", "command")]
    private record TestAttributeOptions : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public bool? Force { get; set; }

        [CliOption("--output")]
        public string? Output { get; set; }
    }

    [CliTool("mytool")]
    private sealed record TestComputedCommandPartsOptions : CommandLineToolOptions
    {
        public TestComputedCommandPartsOptions(string action)
        {
            CommandParts = ["resource", action];
        }

        [CliFlag("--force")]
        public bool? Force { get; init; }
    }

    [CliTool("processor")]
    private record TestPositionalOptions : CommandLineToolOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? FilePath { get; set; }

        [CliArgument(1, Phase = CommandLinePhase.Passthrough)]
        public string? ConfigPath { get; set; }
    }

    [CliTool("jq")]
    private record TestTerminalOptions : CommandLineToolOptions
    {
        [CliOption("--arg")]
        public CliValuePair? Argument { get; set; }

        [CliArgument(0, PrependOptionTerminator = true)]
        public string? Filter { get; set; }

        [CliOption(
            "--run-tests",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal)]
        public string? RunTests { get; set; }

        [CliArgument(1, Phase = CommandLinePhase.Terminal, PrependOptionTerminator = true)]
        public string? TerminalArgument { get; set; }
    }

    [CliTool("mytool")]
    [CliSubCommand("long", "command")]
    [CliCommandAlias("short", IsPreferred = true)]
    private record TestAliasOptions : CommandLineToolOptions;

    [CliTool("liquibase")]
    [CliGlobalOptions]
    private abstract record TestGlobalOptions : CommandLineToolOptions
    {
        [CliOption("--search-path", Format = OptionFormat.EqualsSeparated)]
        public string? SearchPath { get; set; }
    }

    [CliSubCommand("update")]
    private sealed record TestGlobalCommandOptions : TestGlobalOptions
    {
        [CliOption("--changelog-file", Format = OptionFormat.EqualsSeparated)]
        public string? ChangelogFile { get; set; }
    }

    [CliTool("docker")]
    [CliGlobalOptions]
    private abstract record TestMultiLevelGlobalOptions : CommandLineToolOptions
    {
        [CliOption("--context")]
        public string? Context { get; set; }
    }

    [CliSubCommand("buildx", "history", "logs")]
    private sealed record TestMultiLevelCommandOptions : TestMultiLevelGlobalOptions
    {
        [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
        public string? Reference { get; set; }

        [CliFlag("--follow")]
        public bool? Follow { get; set; }
    }
}
