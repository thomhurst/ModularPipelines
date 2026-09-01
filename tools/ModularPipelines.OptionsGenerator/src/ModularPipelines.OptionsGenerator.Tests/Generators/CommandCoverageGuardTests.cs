using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class CommandCoverageGuardTests
{
    [Test]
    public async Task RemovedCommandsAndEmptyKnownGroups_FailWithoutApproval()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake project create"), Command("fake project delete")),
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake status")),
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(current.Violations).Contains(
                violation => violation.Contains("fake project create", StringComparison.Ordinal));
            await Assert.That(current.Violations).Contains(
                violation => violation.Contains("fake project", StringComparison.Ordinal));
            await Assert.That(current.RemovedCommands)
                .IsEquivalentTo(["fake project create", "fake project delete"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExplicitApproval_AllowsShrinkageAndRecordsDiff()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake one"), Command("fake two")),
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake one")),
                outputDirectory,
                approveShrinkage: true);

            await Assert.That(current.Violations).IsEmpty();
            await Assert.That(current.RemovedCommands).IsEquivalentTo(["fake two"]);
            await Assert.That(current.ChangesApproved).IsTrue();
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task SameVersionCommandSubstitution_FailsWithoutApprovalAndReportsBothSides()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake list-engines")) with { ToolVersion = "10.20.1" },
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var currentTool = Tool(Command("fake init")) with
            {
                ToolVersion = "10.20.1",
                CommandCoverage = new CliCommandCoveragePolicy
                {
                    ConditionallyAvailableCommands =
                    [
                        new CliConditionallyAvailableCommand
                        {
                            Command = "fake list-engines",
                            Reason = "Visibility depends on the help context.",
                        },
                    ],
                },
            };
            var current = CommandCoverageGuard.Evaluate(
                currentTool,
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(current.Violations).Contains(
                violation => violation.Contains("version remained '10.20.1'", StringComparison.Ordinal)
                             && violation.Contains("Added: fake init", StringComparison.Ordinal)
                             && violation.Contains("Removed: fake list-engines", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task SameVersionCommandSubstitution_ExplicitApprovalAllowsBothSides()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake list-engines")) with { ToolVersion = "10.20.1" },
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake init")) with { ToolVersion = "10.20.1" },
                outputDirectory,
                approveShrinkage: true);

            await Assert.That(current.Violations).IsEmpty();
            await Assert.That(current.AddedCommands).IsEquivalentTo(["fake init"]);
            await Assert.That(current.RemovedCommands).IsEquivalentTo(["fake list-engines"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task SentinelsAndMinimumCoverage_ProtectFirstGeneration()
    {
        var outputDirectory = CreateOutputDirectory();
        var tool = Tool(Command("fake run")) with
        {
            CommandCoverage = new CliCommandCoveragePolicy
            {
                MinimumCommandCount = 2,
                SentinelCommands = ["fake deploy"],
            },
        };

        try
        {
            var evaluation = CommandCoverageGuard.Evaluate(
                tool,
                outputDirectory,
                approveShrinkage: true);

            await Assert.That(evaluation.Violations).Contains(
                violation => violation.Contains("below the configured minimum", StringComparison.Ordinal));
            await Assert.That(evaluation.Violations).Contains(
                violation => violation.Contains("fake deploy", StringComparison.Ordinal));
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
                outputDirectory,
                approveShrinkage: false);
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
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(current.Violations).IsEmpty();
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
    public async Task ConditionallyAvailableCommands_AllowEnvironmentSpecificOmission()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake community"), Command("fake enterprise")),
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var currentTool = Tool(Command("fake community")) with
            {
                CommandCoverage = new CliCommandCoveragePolicy
                {
                    ConditionallyAvailableCommands =
                    [
                        new CliConditionallyAvailableCommand
                        {
                            Command = "fake enterprise",
                            Reason = "Requires an enterprise license.",
                        },
                    ],
                },
            };
            var current = CommandCoverageGuard.Evaluate(
                currentTool,
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(current.Violations).IsEmpty();
            await Assert.That(current.RemovedCommands).IsEquivalentTo(["fake enterprise"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExclusionsAndConditionallyAvailableCommands_CannotOverlap()
    {
        var outputDirectory = CreateOutputDirectory();
        var tool = Tool(Command("fake community")) with
        {
            CommandCoverage = new CliCommandCoveragePolicy
            {
                Exclusions =
                [
                    new CliCommandCoverageExclusion
                    {
                        Command = "fake  enterprise",
                        Reason = "Unsupported by the generated API.",
                    },
                ],
                ConditionallyAvailableCommands =
                [
                    new CliConditionallyAvailableCommand
                    {
                        Command = "FAKE enterprise",
                        Reason = "Requires an enterprise license.",
                    },
                ],
            },
        };

        try
        {
            void Evaluate() => CommandCoverageGuard.Evaluate(
                tool,
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(Evaluate)
                .Throws<InvalidOperationException>()
                .And.HasMessageContaining("both excluded and conditionally available")
                .And.HasMessageContaining("fake enterprise");
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
                outputDirectory,
                approveShrinkage: false);
            var second = CommandCoverageGuard.Evaluate(
                Tool(Command("fake one"), Command("fake two")),
                outputDirectory,
                approveShrinkage: false);

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
    public async Task MissingManifest_FailsWhenGeneratedApiExists()
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
            void Evaluate() =>
                CommandCoverageGuard.Evaluate(
                    Tool(Command("fake status")),
                    outputDirectory,
                    approveShrinkage: false);

            await Assert.That(Evaluate)
                .Throws<InvalidOperationException>()
                .And.HasMessageContaining("Command coverage manifest is missing for 'fake'")
                .And.HasMessageContaining("Fake.CommandCoverage.json");
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
