using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

/// <summary>
/// Tests the orchestrator's core guarantee: a tool's existing output is never mutated
/// unless scraping and generation fully succeeded, and failures are recorded loudly.
/// </summary>
public class CodeGeneratorOrchestratorTests
{
    private const string ToolOutputDirectory = "src/ModularPipelines.Fake";

    private sealed class FakeCliScraper : ICliScraper
    {
        public string ToolName { get; init; } = "fake";

        public string NamespacePrefix => "Fake";

        public string TargetNamespace => "ModularPipelines.Fake";

        public string OutputDirectory => ToolOutputDirectory;

        public bool Available { get; init; } = true;

        public IReadOnlyList<CliCommandDefinition> Commands { get; init; } = [];

        public IReadOnlyList<CliOptionDefinition> GlobalOptions { get; init; } = [];

        public CliCommandCoveragePolicy CommandCoverage { get; init; } = new();

        public string? Version { get; init; } = "fake 1.0";

        public CliExecutablePrerequisite? ExecutablePrerequisite { get; init; } = new()
        {
            CommandName = "fake",
        };

        public string? DocumentationOutputDirectory { get; init; } =
            Path.Combine("docs", "docs", "mp-packages", "cli");

        public bool GenerateCommandFacade { get; init; } = true;

        public string? ExecutablePrerequisiteMetadataExemption { get; init; }

        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(Available);

        public Task<string?> GetVersionAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(Version);

        public async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var command in Commands)
            {
                yield return command;
            }

