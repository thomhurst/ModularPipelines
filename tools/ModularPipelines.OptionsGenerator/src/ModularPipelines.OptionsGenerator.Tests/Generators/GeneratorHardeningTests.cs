using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.External;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

/// <summary>
/// Tests for the generator hardening fixes: output path collision detection,
/// single-command class name normalization, nullable parameter emission,
/// case-insensitive enum deduplication and shared root command filtering.
/// </summary>
public class GeneratorHardeningTests
{
    private static CliCommandDefinition Command(
        string className,
        string parentClassName,
        string[]? commandParts = null,
        string? subDomainGroup = null,
        string? commandGroupIdentifierOverride = null,
        IReadOnlyList<CliEnumDefinition>? enums = null,
        IReadOnlyList<CliOptionDefinition>? options = null) =>
        new()
        {
            FullCommand = "tool",
            CommandParts = commandParts ?? [],
            ClassName = className,
            ParentClassName = parentClassName,
            ToolNamespacePrefix = "Tool",
            Options = options ?? [],
            SubDomainGroup = subDomainGroup,
            CommandGroupIdentifierOverride = commandGroupIdentifierOverride,
            Enums = enums ?? [],
        };

    [Test]
    public async Task OptionsClassGenerator_Uses_CliOptionValue_For_Optional_Value_Arity()
    {
        var command = Command(
            "ToolRunOptions",
            "ToolOptions",
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--run-tests",
                    PropertyName = "RunTests",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                },
            ]);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("using ModularPipelines.Models;");
        await Assert.That(generated).Contains("public CliOptionValue? RunTests { get; set; }");
    }

    [Test]
    public async Task OptionsClassGenerator_Validates_Explicit_Optional_Values()
    {
        var command = Command(
            "ToolRunOptions",
            "ToolOptions",
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--run-tests",
                    PropertyName = "RunTests",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                    ValidationConstraints = new CliValidationConstraints
                    {
                        MinValue = 1,
                        MaxValue = 3,
                        Pattern = "^[1-3]$",
                    },
                },
            ]);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("[CliOptionValueRange(1, 3)]");
        await Assert.That(generated).Contains("[CliOptionValueRegularExpression(\"^[1-3]$\")]");
        await Assert.That(generated).DoesNotContain("[Range(1, 3)]");
        await Assert.That(generated).DoesNotContain("[RegularExpression(\"^[1-3]$\")]");
    }

    private static CliToolDefinition Tool(params CliCommandDefinition[] commands) =>
        new()
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = commands,
        };

    [Test]
    public async Task Command_Facade_Generation_Can_Be_Disabled_Independently_Of_Options()
    {
        var tool = Tool(Command("ToolRunOptions", "ToolOptions", ["run"])) with
        {
            GenerateCommandFacade = false,
        };

        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);
        var implementationFiles = await new ServiceImplementationGenerator().GenerateAsync(tool);
        var registrationFiles = await new DependencyRegistrationGenerator().GenerateAsync(tool);
        var optionFiles = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(interfaceFiles).IsEmpty();
        await Assert.That(implementationFiles).IsEmpty();
        await Assert.That(registrationFiles).IsEmpty();
        await Assert.That(optionFiles).HasSingleItem();
    }

    [Test]
    public async Task Disabled_Command_Facade_Still_Registers_SubDomain_Services()
    {
        var tool = Tool(Command(
            "ToolGroupRunOptions",
            "ToolOptions",
            ["group", "run"],
            subDomainGroup: "Group")) with
        {
            GenerateCommandFacade = false,
        };

        var registrationFile = (await new DependencyRegistrationGenerator()
            .GenerateAsync(tool))
            .Single();

        await Assert.That(registrationFile.Content)
            .Contains("services.TryAddScoped<IToolGroup, ToolGroup>();");
        await Assert.That(registrationFile.Content)
            .Contains("public static IServiceCollection RegisterToolContext");
        await Assert.That(registrationFile.Content)
            .DoesNotContain("services.TryAddScoped<ITool, Services.Tool>();");
        await Assert.That(registrationFile.Content)
            .DoesNotContain("public static ITool Tool(this IPipelineContext context)");
    }

    [Test]
    public async Task Options_Class_Imports_Models_For_Cli_Option_Value_Pairs()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arg",
            PropertyName = "Arg",
            CSharpType = "IEnumerable<CliValuePair>?",
            IsFlag = false,
        };
        var tool = Tool(Command("ToolExecuteOptions", "ToolOptions", options: [option]));

        var generatedFile = (await new OptionsClassGenerator().GenerateAsync(tool)).Single();

        await Assert.That(generatedFile.Content).Contains("using ModularPipelines.Models;");
    }

    [Test]
    public async Task Options_Class_Emits_Repeatable_Optional_Value_Type()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--attach-debugger",
            PropertyName = "AttachDebugger",
            CSharpType = "IEnumerable<string>?",
            ValueArity = CliOptionValueArity.Optional,
            AcceptsMultipleValues = true,
        };
        var tool = Tool(Command("ToolExecuteOptions", "ToolOptions", options: [option]));

        var generatedFile = (await new OptionsClassGenerator().GenerateAsync(tool)).Single();

        await Assert.That(generatedFile.Content).Contains("using ModularPipelines.Models;");
        await Assert.That(generatedFile.Content)
            .Contains("public IEnumerable<CliOptionValue>? AttachDebugger { get; set; }");
    }

    [Test]
    public async Task Global_Options_Class_Imports_Models_For_Cli_Option_Value_Pairs()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arg",
            PropertyName = "Arg",
            CSharpType = "IEnumerable<CliValuePair>?",
        };
        var tool = Tool() with { GlobalOptions = [option] };

        var generatedFile = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single();

        await Assert.That(generatedFile.Content).Contains("using ModularPipelines.Models;");
    }

    [Test]
    public async Task Options_Class_Emits_Preferred_Short_Form()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--library-path",
            ShortForm = "-L",
            PreferShortForm = true,
            PropertyName = "LibraryPath",
            CSharpType = "IEnumerable<string>?",
            AcceptsMultipleValues = true,
        };
        var tool = Tool(Command("ToolExecuteOptions", "ToolOptions", options: [option]));

        var generatedFile = (await new OptionsClassGenerator().GenerateAsync(tool)).Single();

        await Assert.That(generatedFile.Content)
            .Contains("[CliOption(\"--library-path\", ShortForm = \"-L\", PreferShortForm = true)]");
    }

    [Test]
    public async Task NormalizeCommandClassNames_Renames_Command_Sharing_Base_Class_Name()
    {
        var commands = GeneratorUtils.NormalizeCommandClassNames(
            [Command("AnsibleOptions", "AnsibleOptions")]);

        await Assert.That(commands[0].ClassName).IsEqualTo("AnsibleExecuteOptions");
        await Assert.That(commands[0].ParentClassName).IsEqualTo("AnsibleOptions");
    }

    [Test]
    public async Task NormalizeCommandClassNames_Leaves_Distinct_Names_Untouched()
    {
        var commands = GeneratorUtils.NormalizeCommandClassNames(
            [Command("HelmInstallOptions", "HelmOptions")]);

        await Assert.That(commands[0].ClassName).IsEqualTo("HelmInstallOptions");
    }

    [Test]
    public async Task NormalizeCommandClassNames_Leaves_Executable_Parent_Options_Stable()
    {
        var commands = GeneratorUtils.NormalizeCommandClassNames(
        [
            Command("ToolApplicationSetOptions", "ToolOptions", ["appset"]),
            Command(
                "ToolApplicationSetGetOptions",
                "ToolOptions",
                ["appset", "get"],
                subDomainGroup: "ApplicationSet"),
        ]);

        await Assert.That(commands[0].ClassName).IsEqualTo("ToolApplicationSetOptions");
        await Assert.That(commands[1].ClassName).IsEqualTo("ToolApplicationSetGetOptions");
    }

    [Test]
    public async Task NormalizeCommandClassNames_Does_Not_Overwrite_Real_Execute_Command()
    {
        var commands = GeneratorUtils.NormalizeCommandClassNames(
        [
            Command("ToolOptions", "ToolOptions", ["tool"]),
            Command("ToolExecuteOptions", "ToolOptions", ["execute"]),
        ]);

        await Assert.That(commands[0].ClassName).IsEqualTo("ToolExecuteExecuteOptions");
        await Assert.That(commands[1].ClassName).IsEqualTo("ToolExecuteOptions");
    }

    [Test]
    public async Task SubDomain_Generators_Preserve_Compound_PascalCase()
    {
        var tool = Tool(
            Command(
                "ToolApplicationSetOptions",
                "ToolOptions",
                ["appset"],
                commandGroupIdentifierOverride: "ApplicationSet"),
            Command(
                "ToolApplicationSetGetOptions",
                "ToolOptions",
                ["appset", "get"],
                subDomainGroup: "ApplicationSet",
                commandGroupIdentifierOverride: "ApplicationSet"));

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);
        var implementationFiles = await new ServiceImplementationGenerator().GenerateAsync(tool);
        var registrationFiles = await new DependencyRegistrationGenerator().GenerateAsync(tool);
        var subDomainClass = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolApplicationSet.Generated.cs");
        var subDomainInterface = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "IToolApplicationSet.Generated.cs");

        await Assert.That(subDomainClass.Content).Contains("ToolApplicationSetOptions? options = null");
        await Assert.That(subDomainClass.Content)
            .Contains("public class ToolApplicationSet : IToolApplicationSet");
        await Assert.That(subDomainInterface.Content)
            .Contains("Task<CommandResult> ExecuteAsync(ToolApplicationSetOptions? options = null");
        await Assert.That(interfaceFiles.Single().Content)
            .Contains("IToolApplicationSet ApplicationSet => throw new System.NotSupportedException();");
        await Assert.That(interfaceFiles.Single().Content).DoesNotContain("Appset(");
        await Assert.That(implementationFiles.Single().Content).Contains("IToolApplicationSet ApplicationSet { get; }");
        await Assert.That(registrationFiles.Single().Content)
            .Contains("TryAddScoped<IToolApplicationSet, ToolApplicationSet>()");
        await Assert.That(GeneratorUtils.GetNonCollidingRootCommands(tool)).IsEmpty();
    }

    [Test]
    public async Task DependencyRegistrationGenerator_Uses_CompileTime_Integration_Marker()
    {
        var files = await new DependencyRegistrationGenerator().GenerateAsync(
            Tool(Command("ToolRunOptions", "ToolOptions", ["run"])));
        var content = files.Single().Content;

        await Assert.That(content).Contains("using ModularPipelines.Attributes;");
        await Assert.That(content).Contains("[ModularPipelinesIntegration]");
        await Assert.That(content).DoesNotContain("IPipelineContext");
        await Assert.That(content).DoesNotContain("GetRequiredService<ITool>");
        await Assert.That(content).DoesNotContain("ModuleInitializer");
        await Assert.That(content).DoesNotContain("ModularPipelinesContextRegistry");
    }

    [Test]
    public async Task DependencyRegistrationGenerator_Formats_Complete_File()
    {
        var content = (await new DependencyRegistrationGenerator().GenerateAsync(
                Tool(Command("ToolRunOptions", "ToolOptions", ["run"]))))
            .Single()
            .Content
            .ReplaceLineEndings("\n");
        var expected = """
            // <auto-generated>
            // This file was generated by ModularPipelines.OptionsGenerator.
            // Do not edit this file manually.
            // </auto-generated>

            #nullable enable

            using System.CodeDom.Compiler;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;
            using ModularPipelines.Attributes;
            using ModularPipelines.Tool.Services;

            namespace ModularPipelines.Tool.Extensions;

            /// <summary>
            /// Generated extensions for registering tool services.
            /// </summary>
            [GeneratedCode("ModularPipelines.OptionsGenerator", "2.0.0")]
            public static class ToolExtensions
            {
                /// <summary>
                /// Registers tool services with the dependency injection container.
                /// </summary>
                /// <param name="services">The service collection.</param>
                /// <returns>The service collection for chaining.</returns>
                [ModularPipelinesIntegration]
                public static IServiceCollection RegisterToolContext(this IServiceCollection services)
                {
                    services.TryAddScoped<ITool, Services.Tool>();
                    return services;
                }
            }
            """.ReplaceLineEndings("\n") + "\n";

        await Assert.That(content).IsEqualTo(expected);
    }

    [Test]
    public async Task SubDomain_Generators_Preserve_Existing_Word_Boundaries()
    {
        var tool = Tool(Command(
            "ToolWorkspaceAddOnsGetOptions",
            "ToolOptions",
            ["workspace-add-ons", "get"],
            subDomainGroup: "WorkspaceAddOns"));

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);
        var subDomainClass = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolWorkspaceAddOns.Generated.cs");

        await Assert.That(subDomainClass.RelativePath).EndsWith("ToolWorkspaceAddOns.Generated.cs");
        await Assert.That(interfaceFiles.Single().Content)
            .Contains("IToolWorkspaceAddOns WorkspaceAddOns => throw new System.NotSupportedException();");
    }

    [Test]
    public async Task SubDomainClassGenerator_Exposes_Nested_Parent_Command()
    {
        var tool = Tool(
            Command("ToolVexOptions", "ToolOptions", ["vex"]),
            Command("ToolVexRepoOptions", "ToolOptions", ["vex", "repo"], subDomainGroup: "vex"),
            Command("ToolVexRepoDownloadOptions", "ToolOptions", ["vex", "repo", "download"], subDomainGroup: "vex"));

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var repoService = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolVexRepo.Generated.cs");

        await Assert.That(repoService.Content).Contains("ToolVexRepoOptions? options = null");
        await Assert.That(repoService.Content).Contains("public virtual async Task<CommandResult> ExecuteAsync(");
    }

    [Test]
    public async Task SubDomain_Generators_Expose_Executable_Parent_And_Child()
    {
        var tool = Tool(
            Command("ToolGroupOptions", "ToolOptions", ["group"]),
            Command(
                "ToolGroupChildOptions",
                "ToolOptions",
                ["group", "child"],
                subDomainGroup: "group"));

        var optionFiles = await new OptionsClassGenerator().GenerateAsync(tool);
        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);
        var groupService = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolGroup.Generated.cs");

        await Assert.That(groupService.Content).Contains("ToolGroupOptions? options = null");
        await Assert.That(groupService.Content)
            .Contains("public virtual async Task<CommandResult> ExecuteAsync(");
        await Assert.That(groupService.Content)
            .Contains("public virtual async Task<CommandResult> ChildAsync(");
        await Assert.That(interfaceFiles.Single().Content)
            .Contains("IToolGroup Group => throw new System.NotSupportedException();");
        await Assert.That(optionFiles.Single(file =>
                file.RelativePath.EndsWith("ToolGroupOptions.Generated.cs")).Content)
            .Contains("[CliSubCommand(\"group\")]");
        await Assert.That(optionFiles.Single(file =>
                file.RelativePath.EndsWith("ToolGroupChildOptions.Generated.cs")).Content)
            .Contains("[CliSubCommand(\"group\", \"child\")]");
    }

    [Test]
    public async Task SubDomain_Generators_Deduplicate_Identical_Executable_Parents()
    {
        var parent = Command("ToolGroupOptions", "ToolOptions", ["group"]);
        var tool = Tool(
            parent,
            parent,
            Command(
                "ToolGroupChildOptions",
                "ToolOptions",
                ["group", "child"],
                subDomainGroup: "group"));

        var groupService = (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath).Equals(
                "ToolGroup.Generated.cs",
                StringComparison.Ordinal));

        await Assert.That(groupService.Content.Split("ExecuteAsync(")).Count().IsEqualTo(2);
    }

    [Test]
    public async Task SubDomain_Parent_Requires_Options_When_It_Has_Required_Operands()
    {
        var parent = Command("ToolServiceOptions", "ToolOptions", ["service"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Service",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
        };
        var tool = Tool(
            parent,
            Command(
                "ToolServiceListOptions",
                "ToolOptions",
                ["service", "list"],
                subDomainGroup: "service"));

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var service = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolService.Generated.cs");

        await Assert.That(service.Content)
            .Contains("ToolServiceOptions options,");
        await Assert.That(service.Content)
            .DoesNotContain("new ToolServiceOptions()");
    }

    [Test]
    public async Task SubDomain_Generators_Expose_Kubectl_ClusterInfo_Parent()
    {
        var tool = Tool(
            Command(
                "KubernetesClusterInfoOptions",
                "KubernetesOptions",
                ["cluster-info"]),
            Command(
                "KubernetesClusterInfoDumpOptions",
                "KubernetesOptions",
                ["cluster-info", "dump"],
                subDomainGroup: "clusterinfo")) with
        {
            ToolName = "kubectl",
            NamespacePrefix = "Kubernetes",
            TargetNamespace = "ModularPipelines.Kubernetes",
            OutputDirectory = "src/ModularPipelines.Kubernetes",
        };

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);
        var implementationFiles = await new ServiceImplementationGenerator().GenerateAsync(tool);
        var registrationFiles = await new DependencyRegistrationGenerator().GenerateAsync(tool);
        var clusterInfoService = subDomainFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "KubernetesClusterInfo.Generated.cs");

        await Assert.That(clusterInfoService.Content)
            .Contains("KubernetesClusterInfoOptions? options = null");
        await Assert.That(clusterInfoService.Content)
            .Contains("public virtual async Task<CommandResult> ExecuteAsync(");
        await Assert.That(clusterInfoService.Content)
            .Contains("public virtual async Task<CommandResult> DumpAsync(");
        await Assert.That(interfaceFiles.Single().Content)
            .Contains("IKubernetesClusterInfo ClusterInfo => throw new System.NotSupportedException();");
        await Assert.That(implementationFiles.Single().Content)
            .Contains("IKubernetesClusterInfo ClusterInfo { get; }");
        await Assert.That(registrationFiles.Single().Content)
            .Contains("TryAddScoped<IKubernetesClusterInfo, KubernetesClusterInfo>()");
    }

    [Test]
    public async Task SubDomain_Generator_Interfaces_Facade_Service_But_Not_Nested_Group()
    {
        var tool = Tool(Command(
            "ToolParentImageToolsRunOptions",
            "ToolOptions",
            ["parent", "imagetools", "run"],
            subDomainGroup: "parent"));

        var generatedFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var rootClass = generatedFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolParent.Generated.cs");
        var rootInterface = generatedFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "IToolParent.Generated.cs");
        var childClass = generatedFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "ToolParentImageTools.Generated.cs");

        using (Assert.Multiple())
        {
            await Assert.That(rootClass.Content)
                .Contains("public class ToolParent : IToolParent");
            await Assert.That(rootClass.Content)
                .Contains("public ToolParentImageTools ImageTools =>");
            await Assert.That(rootInterface.Content)
                .Contains("ToolParentImageTools ImageTools => throw new System.NotSupportedException();");
            await Assert.That(rootInterface.Content)
                .Contains("only this top-level facade is interface-backed");
            await Assert.That(childClass.Content)
                .Contains("public class ToolParentImageTools");
            await Assert.That(generatedFiles.Any(file =>
                    Path.GetFileName(file.RelativePath) == "IToolParentImageTools.Generated.cs"))
                .IsFalse();
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Renames_Inherited_Name_Collisions()
    {
        var command = Command("ToolJobSubmitOptions", "ToolOptions", ["job", "submit"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--tool",
                    PropertyName = "Tool",
                    CSharpType = "string?",
                },
                new CliOptionDefinition
                {
                    SwitchName = "--command-parts",
                    PropertyName = "CommandParts",
                    CSharpType = "IEnumerable<string>?",
                },
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--run-settings",
                    PropertyName = "RunSettings",
                    CSharpType = "IEnumerable<string>?",
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("public string? JobTool { get; set; }");
        await Assert.That(generated).Contains("public IEnumerable<string>? JobCommandParts { get; set; }");
        await Assert.That(generated).Contains("public bool? JobArguments { get; set; }");
        await Assert.That(generated).Contains("public IEnumerable<string>? JobRunSettings { get; set; }");
        await Assert.That(generated).DoesNotContain("public new ");
    }

    [Test]
    public async Task OptionsClassGenerator_Reserves_Global_Names_For_Command_Renames()
    {
        var command = Command("ToolJobSubmitOptions", "ToolOptions", ["job", "submit"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
            ],
        };
        var tool = Tool(command) with
        {
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--job-arguments",
                    PropertyName = "JobArguments",
                    CSharpType = "string?",
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool))
            .Single(file => file.Content.Contains("record ToolJobSubmitOptions", StringComparison.Ordinal))
            .Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public bool? CliArguments { get; set; }");
            await Assert.That(generated).DoesNotContain("public bool? JobArguments { get; set; }");
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Deduplicates_Required_And_Optional_Positionals()
    {
        var command = Command("ToolLoadOptions", "ToolOptions") with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Image",
                    CSharpType = "string",
                    PositionIndex = 0,
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "Image",
                    CSharpType = "IEnumerable<string>?",
                    PositionIndex = 1,
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("[property: CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)] IEnumerable<string> Image");
        await Assert.That(generated).DoesNotContain("public string? Image { get; set; }");
        await Assert.That(generated.Split("CliArgument(")).Count().IsEqualTo(2);
    }

    [Test]
    public async Task OptionsClassGenerator_Deduplicates_Renamed_Positionals()
    {
        var command = Command("ToolJobSubmitOptions", "ToolOptions", ["job", "submit"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Arguments",
                    CSharpType = "string",
                    PositionIndex = 0,
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "Arguments",
                    CSharpType = "IEnumerable<string>?",
                    PositionIndex = 1,
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("IEnumerable<string> JobArguments");
        await Assert.That(generated).DoesNotContain("CliArguments");
        await Assert.That(generated.Split("CliArgument(")).Count().IsEqualTo(2);
    }

    [Test]
    public async Task OptionsClassGenerator_Uses_Consistent_Line_Endings()
    {
        var command = Command("ToolRunOptions", "ToolOptions") with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--first",
                    PropertyName = "First",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--second",
                    PropertyName = "Second",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;
        var contentWithoutLineEndings = generated.Replace(Environment.NewLine, string.Empty);

        using (Assert.Multiple())
        {
            await Assert.That(contentWithoutLineEndings).DoesNotContain('\r');
            await Assert.That(contentWithoutLineEndings).DoesNotContain('\n');
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Allows_Required_Constructor_Operand_To_Skip_Validation()
    {
        var command = Command("ToolDiffOptions", "ToolOptions", ["diff"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Target",
                    CSharpType = "string?",
                    PositionIndex = 0,
                    IsRequired = true,
                    IsValidationRequired = false,
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("string Target");
            await Assert.That(generated).Contains("CliArgument(0, Phase = CommandLinePhase.EarlyOperand)");
            await Assert.That(generated).DoesNotContain("Required = true");
        }
    }

    [Test]
    public async Task CommandTreeNode_Rejects_Conflicting_Explicit_Identifiers()
    {
        var commands = new[]
        {
            Command("ToolGroupFirstOptions", "ToolOptions", ["group", "cloud-shell", "first"]) with
            {
                CommandPartIdentifierOverrides = new Dictionary<int, string> { [1] = "Cloudshell" },
            },
            Command("ToolGroupSecondOptions", "ToolOptions", ["group", "cloud-shell", "second"]) with
            {
                CommandPartIdentifierOverrides = new Dictionary<int, string> { [1] = "CloudShell" },
            },
        };

        await Assert.That(() => CommandTreeNode.BuildTree("Tool", "Group", commands))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Cloudshell, CloudShell");
    }

    [Test]
    public async Task Local_Option_And_Argument_Name_Collisions_Are_Disambiguated()
    {
        var command = Command("ToolCreateOptions", "ToolOptions", ["create"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--filename",
                    PropertyName = "Filename",
                    CSharpType = "string?",
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Filename",
                    CSharpType = "IEnumerable<string>?",
                    PositionIndex = 0,
                },
            ],
        };

        var resolved = InheritedPropertyCollisionResolver.Resolve(Tool(command));
        var resolvedCommand = resolved.Commands.Single();

        using (Assert.Multiple())
        {
            await Assert.That(resolvedCommand.Options.Single().PropertyName)
                .IsEqualTo("Filename");
            await Assert.That(resolvedCommand.PositionalArguments.Single().PropertyName)
                .IsEqualTo("FilenameArgument");
        }
    }

    [Test]
    public async Task Record_Reserved_Property_Names_Are_Disambiguated()
    {
        var command = Command("ToolCreateOptions", "ToolOptions", ["create"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--clone",
                    PropertyName = "Clone",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
            ],
        };

        var resolved = InheritedPropertyCollisionResolver.Resolve(Tool(command));

        await Assert.That(resolved.Commands.Single().Options.Single().PropertyName)
            .IsEqualTo("CliClone");
    }

    [Test]
    public async Task Inherited_Property_Collision_Resolution_Is_Idempotent()
    {
        var command = Command("ToolAdditionalRunOptions", "ToolOptions", ["additional", "run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "string?",
                },
            ],
        };

        var first = InheritedPropertyCollisionResolver.Resolve(Tool(command));
        var second = InheritedPropertyCollisionResolver.Resolve(first);
        var firstName = first.Commands.Single().Options.Single().PropertyName;
        var secondName = second.Commands.Single().Options.Single().PropertyName;

        using (Assert.Multiple())
        {
            await Assert.That(firstName).IsEqualTo("CliArguments");
            await Assert.That(secondName).IsEqualTo(firstName);
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Marks_Secret_Positional_Arguments()
    {
        var command = Command("ToolAuthOptions", "ToolOptions", ["auth"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "RequiredToken",
                    CSharpType = "string?",
                    IsRequired = true,
                    IsSecret = true,
                    PositionIndex = 0,
                },
                new CliPositionalArgument
                {
                    PropertyName = "OptionalToken",
                    CSharpType = "string?",
                    IsSecret = true,
                    PositionIndex = 1,
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains(
            $"using ModularPipelines.Secrets;{Environment.NewLine}using System.CodeDom.Compiler;");
        await Assert.That(generated).Contains("[property: SecretValue, CliArgument(0");
        await Assert.That(generated).Contains($"[SecretValue]{Environment.NewLine}    [CliArgument(1");
    }

    [Test]
    public async Task Generated_Secret_Options_Compile_With_Secrets_Import()
    {
        var command = Command("ToolAuthOptions", "ToolOptions", ["auth"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--token",
                    PropertyName = "Token",
                    CSharpType = "string?",
                    IsRequired = true,
                    IsSecret = true,
                    SecretValueKeys = ["token"],
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;
        var normalized = GeneratorUtils.EnsureRequiredUsings(
            generated.Replace(
                $"using ModularPipelines.Secrets;{Environment.NewLine}",
                string.Empty,
                StringComparison.Ordinal));
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            "secret-option",
            [
                CSharpSyntaxTree.ParseText(
                    "namespace ModularPipelines.Attributes { "
                    + "public sealed class CliOptionAttribute(string name) : System.Attribute; "
                    + "public sealed class CliSubCommandAttribute(params string[] commandParts) : System.Attribute; } "
                    + "namespace ModularPipelines.Secrets { "
                    + "public sealed class SecretValueAttribute(params string[] keys) : System.Attribute; } "
                    + "namespace ModularPipelines.Tool.Options { public record ToolOptions; }"),
                CSharpSyntaxTree.ParseText(normalized),
            ],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errors = compilation.GetDiagnostics()
            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(static diagnostic => diagnostic.ToString())
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(normalized).Contains(
                $"using ModularPipelines.Secrets;{Environment.NewLine}using System.CodeDom.Compiler;");
            await Assert.That(normalized).Contains(
                "[property: SecretValue(\"token\"), CliOption(\"--token\")]");
            await Assert.That(errors).IsEmpty();
        }
    }

    [Test]
    public async Task Required_Generated_Usings_Are_Added_Once()
    {
        const string source = "using System.CodeDom.Compiler;\n"
            + "public sealed class ToolService(ICommandContext context)\n"
            + "{\n"
            + "    [SecretValue] public string? Token { get; init; }\n"
            + "}\n";

        var normalized = GeneratorUtils.EnsureRequiredUsings(source);
        var normalizedAgain = GeneratorUtils.EnsureRequiredUsings(normalized);

        using (Assert.Multiple())
        {
            await Assert.That(normalized).StartsWith(
                "using ModularPipelines.Context;\n"
                    + "using ModularPipelines.Secrets;\n"
                    + "using System.CodeDom.Compiler;");
            await Assert.That(normalizedAgain).IsEqualTo(normalized);
        }
    }

    [Test]
    public async Task EnsureNoDuplicateFilePaths_Throws_On_Case_Variant_Duplicate()
    {
        await Assert.That(() => GeneratorUtils.EnsureNoDuplicateFilePaths(
            [
                new GeneratedFile { RelativePath = "Options/AppSetOptions.Generated.cs", Content = "a" },
                new GeneratedFile { RelativePath = "options/appsetoptions.generated.cs", Content = "b" },
            ]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("AppSetOptions.Generated.cs");
    }

    [Test]
    public async Task EnsureNoDuplicateFilePaths_Allows_Distinct_Paths()
    {
        await Assert.That(() => GeneratorUtils.EnsureNoDuplicateFilePaths(
            [
                new GeneratedFile { RelativePath = "Options/AOptions.Generated.cs", Content = "a" },
                new GeneratedFile { RelativePath = "Options/BOptions.Generated.cs", Content = "b" },
            ]))
            .ThrowsNothing();
    }

    [Test]
    public async Task EnsureNoDuplicateFilePaths_Throws_When_Path_Was_Emitted_By_An_Earlier_Tool()
    {
        var previouslyEmitted = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "src/Shared/Options/AOptions.Generated.cs",
        };

        await Assert.That(() => GeneratorUtils.EnsureNoDuplicateFilePaths(
            [
                new GeneratedFile { RelativePath = @"src\Shared\Options\AOptions.Generated.cs", Content = "a" },
            ],
            previouslyEmitted))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task EnsureNoDuplicateFilePaths_Treats_Slash_Directions_As_Equal()
    {
        await Assert.That(() => GeneratorUtils.EnsureNoDuplicateFilePaths(
            [
                new GeneratedFile { RelativePath = "Options/AOptions.Generated.cs", Content = "a" },
                new GeneratedFile { RelativePath = @"Options\AOptions.Generated.cs", Content = "b" },
            ]))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GenerateServiceMethod_Emits_Nullable_Optional_Parameters()
    {
        var sb = new StringBuilder();
        GeneratorUtils.GenerateServiceMethod(sb, "Execute", Command("ToolExecuteOptions", "ToolOptions"));

        var generated = sb.ToString();

        await Assert.That(generated).Contains("ToolExecuteOptions? options = null");
        await Assert.That(generated).Contains("CommandExecutionOptions? executionOptions = null");
        await Assert.That(generated).DoesNotContain("options = default");
    }

    [Test]
    public async Task ServiceInterfaceGenerator_Emits_Default_Command_Implementations()
    {
        var tool = Tool(Command("ToolRunOptions", "ToolOptions", ["run"]));

        var generated = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains("Task<CommandResult> RunAsync(");
        await Assert.That(generated)
            .Contains("    => throw new System.NotSupportedException();");
    }

    [Test]
    public async Task ServiceInterfaceGenerator_Emits_Default_SubDomain_Implementations()
    {
        var tool = Tool(Command(
            "ToolArtifactAddOptions",
            "ToolOptions",
            ["artifact", "add"],
            subDomainGroup: "Artifact"));

        var generated = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated)
            .Contains("IToolArtifact Artifact => throw new System.NotSupportedException();");
    }

    [Test]
    public async Task GenerateMethodNameFromCommandParts_Handles_Snake_Case()
    {
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts(["build_server"]);

        await Assert.That(result).IsEqualTo("BuildServer");
    }

    [Test]
    public async Task GenerateMethodNameFromCommandParts_Handles_Kebab_Case()
    {
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts(["app-set", "create"]);

        await Assert.That(result).IsEqualTo("AppSetCreate");
    }

    [Test]
    public async Task EscapeXmlComment_Normalizes_Linux_Runner_Home_Paths()
    {
        var result = GeneratorUtils.EscapeXmlComment(
            "path to the file containing cached repository indexes (default \"/home/runner/.cache/helm/repository\")");

        await Assert.That(result).Contains("~/.cache/helm/repository");
        await Assert.That(result).DoesNotContain("/home/runner");
    }

    [Test]
    public async Task EscapeXmlComment_Normalizes_Windows_Runner_Home_Paths()
    {
        var result = GeneratorUtils.EscapeXmlComment(@"default C:\Users\runneradmin\.config\tool");

        await Assert.That(result).Contains(@"~\.config\tool");
        await Assert.That(result).DoesNotContain("runneradmin");
    }

    [Test]
    public async Task EscapeXmlComment_Normalizes_Current_User_Home_Path()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var path = Path.Combine(homeDirectory, ".config", "tool");

        var result = GeneratorUtils.EscapeXmlComment($"default {path}");

        await Assert.That(result).StartsWith("default ~");
        await Assert.That(result).DoesNotContain(homeDirectory);
    }

    [Test]
    public async Task EscapeXmlComment_Removes_Current_Directory_Default()
    {
        var description =
            $"Search the current directory when omitted. [default: {Environment.CurrentDirectory}{Path.DirectorySeparatorChar}]";

        var result = GeneratorUtils.EscapeXmlComment(description);

        await Assert.That(result).IsEqualTo("Search the current directory when omitted.");
    }

    [Test]
    public async Task EscapeXmlComment_Preserves_Unrelated_Home_Shaped_Paths()
    {
        const string path = "/home/site/deployments/tools/";

        var result = GeneratorUtils.EscapeXmlComment($"deployment path {path}");

        await Assert.That(result).Contains(path);
    }

    [Test]
    public async Task IsSecretOption_Detects_Passphrase()
    {
        var result = GeneratorUtils.IsSecretOption("SshPassphrase", isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task EnumGenerator_Emits_Runtime_EnumValue_Attribute()
    {
        var enumDefinition = new CliEnumDefinition
        {
            EnumName = "ToolOutput",
            Values = [new CliEnumValue { MemberName = "Json", CliValue = "json-output" }],
        };
        var tool = Tool(Command(
            "ToolGetOptions",
            "ToolOptions",
            ["get"],
            enums: [enumDefinition]));

        var generatedFile = (await new EnumGenerator().GenerateAsync(tool)).Single();

        await Assert.That(generatedFile.Content).Contains("using ModularPipelines.Attributes;");
        await Assert.That(generatedFile.Content).Contains("[EnumValue(\"json-output\")]");
        await Assert.That(generatedFile.Content).DoesNotContain("[Description(");
    }

    [Test]
    public async Task EnumGenerator_Preserves_Aliases_With_Unique_Member_Names()
    {
        var enumDefinition = new CliEnumDefinition
        {
            EnumName = "ToolMaintenanceWindowDay",
            Values =
            [
                new CliEnumValue { MemberName = "Friday", CliValue = "friday" },
                new CliEnumValue { MemberName = "Fri", CliValue = "fri" },
                new CliEnumValue { MemberName = "Friday", CliValue = "FRIDAY" },
                new CliEnumValue { MemberName = "Fri", CliValue = "FRI" },
                new CliEnumValue { MemberName = "Fri", CliValue = "fri" },
            ],
        };
        var generated = (await new EnumGenerator().GenerateAsync(Tool(Command(
            "ToolGetOptions",
            "ToolOptions",
            ["get"],
            enums: [enumDefinition])))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("    Friday,");
            await Assert.That(generated).Contains("    Fri,");
            await Assert.That(generated).Contains("    FridayUppercase,");
            await Assert.That(generated).Contains("    FriUppercase");
            await Assert.That(generated.Split("[EnumValue(\"fri\")]", StringSplitOptions.None).Length)
                .IsEqualTo(2);
        }
    }

    [Test]
    public async Task Case_Variant_Enum_Names_Fail_The_Duplicate_Path_Check()
    {
        static CliEnumDefinition EnumDef(string name) => new()
        {
            EnumName = name,
            Values = [new CliEnumValue { MemberName = "Json", CliValue = "json" }],
        };

        var tool = Tool(
            Command("ToolAOptions", "ToolOptions", ["a"], enums: [EnumDef("ToolAppSetLogformat")]),
            Command("ToolBOptions", "ToolOptions", ["b"], enums: [EnumDef("ToolAppsetLogformat")]));

        // Case-variant enum names are a scraper bug. AllEnums keeps both (dropping one
        // would leave dangling type references), and the duplicate-path check turns the
        // resulting file collision into a loud failure.
        await Assert.That(tool.AllEnums.Count).IsEqualTo(2);

        var enumFiles = await new EnumGenerator().GenerateAsync(tool);

        await Assert.That(() => GeneratorUtils.EnsureNoDuplicateFilePaths(enumFiles))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetNonCollidingRootCommands_Throws_When_Commands_Normalize_To_The_Same_Method_Name()
    {
        var tool = Tool(
            Command("ToolBuildServerOptions", "ToolOptions", ["build-server"]),
            Command("ToolBuildServer2Options", "ToolOptions", ["build_server"]));

        await Assert.That(() => GeneratorUtils.GetNonCollidingRootCommands(tool))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("BuildServer");
    }

    [Test]
    public async Task SubDomainClassGenerator_Throws_On_Duplicate_Parent_Command_Definitions()
    {
        var tool = Tool(
            Command("ToolNetworkCreateOptions", "ToolOptions", ["network", "create"], subDomainGroup: "network"),
            Command("ToolNetworkOptions", "ToolOptions", ["network"]) with
            {
                FullCommand = "tool network",
            },
            Command("ToolNetwork2Options", "ToolOptions", ["Network"]) with
            {
                FullCommand = "tool Network",
            });

        void Generate() => _ = new SubDomainClassGenerator().GenerateAsync(tool);

        await Assert.That(Generate)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Network (tool network, tool Network)");
    }

    [Test]
    public async Task SubDomainClassGenerator_Throws_When_Direct_Commands_Normalize_To_Same_Method()
    {
        var tool = Tool(
            Command(
                "ToolNetworkFooBarOptions",
                "ToolOptions",
                ["network", "foo-bar"],
                subDomainGroup: "network"),
            Command(
                "ToolNetworkFooBar2Options",
                "ToolOptions",
                ["network", "foo_bar"],
                subDomainGroup: "network"));

        void Generate() => _ = new SubDomainClassGenerator().GenerateAsync(tool);

        await Assert.That(Generate)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("FooBar");
    }

    [Test]
    public async Task SubDomainClassGenerator_Throws_When_Parents_Normalize_To_Same_Child()
    {
        var tool = Tool(
            Command(
                "ToolAppFooBarOptions",
                "ToolOptions",
                ["app", "foo-bar"],
                subDomainGroup: "app") with
            {
                FullCommand = "tool app foo-bar",
            },
            Command(
                "ToolAppFooBar2Options",
                "ToolOptions",
                ["app", "foo_bar"],
                subDomainGroup: "app") with
            {
                FullCommand = "tool app foo_bar",
            },
            Command(
                "ToolAppFooBarChildOptions",
                "ToolOptions",
                ["app", "foo-bar", "child"],
                subDomainGroup: "app"));

        void Generate() => _ = new SubDomainClassGenerator().GenerateAsync(tool);

        await Assert.That(Generate)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("tool app foo-bar, tool app foo_bar");
    }

    [Test]
    public async Task SubDomainClassGenerator_Disambiguates_Literal_Async_Command()
    {
        var tool = Tool(
            Command(
                "ToolBedrockInvokeDataAutomationOptions",
                "ToolOptions",
                ["bedrock", "invoke-data-automation"],
                subDomainGroup: "bedrock") with
            {
                FullCommand = "tool bedrock invoke-data-automation",
            },
            Command(
                "ToolBedrockInvokeDataAutomationAsyncOptions",
                "ToolOptions",
                ["bedrock", "invoke-data-automation-async"],
                subDomainGroup: "bedrock") with
            {
                FullCommand = "tool bedrock invoke-data-automation-async",
            });

        var service = (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath) == "ToolBedrock.Generated.cs")
            .Content;

        await Assert.That(service)
            .Contains("InvokeDataAutomationAsync(");
        await Assert.That(service)
            .Contains("InvokeDataAutomationAsyncCommandAsync(");
    }

    [Test]
    public async Task SubDomainClassGenerator_Selects_Unique_Async_Disambiguator()
    {
        var tool = Tool(
            Command(
                "ToolAppFooOptions",
                "ToolOptions",
                ["app", "foo"],
                subDomainGroup: "app") with
            {
                FullCommand = "tool app foo",
            },
            Command(
                "ToolAppFooAsyncOptions",
                "ToolOptions",
                ["app", "foo-async"],
                subDomainGroup: "app") with
            {
                FullCommand = "tool app foo-async",
            },
            Command(
                "ToolAppFooAsyncCommandOptions",
                "ToolOptions",
                ["app", "foo-async-command"],
                subDomainGroup: "app") with
            {
                FullCommand = "tool app foo-async-command",
            });

        var service = (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath) == "ToolApp.Generated.cs")
            .Content;

        await Assert.That(service).Contains("FooAsync(");
        await Assert.That(service).Contains("FooAsyncCommandAsync(");
        await Assert.That(service).Contains("FooAsyncCommand2Async(");
    }

    [Test]
    public async Task GetNonCollidingRootCommands_Filters_Commands_Colliding_With_SubDomains()
    {
        var tool = Tool(
            Command("ToolNetworkCreateOptions", "ToolOptions", ["network", "create"], subDomainGroup: "network"),
            Command("ToolNetworkOptions", "ToolOptions", ["network"]),
            Command("ToolVersionOptions", "ToolOptions", ["version"]));

        var rootCommands = GeneratorUtils.GetNonCollidingRootCommands(tool);

        await Assert.That(rootCommands.Count).IsEqualTo(1);
        await Assert.That(rootCommands[0].ClassName).IsEqualTo("ToolVersionOptions");
    }

    [Test]
    public async Task GeneratedCodeAttribute_Contains_A_Version()
    {
        await Assert.That(GeneratorUtils.GeneratedCodeAttribute)
            .Contains($"\"{GeneratorUtils.GeneratorVersion}\"");
    }
}
