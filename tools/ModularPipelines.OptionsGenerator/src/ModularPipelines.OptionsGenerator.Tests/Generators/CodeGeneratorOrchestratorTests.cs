using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

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

        public string NamespacePrefix { get; init; } = "Fake";

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

        public bool GenerateCode { get; init; } = true;

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
            GenerateCode = GenerateCode,
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

    private sealed class DiagnosticCliScraper(ICliCommandExecutor executor)
        : CliScraperBase(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<DiagnosticCliScraper>.Instance)
    {
        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => ToolOutputDirectory;

        public override CliToolDefinition CreateToolDefinition() =>
            base.CreateToolDefinition() with
            {
                CommandCoverage = new CliCommandCoveragePolicy
                {
                    MinimumCommandCount = 2,
                },
                ExecutablePrerequisite = new CliExecutablePrerequisite
                {
                    CommandName = "fake",
                },
            };

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(FakeCommand());
    }

    private sealed class DiagnosticExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "--version"
                    ? "fake 1.0"
                    : "RAW ROOT HELP: fake run only\nUsage: fake [flags]\nOptions:\n  --value string",
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
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

    private static CodeGeneratorOrchestrator Orchestrator(ICliScraper scraper, params ICodeGenerator[] generators) =>
        new(
            [scraper],
            htmlScrapers: [],
            generators,
            NullLogger<CodeGeneratorOrchestrator>.Instance);

    [Test]
    public async Task FirstGenerationCoverageFailure_WritesRawHelpDiagnostics()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);
        var diagnosticsPath = Path.Combine(
            outputRoot,
            "artifacts",
            "options-generator-diagnostics",
            "fake",
            "command-coverage-failure.json");

        try
        {
            var scraper = new DiagnosticCliScraper(new DiagnosticExecutor());

            var result = await Orchestrator(scraper, new FakeGenerator())
                .GenerateAsync("fake", outputRoot);

            await Assert.That(result.HasErrors).IsTrue();
            await Assert.That(result.Errors[0].Message).Contains("Raw help diagnostics");
            await Assert.That(File.Exists(diagnosticsPath)).IsTrue();
            var diagnostics = await File.ReadAllTextAsync(diagnosticsPath);
            await Assert.That(diagnostics).Contains("RAW ROOT HELP: fake run only");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task CliCoverageDiagnosticCancellation_Propagates()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputRoot);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        try
        {
            var scraper = new DiagnosticCliScraper(new DiagnosticExecutor());

            await Assert.That(async () => await Orchestrator(scraper, new FakeGenerator())
                    .GenerateAsync("fake", outputRoot, cancellationToken: cancellationTokenSource.Token))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

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
    public async Task Handwritten_Tool_Validates_Coverage_Without_Generation_Or_Cleanup()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();
        var generatorCalled = false;
        var scraper = new FakeCliScraper
        {
            Commands = [FakeCommand()],
            GenerateCode = false,
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
            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);

            using (Assert.Multiple())
            {
                await Assert.That(result.HasErrors).IsFalse();
                await Assert.That(generatorCalled).IsFalse();
                await Assert.That(File.Exists(existingFile)).IsTrue();
                await Assert.That(File.Exists(Path.Combine(
                        outputRoot,
                        ToolOutputDirectory,
                        "AssemblyInfo.Generated.cs")))
                    .IsFalse();
            }
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
    public async Task Selective_Generation_Preserves_Overlapping_Tool_Output()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-orchestrator-tests", Guid.NewGuid().ToString("N"));
        var fakeScraper = new FakeCliScraper { Commands = [FakeCommand()] };
        var fakeBoxScraper = new FakeCliScraper
        {
            ToolName = "fakebox",
            NamespacePrefix = "FakeBox",
        };
        var baseline = CommandCoverageGuard.Evaluate(
            fakeScraper.CreateToolDefinition() with { Commands = [FakeCommand()] },
            outputRoot,
            approveShrinkage: false);
        await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
        var overlappingPaths = new[]
        {
            Path.Combine(ToolOutputDirectory, "Options", "FakeBoxOldOptions.Generated.cs"),
            Path.Combine(ToolOutputDirectory, "Enums", "FakeBoxMode.Generated.cs"),
            Path.Combine(ToolOutputDirectory, "Services", "FakeBox.Generated.cs"),
            Path.Combine(ToolOutputDirectory, "Extensions", "FakeBoxExtensions.Generated.cs"),
        };
        foreach (var relativePath in overlappingPaths)
        {
            var fullPath = Path.Combine(outputRoot, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await File.WriteAllTextAsync(fullPath, "// <auto-generated />\n");
        }

        try
        {
            var orchestrator = new CodeGeneratorOrchestrator(
                [fakeScraper, fakeBoxScraper],
                htmlScrapers: [],
                [new FakeGenerator()],
                NullLogger<CodeGeneratorOrchestrator>.Instance);

            var result = await orchestrator.GenerateAsync("fake", outputRoot);

            using (Assert.Multiple())
            {
                await Assert.That(result.Errors.Select(static error => error.Message)).IsEmpty();
                foreach (var relativePath in overlappingPaths)
                {
                    await Assert.That(File.Exists(Path.Combine(outputRoot, relativePath))).IsTrue();
                    await Assert.That(result.FilesDeleted).DoesNotContain(relativePath);
                }
            }
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Successful_Generation_Removes_Casing_Variant_Output()
    {
        var (outputRoot, existingFile) = await CreateOutputRootWithExistingFileAsync();
        var optionsDirectory = Path.Combine(outputRoot, ToolOutputDirectory, "Options");
        File.Delete(existingFile);
        var stalePath = Path.Combine(optionsDirectory, "FakeRunoptions.Generated.cs");
        var currentPath = Path.Combine(optionsDirectory, "FakeRunOptions.Generated.cs");
        await File.WriteAllTextAsync(
            stalePath,
            "// <auto-generated>\npublic record FakeRunoptions;");
        var isCaseSensitive = !File.Exists(currentPath);

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
                        Content = "// <auto-generated>\npublic record FakeRunOptions;",
                    },
                ],
            };

            var result = await Orchestrator(scraper, generator).GenerateAsync("fake", outputRoot);
            var generatedFileCount = Directory.GetFiles(optionsDirectory)
                .Select(Path.GetFileName)
                .Count(name => name!.Equals("FakeRunOptions.Generated.cs", StringComparison.OrdinalIgnoreCase));

            await Assert.That(result.Errors).IsEmpty();
            await Assert.That(generatedFileCount).IsEqualTo(1);
            if (isCaseSensitive)
            {
                await Assert.That(File.Exists(stalePath)).IsFalse();
                await Assert.That(File.Exists(currentPath)).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    [Arguments("// <auto-generated>")]
    [Arguments("// <auto-generated/>")]
    [Arguments("// <AUTO-GENERATED />")]
    public async Task Successful_Generation_Removes_Legacy_Layout_But_Preserves_Other_Files(
        string generatedMarker)
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
            $"{generatedMarker}\npublic record FakeOldOptions;");
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
            OnGenerate = tool => tool.Commands.Select(command =>
            {
                var commandParts = string.Join(
                    ", ",
                    command.CommandParts.Select(static part => $"\"{part}\""));
                return new GeneratedFile
                {
                    RelativePath = Path.Combine(
                        ToolOutputDirectory,
                        "Options",
                        $"{command.ClassName}.Generated.cs"),
                    Content = "// <auto-generated />\n"
                              + "using ModularPipelines.Attributes;\n"
                              + $"[CliSubCommand({commandParts})] "
                              + $"public record {command.ClassName} : {command.ParentClassName};",
                };
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
            await Assert.That(shrinkResult.GetSummary())
                .Contains("WARNING: COMMAND COVERAGE SHRINKAGE DETECTED")
                .And.Contains("Removed: fake deploy");
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
            await Assert.That(approved.GetSummary())
                .Contains("WARNING: COMMAND COVERAGE SHRINKAGE DETECTED")
                .And.Contains("Removed (approved): fake deploy");
            await Assert.That(approved.GetSummary()).Contains(
                "Baseline comparison: 2 commands at fake 1.0 -> 1 commands at fake 1.0");

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
