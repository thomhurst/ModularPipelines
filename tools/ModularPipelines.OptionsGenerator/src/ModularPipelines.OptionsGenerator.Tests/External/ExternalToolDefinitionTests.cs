using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.External;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

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
    public async Task External_Metadata_Requires_Stable_Ownership_Id()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"ownershipId\": \"private-widget-integration\"",
                    "\"ownershipId\": \"\"",
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
    public async Task External_Metadata_Rejects_Command_Parts_That_Produce_Invalid_Method_Names()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var metadata = ValidMetadata("generated")
                .Replace(
                    "\"fullCommand\": \"private-widget deploy\"",
                    "\"fullCommand\": \"private-widget config:set\"",
                    StringComparison.Ordinal)
                .Replace(
                    "\"commandParts\": [\"deploy\"]",
                    "\"commandParts\": [\"config:set\"]",
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
    public async Task External_Metadata_Escapes_Generated_CSharp_String_Literals()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory);
            var command = tool.Commands.Single();
            var option = command.Options.Single() with
            {
                SwitchName = "--environment\"quoted",
                ShortForm = "-\"",
                ValidationConstraints = new() { Pattern = "^\"quoted\"$" },
            };
            tool = tool with
            {
                Commands = [command with { Options = [option] }],
            };

            await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory);

            var generatedSource = await File.ReadAllTextAsync(Path.Combine(
                outputDirectory,
                "generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs"));
            var syntaxErrors = CSharpSyntaxTree.ParseText(generatedSource)
                .GetDiagnostics()
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .ToArray();
            await Assert.That(syntaxErrors).IsEmpty();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Rejects_Linked_Cleanup_Directory()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var generatedDirectory = Path.Combine(outputDirectory, "generated");
        var outsideDirectory = Path.Combine(workspace, "outside");
        var linkedEnumsDirectory = Path.Combine(generatedDirectory, "Enums");
        var outsideGeneratedFile = Path.Combine(
            outsideDirectory,
            "PrivateWidgetStale.Generated.cs");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory);
            Directory.CreateDirectory(generatedDirectory);
            Directory.CreateDirectory(outsideDirectory);
            await File.WriteAllTextAsync(outsideGeneratedFile, "// <auto-generated>");
            try
            {
                Directory.CreateSymbolicLink(linkedEnumsDirectory, outsideDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
            await Assert.That(File.Exists(outsideGeneratedFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Preserves_Unmarked_Generated_Files_On_First_Run()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var existingFile = Path.Combine(
            outputDirectory,
            "generated",
            "Options",
            "PrivateWidgetLegacy.Generated.cs");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            Directory.CreateDirectory(Path.GetDirectoryName(existingFile)!);
            await File.WriteAllTextAsync(existingFile, "// Generated by another tool.");
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);

            await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory);

            await Assert.That(File.Exists(existingFile)).IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Rejects_Unowned_Existing_Output()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var existingFile = Path.Combine(
            outputDirectory,
            "generated",
            "Options",
            "PrivateWidgetDeployOptions.Generated.cs");
        const string existingContent = "// Hand-written file.";

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            Directory.CreateDirectory(Path.GetDirectoryName(existingFile)!);
            await File.WriteAllTextAsync(existingFile, existingContent);
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
            await Assert.That(await File.ReadAllTextAsync(existingFile))
                .IsEqualTo(existingContent);
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Rejects_Ownership_Manifest_For_Another_Tool()
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

            var firstService = Path.Combine(
                outputDirectory,
                "generated-a",
                "Services",
                "IPrivateWidget.Generated.cs");
            var command = firstTool.Commands.Single();
            var secondTool = firstTool with
            {
                OwnershipId = "other-widget-integration",
                ToolName = "other-widget",
                OutputDirectory = "generated-b",
                Commands =
                [
                    command with
                    {
                        FullCommand = "other-widget deploy",
                    },
                ],
            };

            await Assert.That(async () =>
                    await orchestrator.GenerateFromDefinitionAsync(secondTool, outputDirectory))
                .Throws<InvalidDataException>();
            await Assert.That(File.Exists(firstService)).IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Rejects_Path_Claimed_By_Another_Owner()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var claimedFile = Path.Combine(
                outputDirectory,
                "generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs");
            var originalContent = await File.ReadAllTextAsync(claimedFile);
            var command = firstTool.Commands.Single();
            var secondTool = firstTool with
            {
                OwnershipId = "other-widget-integration",
                ToolName = "other-widget",
                NamespacePrefix = "OtherWidget",
                TargetNamespace = "Example.Build.OtherWidget",
                Commands =
                [
                    command with
                    {
                        FullCommand = "other-widget deploy",
                        ParentClassName = "OtherWidgetOptions",
                        ToolNamespacePrefix = "OtherWidget",
                    },
                ],
            };

            await Assert.That(async () =>
                    await orchestrator.GenerateFromDefinitionAsync(secondTool, outputDirectory))
                .Throws<InvalidOperationException>();
            await Assert.That(await File.ReadAllTextAsync(claimedFile))
                .IsEqualTo(originalContent);
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    ".modular-pipelines-options",
                    "OtherWidget.files")))
                .IsFalse();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Isolates_Prefix_Related_Tool_Ownership()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var template = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var command = template.Commands.Single();
            var fooBar = template with
            {
                OwnershipId = "foo-bar-integration",
                ToolName = "foo-bar",
                NamespacePrefix = "FooBar",
                TargetNamespace = "Example.Build.FooBar",
                Commands =
                [
                    command with
                    {
                        FullCommand = "foo-bar deploy",
                        ClassName = "FooBarDeployOptions",
                        ParentClassName = "FooBarOptions",
                        ToolNamespacePrefix = "FooBar",
                    },
                ],
            };
            var foo = template with
            {
                OwnershipId = "foo-integration",
                ToolName = "foo",
                NamespacePrefix = "Foo",
                TargetNamespace = "Example.Build.Foo",
                Commands =
                [
                    command with
                    {
                        FullCommand = "foo deploy",
                        ClassName = "FooDeployOptions",
                        ParentClassName = "FooOptions",
                        ToolNamespacePrefix = "Foo",
                    },
                ],
            };

            await orchestrator.GenerateFromDefinitionAsync(fooBar, outputDirectory);
            var fooBarOptions = Path.Combine(
                outputDirectory,
                "generated",
                "Options",
                "FooBarDeployOptions.Generated.cs");
            var sharedAssemblyInfo = Path.Combine(
                outputDirectory,
                "generated",
                "AssemblyInfo.Generated.cs");
            await Assert.That(File.Exists(fooBarOptions)).IsTrue();
            await Assert.That(File.Exists(sharedAssemblyInfo)).IsFalse();

            await orchestrator.GenerateFromDefinitionAsync(foo, outputDirectory);

            await Assert.That(File.Exists(fooBarOptions)).IsTrue();
            await Assert.That(File.Exists(sharedAssemblyInfo)).IsFalse();
            foreach (var manifest in Directory.GetFiles(
                         Path.Combine(outputDirectory, ".modular-pipelines-options"),
                         "*.files"))
            {
                await Assert.That(await File.ReadAllTextAsync(manifest))
                    .DoesNotContain("AssemblyInfo.Generated.cs");
            }
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Duplicate_Generated_Member_Names()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var command = tool.Commands.Single();
            tool = tool with
            {
                Commands =
                [
                    command with
                    {
                        PositionalArguments =
                        [
                            new CliPositionalArgument
                            {
                                PropertyName = "environment",
                                CSharpType = "string?",
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Conflicting_Same_Name_Enums()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            var environment = new CliEnumDefinition
            {
                EnumName = "PrivateWidgetEnvironment",
                Values =
                [
                    new CliEnumValue
                    {
                        MemberName = "Production",
                        CliValue = "production",
                    },
                ],
            };
            tool = tool with
            {
                Commands =
                [
                    deploy with
                    {
                        Enums = [environment],
                    },
                    deploy with
                    {
                        FullCommand = "private-widget destroy",
                        CommandParts = ["destroy"],
                        ClassName = "PrivateWidgetDestroyOptions",
                        Enums =
                        [
                            environment with
                            {
                                Values =
                                [
                                    new CliEnumValue
                                    {
                                        MemberName = "Staging",
                                        CliValue = "staging",
                                    },
                                ],
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Includes_Option_Local_Enum()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            var environment = new CliEnumDefinition
            {
                EnumName = "PrivateWidgetEnvironment",
                Values =
                [
                    new CliEnumValue
                    {
                        MemberName = "Production",
                        CliValue = "production",
                    },
                ],
            };
            tool = tool with
            {
                Commands =
                [
                    deploy with
                    {
                        Options =
                        [
                            deploy.Options.Single() with
                            {
                                CSharpType = "PrivateWidgetEnvironment?",
                                EnumDefinition = environment,
                            },
                        ],
                    },
                ],
            };

            await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory);

            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    "generated",
                    "Enums",
                    "PrivateWidgetEnvironment.Generated.cs")))
                .IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Duplicate_Enum_Member_Names()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            tool = tool with
            {
                Commands =
                [
                    deploy with
                    {
                        Enums =
                        [
                            new CliEnumDefinition
                            {
                                EnumName = "PrivateWidgetEnvironment",
                                Values =
                                [
                                    new CliEnumValue
                                    {
                                        MemberName = "Production",
                                        CliValue = "production",
                                    },
                                    new CliEnumValue
                                    {
                                        MemberName = "Production",
                                        CliValue = "prod",
                                    },
                                ],
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Secret_Keys_On_Nonsecret_Option()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            var option = deploy.Options.Single() with
            {
                IsKeyValue = true,
                SecretValueKeys = ["token"],
            };
            tool = tool with
            {
                Commands = [deploy with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Secret_Keys_On_Scalar_Option()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            var option = deploy.Options.Single() with
            {
                IsSecret = true,
                SecretValueKeys = ["token"],
            };
            tool = tool with
            {
                Commands = [deploy with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Whitespace_Short_Form()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            var option = deploy.Options.Single() with
            {
                ShortForm = " ",
                PreferShortForm = true,
            };
            tool = tool with
            {
                Commands = [deploy with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Command_Switch_Colliding_With_Global_Alias()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            tool = tool with
            {
                GlobalOptions =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--global-environment",
                        ShortForm = "--environment",
                        PropertyName = "GlobalEnvironment",
                        CSharpType = "string?",
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
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

    [Test]
    public async Task External_Metadata_Preserves_Coverage_Baseline_When_Output_Moves()
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
            var deploy = firstTool.Commands.Single();
            firstTool = firstTool with
            {
                Commands =
                [
                    deploy,
                    deploy with
                    {
                        FullCommand = "private-widget destroy",
                        CommandParts = ["destroy"],
                        ClassName = "PrivateWidgetDestroyOptions",
                    },
                ],
            };
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated-b"));
            var secondTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);

            await Assert.That(async () =>
                    await orchestrator.GenerateFromDefinitionAsync(secondTool, outputDirectory))
                .Throws<InvalidOperationException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Preserves_Coverage_Baseline_Across_Case_Only_Output_Rename()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            if (!IsCaseSensitiveFileSystem(workspace))
            {
                return;
            }

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = firstTool.Commands.Single();
            firstTool = firstTool with
            {
                Commands =
                [
                    deploy,
                    deploy with
                    {
                        FullCommand = "private-widget destroy",
                        CommandParts = ["destroy"],
                        ClassName = "PrivateWidgetDestroyOptions",
                    },
                ],
            };
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var secondTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);

            await Assert.That(async () =>
                    await orchestrator.GenerateFromDefinitionAsync(
                        secondTool with { OutputDirectory = "Generated" },
                        outputDirectory))
                .Throws<InvalidOperationException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Reconciles_Ownership_When_Namespace_Prefix_Changes()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var oldManifest = Path.Combine(
                outputDirectory,
                ".modular-pipelines-options",
                "PrivateWidget.files");
            var oldService = Path.Combine(
                outputDirectory,
                "generated",
                "Services",
                "IPrivateWidget.Generated.cs");
            await Assert.That(File.Exists(oldManifest)).IsTrue();
            await Assert.That(File.Exists(oldService)).IsTrue();

            var renamedTool = firstTool with
            {
                NamespacePrefix = "RenamedWidget",
                Commands = firstTool.Commands
                    .Select(command => command with
                    {
                        ParentClassName = "RenamedWidgetOptions",
                        ToolNamespacePrefix = "RenamedWidget",
                    })
                    .ToList(),
            };
            var result = await orchestrator.GenerateFromDefinitionAsync(
                renamedTool,
                outputDirectory);

            await Assert.That(File.Exists(oldManifest)).IsFalse();
            await Assert.That(File.Exists(oldService)).IsFalse();
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    ".modular-pipelines-options",
                    "RenamedWidget.files")))
                .IsTrue();
            await Assert.That(result.FilesDeleted.Select(path => path.Replace('\\', '/')))
                .Contains(".modular-pipelines-options/PrivateWidget.files");
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Preserves_Enum_Ordinals_When_Output_Moves()
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
            var deploy = firstTool.Commands.Single();
            var environment = new CliEnumDefinition
            {
                EnumName = "PrivateWidgetEnvironment",
                Values =
                [
                    new CliEnumValue
                    {
                        MemberName = "Production",
                        CliValue = "production",
                    },
                    new CliEnumValue
                    {
                        MemberName = "Staging",
                        CliValue = "staging",
                    },
                ],
            };
            firstTool = firstTool with
            {
                Commands =
                [
                    deploy with
                    {
                        Enums = [environment],
                    },
                ],
            };
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var secondTool = firstTool with
            {
                OutputDirectory = "generated-b",
                Commands =
                [
                    deploy with
                    {
                        Enums =
                        [
                            environment with
                            {
                                Values =
                                [
                                    environment.Values[1],
                                    new CliEnumValue
                                    {
                                        MemberName = "Development",
                                        CliValue = "development",
                                    },
                                    environment.Values[0],
                                ],
                            },
                        ],
                    },
                ],
            };
            await orchestrator.GenerateFromDefinitionAsync(secondTool, outputDirectory);

            var generatedEnum = await File.ReadAllTextAsync(Path.Combine(
                outputDirectory,
                "generated-b",
                "Enums",
                "PrivateWidgetEnvironment.Generated.cs"));
            await Assert.That(generatedEnum).Contains("Production = 0");
            await Assert.That(generatedEnum).Contains("Staging = 1");
            await Assert.That(generatedEnum).Contains("Development = 2");
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Reconciles_Case_Only_Output_Renames()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            if (!IsCaseSensitiveFileSystem(workspace))
            {
                return;
            }

            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var firstTool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            await orchestrator.GenerateFromDefinitionAsync(firstTool, outputDirectory);

            var oldOptionsPath = Path.Combine(
                outputDirectory,
                "generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs");
            var newOptionsPath = Path.Combine(
                outputDirectory,
                "Generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs");
            await Assert.That(File.Exists(oldOptionsPath)).IsTrue();

            await orchestrator.GenerateFromDefinitionAsync(
                firstTool with { OutputDirectory = "Generated" },
                outputDirectory);

            await Assert.That(File.Exists(oldOptionsPath)).IsFalse();
            await Assert.That(File.Exists(newOptionsPath)).IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Missing_Compatibility_Forwarding_Target()
    {
        var workspace = CreateTemporaryDirectory();
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
            var tool = await ExternalToolDefinitionLoader.LoadAsync(
                metadataPath,
                outputDirectory);
            var deploy = tool.Commands.Single();
            tool = tool with
            {
                Commands =
                [
                    deploy with
                    {
                        CompatibilityProperties =
                        [
                            new CliCompatibilityProperty
                            {
                                PropertyName = "OldEnvironment",
                                CSharpType = "string?",
                                ForwardToPropertyName = "Environmnt",
                                ObsoleteMessage = "Use Environment.",
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Flag_With_Unrenderable_Type()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var option = command.Options.Single() with
            {
                IsFlag = true,
                CSharpType = "string?",
            };
            tool = tool with
            {
                Commands = [command with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Declaration_Invalid_Types()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            foreach (var invalidType in new[] { "void", "var" })
            {
                var tool = await LoadValidToolAsync(workspace, outputDirectory);
                var command = tool.Commands.Single();
                var option = command.Options.Single() with { CSharpType = invalidType };
                tool = tool with
                {
                    Commands = [command with { Options = [option] }],
                };

                await Assert.That(async () =>
                        await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                    .Throws<InvalidDataException>();
            }
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Requires_Custom_Optional_Collection_Shape()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var option = command.Options.Single() with
            {
                CSharpType = "PrivatePackage.CustomValues?",
                ValueArity = CliOptionValueArity.Optional,
            };
            tool = tool with
            {
                Commands = [command with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Preserves_Custom_Optional_Collection_Shape()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var option = command.Options.Single() with
            {
                CSharpType = "PrivatePackage.CustomValues?",
                ValueArity = CliOptionValueArity.Optional,
                IsCollection = true,
            };
            tool = tool with
            {
                Commands = [command with { Options = [option] }],
            };

            var result = await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory);
            var generatedOptions = await File.ReadAllTextAsync(Path.Combine(
                outputDirectory,
                "generated",
                "Options",
                "PrivateWidgetDeployOptions.Generated.cs"));

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(generatedOptions).Contains("IEnumerable<CliOptionValue>? Environment");
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Command_Member_Colliding_With_Global_Property()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            tool = tool with
            {
                GlobalOptions =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--global-environment",
                        PropertyName = "Environment",
                        CSharpType = "string?",
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Compatibility_Forwarding_To_Init_Only_Property()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            tool = tool with
            {
                Commands =
                [
                    command with
                    {
                        CompatibilityProperties =
                        [
                            new CliCompatibilityProperty
                            {
                                PropertyName = "LegacyTool",
                                CSharpType = "string?",
                                ForwardToPropertyName = "Tool",
                                ObsoleteMessage = "Use Tool.",
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Incompatible_Compatibility_Forwarding_Type()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            tool = tool with
            {
                Commands =
                [
                    command with
                    {
                        CompatibilityProperties =
                        [
                            new CliCompatibilityProperty
                            {
                                PropertyName = "LegacyEnvironment",
                                CSharpType = "int?",
                                ForwardToPropertyName = "Environment",
                                ObsoleteMessage = "Use Environment.",
                            },
                        ],
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Execute_Child_Collision()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var parent = command with
            {
                FullCommand = "private-widget config",
                CommandParts = ["config"],
                ClassName = "PrivateWidgetConfigOptions",
                SubDomainGroup = null,
            };
            var nested = command with
            {
                FullCommand = "private-widget config execute-async run",
                CommandParts = ["config", "execute-async", "run"],
                ClassName = "PrivateWidgetConfigExecuteAsyncRunOptions",
                SubDomainGroup = "Config",
            };
            tool = tool with { Commands = [parent, nested] };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidOperationException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Undeclared_Parent_Options_Class()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            tool = tool with
            {
                Commands = [command with { ParentClassName = "MissingOptions" }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Whitespace_In_Primary_Switch_Name()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var option = command.Options.Single() with { SwitchName = "--output file" };
            tool = tool with
            {
                Commands = [command with { Options = [option] }],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Metadata_Rejects_Required_Global_Options()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            tool = tool with
            {
                GlobalOptions =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--required-global",
                        PropertyName = "RequiredGlobal",
                        CSharpType = "string",
                        IsRequired = true,
                    },
                ],
            };

            await Assert.That(async () =>
                    await CreateOrchestrator().GenerateFromDefinitionAsync(tool, outputDirectory))
                .Throws<InvalidDataException>();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Recovers_After_Output_Write_Failure()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var coveragePath = CommandCoverageGuard.GetManifestPath(tool, outputDirectory);
            Directory.CreateDirectory(coveragePath);

            try
            {
                await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);
                throw new InvalidOperationException("Generation unexpectedly succeeded.");
            }
            catch (Exception exception)
                when (exception is IOException or UnauthorizedAccessException)
            {
                // Expected: the coverage manifest path is deliberately a directory.
            }

            var optionsRelativePath = Path.Combine(
                    "generated",
                    "Options",
                    "PrivateWidgetDeployOptions.Generated.cs")
                .Replace('\\', '/');
            var ownershipPath = Path.Combine(
                outputDirectory,
                ".modular-pipelines-options",
                "PrivateWidget.files");
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    optionsRelativePath)))
                .IsTrue();
            await Assert.That(await File.ReadAllTextAsync(ownershipPath))
                .Contains(optionsRelativePath);

            Directory.Delete(coveragePath);
            var result = await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(File.Exists(coveragePath)).IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Recovers_Enum_Baselines_After_Interrupted_Output_Move()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var command = tool.Commands.Single();
            var environment = new CliEnumDefinition
            {
                EnumName = "PrivateWidgetEnvironment",
                Values =
                [
                    new CliEnumValue
                    {
                        MemberName = "Production",
                        CliValue = "production",
                    },
                ],
            };
            tool = tool with
            {
                Commands = [command with { Enums = [environment] }],
            };
            await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);

            var movedTool = tool with { OutputDirectory = "moved" };
            var movedCoveragePath = CommandCoverageGuard.GetManifestPath(
                movedTool,
                outputDirectory);
            Directory.CreateDirectory(movedCoveragePath);

            try
            {
                await orchestrator.GenerateFromDefinitionAsync(movedTool, outputDirectory);
                throw new InvalidOperationException("Generation unexpectedly succeeded.");
            }
            catch (Exception exception)
                when (exception is IOException or UnauthorizedAccessException)
            {
                // Expected: source files and the recovery journal were written first.
            }

            Directory.Delete(movedCoveragePath);
            var result = await orchestrator.GenerateFromDefinitionAsync(
                movedTool,
                outputDirectory);

            await Assert.That(result.HasErrors).IsFalse();
            await Assert.That(File.Exists(Path.Combine(
                    outputDirectory,
                    "moved",
                    "Enums",
                    "PrivateWidgetEnvironment.Generated.cs")))
                .IsTrue();
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Releases_Ownership_When_Stale_File_Loses_Marker()
    {
        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var documentedTool = tool with { DocumentationOutputDirectory = "docs" };
            await orchestrator.GenerateFromDefinitionAsync(documentedTool, outputDirectory);

            var documentationRelativePath = Path.Combine(
                    "docs",
                    "private-widget.md")
                .Replace('\\', '/');
            var documentationPath = Path.Combine(
                outputDirectory,
                documentationRelativePath);
            var ownershipPath = Path.Combine(
                outputDirectory,
                ".modular-pipelines-options",
                "PrivateWidget.files");
            const string handAuthoredContent = "# Hand-authored documentation";
            await File.WriteAllTextAsync(documentationPath, handAuthoredContent);

            await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);

            await Assert.That(File.Exists(documentationPath)).IsTrue();
            await Assert.That(await File.ReadAllTextAsync(ownershipPath))
                .DoesNotContain(documentationRelativePath);
            await Assert.That(async () =>
                    await orchestrator.GenerateFromDefinitionAsync(documentedTool, outputDirectory))
                .Throws<InvalidDataException>();
            await Assert.That(await File.ReadAllTextAsync(documentationPath))
                .IsEqualTo(handAuthoredContent);
        }
        finally
        {
            Directory.Delete(workspace, recursive: true);
        }
    }

    [Test]
    public async Task External_Generation_Retains_Ownership_When_Stale_Delete_Fails()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var workspace = CreateTemporaryDirectory();
        var outputDirectory = Path.Combine(workspace, "integration");
        var orchestrator = CreateOrchestrator();

        try
        {
            var tool = await LoadValidToolAsync(workspace, outputDirectory);
            var documentedTool = tool with { DocumentationOutputDirectory = "docs" };
            await orchestrator.GenerateFromDefinitionAsync(documentedTool, outputDirectory);

            var documentationRelativePath = Path.Combine(
                    "docs",
                    "private-widget.md")
                .Replace('\\', '/');
            var documentationPath = Path.Combine(
                outputDirectory,
                documentationRelativePath);
            var ownershipPath = Path.Combine(
                outputDirectory,
                ".modular-pipelines-options",
                "PrivateWidget.files");

            await using (new FileStream(
                             documentationPath,
                             FileMode.Open,
                             FileAccess.Read,
                             FileShare.Read))
            {
                await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);
                await Assert.That(File.Exists(documentationPath)).IsTrue();
                await Assert.That(await File.ReadAllTextAsync(ownershipPath))
                    .Contains(documentationRelativePath);
            }

            await orchestrator.GenerateFromDefinitionAsync(tool, outputDirectory);

            await Assert.That(File.Exists(documentationPath)).IsFalse();
            await Assert.That(await File.ReadAllTextAsync(ownershipPath))
                .DoesNotContain(documentationRelativePath);
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

    private static async Task<CliToolDefinition> LoadValidToolAsync(
        string workspace,
        string outputDirectory)
    {
        var metadataPath = Path.Combine(workspace, "private-widget.json");
        await File.WriteAllTextAsync(metadataPath, ValidMetadata("generated"));
        return await ExternalToolDefinitionLoader.LoadAsync(metadataPath, outputDirectory);
    }

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

    private static bool IsCaseSensitiveFileSystem(string directory)
    {
        var probePath = Path.Combine(
            directory,
            $"case-probe-a{Guid.NewGuid():N}");

        try
        {
            File.WriteAllText(probePath, string.Empty);
            return !File.Exists(Path.Combine(
                directory,
                Path.GetFileName(probePath).ToUpperInvariant()));
        }
        finally
        {
            File.Delete(probePath);
        }
    }

    private static string ValidMetadata(string outputDirectory, int schemaVersion = 1) =>
        $$"""
          {
            "schemaVersion": {{schemaVersion}},
            "tool": {
              "ownershipId": "private-widget-integration",
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
