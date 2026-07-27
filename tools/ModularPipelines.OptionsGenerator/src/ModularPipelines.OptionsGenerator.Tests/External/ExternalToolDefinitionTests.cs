using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.External;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.External;

public class ExternalToolDefinitionTests
{
    [Test]
    public async Task External_Cli_Invocation_Generates_From_Another_Working_Directory()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var executablePath = Path.Combine(
                AppContext.BaseDirectory,
                OperatingSystem.IsWindows()
                    ? "ModularPipelines.OptionsGenerator.exe"
                    : "ModularPipelines.OptionsGenerator");
            var startInfo = new ProcessStartInfo(executablePath)
            {
                WorkingDirectory = workspace,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            startInfo.ArgumentList.Add("--input");
            startInfo.ArgumentList.Add("private-widget.json");
            startInfo.ArgumentList.Add("--output-dir");
            startInfo.ArgumentList.Add("integration");

            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Could not start the generator process.");
            var standardOutput = process.StandardOutput.ReadToEndAsync();
            var standardError = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            var output = await standardOutput;
            var error = await standardError;

            await Assert.That(process.ExitCode)
                .IsEqualTo(0)
                .Because($"stdout: {output}{Environment.NewLine}stderr: {error}");
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    "generated",
                    "Options",
                    "PrivateWidgetDeployOptions.Generated.cs")))
                .IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Generates_Deterministic_Output_Outside_Repository()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory);
            var orchestrator = CreateOrchestrator();

            var firstResult = await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);
            var firstFiles = ReadGeneratedFiles(outputDirectory);
            var secondResult = await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);
            var secondFiles = ReadGeneratedFiles(outputDirectory);

            await Assert.That(firstResult.HasErrors).IsFalse();
            await Assert.That(secondResult.HasErrors).IsFalse();
            await Assert.That(firstFiles.Keys).IsEquivalentTo(secondFiles.Keys);
            foreach (var path in firstFiles.Keys)
            {
                await Assert.That(secondFiles[path]).IsEqualTo(firstFiles[path]);
            }

            var optionsPath = Path.Combine(
                "generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs");
            await Assert.That(firstFiles.ContainsKey(optionsPath)).IsTrue();
            await Assert.That(firstFiles.Keys.Any(path =>
                    path.Contains("docs", StringComparison.OrdinalIgnoreCase)))
                .IsFalse();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Output_Outside_Selected_Root()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("../escaped"));

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Unknown_Schema_Version()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(
                metadataPath,
                ValidMetadata("generated", schemaVersion: 2));

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Linked_Output_Component()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var outsideDirectory = Path.Combine(workspace, "outside");
        var linkedDirectory = Path.Combine(outputDirectory, "linked");

        try
        {
            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(outsideDirectory);
            try
            {
                Directory.CreateSymbolicLink(linkedDirectory, outsideDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("linked"));

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Linked_Output_Root()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var outsideDirectory = Path.Combine(workspace, "outside");

        try
        {
            Directory.CreateDirectory(outsideDirectory);
            try
            {
                Directory.CreateSymbolicLink(outputDirectory, outsideDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Invalid_CSharp_Identifiers()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"className\": \"PrivateWidgetDeployOptions\"",
                    "\"className\": \"123Deploy\"",
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, metadata);

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Reserved_CSharp_Keywords()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"className\": \"PrivateWidgetDeployOptions\"",
                    "\"className\": \"class\"",
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, metadata);

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Mismatched_Full_Command()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"fullCommand\": \"private-widget deploy\"",
                    "\"fullCommand\": \"private-widget destroy\"",
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, metadata);

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Rejects_Link_Added_After_Metadata_Validation()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var generatedDirectory = Path.Combine(outputDirectory, "generated");
        var outsideDirectory = Path.Combine(workspace, "outside");
        var linkedOptionsDirectory = Path.Combine(generatedDirectory, "Options");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory);
            Directory.CreateDirectory(generatedDirectory);
            Directory.CreateDirectory(outsideDirectory);
            try
            {
                Directory.CreateSymbolicLink(linkedOptionsDirectory, outsideDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
            await Assert.That(Directory.GetFiles(outsideDirectory)).IsEmpty();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Traversal_In_Enum_Name()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"options\": [",
                    """
                    "enums": [
                      {
                        "enumName": "../../../../outside",
                        "values": [
                          {
                            "memberName": "Production",
                            "cliValue": "production"
                          }
                        ]
                      }
                    ],
                    "options": [
                    """,
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, metadata);

            await Assert.That(async () =>
                    await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Reconciles_Previously_Owned_Documentation()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            var firstMetadata = ValidMetadata("generated")
                .Replace(
                    "\"documentationOutputDirectory\": null",
                    "\"documentationOutputDirectory\": \"docs-a\"",
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, firstMetadata);
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var oldDocumentation = Path.Combine(
                outputDirectory,
                "docs-a",
                "private-widget.md");
            await Assert.That(File.Exists(oldDocumentation)).IsTrue();

            var secondMetadata = ValidMetadata("generated")
                .Replace(
                    "\"toolName\": \"private-widget\"",
                    "\"toolName\": \"private-widget-next\"",
                    StringComparison.Ordinal)
                .Replace(
                    "\"fullCommand\": \"private-widget deploy\"",
                    "\"fullCommand\": \"private-widget-next deploy\"",
                    StringComparison.Ordinal)
                .Replace(
                    "\"documentationOutputDirectory\": null",
                    "\"documentationOutputDirectory\": \"docs-b\"",
                    StringComparison.Ordinal);
            await File.WriteAllTextAsync(metadataPath, secondMetadata);
            var secondTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var result = await orchestrator.GenerateFromDefinitionAsync(
                secondTool,
                outputDirectory,
                approveCommandCoverageShrinkage: true);

            await Assert.That(File.Exists(oldDocumentation)).IsFalse();
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    "docs-b",
                    "private-widget-next.md")))
                .IsTrue();
            await Assert.That(result.FilesDeleted.Select(path => path.Replace('\\', '/')))
                .Contains("docs-a/private-widget.md");
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    ".modular-pipelines-options",
                    "PrivateWidget.files")))
                .IsTrue();

            await File.WriteAllTextAsync(
                metadataPath,
                ValidMetadata("generated").Replace(
                    "\"toolName\": \"private-widget\"",
                    "\"toolName\": \"private-widget-next\"",
                    StringComparison.Ordinal).Replace(
                    "\"fullCommand\": \"private-widget deploy\"",
                    "\"fullCommand\": \"private-widget-next deploy\"",
                    StringComparison.Ordinal));
            var thirdTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var thirdResult = await orchestrator.GenerateFromDefinitionAsync(
                thirdTool,
                outputDirectory,
                approveCommandCoverageShrinkage: true);

            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    "docs-b",
                    "private-widget-next.md")))
                .IsFalse();
            await Assert.That(thirdResult.FilesDeleted.Select(path => path.Replace('\\', '/')))
                .Contains("docs-b/private-widget-next.md");
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Reconciles_Previously_Owned_Command_Coverage()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated-a"));
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var oldCoverage = Path.Combine(
                outputDirectory,
                "generated-a",
                "Generated",
                "PrivateWidget.CommandCoverage.json");
            await Assert.That(File.Exists(oldCoverage)).IsTrue();

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated-b"));
            var secondTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var result = await orchestrator.GenerateFromDefinitionAsync(
                secondTool,
                outputDirectory);

            await Assert.That(File.Exists(oldCoverage)).IsFalse();
            await Assert.That(result.FilesDeleted.Select(path => path.Replace('\\', '/')))
                .Contains("generated-a/Generated/PrivateWidget.CommandCoverage.json");
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    private static CodeGeneratorOrchestrator CreateOrchestrator() =>
        new(
            cliScrapers: [],
            htmlScrapers: [],
            generators:
            [
                new OptionsClassGenerator(),
                new EnumGenerator(),
                new ServiceInterfaceGenerator(),
                new ServiceImplementationGenerator(),
                new SubDomainClassGenerator(),
                new GlobalOptionsBaseGenerator(),
                new DependencyRegistrationGenerator(),
                new MarkdownDocumentationGenerator(),
            ],
            NullLogger<CodeGeneratorOrchestrator>.Instance);

    private static Dictionary<string, string> ReadGeneratedFiles(string outputDirectory) =>
        Directory.GetFiles(outputDirectory, "*", SearchOption.AllDirectories)
            .ToDictionary(
                path => Path.GetRelativePath(outputDirectory, path),
                File.ReadAllText,
                StringComparer.OrdinalIgnoreCase);

    private static string CreateTemporaryDirectory()
    {
        var path = Path.Combine(
            Path.GetTempPath(),
            "ModularPipelines.OptionsGenerator.ExternalTests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static string ValidMetadata(string outputDirectory, int schemaVersion = 1) =>
        $$"""
          {
            "schemaVersion": {{schemaVersion}},
            "tool": {
              "toolName": "private-widget",
              "namespacePrefix": "PrivateWidget",
              "targetNamespace": "Example.Build.PrivateWidget",
              "outputDirectory": "{{outputDirectory}}",
              "documentationOutputDirectory": null,
              "executablePrerequisiteMetadataExemption": "Installation is controlled by the private repository.",
              "commands": [
                {
                  "fullCommand": "private-widget deploy",
                  "commandParts": ["deploy"],
                  "className": "PrivateWidgetDeployOptions",
                  "parentClassName": "PrivateWidgetOptions",
                  "toolNamespacePrefix": "PrivateWidget",
                  "description": "Deploys a private widget.",
                  "options": [
                    {
                      "switchName": "--environment",
                      "propertyName": "Environment",
                      "cSharpType": "string?",
                      "description": "Deployment environment."
                    }
                  ]
                }
              ]
            }
          }
          """;
}