            await Task.CompletedTask;
        }

        public CliToolDefinition CreateToolDefinition() => new()
        {
            ToolName = ToolName,
            NamespacePrefix = NamespacePrefix,
            TargetNamespace = TargetNamespace,
            OutputDirectory = OutputDirectory,
            Commands = [],
            CommandCoverage = CommandCoverage,
            GlobalOptions = GlobalOptions,
            DocumentationOutputDirectory = DocumentationOutputDirectory,
            GenerateCommandFacade = GenerateCommandFacade,
            ExecutablePrerequisite = ExecutablePrerequisite,
            ExecutablePrerequisiteMetadataExemption = ExecutablePrerequisiteMetadataExemption,
        };
    }

    private sealed class FakeGenerator : ICodeGenerator
    {
        public Func<CliToolDefinition, IReadOnlyList<GeneratedFile>> OnGenerate { get; init; } = _ => [];

        public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
            => Task.FromResult(OnGenerate(tool));
    }

    private static CliCommandDefinition FakeCommand() => new()
    {
        FullCommand = "fake run",
        CommandParts = ["run"],
        ClassName = "FakeRunOptions",
        ParentClassName = "FakeOptions",
        ToolNamespacePrefix = "Fake",
        Options = [],
    };

    private static CodeGeneratorOrchestrator Orchestrator(FakeCliScraper scraper, params ICodeGenerator[] generators) =>
        new(
            [scraper],
            htmlScrapers: [],
            generators,
            NullLogger<CodeGeneratorOrchestrator>.Instance);

    [Test]
    public async Task Cli_Metadata_Is_Preserved_For_Generators()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var prerequisite = new CliExecutablePrerequisite
        {
            CommandName = "fake-cli",
            SupportedVersion = "1.2.3",
        };
        CliToolDefinition? generatedTool = null;
        var scraper = new FakeCliScraper
        {
            Commands = [FakeCommand()],
            DocumentationOutputDirectory = null,
            GenerateCommandFacade = false,
            ExecutablePrerequisite = prerequisite,
        };
        var generator = new FakeGenerator
        {
            OnGenerate = tool =>
            {
                generatedTool = tool;
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(generatedTool).IsNotNull();
            await Assert.That(generatedTool!.DocumentationOutputDirectory).IsNull();
            await Assert.That(generatedTool.GenerateCommandFacade).IsFalse();
            await Assert.That(generatedTool!.ExecutablePrerequisite).IsSameReferenceAs(prerequisite);
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Existing_Generated_Options_Are_Used_As_Api_Baseline()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var scraper = new FakeCliScraper { Commands = [FakeCommand()] };
        await Orchestrator(scraper, new FakeGenerator()).GenerateAsync("fake", outputRoot);
        var optionsDirectory = Path.Combine(outputRoot, ToolOutputDirectory, "Options");
        Directory.CreateDirectory(optionsDirectory);
        await File.WriteAllTextAsync(
            Path.Combine(optionsDirectory, "FakeRunOptions.Generated.cs"),
            """
            // <auto-generated>
            // Generated compatibility baseline.
            // </auto-generated>

            using ModularPipelines.Attributes;

            public record FakeRunOptions
            {
                [CliFlag("--removed")]
                public bool? Removed { get; set; }
            }
            """);
        CliToolDefinition? generatedTool = null;
        var generator = new FakeGenerator
        {
            OnGenerate = tool =>
            {
                generatedTool = tool;
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            var compatibilityProperty = generatedTool!.Commands.Single().CompatibilityProperties.Single();
            using (Assert.Multiple())
            {
                await Assert.That(compatibilityProperty.PropertyName).IsEqualTo("Removed");
                await Assert.That(compatibilityProperty.CSharpType).IsEqualTo("bool?");
                await Assert.That(compatibilityProperty.ForwardToPropertyName).IsNull();
            }
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Global_Compatibility_Names_Are_Reserved_Before_Collision_Resolution()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        await Orchestrator(
                new FakeCliScraper { Commands = [FakeCommand()] },
                new FakeGenerator())
            .GenerateAsync("fake", outputRoot);
        var optionsDirectory = Path.Combine(outputRoot, ToolOutputDirectory, "Options");
        Directory.CreateDirectory(optionsDirectory);
        await File.WriteAllTextAsync(
            Path.Combine(optionsDirectory, "FakeOptions.Generated.cs"),
            """
            public record FakeOptions
            {
                [Obsolete("Retained for compatibility.")]
                public IEnumerable<string>? CliArguments { get; set; }
            }
            """);
        CliToolDefinition? generatedTool = null;
        var scraper = new FakeCliScraper
        {
            Commands = [FakeCommand()],
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "IEnumerable<string>?",
                },
            ],
        };
        var generator = new FakeGenerator
        {
            OnGenerate = tool =>
            {
                generatedTool = tool;
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.Errors).IsEmpty();
            await Assert.That(generatedTool!.GlobalOptions.Single().PropertyName)
                .IsEqualTo("CliArguments2");
            await Assert.That(generatedTool.GlobalCompatibilityProperties.Single().PropertyName)
                .IsEqualTo("CliArguments");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Catalog_Metadata_Is_Applied_Before_Every_Generator()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var generatedTools = new List<CliToolDefinition>();
        var scraper = new FakeCliScraper
        {
            ToolName = "terraform",
            Commands = [FakeCommand()],
            ExecutablePrerequisite = null,
        };
        var firstGenerator = new FakeGenerator
        {
            OnGenerate = tool =>
            {
                generatedTools.Add(tool);
                return [];
            },
        };
        var secondGenerator = new FakeGenerator
        {
            OnGenerate = tool =>
            {
                generatedTools.Add(tool);
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, firstGenerator, secondGenerator)
                .GenerateAsync("terraform", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(generatedTools).Count().IsEqualTo(2);
            await Assert.That(generatedTools.All(
                    tool => tool.ExecutablePrerequisite?.CommandName == "terraform"))
                .IsTrue();
            await Assert.That(generatedTools.All(
                    tool => tool.ExecutablePrerequisite?.InstallationUrl ==
                            "https://developer.hashicorp.com/terraform/install"))
                .IsTrue();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Missing_Metadata_Is_Rejected_Before_Generators_Run()
    {
        await AssertMetadataFailureBeforeGeneratorsRun(
            "unregistered-cli",
            prerequisite: null,
            expectedMessage: "no executable prerequisite metadata or explicit exemption");
    }

    [Test]
    public async Task Invalid_Metadata_Is_Rejected_Before_Generators_Run()
    {
        await AssertMetadataFailureBeforeGeneratorsRun(
            "fake",
            new CliExecutablePrerequisite
            {
                CommandName = "fake",
                InstallationUrl = "http://insecure.example.test/install",
            },
            "invalid HTTPS installation URL");
    }

    [Test]
    public async Task Cli_Compatibility_Forwarding_Is_Validated_Before_Generation()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var generatorCalled = false;
        var command = FakeCommand() with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyValue",
                    CSharpType = "string?",
                    ForwardToPropertyName = "MissingValue",
                    ObsoleteMessage = "Use MissingValue.",
                },
            ],
        };
        var scraper = new FakeCliScraper { Commands = [command] };
        var generator = new FakeGenerator
        {
            OnGenerate = _ =>
            {
                generatorCalled = true;
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors.Single().Message)
                .Contains("forwards to missing property 'MissingValue'");
            await Assert.That(generatorCalled).IsFalse();
        }
        finally
        {
            if (Directory.Exists(outputRoot))
            {
                Directory.Delete(outputRoot, recursive: true);
            }
        }
    }

    private static async Task AssertMetadataFailureBeforeGeneratorsRun(
        string toolName,
        CliExecutablePrerequisite? prerequisite,
        string expectedMessage)
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var generatorCalled = false;
        var scraper = new FakeCliScraper
        {
            ToolName = toolName,
            Commands = [FakeCommand()],
            ExecutablePrerequisite = prerequisite,
        };
        var generator = new FakeGenerator
        {
            OnGenerate = _ =>
            {
                generatorCalled = true;
                return [];
            },
        };

        try
        {
            var result = await Orchestrator(scraper, generator).GenerateAsync(toolName, outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors.Any(
                    error => error.Message.Contains(
                        expectedMessage,
                        StringComparison.Ordinal)))
                .IsTrue();
            await Assert.That(generatorCalled).IsFalse();
        }
        finally
        {
            if (Directory.Exists(outputRoot))
            {
                Directory.Delete(outputRoot, recursive: true);
            }
        }
    }

    /// <summary>
    /// Creates a temp output root containing one pre-existing generated file for the tool,
    /// returning both paths.
    /// </summary>
    private static async Task<(string OutputRoot, string ExistingFile)> CreateOutputRootWithExistingFileAsync()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var baselineTool = new FakeCliScraper().CreateToolDefinition() with
        {
            Commands = [FakeCommand()],
        };
        var baseline = CommandCoverageGuard.Evaluate(
            baselineTool,
            outputRoot,
            approveShrinkage: false);
        await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

        var optionsDir = Path.Combine(outputRoot, ToolOutputDirectory, "Options");
        Directory.CreateDirectory(optionsDir);

        var existingFile = Path.Combine(optionsDir, "FakeOldOptions.Generated.cs");
        File.WriteAllText(
            existingFile,
            """
            // <auto-generated>
            [CliSubCommand("run")]
            public record FakeOldOptions;
            // </auto-generated>
            """);

        return (outputRoot, existingFile);
    }

    [Test]
    public async Task Generator_Failure_Leaves_Existing_Output_Untouched()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };
            var throwingGenerator = new FakeGenerator
            {
                OnGenerate = _ => throw new InvalidOperationException("generator exploded"),
            };

            var result = await Orchestrator(scraper, throwingGenerator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(File.Exists(existingFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Operand_Coverage_Failure_Leaves_Existing_Output_Untouched()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();
        var generatorCalled = false;

        try
        {
            var invalidCommand = FakeCommand() with
            {
                HasOperandTakingUsage = true,
                UsageSynopsis = "fake run <target>",
            };
            var scraper = new FakeCliScraper { Commands = [invalidCommand] };
            var generator = new FakeGenerator
            {
                OnGenerate = _ =>
                {
                    generatorCalled = true;
                    return [];
                },
            };

            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            using (Assert.Multiple())
            {
                await Assert.That(result.HasErrors).IsTrue();
                await Assert.That(result.Errors[0].Message).Contains("no CliPositionalArgument values");
                await Assert.That(generatorCalled).IsFalse();
                await Assert.That(File.Exists(existingFile)).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Duplicate_Output_Paths_Fail_Without_Mutating_Existing_Output()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };
            GeneratedFile File1() => new()
            {
                RelativePath = Path.Combine(ToolOutputDirectory, "Options", "FakeOptions.Generated.cs"),
                Content = "// new",
            };

            var result = await Orchestrator(
                    scraper,
                    new FakeGenerator { OnGenerate = _ => [File1()] },
                    new FakeGenerator { OnGenerate = _ => [File1()] })
                .GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors[0].Message).Contains("same output path");
            await Assert.That(File.Exists(existingFile)).IsTrue();
            await Assert.That(File.Exists(Path.Combine(outputRoot, ToolOutputDirectory, "Options", "FakeOptions.Generated.cs"))).IsFalse();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Zero_Commands_For_Cli_Only_Tool_Is_An_Error_And_Writes_Nothing()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();

        try
        {
            var scraper = new FakeCliScraper { Commands = [] };

            var result = await Orchestrator(scraper, new FakeGenerator()).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors[0].Message).Contains("produced no commands");
            await Assert.That(File.Exists(existingFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Unavailable_Cli_Only_Tool_Is_An_Error_Not_A_Silent_Skip()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);

        try
        {
            var scraper = new FakeCliScraper { Available = false };

            var result = await Orchestrator(scraper, new FakeGenerator()).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors[0].Message).Contains("not available on PATH");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Write_Failure_Preserves_Old_Files_Because_Prune_Runs_Last()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };
            var generator = new FakeGenerator
            {
                OnGenerate = _ =>
                [
                    new GeneratedFile
                    {
                        RelativePath = Path.Combine(ToolOutputDirectory, "Options", "FakeNewOptions.Generated.cs"),
                        Content = "// new",
                    },
                    new GeneratedFile
                    {
                        // Embedded null char makes the write itself fail mid-loop
                        RelativePath = Path.Combine(ToolOutputDirectory, "Options", "Fake\0Invalid.Generated.cs"),
                        Content = "// bad",
                    },
                ],
            };

            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(File.Exists(existingFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Successful_Generation_Replaces_Old_Output()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };
            var generator = new FakeGenerator
            {
                OnGenerate = _ =>
                [
                    new GeneratedFile
                    {
                        RelativePath = Path.Combine(ToolOutputDirectory, "Options", "FakeRunOptions.Generated.cs"),
                        Content = "// new",
                    },
                ],
            };

            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(File.Exists(existingFile)).IsFalse();
            await Assert.That(File.Exists(Path.Combine(outputRoot, ToolOutputDirectory, "Options", "FakeRunOptions.Generated.cs"))).IsTrue();
            await Assert.That(result.FilesDeleted)
                .Contains(Path.GetRelativePath(outputRoot, existingFile));
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Successful_Generation_Removes_Legacy_Layout_But_Preserves_Other_Files()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var legacyOptionsDirectory = Path.Combine(outputRoot, ToolOutputDirectory, "Generated", "Options");
        Directory.CreateDirectory(legacyOptionsDirectory);

        var staleGeneratedFile = Path.Combine(legacyOptionsDirectory, "FakeOldOptions.cs");
        var handWrittenFile = Path.Combine(legacyOptionsDirectory, "FakeCustomOptions.cs");
        var otherToolGeneratedFile = Path.Combine(legacyOptionsDirectory, "OtherOldOptions.cs");
        var coverageManifest = Path.Combine(
            outputRoot,
            ToolOutputDirectory,
            "Generated",
            "Fake.CommandCoverage.json");
        await File.WriteAllTextAsync(
            staleGeneratedFile,
            """
            // <auto-generated>
            public record FakeOldOptions;
            // </auto-generated>
            """);
        await File.WriteAllTextAsync(handWrittenFile, "public record FakeCustomOptions;");
        await File.WriteAllTextAsync(
            otherToolGeneratedFile,
            """
            // <auto-generated>
            public record OtherOldOptions;
            // </auto-generated>
            """);

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };

            var result = await Orchestrator(scraper, new FakeGenerator())
                .GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(File.Exists(staleGeneratedFile)).IsFalse();
            await Assert.That(File.Exists(handWrittenFile)).IsTrue();
            await Assert.That(File.Exists(otherToolGeneratedFile)).IsTrue();
            await Assert.That(File.Exists(coverageManifest)).IsTrue();
            await Assert.That(result.FilesDeleted)
                .Contains(Path.GetRelativePath(outputRoot, staleGeneratedFile));
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Successful_Generation_Removes_Only_Stale_Marked_Documentation()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var documentationDirectory = Path.Combine(outputRoot, "docs", "docs", "mp-packages", "cli");
        Directory.CreateDirectory(documentationDirectory);

        var staleGeneratedFile = Path.Combine(documentationDirectory, "removed-tool.md");
        var handWrittenFile = Path.Combine(documentationDirectory, "hand-written.md");
        await File.WriteAllTextAsync(staleGeneratedFile, "---\n<!-- This file is generated by ModularPipelines.OptionsGenerator. -->\n");
        await File.WriteAllTextAsync(handWrittenFile, "---\n# Hand-written\n");

        try
        {
            var scraper = new FakeCliScraper { Commands = [FakeCommand()] };

            var result = await Orchestrator(scraper, new MarkdownDocumentationGenerator())
                .GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(File.Exists(Path.Combine(documentationDirectory, "fake.md"))).IsTrue();
            await Assert.That(File.Exists(staleGeneratedFile)).IsFalse();
            await Assert.That(File.Exists(handWrittenFile)).IsTrue();
            await Assert.That(result.FilesDeleted)
                .Contains(Path.GetRelativePath(outputRoot, staleGeneratedFile));
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Successful_Generation_Preserves_Global_Options()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);

        try
        {
            var globalOption = new CliOptionDefinition
            {
                SwitchName = "--search-path",
                PropertyName = "SearchPath",
                CSharpType = "string?",
            };
            var scraper = new FakeCliScraper
            {
                Commands = [FakeCommand()],
                GlobalOptions = [globalOption],
            };
            IReadOnlyList<CliOptionDefinition>? generatedGlobalOptions = null;
            var generator = new FakeGenerator
            {
                OnGenerate = tool =>
                {
                    generatedGlobalOptions = tool.GlobalOptions;
                    return [];
                },
            };

            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(generatedGlobalOptions).IsEquivalentTo([globalOption]);
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task CommandCoverageShrinkage_FailsBeforeMutatingGeneratedOutput()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);
        var generator = new FakeGenerator
        {
            OnGenerate = tool => tool.Commands.Select(command => new GeneratedFile
            {
                RelativePath = Path.Combine(
                    ToolOutputDirectory,
                    "Options",
                    $"{command.ClassName}.Generated.cs"),
                Content = $"// {command.FullCommand}",
            }).ToArray(),
        };
        var firstCommand = FakeCommand();
        var secondCommand = firstCommand with
        {
            FullCommand = "fake deploy",
            CommandParts = ["deploy"],
            ClassName = "FakeDeployOptions",
        };

        try
        {
            var baselineResult = await Orchestrator(
                    new FakeCliScraper { Commands = [firstCommand, secondCommand] },
                    generator)
                .GenerateAsync("fake", outputRoot);
            var deployFile = Path.Combine(
                outputRoot,
                ToolOutputDirectory,
                "Options",
                "FakeDeployOptions.Generated.cs");

            var shrinkResult = await Orchestrator(
                    new FakeCliScraper { Commands = [firstCommand] },
                    generator)
                .GenerateAsync("fake", outputRoot);

            await Assert.That(baselineResult.HasErrors).IsFalse();
            await Assert.That(shrinkResult.HasErrors).IsTrue();
            await Assert.That(shrinkResult.GetSummary()).Contains("Removed: fake deploy");
            await Assert.That(File.Exists(deployFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task ApprovedCommandCoverageShrinkage_UpdatesBaseline()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);
        var generator = new FakeGenerator();
        var firstCommand = FakeCommand();
        var secondCommand = firstCommand with
        {
            FullCommand = "fake deploy",
            CommandParts = ["deploy"],
            ClassName = "FakeDeployOptions",
        };

        try
        {
            await Orchestrator(
                    new FakeCliScraper { Commands = [firstCommand, secondCommand] },
                    generator)
                .GenerateAsync("fake", outputRoot);

            var approved = await Orchestrator(
                    new FakeCliScraper { Commands = [firstCommand] },
                    generator)
                .GenerateAsync(
                    "fake",
                    outputRoot,
                    approveCommandCoverageShrinkage: true);

            await Assert.That(approved.HasErrors).IsFalse();
            await Assert.That(approved.GetSummary()).Contains("Removed (approved): fake deploy");

            var manifest = await File.ReadAllTextAsync(Path.Combine(
                outputRoot,
                ToolOutputDirectory,
                "Generated",
                "Fake.CommandCoverage.json"));
            await Assert.That(manifest).Contains("\"commandCount\": 1");
            await Assert.That(manifest).DoesNotContain("fake deploy");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }
}
