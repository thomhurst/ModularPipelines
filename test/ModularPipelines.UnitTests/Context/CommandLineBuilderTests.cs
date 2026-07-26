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

    [CliTool("mytool")]
    [CliCommand("mytool", "sub", "command")]
    private record TestAttributeOptions : CommandLineToolOptions
    {
        [CliFlag("--force")]
        public bool? Force { get; set; }

        [CliOption("--output")]
        public string? Output { get; set; }
    }

    [CliTool("processor")]
    [CliCommand("processor")]
    private record TestPositionalOptions : CommandLineToolOptions
    {
        [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
        public string? FilePath { get; set; }

        [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
        public string? ConfigPath { get; set; }
    }

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
}
