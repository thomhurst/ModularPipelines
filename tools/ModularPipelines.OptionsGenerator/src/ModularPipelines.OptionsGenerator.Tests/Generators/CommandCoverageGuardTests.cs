using System.Text.Json;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

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
    public async Task BlanketApproval_RejectsTruncatedAwsServiceScrape()
    {
        var outputDirectory = CreateOutputDirectory();
        var baselineCommands = new[]
        {
            "aws apigateway get-model",
            "aws ec2 describe-instances",
            "aws ec2 modify-vpc-attribute",
            "aws efs create-file-system",
            "aws iam create-role",
            "aws lambda create-function",
            "aws s3api create-bucket",
            "aws sts get-caller-identity",
        };

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(baselineCommands.Select(Command).ToArray()) with { ToolVersion = "aws-cli/2.36.29" },
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("aws sts get-caller-identity")) with { ToolVersion = "aws-cli/2.36.35" },
                outputDirectory,
                approveShrinkage: true);

            await Assert.That(current.Violations).Contains(
                violation => violation.Contains("Blanket approval cannot authorize 7 command removals", StringComparison.Ordinal)
                             && violation.Contains("aws-cli/2.36.29", StringComparison.Ordinal)
                             && violation.Contains("aws-cli/2.36.35", StringComparison.Ordinal)
                             && violation.Contains("aws apigateway get-model", StringComparison.Ordinal)
                             && violation.Contains("explicit command coverage exclusions", StringComparison.Ordinal));
            await Assert.That(current.PreviousCommandCount).IsEqualTo(8);
            await Assert.That(current.PreviousToolVersion).IsEqualTo("aws-cli/2.36.29");
            await Assert.That(current.ChangesApproved).IsFalse();
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task CoverageFailureDiagnostics_PreserveRelevantRawHelpAndMissingParents()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(
                    Command("aws apigateway models get-model"),
                    Command("aws ec2 describe-instances"),
                    Command("aws ec2 modify-vpc-attribute"),
                    Command("aws efs create-file-system"),
                    Command("aws iam create-role"),
                    Command("aws lambda create-function"),
                    Command("aws s3api create-bucket")) with
                {
                    ToolName = "aws",
                    ToolVersion = "aws-cli/2.36.29",
                },
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("aws ec2 describe-instances")) with
                {
                    ToolName = "aws",
                    ToolVersion = "aws-cli/2.36.35",
                },
                outputDirectory,
                approveShrinkage: true);
            var provenance = new CliScrapeProvenance();
            provenance.Record(
                ["aws"],
                "help",
                Result("RAW ROOT HELP: ec2 only"));
            provenance.Record(
                ["aws", "apigateway"],
                "apigateway help",
                Result("RAW APIGATEWAY HELP: models omitted"));
            provenance.PreserveGroupHelp(
                ["aws", "apigateway"],
                "RAW APIGATEWAY HELP: models omitted");
            provenance.Record(
                ["aws", "ec2"],
                "ec2 help",
                Result("RAW EC2 HELP: describe-instances only"));
            provenance.PreserveGroupHelp(
                ["aws", "ec2"],
                "RAW EC2 HELP: describe-instances only");

            var path = await provenance.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                current,
                CancellationToken.None);
            var json = await File.ReadAllTextAsync(path!);
            using var diagnostics = JsonDocument.Parse(json);
            var missingHelpPaths = diagnostics.RootElement
                .GetProperty("missingHelpPaths")
                .EnumerateArray()
                .Select(static element => element.GetString())
                .ToArray();

            await Assert.That(json).Contains("RAW ROOT HELP: ec2 only");
            await Assert.That(json).Contains("RAW APIGATEWAY HELP: models omitted");
            await Assert.That(json).Contains("RAW EC2 HELP: describe-instances only");
            await Assert.That(missingHelpPaths).Contains("aws apigateway models");
            await Assert.That(json).Contains("aws-cli/2.36.29");
            await Assert.That(json).Contains("aws-cli/2.36.35");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task CoverageFailureDiagnostics_DiscardUnpreservedLeafRawHelp()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool(Command("fake group leaf nested")),
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake keep")),
                outputDirectory,
                approveShrinkage: false);
            var provenance = new CliScrapeProvenance();
            provenance.Record(["fake"], "--help", Result("RAW ROOT HELP"));
            provenance.Record(
                ["fake", "group"],
                "group --help",
                Result("RAW GROUP HELP"));
            provenance.PreserveGroupHelp(["fake", "group"], "RAW GROUP HELP");
            provenance.Record(
                ["fake", "group", "leaf"],
                "group leaf --help",
                Result("RAW LEAF HELP"));
            provenance.DiscardLeafHelp(["fake", "group", "leaf"]);

            var path = await provenance.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                current,
                CancellationToken.None);
            var json = await File.ReadAllTextAsync(path!);

            using (Assert.Multiple())
            {
                await Assert.That(json).Contains("RAW ROOT HELP");
                await Assert.That(json).Contains("RAW GROUP HELP");
                await Assert.That(json).DoesNotContain("RAW LEAF HELP");
            }
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task CoverageFailureDiagnostics_AreWrittenWithoutRemovedCommands()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake run")) with
                {
                    CommandCoverage = new CliCommandCoveragePolicy
                    {
                        MinimumCommandCount = 2,
                    },
                },
                outputDirectory,
                approveShrinkage: false);
            var provenance = new CliScrapeProvenance();
            provenance.Record(
                ["fake"],
                "--help",
                Result("RAW ROOT HELP: run only"));

            var path = await provenance.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                current,
                CancellationToken.None);
            var json = await File.ReadAllTextAsync(path!);

            await Assert.That(path).IsNotNull();
            await Assert.That(json).Contains("RAW ROOT HELP: run only");
            await Assert.That(json).Contains("\"removedCommands\": []");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task PreservedGroupHelp_ReplacesEarlierRawInvocationOutput()
    {
        var outputDirectory = CreateOutputDirectory();

        try
        {
            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake run")),
                outputDirectory,
                approveShrinkage: false);
            var provenance = new CliScrapeProvenance();
            provenance.Record(
                ["fake"],
                "--help",
                Result("RAW ROOT HELP"));
            provenance.PreserveGroupHelp(
                ["fake"],
                "RAW ROOT HELP\n\nCommands:\n  run  Discovered by supplemental inventory.");

            var path = await provenance.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory,
                current,
                CancellationToken.None);
            var json = await File.ReadAllTextAsync(path!);

            await Assert.That(json).Contains("Discovered by supplemental inventory");
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task DocumentedExclusions_AllowMassRemovalWithoutBlanketApproval()
    {
        var outputDirectory = CreateOutputDirectory();
        var removedCommands = Enumerable.Range(1, 6)
            .Select(index => $"fake removed-{index}")
            .ToArray();

        try
        {
            var baseline = CommandCoverageGuard.Evaluate(
                Tool([Command("fake keep"), .. removedCommands.Select(Command)]),
                outputDirectory,
                approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(
                Tool(Command("fake keep")) with
                {
                    CommandCoverage = new CliCommandCoveragePolicy
                    {
                        Exclusions = removedCommands
                            .Select(command => new CliCommandCoverageExclusion
                            {
                                Command = command,
                                Reason = "Removed upstream and reviewed in issue #4465.",
                            })
                            .ToArray(),
                    },
                },
                outputDirectory,
                approveShrinkage: false);

            await Assert.That(current.Violations).IsEmpty();
            await Assert.That(current.RemovedCommands).Count().IsEqualTo(6);
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

    private static CliCommandResult Result(string standardOutput) => new()
    {
        StandardOutput = standardOutput,
        StandardError = string.Empty,
        ExitCode = 0,
    };
}
