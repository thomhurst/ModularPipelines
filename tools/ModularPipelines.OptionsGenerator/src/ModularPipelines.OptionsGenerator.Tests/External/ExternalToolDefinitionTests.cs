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
