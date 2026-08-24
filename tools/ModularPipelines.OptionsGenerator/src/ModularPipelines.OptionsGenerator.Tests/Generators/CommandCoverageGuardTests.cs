using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class CommandCoverageGuardTests
{
    [Test]
    public async Task RemovedCommandsAndEmptyKnownGroups_AreReported()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake project create"), Command("fake project delete")),
                outputDirectory);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake status")),
                outputDirectory);

            await Assert.That(current.RemovedCommands)
                .IsEquivalentTo(["fake project create", "fake project delete"]);
            await Assert.That(current.KnownGroupsWithoutChildren)
                .IsEquivalentTo(["fake project"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }


    [Test]
    public async Task DocumentedExclusions_AllowIntentionalRemoval()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake community"), Command("fake enterprise")),
                outputDirectory);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var currentTool = Tool(Command("fake community")) with
            {
                CommandCoverage = new CliCommandCoveragePolicy
                {
                    Exclusions =
                    [
                        new CliCommandCoverageExclusion
                        {
                            Command = "fake enterprise",
                            Reason = "Requires an enterprise license.",
                        },
                    ],
                },
            };
            var current = CommandCoverageGuard.Evaluate(
                currentTool,
                outputDirectory);

            await Assert.That(current.Manifest.Exclusions).Count().IsEqualTo(1);
            await Assert.That(current.Manifest.Exclusions[0].Reason)
                .IsEqualTo("Requires an enterprise license.");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Fingerprint_IsStableAcrossDiscoveryOrderAndWhitespace()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var first = CommandCoverageGuard.Evaluate(
                Tool(Command("fake  two"), Command("fake one")),
                outputDirectory);
            var second = CommandCoverageGuard.Evaluate(
                Tool(Command("fake one"), Command("fake two")),
                outputDirectory);

            await Assert.That(second.Manifest.CommandTreeSha256)
                .IsEqualTo(first.Manifest.CommandTreeSha256);
            await Assert.That(second.Manifest.Commands)
                .IsEquivalentTo(["fake one", "fake two"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task CommandGroupAliases_PreserveCanonicalTreeInCoverageBaseline()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(
                    Command("fake builder"),
                    Command("fake builder build"),
                    Command("fake buildx"),
                    Command("fake buildx build")),
                outputDirectory);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var currentTool = Tool(
                Command("fake buildx"),
                Command("fake buildx build")) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use Buildx instead.",
                    },
                ],
            };
            var current = CommandCoverageGuard.Evaluate(
                currentTool,
                outputDirectory);

            await Assert.That(current.RemovedCommands).IsEmpty();
            await Assert.That(current.Manifest.Commands).IsEquivalentTo(
            [
                "fake builder",
                "fake builder build",
                "fake buildx",
                "fake buildx build",
            ]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task MissingManifest_Starts_New_Baseline_When_Generated_Api_Exists()
    {
        var outputDirectory = CreateOutputDirectory();
        var optionsDirectory = CreateGeneratedOptionsDirectory(outputDirectory);
        await File.WriteAllTextAsync(
            Path.Combine(optionsDirectory, "FakeProjectCreateOptions.cs"),
            """
            [CliSubCommand("project", "create")]
            public record FakeProjectCreateOptions;
            """);

        try
        {
            var evaluation = CommandCoverageGuard.Evaluate(
                Tool(Command("fake status")),
                outputDirectory);

            await Assert.That(evaluation.HasPreviousBaseline).IsFalse();
            await Assert.That(evaluation.Manifest.Commands).IsEquivalentTo(["fake status"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    private static string CreateOutputDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "mp-command-coverage-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static string CreateGeneratedOptionsDirectory(string outputDirectory)
    {
        var path = Path.Combine(
            outputDirectory,
            "src",
            "ModularPipelines.Fake",
            "Options");
        Directory.CreateDirectory(path);
        return path;
    }

    private static CliToolDefinition Tool(params CliCommandDefinition[] commands) => new()
    {
        ToolName = "fake",
        NamespacePrefix = "Fake",
        TargetNamespace = "ModularPipelines.Fake",
        OutputDirectory = "src/ModularPipelines.Fake",
        Commands = commands,
    };

    private static CliCommandDefinition Command(string fullCommand) => new()
    {
        FullCommand = fullCommand,
        CommandParts = fullCommand.Split(' ').Skip(1).ToArray(),
        ClassName = fullCommand.Replace(" ", string.Empty, StringComparison.Ordinal) + "Options",
        ParentClassName = "FakeOptions",
        ToolNamespacePrefix = "Fake",
        Options = [],
    };
}
