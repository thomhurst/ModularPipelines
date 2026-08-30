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
        IReadOnlyList<CliOptionDefinition>? options = null,
        IReadOnlyList<CliCompatibilityMethod>? compatibilityMethods = null) =>
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
            CompatibilityMethods = compatibilityMethods ?? [],
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
    public async Task OptionsClassGenerator_Imports_Preserved_KeyValue_Types()
    {
        var command = Command(
            "ToolRunOptions",
            "ToolOptions",
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--labels",
                    PropertyName = "Labels",
                    CSharpType = "string?",
                },
            ]) with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyLabels",
                    CSharpType = "IReadOnlyList<KeyValue>?",
                    ObsoleteMessage = "Use Labels instead.",
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("using ModularPipelines.Models;");
        await Assert.That(generated).Contains("public IReadOnlyList<KeyValue>? LegacyLabels { get; set; }");
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

    [Test]
    public async Task OptionsClassGenerator_Imports_Preserved_Enum_Types()
    {
        var command = Command(
            "ToolEventsOptions",
            "ToolOptions",
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "ToolEventsOutput?",
                },
            ]);
        var tool = Tool(command) with
        {
            CompatibilityEnums =
            [
                new CliEnumDefinition
                {
                    EnumName = "ToolEventsOutput",
                    Values = [new CliEnumValue { MemberName = "Json", CliValue = "json" }],
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains("using ModularPipelines.Tool.Enums;");
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

    private static GeneratedApiProperty BaselineProperty(
        string propertyName,
        string cSharpType,
        string? switchName = null,
        int? argumentPosition = null,
        bool isRequired = false,
        bool isCompatibility = false,
        string? forwardToPropertyName = null,
        bool useInitAccessor = false,
        CommandLinePhase? phase = null,
        bool omitPhase = false,
        CliCompatibilityForwardingKind forwardingKind = CliCompatibilityForwardingKind.Direct,
        CliOptionValueArity valueArity = CliOptionValueArity.Required,
        string? negatedSwitchName = null) =>
        new(
            propertyName,
            cSharpType,
            switchName,
            argumentPosition,
            isRequired,
            isCompatibility,
            forwardToPropertyName,
            null,
            useInitAccessor,
            ForwardingKind: forwardingKind,
            ValueArity: valueArity,
            Phase: phase ?? (argumentPosition is not null && !omitPhase
                ? CommandLinePhase.EarlyOperand
                : null),
            NegatedSwitchName: negatedSwitchName);

    private static CliOptionDefinition RequiredOption(string switchName, string propertyName) =>
        new()
        {
            SwitchName = switchName,
            PropertyName = propertyName,
            CSharpType = "string",
            IsRequired = true,
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
    public async Task Command_Facade_Compatibility_Accessor_Is_Hidden_And_Obsolete()
    {
        var registrationFile = (await new DependencyRegistrationGenerator()
            .GenerateAsync(Tool(Command("ToolRunOptions", "ToolOptions", ["run"]))))
            .Single();

        var expectedDeclaration = string.Join(Environment.NewLine,
        [
            "    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]",
            "    [global::System.Obsolete(\"Use context.Tools.Get<global::ModularPipelines.Tool.Services.ITool>().\")]",
            "    public static ITool Tool(this IPipelineContext context)",
        ]);

        await Assert.That(registrationFile.Content).Contains(expectedDeclaration);
    }

    #region NormalizeCommandClassNames

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
        await Assert.That(content).Contains("context.Services.GetRequiredService<ITool>()");
        await Assert.That(content).DoesNotContain("ModuleInitializer");
        await Assert.That(content).DoesNotContain("ModularPipelinesContextRegistry");
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

    #endregion

    #region Positional argument deduplication

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

    #endregion

    #region Compatibility properties

    [Test]
    public async Task OptionsClassGenerator_Emits_NonCli_Compatibility_Properties()
    {
        const string obsoleteMessage = "Use \"NewName\".\r\nPath:\tC:\\tool";
        var command = Command("ToolBuildOptions", "ToolOptions") with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "OldName",
                    CSharpType = "bool?",
                    ForwardToPropertyName = "NewName",
                    ObsoleteMessage = obsoleteMessage,
                },
                new CliCompatibilityProperty
                {
                    PropertyName = "RemovedFlag",
                    CSharpType = "bool?",
                    ObsoleteMessage = "This flag has no effect.",
                },
            ],
        };

        var files = await new OptionsClassGenerator().GenerateAsync(Tool(command));
        var generated = files.Single().Content;

        await Assert.That(generated)
            .Contains($"[Obsolete({GeneratorUtils.FormatStringLiteral(obsoleteMessage)})]");
        await Assert.That(generated).Contains("get => NewName;");
        await Assert.That(generated).Contains("set => NewName = value;");
        await Assert.That(generated).Contains("public bool? RemovedFlag { get; set; }");
        await Assert.That(generated).DoesNotContain("CliFlag(\"--removed-flag\")");
    }

    [Test]
    public async Task OptionsClassGenerator_Emits_Converted_Compatibility_Properties()
    {
        var command = Command("ToolBuildOptions", "ToolOptions") with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Outputs",
                    CSharpType = "IEnumerable<string>?",
                },
                new CliOptionDefinition
                {
                    SwitchName = "--timestamp",
                    PropertyName = "TimestampValue",
                    CSharpType = "string?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "Output",
                    CSharpType = "string?",
                    ForwardToPropertyName = "Outputs",
                    ForwardingKind = CliCompatibilityForwardingKind.ScalarToCollection,
                    ObsoleteMessage = "Use Outputs instead.",
                },
                new CliCompatibilityProperty
                {
                    PropertyName = "Timestamp",
                    CSharpType = "int?",
                    ForwardToPropertyName = "TimestampValue",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableInt32ToString,
                    ObsoleteMessage = "Use TimestampValue instead.",
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public IEnumerable<string>? Outputs { get; set; }");
            await Assert.That(generated).Contains("public string? TimestampValue { get; set; }");
            await Assert.That(generated).Contains("get => Outputs?.FirstOrDefault();");
            await Assert.That(generated).Contains("set => Outputs = value is null ? null : [value];");
            await Assert.That(generated).Contains("int.TryParse(TimestampValue");
            await Assert.That(generated).Contains(
                "set => TimestampValue = value?.ToString(global::System.Globalization.CultureInfo.InvariantCulture);");
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Emits_Case_Variant_Compatibility_Alias()
    {
        var command = Command("ToolBuildOptions", "ToolOptions") with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--nologo",
                    PropertyName = "NoLogo",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "Nologo",
                    CSharpType = "bool?",
                    ForwardToPropertyName = "NoLogo",
                    ObsoleteMessage = "Use NoLogo instead.",
                },
            ],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(command))).Single().Content;

        await Assert.That(generated).Contains("public bool? NoLogo { get; set; }");
        await Assert.That(generated).Contains("public bool? Nologo");
        await Assert.That(generated).Contains("get => NoLogo;");
    }

    [Test]
    public async Task OptionsClassGenerator_Renames_Global_Compatibility_Targets()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyArguments",
                    CSharpType = "IEnumerable<string>?",
                    ForwardToPropertyName = "Arguments",
                    ObsoleteMessage = "Use Arguments instead.",
                },
            ],
        };
        var tool = Tool(command) with
        {
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

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool))
            .Single(file => file.Content.Contains("record ToolRunOptions", StringComparison.Ordinal))
            .Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("get => CliArguments;");
            await Assert.That(generated).Contains("set => CliArguments = value;");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Removed_Scraped_Options()
    {
        var command = Command("ToolBuildOptions", "ToolOptions", ["build"]);
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("RemovedFlag", "bool?", switchName: "--removed-flag")]);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public bool? RemovedFlag { get; set; }");
            await Assert.That(generated).Contains("RemovedFlag is no longer supported");
            await Assert.That(generated).DoesNotContain("CliFlag(\"--removed-flag\")");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Reads_Converted_Compatibility_Accessors()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuildOptions.Generated.cs"),
                "public record ToolBuildOptions { "
                + "[CliOption(\"--output\")] public IEnumerable<string>? Outputs { get; set; } "
                + "[Obsolete(\"Use Outputs instead.\")] public string? Output { get => Outputs?.FirstOrDefault(); set => Outputs = value is null ? null : [value]; } "
                + "}");
            var command = Command(
                "ToolBuildOptions",
                "ToolOptions",
                ["build"],
                options:
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--output",
                        PropertyName = "Outputs",
                        CSharpType = "IEnumerable<string>?",
                    },
                ]) with
            {
                CompatibilityProperties =
                [
                    new CliCompatibilityProperty
                    {
                        PropertyName = "Output",
                        CSharpType = "string?",
                        ForwardToPropertyName = "Outputs",
                        ForwardingKind = CliCompatibilityForwardingKind.ScalarToCollection,
                        ObsoleteMessage = "Use Outputs instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);

            await Assert.That(preserved.Commands.Single().CompatibilityProperties.Single().ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.ScalarToCollection);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Does_Not_Preserve_Unshipped_Optional_Facade()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var projectDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        var optionsDirectory = Path.Combine(projectDirectory, "Options");
        var servicesDirectory = Path.Combine(projectDirectory, "Services");
        Directory.CreateDirectory(optionsDirectory);
        Directory.CreateDirectory(servicesDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(projectDirectory, "PublicAPI.Shipped.txt"),
                "ModularPipelines.Tool.Options.ToolOptions");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolOptions.Generated.cs"),
                "public record ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolRunOptions.Generated.cs"),
                "[CliSubCommand(\"run\")] public record ToolRunOptions(string Name) : ToolOptions "
                + "{ public ToolRunOptions() : this(default(string)!) { } }");
            await File.WriteAllTextAsync(
                Path.Combine(servicesDirectory, "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public void RunAsync(ToolRunOptions? options = null) { } }");
            var command = Command(
                "ToolRunOptions",
                "ToolOptions",
                ["run"],
                options: [RequiredOption("--name", "Name")]);

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var preservedCommand = preserved.Commands.Single();
            var generatedOptions = (await new OptionsClassGenerator().GenerateAsync(preserved)).Single().Content;
            var generatedService = (await new ServiceImplementationGenerator().GenerateAsync(preserved)).Single().Content;

            using (Assert.Multiple())
            {
                await Assert.That(preservedCommand.PreserveOptionalOptionsParameter).IsFalse();
                await Assert.That(generatedOptions).DoesNotContain("public ToolRunOptions()");
                await Assert.That(generatedService).Contains("ToolRunOptions options,");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Required_Positional_Names()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Dep",
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
        };
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "DepVersion",
                "IEnumerable<string>",
                argumentPosition: 0,
                isRequired: true)]);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("IEnumerable<string> DepVersion");
            await Assert.That(generated).Contains("public IEnumerable<string> Dep");
            await Assert.That(generated).Contains("get => DepVersion;");
            await Assert.That(generated).Contains("init => DepVersion = value;");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Prefers_Matching_Cli_Identity_For_Duplicate_Names()
    {
        var command = Command("ToolLoginOptions", "ToolOptions", ["login"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--server",
                    PropertyName = "Server",
                    CSharpType = "string?",
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Server",
                    CSharpType = "string?",
                    PositionIndex = 0,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Server", "string", argumentPosition: 0, isRequired: true)]);

        await Assert.That(preserved.PositionalArguments.Single().CSharpType).IsEqualTo("string");
        await Assert.That(preserved.PositionalArguments.Single().IsRequired).IsTrue();
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Name_After_Local_Collision_Rename()
    {
        var command = Command("ToolLoginOptions", "ToolOptions", ["login"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--server",
                    PropertyName = "Server",
                    CSharpType = "string?",
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Server",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyServer",
                    CSharpType = "string?",
                    ForwardToPropertyName = "Server",
                    ObsoleteMessage = "Use Server instead.",
                },
            ],
        };
        var collisionResolved = InheritedPropertyCollisionResolver.Resolve(Tool(command))
            .Commands.Single();

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            collisionResolved,
            [BaselineProperty("Server", "string", argumentPosition: 0, isRequired: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo("ServerOption");
            await Assert.That(preserved.PositionalArguments.Single().PropertyName).IsEqualTo("Server");
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardToPropertyName)
                .IsEqualTo("ServerOption");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Keeps_Historical_Alias_On_Restored_Collision_Target()
    {
        var command = Command("ToolLoginOptions", "ToolOptions", ["login"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--server",
                    PropertyName = "Server",
                    CSharpType = "string?",
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Server",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyServer",
                    CSharpType = "string?",
                    ForwardToPropertyName = "Server",
                    ObsoleteMessage = "Use Server instead.",
                },
            ],
        };
        var collisionResolved = InheritedPropertyCollisionResolver.Resolve(Tool(command))
            .Commands.Single();

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            collisionResolved,
            [
                BaselineProperty("Server", "string", argumentPosition: 0, isRequired: true),
                BaselineProperty(
                    "LegacyServer",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "Server"),
            ]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo("ServerOption");
            await Assert.That(preserved.PositionalArguments.Single().PropertyName).IsEqualTo("Server");
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardToPropertyName)
                .IsEqualTo("Server");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retargets_Transitive_Compatibility_Aliases()
    {
        var command = Command("ToolChecksumOptions", "ToolOptions", ["checksum"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--changeset-author",
                    PropertyName = "ChangesetAuthor",
                    CSharpType = "string?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyAuthor",
                    CSharpType = "string?",
                    ForwardToPropertyName = "ChangeSetAuthor",
                    ObsoleteMessage = "Use ChangeSetAuthor instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty(
                    "ChangeSetAuthor",
                    "string?",
                    switchName: "--changeset-author"),
                BaselineProperty(
                    "LegacyAuthor",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "ChangeSetAuthor"),
            ]);

        var aliases = preserved.CompatibilityProperties.ToDictionary(
            static property => property.PropertyName,
            StringComparer.Ordinal);
        using (Assert.Multiple())
        {
            await Assert.That(aliases["ChangeSetAuthor"].ForwardToPropertyName)
                .IsEqualTo("ChangesetAuthor");
            await Assert.That(aliases["LegacyAuthor"].ForwardToPropertyName)
                .IsEqualTo("ChangesetAuthor");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Drops_Forwarding_When_Target_Was_Removed()
    {
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            Command("ToolChecksumOptions", "ToolOptions", ["checksum"]),
            [
                BaselineProperty(
                    "ChangeSetAuthor",
                    "string?",
                    switchName: "--changeset-author"),
                BaselineProperty(
                    "ChangesetAuthor",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "ChangeSetAuthor"),
            ]);
        var aliases = preserved.CompatibilityProperties.ToDictionary(
            static property => property.PropertyName,
            StringComparer.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(aliases["ChangeSetAuthor"].ForwardToPropertyName).IsNull();
            await Assert.That(aliases["ChangesetAuthor"].ForwardToPropertyName).IsNull();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Composes_Forwarding_Conversions()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            Options = [RequiredOption("--count", "Count")],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyCount",
                    CSharpType = "int?",
                    ForwardToPropertyName = "NullableCount",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableInt32ToString,
                    ObsoleteMessage = "Use Count instead.",
                },
                new CliCompatibilityProperty
                {
                    PropertyName = "NullableCount",
                    CSharpType = "string?",
                    ForwardToPropertyName = "Count",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableStringToRequiredString,
                    UseInitAccessor = true,
                    ObsoleteMessage = "Use Count instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, []);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var legacyCount = preserved.CompatibilityProperties.Single(property =>
            property.PropertyName.Equals("LegacyCount", StringComparison.Ordinal));
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(legacyCount.ForwardToPropertyName).IsEqualTo("Count");
            await Assert.That(legacyCount.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableInt32ToRequiredString);
            await Assert.That(generated).Contains("value?.ToString(global::System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "LegacyCount",
            "Count",
            CliCompatibilityForwardingKind.NullableInt32ToRequiredString);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Composes_Integer_To_Collection_Forwarding()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--count",
                    PropertyName = "CountValues",
                    CSharpType = "IEnumerable<string>?",
                    AcceptsMultipleValues = true,
                    IsCollection = true,
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyCount",
                    CSharpType = "int?",
                    ForwardToPropertyName = "Count",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableInt32ToString,
                    ObsoleteMessage = "Use CountValues instead.",
                },
                new CliCompatibilityProperty
                {
                    PropertyName = "Count",
                    CSharpType = "string?",
                    ForwardToPropertyName = "CountValues",
                    ForwardingKind = CliCompatibilityForwardingKind.ScalarToCollection,
                    ObsoleteMessage = "Use CountValues instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, []);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var legacyCount = preserved.CompatibilityProperties.Single(property =>
            property.PropertyName.Equals("LegacyCount", StringComparison.Ordinal));
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(legacyCount.ForwardToPropertyName).IsEqualTo("CountValues");
            await Assert.That(legacyCount.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
            await Assert.That(generated).Contains("CountValues = value is null ? null : [value.Value.ToString(");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "LegacyCount",
            "CountValues",
            CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Composes_Integer_To_Optional_Value_Forwarding()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--color",
                    PropertyName = "ColorOption",
                    CSharpType = "CliOptionValue?",
                    ValueArity = CliOptionValueArity.Optional,
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyColor",
                    CSharpType = "int?",
                    ForwardToPropertyName = "Color",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableInt32ToString,
                    ObsoleteMessage = "Use ColorOption instead.",
                },
                new CliCompatibilityProperty
                {
                    PropertyName = "Color",
                    CSharpType = "string?",
                    ForwardToPropertyName = "ColorOption",
                    ForwardingKind = CliCompatibilityForwardingKind.NullableStringToCliOptionValue,
                    ObsoleteMessage = "Use ColorOption instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, []);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var legacyColor = preserved.CompatibilityProperties.Single(property =>
            property.PropertyName.Equals("LegacyColor", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(legacyColor.ForwardToPropertyName).IsEqualTo("ColorOption");
            await Assert.That(legacyColor.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue);
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "LegacyColor",
            "ColorOption",
            CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Composes_Forwarding_When_Target_Is_Renamed()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--count",
                    PropertyName = "CountValues",
                    CSharpType = "IEnumerable<string>?",
                    AcceptsMultipleValues = true,
                    IsCollection = true,
                },
            ],
        };
        var baseline = new GeneratedApiProperty[]
        {
            BaselineProperty("Count", "string?", switchName: "--count"),
            new(
                "LegacyCount",
                "int?",
                null,
                null,
                false,
                true,
                "Count",
                "Use Count instead.",
                ForwardingKind: CliCompatibilityForwardingKind.NullableInt32ToString),
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, baseline);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var legacyCount = preserved.CompatibilityProperties.Single(property =>
            property.PropertyName.Equals("LegacyCount", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(legacyCount.ForwardToPropertyName).IsEqualTo("CountValues");
            await Assert.That(legacyCount.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "LegacyCount",
            "CountValues",
            CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Does_Not_Forward_Boolean_To_Enum_Like_String()
    {
        var command = Command("AzAksCreateOptions", "AzOptions", ["aks", "create"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--apiserver-subnet-id",
                    PropertyName = "ApiServerSubnetId",
                    CSharpType = "string?",
                    Description = "Allowed values: silent, silentPreferred, interactive.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("ApiserverSubnetId", "bool?", switchName: "--apiserver-subnet-id")]);
        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo("ApiServerSubnetId");
            await Assert.That(preserved.CompatibilityProperties).IsEmpty();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Does_Not_Preserve_Flag_As_Enum_Like_String()
    {
        var command = Command("WingetInstallOptions", "WingetOptions", ["install"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--authentication-mode",
                    PropertyName = "AuthenticationMode",
                    CSharpType = "string?",
                    Description = "Allowed values: silent, silentPreferred, interactive.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("AuthenticationMode", "bool?", switchName: "--authentication-mode")]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("AuthenticationMode");
            await Assert.That(preserved.Options.Single().CSharpType).IsEqualTo("string?");
            await Assert.That(preserved.CompatibilityProperties).IsEmpty();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Flag_That_Became_A_Value()
    {
        var command = Command("BrewInfoOptions", "BrewOptions", ["info"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--json",
                    PropertyName = "Json",
                    CSharpType = "string?",
                    IsFlag = false,
                    Description = "Allowed values: true, false.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Json", "bool?", switchName: "--json")]);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var option = preserved.Options.Single();
        var alias = preserved.CompatibilityProperties.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.PropertyName).IsEqualTo("JsonValue");
            await Assert.That(alias.PropertyName).IsEqualTo("Json");
            await Assert.That(alias.ForwardToPropertyName).IsEqualTo("JsonValue");
            await Assert.That(alias.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableBooleanToString);
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "Json",
            "JsonValue",
            CliCompatibilityForwardingKind.NullableBooleanToString);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved)))
            .Single().Content;
        await Assert.That(generated)
            .Contains("set => JsonValue = value == true ? \"true\" : null;");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Flag_That_Became_Multiple_Values()
    {
        var command = Command("AzStackWhatifCreateOptions", "AzOptions", ["stack-whatif", "create"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--deny-settings-excluded-actions",
                    PropertyName = "DenySettingsExcludedActions",
                    CSharpType = "IEnumerable<string>?",
                    AcceptsMultipleValues = true,
                    IsCollection = true,
                    Description = "Allowed values: true, false.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("DenySettingsExcludedActions", "bool?", switchName: "--deny-settings-excluded-actions")]);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var option = preserved.Options.Single();
        var alias = preserved.CompatibilityProperties.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.PropertyName).IsEqualTo("DenySettingsExcludedActionsValues");
            await Assert.That(alias.PropertyName).IsEqualTo("DenySettingsExcludedActions");
            await Assert.That(alias.ForwardToPropertyName).IsEqualTo("DenySettingsExcludedActionsValues");
            await Assert.That(alias.ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableBooleanToStringCollection);
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "DenySettingsExcludedActions",
            "DenySettingsExcludedActionsValues",
            CliCompatibilityForwardingKind.NullableBooleanToStringCollection);

        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved)))
            .Single().Content;
        await Assert.That(generated)
            .Contains("set => DenySettingsExcludedActionsValues = value == true ? [\"true\"] : null;");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Drops_Unsafe_Preexisting_Boolean_String_Alias()
    {
        var command = Command("WingetInstallOptions", "WingetOptions", ["install"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--authentication-mode",
                    PropertyName = "AuthenticationMode",
                    CSharpType = "string?",
                    Description = "Allowed values: silent, silentPreferred, interactive.",
                },
            ],
        };
        var baseline = new GeneratedApiProperty[]
        {
            BaselineProperty(
                "AuthenticationModeValue",
                "string?",
                switchName: "--authentication-mode"),
            BaselineProperty(
                "AuthenticationMode",
                "bool?",
                switchName: "--authentication-mode",
                isCompatibility: true,
                forwardToPropertyName: "AuthenticationModeValue",
                forwardingKind: CliCompatibilityForwardingKind.NullableBooleanToString),
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, baseline);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("AuthenticationMode");
            await Assert.That(preserved.CompatibilityProperties)
                .Contains(property => property.PropertyName == "AuthenticationModeValue"
                                      && property.CSharpType == "string?");
            await Assert.That(preserved.CompatibilityProperties)
                .DoesNotContain(property => property.CSharpType == "bool?");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Maps_Pulumi_Local_Flag_To_Local_Backend()
    {
        var command = Command("PulumiLogoutOptions", "PulumiOptions", ["logout"]) with
        {
            FullCommand = "pulumi logout",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--local",
                    PropertyName = "Local",
                    CSharpType = "string?",
                    IsFlag = false,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Local", "bool?", switchName: "--local")]);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var alias = preserved.CompatibilityProperties.Single();

        await Assert.That(alias.ForwardingKind)
            .IsEqualTo(CliCompatibilityForwardingKind.NullableBooleanToLocalBackendString);
        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "Local",
            "LocalValue",
            CliCompatibilityForwardingKind.NullableBooleanToLocalBackendString);
    }

    private static async Task AssertCompatibilityForwardingRoundTrips(
        CliCommandDefinition generatedCommand,
        string compatibilityPropertyName,
        string expectedTarget,
        CliCompatibilityForwardingKind expectedKind)
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(generatedCommand)))
                .Single().Content;
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, $"{generatedCommand.ClassName}.Generated.cs"),
                generated);
            var current = generatedCommand with { CompatibilityProperties = [] };
            var roundTripped = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var alias = roundTripped.Commands.Single().CompatibilityProperties.Single(property =>
                property.PropertyName.Equals(compatibilityPropertyName, StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(alias.ForwardToPropertyName).IsEqualTo(expectedTarget);
                await Assert.That(alias.ForwardingKind).IsEqualTo(expectedKind);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Drops_Alias_Shadowed_By_Live_Property()
    {
        var command = Command("ToolChecksumOptions", "ToolOptions", ["checksum"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--changeset-author",
                    PropertyName = "ChangesetAuthor",
                    CSharpType = "string?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "ChangesetAuthor",
                    CSharpType = "string?",
                    ForwardToPropertyName = "ChangeSetAuthor",
                    ObsoleteMessage = "Use ChangeSetAuthor instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty(
                    "ChangeSetAuthor",
                    "string?",
                    switchName: "--changeset-author"),
                BaselineProperty(
                    "ChangesetAuthor",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "ChangeSetAuthor"),
            ]);

        await Assert.That(preserved.CompatibilityProperties).HasSingleItem();
        var alias = preserved.CompatibilityProperties.Single();
        await Assert.That(alias.PropertyName).IsEqualTo("ChangeSetAuthor");
        await Assert.That(alias.ForwardToPropertyName).IsEqualTo("ChangesetAuthor");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Case_Variant_Alias_Forwarding()
    {
        var command = Command("ToolChecksumOptions", "ToolOptions", ["checksum"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--changeset-author",
                    PropertyName = "ChangeSetAuthor",
                    CSharpType = "string?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty("ChangeSetAuthor", "string?", isCompatibility: true),
                BaselineProperty("ChangesetAuthor", "string?", isCompatibility: true),
            ]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardToPropertyName)
                .IsEqualTo("ChangeSetAuthor");
            await Assert.That(generated).Contains("get => ChangeSetAuthor;");
            await Assert.That(generated).Contains("set => ChangeSetAuthor = value;");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Renames_Live_Property_Shadowing_Forwarded_Alias()
    {
        var command = Command("ToolPushOptions", "ToolOptions", ["push"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "string?",
                },
                new CliOptionDefinition
                {
                    SwitchName = "--legacy-output",
                    PropertyName = "LegacyOutput",
                    CSharpType = "string?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "ScrapedLegacyOutput",
                    CSharpType = "string?",
                    ForwardToPropertyName = "LegacyOutput",
                    ObsoleteMessage = "Use LegacyOutput instead.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty("Output", "string?", switchName: "--output"),
                BaselineProperty(
                    "LegacyOutput",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "Output"),
                BaselineProperty(
                    "VeryLegacyOutput",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "LegacyOutput"),
            ]);

        var options = preserved.Options.ToDictionary(
            static option => option.PropertyName,
            StringComparer.Ordinal);
        var aliases = preserved.CompatibilityProperties.ToDictionary(
            static property => property.PropertyName,
            StringComparer.Ordinal);
        using (Assert.Multiple())
        {
            await Assert.That(options["Output"].SwitchName).IsEqualTo("--output");
            await Assert.That(options["LegacyOutputOption"].SwitchName).IsEqualTo("--legacy-output");
            await Assert.That(aliases["LegacyOutput"].ForwardToPropertyName).IsEqualTo("Output");
            await Assert.That(aliases["VeryLegacyOutput"].ForwardToPropertyName)
                .IsEqualTo("Output");
            await Assert.That(aliases["ScrapedLegacyOutput"].ForwardToPropertyName)
                .IsEqualTo("LegacyOutputOption");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Scalar_To_Collection_Changes()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--command-options",
                    PropertyName = "CommandOptions",
                    CSharpType = "IEnumerable<string>?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("CommandOptions", "string?", switchName: "--command-options")]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("CommandOptionsValues");
            await Assert.That(generated)
                .Contains("IEnumerable<string>? CommandOptionsValues");
            await Assert.That(generated)
                .Contains("public string? CommandOptions");
            await Assert.That(generated)
                .Contains("get => CommandOptionsValues?.FirstOrDefault();");
            await Assert.That(generated)
                .Contains("set => CommandOptionsValues = value is null ? null : [value];");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Renamed_Scalar_To_Collection_Changes()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--command-options",
                    PropertyName = "CommandOptionValues",
                    CSharpType = "IEnumerable<string>?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("CommandOptions", "string?", switchName: "--command-options")]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("CommandOptionValues");
            await Assert.That(generated)
                .Contains("IEnumerable<string>? CommandOptionValues");
            await Assert.That(generated)
                .Contains("public string? CommandOptions");
            await Assert.That(generated)
                .Contains("get => CommandOptionValues?.FirstOrDefault();");
            await Assert.That(generated)
                .Contains("set => CommandOptionValues = value is null ? null : [value];");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Enum_Option_Instead_Of_Disconnected_Alias()
    {
        var command = Command("ToolUpdateOptions", "ToolOptions", ["update"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--labels",
                    PropertyName = "Labels",
                    CSharpType = "IReadOnlyList<KeyValue>?",
                    IsKeyValue = true,
                    AcceptsMultipleValues = true,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Labels",
                "global::ModularPipelines.Tool.Enums.ToolLabels?",
                switchName: "--labels")]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;
        var option = preserved.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.PropertyName).IsEqualTo("Labels");
            await Assert.That(option.CSharpType)
                .IsEqualTo("global::ModularPipelines.Tool.Enums.ToolLabels?");
            await Assert.That(preserved.CompatibilityProperties).IsEmpty();
            await Assert.That(generated)
                .Contains("[CliOption(\"--labels\")]");
            await Assert.That(generated)
                .Contains("public global::ModularPipelines.Tool.Enums.ToolLabels? Labels { get; set; }");
            await Assert.That(generated).DoesNotContain("[Obsolete(");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Reads_Nullable_To_Required_Compatibility_Accessors()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolRunOptions.Generated.cs"),
                "public record ToolRunOptions { "
                + "[CliArgument(0)] public string Subcommand { get; set; } "
                + "[Obsolete(\"Use Subcommand instead.\")] public string? Args { get => Subcommand; set => Subcommand = value ?? string.Empty; } "
                + "}");
            var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Subcommand",
                        CSharpType = "string",
                        PositionIndex = 0,
                    },
                ],
                CompatibilityProperties =
                [
                    new CliCompatibilityProperty
                    {
                        PropertyName = "Args",
                        CSharpType = "string?",
                        ForwardToPropertyName = "Subcommand",
                        ForwardingKind = CliCompatibilityForwardingKind.NullableStringToRequiredString,
                        ObsoleteMessage = "Use Subcommand instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);

            await Assert.That(preserved.Commands.Single().CompatibilityProperties.Single().ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableStringToRequiredString);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Renamed_Nullable_String_As_Required_String()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Subcommand",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Args", "string?", argumentPosition: 0)]);
        ExternalToolDefinitionLoader.ValidateCompatibilityMetadata(preserved, []);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableStringToRequiredString);
            await Assert.That(generated).Contains("public string? Args");
            await Assert.That(generated).Contains("get => Subcommand;");
            await Assert.That(generated).Contains("init => Subcommand = value ?? string.Empty;");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Preexisting_Name_When_Required_Member_Is_Restored()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Args",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty("Subcommand", "string", argumentPosition: 0, isRequired: true),
                BaselineProperty("Args", "string?", argumentPosition: 0),
            ]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Subcommand");
            await Assert.That(preserved.CompatibilityProperties.Single().CSharpType)
                .IsEqualTo("string?");
            await Assert.That(generated).Contains("public string? Args");
            await Assert.That(generated).Contains("get => Subcommand;");
            await Assert.That(generated).Contains("init => Subcommand = value ?? string.Empty;");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Reassigned_Property_Names()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--different",
                    PropertyName = "CommandOptions",
                    CSharpType = "string?",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("CommandOptions", "string?", switchName: "--command-options")]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.CommandOptions changed CLI switch or argument position");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Reused_Compatibility_Property_With_Different_Type()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--restored",
                    PropertyName = "RemovedFlag",
                    CSharpType = "string?",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("RemovedFlag", "bool?", isCompatibility: true)]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.RemovedFlag changed type from bool? to string?");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Changed_Negated_Switch()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--feature",
                    NegatedSwitchName = "--without-feature",
                    PropertyName = "Feature",
                    CSharpType = "bool?",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [
                    BaselineProperty(
                        "Feature",
                        "bool?",
                        switchName: "--feature",
                        negatedSwitchName: "--no-feature"),
                ]));

        await Assert.That(exception.Message)
            .Contains(
                "ToolCopyOptions.Feature changed negated CLI switch from "
                + "--no-feature to --without-feature");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Allows_Reintroduced_Compatibility_Property()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--restored",
                    PropertyName = "RemovedFlag",
                    CSharpType = "bool?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("RemovedFlag", "bool?", isCompatibility: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo("RemovedFlag");
            await Assert.That(preserved.CompatibilityProperties).IsEmpty();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Allows_Compatibility_Property_To_Back_Restored_Option()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--current-flag",
                    PropertyName = "CurrentFlag",
                    CSharpType = "bool?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "RemovedFlag",
                    CSharpType = "bool?",
                    ForwardToPropertyName = "CurrentFlag",
                    ObsoleteMessage = "Use CurrentFlag.",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("RemovedFlag", "bool?", isCompatibility: true)]);

        await Assert.That(preserved.CompatibilityProperties.Single().ForwardToPropertyName)
            .IsEqualTo("CurrentFlag");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Conflicting_Supplied_Compatibility_Property_Type()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "RemovedFlag",
                    CSharpType = "string?",
                    ObsoleteMessage = "Still retained.",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("RemovedFlag", "bool?", isCompatibility: true)]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.RemovedFlag compatibility property changed type from bool? to string?");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Conflicting_Supplied_Compatibility_Forwarding()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "OldName",
                    CSharpType = "string?",
                    ForwardToPropertyName = "DifferentName",
                    ObsoleteMessage = "Use DifferentName.",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [
                    BaselineProperty(
                        "OldName",
                        "string?",
                        isCompatibility: true,
                        forwardToPropertyName: "CurrentName"),
                ]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.OldName compatibility property changed forwarding target from CurrentName to DifferentName");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Conflicting_Supplied_Alias_For_Formerly_Active_Property()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--removed-flag",
                    PropertyName = "CurrentFlag",
                    CSharpType = "bool?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "RemovedFlag",
                    CSharpType = "string?",
                    ForwardToPropertyName = "CurrentFlag",
                    ObsoleteMessage = "Use CurrentFlag.",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("RemovedFlag", "bool?", switchName: "--removed-flag")]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.RemovedFlag compatibility property changed type from bool? to string?");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Conflicting_Supplied_Forwarding_For_Formerly_Active_Property()
    {
        var command = Command("ToolCopyOptions", "ToolOptions", ["copy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--removed-flag",
                    PropertyName = "CurrentFlag",
                    CSharpType = "bool?",
                },
            ],
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "RemovedFlag",
                    CSharpType = "bool?",
                    ForwardToPropertyName = "DifferentFlag",
                    ObsoleteMessage = "Use DifferentFlag.",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("RemovedFlag", "bool?", switchName: "--removed-flag")]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.RemovedFlag compatibility property changed forwarding target from CurrentFlag to DifferentFlag");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Baseline_Optional_Value_Type()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--progress",
                    PropertyName = "Progress",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Progress", "string?", switchName: "--progress")]);
        var progress = preserved.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(progress.CSharpType).IsEqualTo("string?");
            await Assert.That(progress.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Optional_Member_Becoming_Required()
    {
        var command = Command("ToolNewOptions", "ToolOptions", ["new"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Name",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Name", "string?", argumentPosition: 0)]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.PositionalArguments.Single().IsRequired).IsFalse();
            await Assert.That(generated).Contains("public string? Name { get; set; }");
            await Assert.That(generated).DoesNotContain("public record ToolNewOptions(");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Required_Constructor_Order()
    {
        var command = Command("ToolMoveOptions", "ToolOptions", ["move"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--second",
                    PropertyName = "Second",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--first",
                    PropertyName = "First",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty("First", "string", switchName: "--first", isRequired: true),
                BaselineProperty("Second", "string", switchName: "--second", isRequired: true),
            ]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        await Assert.That(generated.IndexOf("string First", StringComparison.Ordinal))
            .IsLessThan(generated.IndexOf("string Second", StringComparison.Ordinal));
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Old_Deconstruct_Arity_For_New_Required_Members()
    {
        var command = Command("ToolMoveOptions", "ToolOptions", ["move"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--source",
                    PropertyName = "Source",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--destination",
                    PropertyName = "Destination",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Source", "string", switchName: "--source", isRequired: true)]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public ToolMoveOptions(string Source)");
            await Assert.That(generated).Contains(": this(Source, default(string)!)");
            await Assert.That(generated).Contains("public void Deconstruct(out string Source)");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Required_Member_Demoted_To_Optional()
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
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("Target", "string", argumentPosition: 0, isRequired: true)]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        await Assert.That(generated).Contains("string Target");
        await Assert.That(generated).DoesNotContain("public string? Target");
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
    public async Task ApiCompatibilityPreserver_Retains_Removed_Optional_Positional_Operands()
    {
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            Command("ToolInstallOptions", "ToolOptions", ["install"]),
            [BaselineProperty("Name", "string?", argumentPosition: 1)]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public string? Name { get; set; }");
            await Assert.That(generated).Contains("Name is no longer supported");
            await Assert.That(generated).DoesNotContain("CliArgument(1)");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Removed_Required_Positional_Operands()
    {
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            Command("ToolAddOptions", "ToolOptions", ["add"]),
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("CliArgument(0, Phase = CommandLinePhase.Passthrough, Required = true)");
            await Assert.That(generated).Contains("string Package");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Renames_Option_Colliding_With_Restored_Operand()
    {
        var command = Command(
            "ToolAddOptions",
            "ToolOptions",
            ["add"],
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--package",
                    PropertyName = "Package",
                    CSharpType = "string?",
                },
            ]) with
        {
            DocumentationExampleValues = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["Package"] = "current-package",
            },
        };
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Package");
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("PackageOption");
            await Assert.That(preserved.DocumentationExampleValues.Keys)
                .IsEquivalentTo(["Package", "PackageOption"]);
            await Assert.That(preserved.DocumentationExampleValues["Package"])
                .IsEqualTo("current-package");
            await Assert.That(preserved.DocumentationExampleValues["PackageOption"])
                .IsEqualTo("current-package");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Explicit_Collision_Documentation()
    {
        var command = Command(
            "ToolAddOptions",
            "ToolOptions",
            ["add"],
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--package",
                    PropertyName = "Package",
                    CSharpType = "string?",
                },
            ]) with
        {
            DocumentationExampleValues = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["Package"] = "operand-package",
                ["PackageOption"] = "option-package",
            },
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.DocumentationExampleValues["Package"])
                .IsEqualTo("operand-package");
            await Assert.That(preserved.DocumentationExampleValues["PackageOption"])
                .IsEqualTo("option-package");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Reuses_Occupied_Positional_Slot()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Replacement",
                    CSharpType = "string?",
                    PositionIndex = 0,
                    Phase = CommandLinePhase.Passthrough,
                    PrependOptionTerminator = true,
                },
            ],
            DocumentationExampleValues = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["Replacement"] = "package",
            },
        };
        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);

        await Assert.That(preserved.PositionalArguments).HasSingleItem();
        var argument = preserved.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Package");
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.PrependOptionTerminator).IsTrue();
            await Assert.That(preserved.DocumentationExampleValues.Keys)
                .IsEquivalentTo(["Package"]);
            await Assert.That(preserved.DocumentationExampleValues["Package"])
                .IsEqualTo("package");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Prefers_Explicit_Occupied_Slot_Documentation()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Replacement",
                    CSharpType = "string?",
                    PositionIndex = 0,
                    Phase = CommandLinePhase.Passthrough,
                },
            ],
            DocumentationExampleValues = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["Replacement"] = "fallback-package",
                ["Package"] = "explicit-package",
            },
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.DocumentationExampleValues.Keys)
                .IsEquivalentTo(["Package"]);
            await Assert.That(preserved.DocumentationExampleValues["Package"])
                .IsEqualTo("explicit-package");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Constructor_When_Required_Member_Is_Added()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Package",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(command, []);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public ToolAddOptions()");
            await Assert.That(generated).Contains(": this(default(string)!)");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Previously_Generated_Secondary_Constructors()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolAddOptions.Generated.cs"),
                "public record ToolAddOptions([property: CliArgument(0)] string Package) "
                + "{ public ToolAddOptions() : this(default!) { } }");
            var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Package",
                        CSharpType = "string",
                        IsRequired = true,
                        PositionIndex = 0,
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var generated = (await new OptionsClassGenerator().GenerateAsync(preserved)).Single().Content;

            await Assert.That(generated).Contains("public ToolAddOptions()");
            await Assert.That(generated).Contains(": this(default(string)!)");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Conflicting_Supplied_Compatibility_Constructors()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolAddOptions.Generated.cs"),
                "public record ToolAddOptions([property: CliArgument(0)] string Package, "
                + "[property: CliArgument(1)] string Destination) "
                + "{ public ToolAddOptions(string LegacyPackage) : this(LegacyPackage, default!) { } }");
            var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Package",
                        CSharpType = "string",
                        IsRequired = true,
                        PositionIndex = 0,
                    },
                    new CliPositionalArgument
                    {
                        PropertyName = "Destination",
                        CSharpType = "string",
                        IsRequired = true,
                        PositionIndex = 1,
                    },
                ],
                CompatibilityConstructors =
                [
                    new CliCompatibilityConstructor
                    {
                        Parameters = [new("DifferentPackage", "string")],
                        PrimaryConstructorArguments = ["default!", "default!"],
                    },
                ],
            };

            var exception = Assert.Throws<InvalidOperationException>(() =>
                GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root));

            await Assert.That(exception.Message)
                .Contains("Compatibility constructor (string) conflicts with the generated baseline contract");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Does_Not_Retain_Nullability_Only_Constructor_Duplicates()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolAddOptions.Generated.cs"),
                "public record ToolAddOptions([property: CliArgument(0)] string Package) "
                + "{ public ToolAddOptions(string? LegacyPackage) : this(LegacyPackage!) { } }");
            var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Package",
                        CSharpType = "string",
                        IsRequired = true,
                        PositionIndex = 0,
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var generated = (await new OptionsClassGenerator().GenerateAsync(preserved)).Single().Content;

            await Assert.That(generated).DoesNotContain("public ToolAddOptions(string? LegacyPackage)");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Extends_Deconstruct_Preservation_For_New_Required_Members()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolMoveOptions.Generated.cs"),
                "public record ToolMoveOptions([property: CliOption(\"--source\")] string Source, "
                + "[property: CliOption(\"--destination\")] string Destination) "
                + "{ public ToolMoveOptions(string Source) : this(Source, default!) { } "
                + "public void Deconstruct(out string Source) { Source = this.Source; } }");
            var command = Command("ToolMoveOptions", "ToolOptions", ["move"]) with
            {
                Options =
                [
                    RequiredOption("--source", "Source"),
                    RequiredOption("--destination", "Destination"),
                    RequiredOption("--force", "Force"),
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var generated = (await new OptionsClassGenerator().GenerateAsync(preserved)).Single().Content;

            using (Assert.Multiple())
            {
                await Assert.That(generated).Contains("public ToolMoveOptions(string Source, string Destination)");
                await Assert.That(generated).Contains(": this(Source, Destination, default(string)!)");
                await Assert.That(generated)
                    .Contains("public void Deconstruct(out string Source, out string Destination)");
                await Assert.That(generated).Contains("public ToolMoveOptions(string Source)");
                await Assert.That(generated)
                    .Contains(": this(Source, default(string)!, default(string)!)");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Command_Group_Execute_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolEditAddOptions.Generated.cs"),
                "public record ToolEditAddOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolEditAdd.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolEditAdd { public Task ExecuteAsync(ToolEditAddOptions? options = null) => Task.CompletedTask; }");
            var parent = Command(
                "ToolEditAddOptions",
                "ToolOptions",
                ["edit", "add"],
                subDomainGroup: "edit");
            var child = Command(
                "ToolEditAddSecretOptions",
                "ToolOptions",
                ["edit", "add", "secret"],
                subDomainGroup: "edit");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(parent, child), root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith(
                    "ToolEditAdd.Generated.cs",
                    StringComparison.Ordinal))
                .Content;

            await Assert.That(generated).Contains("Task<CommandResult> ExecuteAsync(");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Removed_Command_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolRemovedOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"removed\")] "
                + "public record ToolRemovedOptions : ToolOptions { "
                + "[Range(0, 6)] "
                + "[RegularExpression(\"^[0-6]$\")] "
                + "[CliFlag(\"--force\", ShortForm = \"-f\", PreferShortForm = true, Phase = CommandLinePhase.Terminal)] "
                + "public int? Force { get; set; } "
                + "[CliFlag(\"--feature\", NegatedName = \"--no-feature\")] "
                + "public bool? Feature { get; set; } "
                + "[CliOptionValueRange(1, 3)] "
                + "[CliOptionValueRegularExpression(\"^[1-3]$\")] "
                + "[CliOption(\"--pull\", ShortForm = \"-p\", PreferShortForm = true, "
                + "Format = OptionFormat.EqualsSeparated, ValueArity = CliOptionValueArity.Optional)] "
                + "public CliOptionValue? Pull { get; set; } "
                + "[SecretValue(\"password\", \"token\")] "
                + "[CliOption(\"--arguments\", GroupValues = true)] "
                + "public IEnumerable<string>? Arguments { get; set; } "
                + "[SecretValue] "
                + "[CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true, "
                + "PrependOptionTerminatorIfValueStartsWithDash = true)] "
                + "public string? Operand { get; set; } }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public Task RemovedAsync(ToolRemovedOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);
            var restored = preserved.Commands.Single(command =>
                command.ClassName.Equals("ToolRemovedOptions", StringComparison.Ordinal));
            var force = restored.Options.Single(option => option.PropertyName == "Force");
            var feature = restored.Options.Single(option => option.PropertyName == "Feature");
            var pull = restored.Options.Single(option => option.PropertyName == "Pull");
            var arguments = restored.Options.Single(option => option.PropertyName == "Arguments");
            var operand = restored.PositionalArguments.Single();
            var generated = (await new ServiceInterfaceGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("ITool.Generated.cs", StringComparison.Ordinal))
                .Content;
            var generatedOptions = (await new OptionsClassGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("ToolRemovedOptions.Generated.cs", StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(restored.CommandParts).IsEquivalentTo(["removed"]);
                await Assert.That(restored.IsCompatibilityOnly).IsTrue();
                await Assert.That(force.IsFlag).IsTrue();
                await Assert.That(force.ShortForm).IsEqualTo("-f");
                await Assert.That(force.PreferShortForm).IsTrue();
                await Assert.That(force.Phase).IsEqualTo(CommandLinePhase.Terminal);
                await Assert.That(force.ValidationConstraints!.MinValue).IsEqualTo(0);
                await Assert.That(force.ValidationConstraints.MaxValue).IsEqualTo(6);
                await Assert.That(force.ValidationConstraints.Pattern).IsEqualTo("^[0-6]$");
                await Assert.That(feature.NegatedSwitchName).IsEqualTo("--no-feature");
                await Assert.That(pull.IsFlag).IsFalse();
                await Assert.That(pull.ShortForm).IsEqualTo("-p");
                await Assert.That(pull.PreferShortForm).IsTrue();
                await Assert.That(pull.ValueSeparator).IsEqualTo("=");
                await Assert.That(pull.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
                await Assert.That(pull.ValidationConstraints!.MinValue).IsEqualTo(1);
                await Assert.That(pull.ValidationConstraints.MaxValue).IsEqualTo(3);
                await Assert.That(pull.ValidationConstraints.Pattern).IsEqualTo("^[1-3]$");
                await Assert.That(arguments.GroupValues).IsTrue();
                await Assert.That(arguments.IsSecret).IsTrue();
                await Assert.That(arguments.SecretValueKeys).IsEquivalentTo(["password", "token"]);
                await Assert.That(operand.Phase).IsEqualTo(CommandLinePhase.Passthrough);
                await Assert.That(operand.IsSecret).IsTrue();
                await Assert.That(operand.PrependOptionTerminator).IsTrue();
                await Assert.That(operand.PrependOptionTerminatorIfValueStartsWithDash).IsTrue();
                await Assert.That(generatedOptions)
                    .Contains("[CliOption(\"--pull\", ShortForm = \"-p\", PreferShortForm = true, Format = OptionFormat.EqualsSeparated, ValueArity = CliOptionValueArity.Optional)]");
                await Assert.That(generatedOptions)
                    .Contains("[CliFlag(\"--feature\", NegatedName = \"--no-feature\")]");
                await Assert.That(generatedOptions).Contains("[Range(0, 6)]");
                await Assert.That(generatedOptions).Contains("[RegularExpression(\"^[0-6]$\")]");
                await Assert.That(generatedOptions).Contains("[CliOptionValueRange(1, 3)]");
                await Assert.That(generatedOptions)
                    .Contains("[CliOptionValueRegularExpression(\"^[1-3]$\")]");
                await Assert.That(generatedOptions)
                    .Contains("[CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true, PrependOptionTerminatorIfValueStartsWithDash = true)]");
                await Assert.That(generatedOptions).Contains("[SecretValue(\"password\", \"token\")]");
                await Assert.That(generatedOptions)
                    .Contains($"[Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]");
                await Assert.That(generated)
                    .Contains($"[Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]");
                await Assert.That(generated).Contains("RemovedAsync(ToolRemovedOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Keeps_ConditionallyAvailable_Command_Active()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolEnterpriseOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"enterprise\")] "
                + "public record ToolEnterpriseOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public Task EnterpriseAsync(ToolEnterpriseOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command("ToolCommunityOptions", "ToolOptions", ["community"])) with
            {
                CommandCoverage = new CliCommandCoveragePolicy
                {
                    ConditionallyAvailableCommands =
                    [
                        new CliConditionallyAvailableCommand
                        {
                            Command = "tool enterprise",
                            Reason = "Requires an enterprise license.",
                        },
                    ],
                },
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var restored = preserved.Commands.Single(command =>
                command.ClassName.Equals("ToolEnterpriseOptions", StringComparison.Ordinal));
            var generatedService = (await new ServiceInterfaceGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("ITool.Generated.cs", StringComparison.Ordinal))
                .Content;
            var generatedOptions = (await new OptionsClassGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith(
                    "ToolEnterpriseOptions.Generated.cs",
                    StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(restored.IsCompatibilityOnly).IsFalse();
                await Assert.That(generatedOptions).DoesNotContain("[Obsolete(");
                await Assert.That(generatedService).DoesNotContain("[Obsolete(");
                await Assert.That(generatedService).Contains("EnterpriseAsync(ToolEnterpriseOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task CompatibilityOnly_SubDomain_Facades_Are_Obsolete()
    {
        var command = Command(
            "ToolRemovedChildOptions",
            "ToolOptions",
            ["removed", "child"],
            subDomainGroup: "removed") with
        {
            IsCompatibilityOnly = true,
        };
        var tool = Tool(command) with
        {
            CommandGroupAliases =
            [
                new CliCommandGroupAlias
                {
                    Alias = "legacy",
                    CanonicalCommand = "removed",
                    ObsoleteMessage = "Use removed instead.",
                },
            ],
        };
        var obsoleteAttribute =
            $"[Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]";
        var rootInterface = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;
        var rootImplementation = (await new ServiceImplementationGenerator().GenerateAsync(tool)).Single().Content;
        var subDomainInterface = (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath).Equals(
                "IToolRemoved.Generated.cs",
                StringComparison.Ordinal))
            .Content;
        var compatibilityOptionsAlias = (await new OptionsClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath).Equals(
                "ToolLegacyChildOptions.Generated.cs",
                StringComparison.Ordinal))
            .Content;

        using (Assert.Multiple())
        {
            await Assert.That(rootInterface)
                .Contains($"{obsoleteAttribute}{Environment.NewLine}    IToolRemoved Removed");
            await Assert.That(rootImplementation)
                .Contains($"{obsoleteAttribute}{Environment.NewLine}    public IToolRemoved Removed");
            await Assert.That(rootImplementation)
                .Contains(
                    "#pragma warning disable CS0618"
                    + $"{Environment.NewLine}        Removed = removed;"
                    + $"{Environment.NewLine}        #pragma warning restore CS0618");
            await Assert.That(subDomainInterface)
                .Contains($"{obsoleteAttribute}{Environment.NewLine}    public Task<CommandResult> ChildAsync");
            await Assert.That(compatibilityOptionsAlias).Contains(obsoleteAttribute);
        }
    }

    [Test]
    public async Task Live_SubDomain_Parent_Property_Is_Not_Obsolete()
    {
        var parent = Command(
            "ToolMixedOptions",
            "ToolOptions",
            ["mixed"]);
        var compatibilityChild = Command(
            "ToolMixedRemovedOptions",
            "ToolOptions",
            ["mixed", "removed"],
            subDomainGroup: "mixed") with
        {
            IsCompatibilityOnly = true,
        };
        var tool = Tool(parent, compatibilityChild);
        var obsoleteProperty =
            $"[Obsolete({GeneratorUtils.FormatStringLiteral(GeneratorUtils.CompatibilityOnlyObsoleteMessage)})]"
            + $"{Environment.NewLine}    IToolMixed Mixed";
        var rootInterface = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(rootInterface).DoesNotContain(obsoleteProperty);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Uses_Root_Identifier_For_Restored_Nested_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupChildOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"child\")] "
                + "public record ToolGroupChildOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupNestedChildOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"nested\", \"child\")] "
                + "public record ToolGroupNestedChildOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroup.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroup { "
                + "public Task ChildAsync(ToolGroupChildOptions? options = null) => Task.CompletedTask; }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupNested.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupNested { "
                + "public Task ChildAsync(ToolGroupNestedChildOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command(
                    "ToolGroupCurrentOptions",
                    "ToolOptions",
                    ["group", "current"],
                    subDomainGroup: "group",
                    commandGroupIdentifierOverride: "Group")),
                root);

            var restored = preserved.Commands
                .Where(command => command.ClassName != "ToolGroupCurrentOptions")
                .ToArray();
            using (Assert.Multiple())
            {
                await Assert.That(restored).Count().IsEqualTo(2);
                await Assert.That(restored.Select(command => command.SubDomainGroup!))
                    .IsEquivalentTo(["group", "group"]);
                await Assert.That(restored.Select(command => command.CommandGroupIdentifierOverride!))
                    .IsEquivalentTo(["Group", "Group"]);
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["group"]);
                await Assert.That(GeneratorUtils.GetSubDomainIdentifier(preserved, "group"))
                    .IsEqualTo("Group");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Custom_Root_Identifier_For_Restored_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolApplicationSetCreateOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"appset\", \"create\")] "
                + "public record ToolApplicationSetCreateOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolApplicationSet.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolApplicationSet { "
                + "public Task CreateAsync(ToolApplicationSetCreateOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command => command.SubDomainGroup == "ApplicationSet");
            using (Assert.Multiple())
            {
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("ApplicationSet");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["ApplicationSet"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Historical_Facade_Casing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolCloudShellScpOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"cloud-shell\", \"scp\")] "
                + "public record ToolCloudShellScpOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolCloudshell { "
                + "public Task ScpAsync(ToolCloudShellScpOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolCloudShellScpOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("Cloudshell");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Cloudshell");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["Cloudshell"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Historical_Facade_Casing_For_Live_Command()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAgentTaskCreateOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"agent-task\", \"create\")] "
                + "public record ToolAgentTaskCreateOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAgenttask.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolAgenttask { "
                + "public Task CreateAsync(ToolAgentTaskCreateOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolAgentTaskCreateOptions",
                "ToolOptions",
                ["agent-task", "create"],
                subDomainGroup: "agent-task") with
            {
                CommandPartIdentifierOverrides = new Dictionary<int, string>
                {
                    [0] = "AgentTask",
                },
            });

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var command = preserved.Commands.Single();
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath).Equals(
                    "ToolAgenttask.Generated.cs",
                    StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(command.CommandGroupIdentifierOverride).IsEqualTo("Agenttask");
                await Assert.That(command.CommandPartIdentifierOverrides[0]).IsEqualTo("AgentTask");
                await Assert.That(generated.Content).Contains("CreateAsync(");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Merges_Compatible_Facade_Casing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupCloudShellNestedOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"cloud-shell\", \"nested\")] "
                + "public record ToolGroupCloudShellNestedOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupCloudShellNestedChildOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"cloud-shell\", \"nested\", \"child\")] "
                + "public record ToolGroupCloudShellNestedChildOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupCloudshell { "
                + "public Task NestedAsync(ToolGroupCloudShellNestedOptions? options = null) => Task.CompletedTask; }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupCloudshellNested.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupCloudshellNested { "
                + "public Task ExecuteAsync(ToolGroupCloudShellNestedOptions? options = null) => Task.CompletedTask; "
                + "public Task ChildAsync(ToolGroupCloudShellNestedChildOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command =>
                command.ClassName == "ToolGroupCloudShellNestedOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.CommandPartIdentifierOverrides[1]).IsEqualTo("Cloudshell");
                await Assert.That(restored.CommandPartIdentifierOverrides[2]).IsEqualTo("Nested");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Applies_Restored_Casing_To_Live_Siblings()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupCloudShellOldOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"cloud-shell\", \"old\")] "
                + "public record ToolGroupCloudShellOldOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupCloudshell { "
                + "public Task OldAsync(ToolGroupCloudShellOldOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolGroupCloudShellCurrentOptions",
                "ToolOptions",
                ["group", "cloud-shell", "current"],
                subDomainGroup: "Group",
                commandGroupIdentifierOverride: "Group"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("ToolGroupCloudshell.Generated.cs", StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(generated.Content).Contains("OldAsync(");
                await Assert.That(generated.Content).Contains("ToolGroupCloudShellOldOptions? options = null");
                await Assert.That(generated.Content).Contains("CurrentAsync(");
                await Assert.That(generated.Content).Contains("ToolGroupCloudShellCurrentOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Applies_Restored_Parent_Casing_To_Live_Child()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupCloudShellOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"cloud-shell\")] "
                + "public record ToolGroupCloudShellOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupCloudshell { "
                + "public Task ExecuteAsync(ToolGroupCloudShellOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolGroupCloudShellCurrentOptions",
                "ToolOptions",
                ["group", "cloud-shell", "current"],
                subDomainGroup: "Group",
                commandGroupIdentifierOverride: "Group"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("ToolGroupCloudshell.Generated.cs", StringComparison.Ordinal));

            await Assert.That(generated.Content).Contains("ExecuteAsync(");
            await Assert.That(generated.Content).Contains("ToolGroupCloudShellOptions? options = null");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Recovers_Length_Changing_Nested_Identifier()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAdminUsersApplicationSetOldOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"admin\", \"users\", \"appset\", \"old\")] "
                + "public record ToolAdminUsersApplicationSetOldOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAdminUsersApplicationSet.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolAdminUsersApplicationSet { "
                + "public Task OldAsync(ToolAdminUsersApplicationSetOldOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command =>
                command.ClassName == "ToolAdminUsersApplicationSetOldOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.CommandPartIdentifierOverrides[1]).IsEqualTo("Users");
                await Assert.That(restored.CommandPartIdentifierOverrides[2]).IsEqualTo("ApplicationSet");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
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
    public async Task ApiCompatibilityPreserver_Preserves_Live_Group_Casing_For_Restored_Sibling()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolCloudShellScpOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"cloud-shell\", \"scp\")] "
                + "public record ToolCloudShellScpOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolCloudshell { "
                + "public Task ScpAsync(ToolCloudShellScpOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolCloudShellListOptions",
                "ToolOptions",
                ["cloud-shell", "list"],
                subDomainGroup: "Cloudshell"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolCloudShellScpOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("Cloudshell");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Cloudshell");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["Cloudshell"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Distinguishes_Literal_Execute_From_Parent_Facade()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolCloudShellExecuteOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"cloud-shell\", \"execute\")] "
                + "public record ToolCloudShellExecuteOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolCloudshell { "
                + "public Task ExecuteAsync(ToolCloudShellExecuteOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolCloudShellExecuteOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("Cloudshell");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Cloudshell");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["Cloudshell"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Literal_Execute_Overload_Beside_Parent()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupNestedExecuteOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"nested\", \"execute\")] "
                + "public record ToolGroupNestedExecuteOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupNested.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupNested { "
                + "public Task ExecuteAsync(ToolGroupNestedExecuteOptions? options = null) => Task.CompletedTask; }");

            var parent = Command(
                "ToolGroupNestedOptions",
                "ToolOptions",
                ["group", "nested"],
                subDomainGroup: "group");
            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(parent), root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath).Equals(
                    "ToolGroupNested.Generated.cs",
                    StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(generated).Contains("ExecuteCommandAsync(");
                await Assert.That(generated).Contains("ToolGroupNestedExecuteOptions? options = null");
                await Assert.That(generated).Contains("ToolGroupNestedOptions? options = null");
                await Assert.That(generated)
                    .Contains("[Obsolete(\"Use the current command facade instead.\")]");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Live_Literal_Execute_Overload_Beside_Parent()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupNestedExecuteOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"nested\", \"execute\")] "
                + "public record ToolGroupNestedExecuteOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupNested.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupNested { "
                + "public Task ExecuteAsync(ToolGroupNestedExecuteOptions? options = null) => Task.CompletedTask; }");

            var parent = Command(
                "ToolGroupNestedOptions",
                "ToolOptions",
                ["group", "nested"],
                subDomainGroup: "group");
            var literalExecute = Command(
                "ToolGroupNestedExecuteOptions",
                "ToolOptions",
                ["group", "nested", "execute"],
                subDomainGroup: "group");
            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(parent, literalExecute),
                root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath).Equals(
                    "ToolGroupNested.Generated.cs",
                    StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(generated).Contains("ExecuteCommandAsync(");
                await Assert.That(generated).Contains("ToolGroupNestedExecuteOptions? options = null");
                await Assert.That(generated).Contains("ToolGroupNestedOptions? options = null");
                await Assert.That(generated).Contains("Use ExecuteCommandAsync instead.");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Distinguishes_Execute_When_Parent_Name_Ends_In_Execute()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolRemoteExecuteExecuteOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"remote-execute\", \"execute\")] "
                + "public record ToolRemoteExecuteExecuteOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolRemoteExecute.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolRemoteExecute { "
                + "public Task ExecuteAsync(ToolRemoteExecuteExecuteOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolRemoteExecuteExecuteOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("RemoteExecute");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("RemoteExecute");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["RemoteExecute"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Removed_Option_Type_Without_Facade()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolLegacyOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"legacy\")] "
                + "public record ToolLegacyOptions : ToolOptions;");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.Commands.Select(static command => command.ClassName))
                .IsEquivalentTo(["ToolCurrentOptions", "ToolLegacyOptions"]);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Reads_Legacy_Generated_File_Names()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        var optionsDirectory = Path.Combine(packageDirectory, "Options");
        var servicesDirectory = Path.Combine(packageDirectory, "Services");
        Directory.CreateDirectory(optionsDirectory);
        Directory.CreateDirectory(servicesDirectory);
        try
        {
            const string generatedHeader = "// <auto-generated>\n// </auto-generated>\n";
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolAuditOptions.cs"),
                generatedHeader
                + "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"audit\")] public record ToolAuditOptions : ToolOptions { "
                + "[CliFlag(\"--eval-all\")] public bool? EvalAll { get; set; } }");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolRemovedOptions.cs"),
                generatedHeader
                + "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"removed\")] public record ToolRemovedOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(servicesDirectory, "ITool.cs"),
                generatedHeader
                + "namespace ModularPipelines.Tool.Services; public interface ITool { "
                + "Task RemovedAsync(ToolRemovedOptions? options = null); }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolAuditOptions", "ToolOptions", ["audit"])) with
                {
                    GenerateCommandFacade = true,
                },
                root);
            var audit = preserved.Commands.Single(command => command.ClassName == "ToolAuditOptions");
            var removed = preserved.Commands.Single(command => command.ClassName == "ToolRemovedOptions");

            using (Assert.Multiple())
            {
                await Assert.That(audit.CompatibilityProperties.Select(static option => option.PropertyName))
                    .Contains("EvalAll");
                await Assert.That(removed.PreserveNamedFacade).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Ignores_Handwritten_Legacy_File_Names()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolManualOptions.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"manual\")] public record ToolManualOptions : ToolOptions;");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.Commands).HasSingleItem();
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    [Arguments("// <auto-generated/>")]
    [Arguments("// <AUTO-GENERATED />")]
    public async Task ApiCompatibilityPreserver_Reads_SelfClosing_Generated_Markers(string marker)
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolLegacyOptions.cs"),
                marker + "\nusing ModularPipelines.Attributes; "
                + "[CliSubCommand(\"legacy\")] public record ToolLegacyOptions : ToolOptions;");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.Commands.Select(static command => command.ClassName))
                .Contains("ToolLegacyOptions");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Distinguishes_Public_Default_From_Private_Interface_Helpers()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        var optionsDirectory = Path.Combine(packageDirectory, "Options");
        var servicesDirectory = Path.Combine(packageDirectory, "Services");
        Directory.CreateDirectory(optionsDirectory);
        Directory.CreateDirectory(servicesDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolPrivateOptions.Generated.cs"),
                "using ModularPipelines.Attributes; [CliSubCommand(\"private\")] "
                + "public record ToolPrivateOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolDefaultOptions.Generated.cs"),
                "using ModularPipelines.Attributes; [CliSubCommand(\"default\")] "
                + "public record ToolDefaultOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(servicesDirectory, "ITool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public interface ITool { "
                + "private Task PrivateAsync(ToolPrivateOptions? options = null) => Task.CompletedTask; "
                + "Task DefaultAsync(ToolDefaultOptions? options = null) => Task.CompletedTask; }");

            var tool = Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])) with
            {
                GenerateCommandFacade = true,
            };
            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            using (Assert.Multiple())
            {
                await Assert.That(preserved.Commands.Single(command =>
                        command.ClassName == "ToolPrivateOptions").PreserveNamedFacade)
                    .IsFalse();
                await Assert.That(preserved.Commands.Single(command =>
                        command.ClassName == "ToolDefaultOptions").PreserveNamedFacade)
                    .IsTrue();
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Maps_Restored_Identifier_To_Live_Group_Key()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAlphaOldOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"foo\", \"old\")] "
                + "public record ToolAlphaOldOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAlpha.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolAlpha { "
                + "public Task OldAsync(ToolAlphaOldOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(
                Command(
                    "ToolAlphaCurrentOptions",
                    "ToolOptions",
                    ["foo", "current"],
                    subDomainGroup: "foo",
                    commandGroupIdentifierOverride: "Alpha"),
                Command(
                    "ToolBetaCurrentOptions",
                    "ToolOptions",
                    ["foo", "beta"],
                    subDomainGroup: "beta-key",
                    commandGroupIdentifierOverride: "Beta"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolAlphaOldOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("foo");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Alpha");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["foo", "beta-key"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Missing_Group_Alongside_Other_Live_Group()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAlphaOldOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"foo\", \"old\")] "
                + "public record ToolAlphaOldOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAlpha.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolAlpha { "
                + "public Task OldAsync(ToolAlphaOldOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolBetaCurrentOptions",
                "ToolOptions",
                ["foo", "beta"],
                subDomainGroup: "beta-key",
                commandGroupIdentifierOverride: "Beta"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolAlphaOldOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("Alpha");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Alpha");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["beta-key", "Alpha"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Infers_Root_Through_Historical_Intermediate_Casing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAlphaCloudShellOldOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"foo\", \"cloud-shell\", \"old\")] "
                + "public record ToolAlphaCloudShellOldOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAlphaCloudshell.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolAlphaCloudshell { "
                + "public Task OldAsync(ToolAlphaCloudShellOldOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolBetaCurrentOptions",
                "ToolOptions",
                ["foo", "beta"],
                subDomainGroup: "beta-key",
                commandGroupIdentifierOverride: "Beta"));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            var restored = preserved.Commands.Single(command => command.ClassName == "ToolAlphaCloudShellOldOptions");
            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsEqualTo("Alpha");
                await Assert.That(restored.CommandGroupIdentifierOverride).IsEqualTo("Alpha");
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["beta-key", "Alpha"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Uses_One_Root_For_Removed_Parent_And_Child()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupNestedOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"nested\")] "
                + "public record ToolGroupNestedOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupNestedChildOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"group\", \"nested\", \"child\")] "
                + "public record ToolGroupNestedChildOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroupNested.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroupNested { "
                + "public Task ExecuteAsync(ToolGroupNestedOptions? options = null) => Task.CompletedTask; "
                + "public Task ChildAsync(ToolGroupNestedChildOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            var restored = preserved.Commands
                .Where(command => command.ClassName != "ToolCurrentOptions")
                .ToArray();
            using (Assert.Multiple())
            {
                await Assert.That(restored).Count().IsEqualTo(2);
                await Assert.That(restored.Select(command => command.SubDomainGroup!))
                    .IsEquivalentTo(["Group", "Group"]);
                await Assert.That(restored.Select(command => command.CommandGroupIdentifierOverride!))
                    .IsEquivalentTo(["Group", "Group"]);
                await Assert.That(preserved.SubDomainGroups).IsEquivalentTo(["Group"]);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Optional_Facade_When_Required_Member_Is_Added()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAddOptions.Generated.cs"),
                "public record ToolAddOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public Task AddAsync(ToolAddOptions? options = null) => Task.CompletedTask; }");
            var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Package",
                        CSharpType = "string",
                        IsRequired = true,
                        PositionIndex = 0,
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var options = (await new OptionsClassGenerator().GenerateAsync(preserved)).Single().Content;
            var service = (await new ServiceImplementationGenerator().GenerateAsync(preserved)).Single().Content;

            using (Assert.Multiple())
            {
                await Assert.That(options).Contains("public ToolAddOptions()");
                await Assert.That(service).Contains("ToolAddOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Leaf_Facade_When_It_Gains_Children()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupChildOptions.Generated.cs"),
                "public record ToolGroupChildOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroup.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolGroup { public Task ChildAsync(ToolGroupChildOptions? options = null) => Task.CompletedTask; }");
            var child = Command(
                "ToolGroupChildOptions",
                "ToolOptions",
                ["group", "child"],
                subDomainGroup: "group");
            var grandchild = Command(
                "ToolGroupChildSubOptions",
                "ToolOptions",
                ["group", "child", "sub"],
                subDomainGroup: "group");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(child, grandchild),
                root);
            var generated = await new SubDomainClassGenerator().GenerateAsync(preserved);
            var groupFacade = generated.Single(file => Path.GetFileName(file.RelativePath)
                .Equals("ToolGroup.Generated.cs", StringComparison.Ordinal));
            var childFacade = generated.Single(file => Path.GetFileName(file.RelativePath)
                .Equals("ToolGroupChild.Generated.cs", StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(groupFacade.Content)
                    .Contains("Task<CommandResult> ChildAsync(");
                await Assert.That(childFacade.Content)
                    .Contains("Task<CommandResult> ExecuteAsync(");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Identifier_Casing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Enums"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupKubeconfigOptions.Generated.cs"),
                "public record ToolGroupKubeconfigOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroup.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolGroup { public Task KubeconfigAsync(ToolGroupKubeconfigOptions? options = null) => Task.CompletedTask; }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Enums", "ToolGroupKubeconfigLogformat.Generated.cs"),
                "public enum ToolGroupKubeconfigLogformat { [EnumValue(\"text\")] Text }");
            var enumDefinition = new CliEnumDefinition
            {
                EnumName = "ToolGroupKubeConfigLogformat",
                Values = [new CliEnumValue { MemberName = "Text", CliValue = "text" }],
            };
            var command = Command(
                "ToolGroupKubeConfigOptions",
                "ToolOptions",
                ["group", "kubeconfig"],
                subDomainGroup: "group",
                enums: [enumDefinition],
                options:
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--logformat",
                        PropertyName = "Logformat",
                        CSharpType = "ToolGroupKubeConfigLogformat?",
                        EnumDefinition = enumDefinition,
                    },
                    new CliOptionDefinition
                    {
                        SwitchName = "--detached-logformat",
                        PropertyName = "DetachedLogformat",
                        CSharpType = "ToolGroupKubeConfigLogformat?",
                    },
                ]) with
            {
                CompatibilityProperties =
                [
                    new CliCompatibilityProperty
                    {
                        PropertyName = "LegacyLogformat",
                        CSharpType = "ToolGroupKubeConfigLogformat?",
                        ObsoleteMessage = "Legacy output format.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root);
            var preservedCommand = preserved.Commands.Single();
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath)
                    .Equals("ToolGroup.Generated.cs", StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(preservedCommand.ClassName)
                    .IsEqualTo("ToolGroupKubeconfigOptions");
                await Assert.That(preservedCommand.Options.Single(option =>
                        option.PropertyName.Equals("Logformat", StringComparison.Ordinal)).EnumDefinition!.EnumName)
                    .IsEqualTo("ToolGroupKubeconfigLogformat");
                await Assert.That(preservedCommand.Options.Single(option =>
                        option.PropertyName.Equals("Logformat", StringComparison.Ordinal)).CSharpType)
                    .IsEqualTo("ToolGroupKubeconfigLogformat?");
                await Assert.That(preservedCommand.Options.Single(option =>
                        option.PropertyName.Equals("DetachedLogformat", StringComparison.Ordinal)).CSharpType)
                    .IsEqualTo("ToolGroupKubeconfigLogformat?");
                await Assert.That(preservedCommand.CompatibilityProperties.Single().CSharpType)
                    .IsEqualTo("ToolGroupKubeconfigLogformat?");
                await Assert.That(preserved.AllEnums.Select(static definition => definition.EnumName))
                    .IsEquivalentTo(["ToolGroupKubeconfigLogformat"]);
                await Assert.That(generated)
                    .Contains("KubeConfigAsync(");
                await Assert.That(generated)
                    .Contains("KubeconfigAsync(");
                await Assert.That(generated)
                    .Contains("ToolGroupKubeconfigOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task EnumReachabilityPruner_Omits_Unreferenced_Command_Enums_But_Keeps_Compatibility_Enums()
    {
        static CliEnumDefinition EnumDefinition(string name) => new()
        {
            EnumName = name,
            Values = [new CliEnumValue { MemberName = "Value", CliValue = "value" }],
        };

        var orphan = EnumDefinition("ToolOrphanMode");
        var used = EnumDefinition("ToolUsedMode");
        var explicitMetadata = EnumDefinition("ToolExplicitMetadataMode");
        var compatibility = EnumDefinition("ToolCompatibilityMode");
        var beforeCommand = Command(
            "ToolRunOptions",
            "ToolOptions",
            ["run"],
            enums: [orphan, used, explicitMetadata],
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--orphan-mode",
                    PropertyName = "OrphanMode",
                    CSharpType = "ToolOrphanMode?",
                    EnumDefinition = orphan,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--used-mode",
                    PropertyName = "UsedMode",
                    CSharpType = "ToolUsedMode?",
                    EnumDefinition = used,
                },
            ]);
        var afterCommand = beforeCommand with
        {
            Options = [.. beforeCommand.Options.Select(option =>
                option.PropertyName == "OrphanMode"
                    ? option with { CSharpType = "string?", EnumDefinition = null }
                    : option)],
        };
        var before = Tool(beforeCommand) with { CompatibilityEnums = [compatibility] };
        var after = Tool(afterCommand) with { CompatibilityEnums = [compatibility] };

        var pruned = EnumReachabilityPruner.PruneDiscardedEnumReferences(before, after);
        var generated = await new EnumGenerator().GenerateAsync(pruned);

        using (Assert.Multiple())
        {
            await Assert.That(pruned.AllEnums.Select(definition => definition.EnumName))
                .IsEquivalentTo([
                    "ToolUsedMode",
                    "ToolExplicitMetadataMode",
                    "ToolCompatibilityMode",
                ]);
            await Assert.That(generated.Select(file => Path.GetFileName(file.RelativePath)))
                .IsEquivalentTo([
                    "ToolUsedMode.Generated.cs",
                    "ToolExplicitMetadataMode.Generated.cs",
                    "ToolCompatibilityMode.Generated.cs",
                ]);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Prefers_Historical_Facade_Group_Casing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAccessApprovalRequestsApproveOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"access-approval\", \"requests\", \"approve\")] "
                + "public record ToolAccessApprovalRequestsApproveOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAccessapprovalRequests.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolAccessapprovalRequests { "
                + "public Task ApproveAsync(ToolAccessApprovalRequestsApproveOptions? options = null) "
                + "=> Task.CompletedTask; }");
            var current = Command(
                "ToolAccessApprovalRequestsApproveOptions",
                "ToolOptions",
                ["access-approval", "requests", "approve"],
                subDomainGroup: "access-approval",
                commandGroupIdentifierOverride: "AccessApproval");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var command = preserved.Commands.Single();
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath)
                    .Equals("ToolAccessapprovalRequests.Generated.cs", StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(command.CommandGroupIdentifierOverride).IsEqualTo("Accessapproval");
                await Assert.That(generated.Content).Contains("ApproveAsync(");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Prefers_Historical_Facade_Group_Casing_For_Normalized_Subdomains()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAccessApprovalRequestsOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"access-approval\", \"requests\")] "
                + "public record ToolAccessApprovalRequestsOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAccessApprovalRequestsApproveOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"access-approval\", \"requests\", \"approve\")] "
                + "public record ToolAccessApprovalRequestsApproveOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolAccessapprovalRequests.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolAccessapprovalRequests { "
                + "public Task ApproveAsync(ToolAccessApprovalRequestsApproveOptions? options = null) "
                + "=> Task.CompletedTask; }");
            var parent = Command(
                "ToolAccessApprovalRequestsOptions",
                "ToolOptions",
                ["access-approval", "requests"],
                subDomainGroup: "AccessApproval");
            var current = Command(
                "ToolAccessApprovalRequestsApproveOptions",
                "ToolOptions",
                ["access-approval", "requests", "approve"],
                subDomainGroup: "AccessApproval");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(parent, current), root);
            var generated = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath)
                    .Equals("ToolAccessapprovalRequests.Generated.cs", StringComparison.Ordinal));

            using (Assert.Multiple())
            {
                await Assert.That(preserved.Commands.Select(command => command.CommandGroupIdentifierOverride!))
                    .IsEquivalentTo(["Accessapproval", "Accessapproval"]);
                await Assert.That(generated.Content).Contains("ApproveAsync(");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Does_Not_Inherit_Group_Identifier_From_Sibling_Branch()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAccessApprovalRequestsApproveOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"access-approval\", \"requests\", \"approve\")] "
                + "public record ToolAccessApprovalRequestsApproveOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolAccessApprovalSettingsOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"access-approval\", \"settings\")] "
                + "public record ToolAccessApprovalSettingsOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolLegacyRequests.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolLegacyRequests { "
                + "public Task ApproveAsync(ToolAccessApprovalRequestsApproveOptions? options = null) "
                + "=> Task.CompletedTask; }");
            var settings = Command(
                "ToolAccessApprovalSettingsOptions",
                "ToolOptions",
                ["access-approval", "settings"],
                subDomainGroup: "AccessApproval");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(settings), root);
            var preservedSettings = preserved.Commands.Single(command =>
                command.CommandParts.SequenceEqual(["access-approval", "settings"]));

            await Assert.That(preservedSettings.CommandGroupIdentifierOverride).IsEqualTo("AccessApproval");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Ignores_Other_Tool_Facades_In_Shared_Package()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolCurrentOptions.Generated.cs"),
                "public record ToolCurrentOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public Task CurrentAsync(ToolCurrentOptions? options = null) => Task.CompletedTask; }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Other.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Other { public Task RemovedAsync(OtherRemovedOptions? options = null) => Task.CompletedTask; }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.Commands).HasSingleItem();
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    [Arguments("Toolbox")]
    [Arguments("IToolbox")]
    [Arguments("ToolBox")]
    [Arguments("IToolBox")]
    public async Task ApiCompatibilityPreserver_Ignores_Overlapping_Tool_Name_Facades(
        string otherTypeName)
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            var otherPrefix = otherTypeName.StartsWith('I') ? otherTypeName[1..] : otherTypeName;
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", $"{otherPrefix}Options.Generated.cs"),
                $"public record {otherPrefix}Options;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", $"{otherPrefix}LegacyOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + $"[CliSubCommand(\"legacy\")] public record {otherPrefix}LegacyOptions : {otherPrefix}Options;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", $"{otherTypeName}.Generated.cs"),
                $"namespace ModularPipelines.Tool.Services; public class {otherTypeName} {{ "
                + $"public Task LegacyAsync({otherPrefix}LegacyOptions? options = null) => Task.CompletedTask; }}");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.Commands).HasSingleItem();
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Ignores_Overlapping_Tool_Name_Enums()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Enums"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolBoxOptions.cs"),
                "// <auto-generated />\npublic record ToolBoxOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolBoxLegacyOptions.cs"),
                "// <auto-generated />\npublic record ToolBoxLegacyOptions : ToolBoxOptions { public ToolBoxMode? Mode { get; init; } }");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Enums", "ToolBoxMode.cs"),
                "// <auto-generated />\npublic enum ToolBoxMode { First }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                root);

            await Assert.That(preserved.CompatibilityEnums).IsEmpty();
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Accepts_Current_Command_Group_Alias_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolBuilderBakeOptions.Generated.cs"),
                "public record ToolBuilderBakeOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolBuilder.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolBuilder { public Task BakeAsync(ToolBuilderBakeOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolBuildxBakeOptions",
                "ToolOptions",
                ["buildx", "bake"],
                subDomainGroup: "Buildx")) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use buildx instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            await Assert.That(preserved.Commands).HasSingleItem();
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Alias_Constructors_For_New_Required_Members()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuildxBakeOptions.Generated.cs"),
                "public record ToolBuildxBakeOptions([property: CliOption(\"--source\")] string Source);");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuilderBakeOptions.Generated.cs"),
                "public record ToolBuilderBakeOptions : ToolBuildxBakeOptions "
                + "{ public ToolBuilderBakeOptions(string Source) : base(Source) { } }");
            var command = Command(
                "ToolBuildxBakeOptions",
                "ToolOptions",
                ["buildx", "bake"],
                subDomainGroup: "Buildx") with
            {
                Options =
                [
                    RequiredOption("--source", "Source"),
                    RequiredOption("--destination", "Destination"),
                ],
            };
            var tool = Tool(command) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use buildx instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var constructor = preserved.Commands.Single()
                .AliasCompatibilityConstructors["ToolBuilderBakeOptions"]
                .Single();

            await Assert.That(constructor.Parameters.Select(static parameter => parameter.PropertyName))
                .IsEquivalentTo(["Source"]);
            await Assert.That(constructor.PrimaryConstructorArguments.Count).IsEqualTo(2);
            await Assert.That(constructor.PrimaryConstructorArguments[0]).IsEqualTo("Source");
            await Assert.That(constructor.PrimaryConstructorArguments[1]).IsEqualTo("default(string)!");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Parameterless_Alias_Constructor_For_New_Required_Members()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuildxBakeOptions.Generated.cs"),
                "public record ToolBuildxBakeOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuilderBakeOptions.Generated.cs"),
                "public record ToolBuilderBakeOptions : ToolBuildxBakeOptions;");
            var command = Command(
                "ToolBuildxBakeOptions",
                "ToolOptions",
                ["buildx", "bake"],
                subDomainGroup: "Buildx") with
            {
                Options = [RequiredOption("--source", "Source")],
            };
            var tool = Tool(command) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use buildx instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var constructor = preserved.Commands.Single()
                .AliasCompatibilityConstructors["ToolBuilderBakeOptions"]
                .Single();

            await Assert.That(constructor.Parameters).IsEmpty();
            await Assert.That(constructor.PrimaryConstructorArguments)
                .IsEquivalentTo(["default(string)!"]);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Command_Group_Alias_Enum_Properties()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        var optionsDirectory = Path.Combine(packageDirectory, "Options");
        var enumsDirectory = Path.Combine(packageDirectory, "Enums");
        Directory.CreateDirectory(optionsDirectory);
        Directory.CreateDirectory(enumsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuildxBakeOptions.Generated.cs"),
                "public record ToolBuildxBakeOptions { "
                + "[CliOption(\"--progress\")] public ToolBuildxBakeProgress? Progress { get; set; } }");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuilderBakeOptions.Generated.cs"),
                "public record ToolBuilderBakeOptions : ToolBuildxBakeOptions { "
                + "[CliOption(\"--progress\")] public new ToolBuilderBakeProgress? Progress { get; set; } }");
            await File.WriteAllTextAsync(
                Path.Combine(enumsDirectory, "ToolBuildxBakeProgress.Generated.cs"),
                "public enum ToolBuildxBakeProgress { [EnumValue(\"plain\")] Plain = 4 }");
            await File.WriteAllTextAsync(
                Path.Combine(enumsDirectory, "ToolBuilderBakeProgress.Generated.cs"),
                "public enum ToolBuilderBakeProgress { [EnumValue(\"plain\")] Plain = 4 }");
            var enumDefinition = new CliEnumDefinition
            {
                EnumName = "ToolBuildxBakeProgress",
                Values =
                [
                    new CliEnumValue { MemberName = "Plain", CliValue = "plain" },
                    new CliEnumValue { MemberName = "Tty", CliValue = "tty" },
                ],
            };
            var command = Command(
                "ToolBuildxBakeOptions",
                "ToolOptions",
                ["buildx", "bake"],
                subDomainGroup: "Buildx",
                options:
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--progress",
                        PropertyName = "Progress",
                        CSharpType = "ToolBuildxBakeProgress?",
                        EnumDefinition = enumDefinition,
                    },
                ]);
            var tool = Tool(command) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use buildx instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var generatedOptions = await new OptionsClassGenerator().GenerateAsync(preserved);
            var generatedAlias = generatedOptions.Single(file => Path.GetFileName(file.RelativePath)
                .Equals("ToolBuilderBakeOptions.Generated.cs", StringComparison.Ordinal)).Content;
            var generatedCanonical = generatedOptions.Single(file => Path.GetFileName(file.RelativePath)
                .Equals("ToolBuildxBakeOptions.Generated.cs", StringComparison.Ordinal)).Content;
            var generatedEnums = await new EnumGenerator().GenerateAsync(preserved);

            using (Assert.Multiple())
            {
                await Assert.That(generatedCanonical)
                    .Contains("public ToolBuildxBakeProgress? Progress { get; set; }");
                await Assert.That(generatedAlias)
                    .Contains("public new ToolBuilderBakeProgress? Progress");
                await Assert.That(generatedAlias)
                    .Contains("(ToolBuildxBakeProgress)(int)value.Value");
                await Assert.That(generatedEnums.Select(file => Path.GetFileName(file.RelativePath)))
                    .Contains("ToolBuildxBakeProgress.Generated.cs");
                await Assert.That(generatedEnums.Select(file => Path.GetFileName(file.RelativePath)))
                    .Contains("ToolBuilderBakeProgress.Generated.cs");
                await Assert.That(generatedEnums.Single(file => Path.GetFileName(file.RelativePath)
                        .Equals("ToolBuilderBakeProgress.Generated.cs", StringComparison.Ordinal)).Content)
                    .Contains("Tty");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Command_Group_Alias_NonEnum_Properties()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuildxBakeOptions.Generated.cs"),
                "public record ToolBuildxBakeOptions { "
                + "[CliOption(\"--label\")] public string Label { get; init; } }");
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolBuilderBakeOptions.Generated.cs"),
                "public record ToolBuilderBakeOptions : ToolBuildxBakeOptions { "
                + "[CliOption(\"--label\")] public new string Label { get; set; } }");
            var command = Command(
                "ToolBuildxBakeOptions",
                "ToolOptions",
                ["buildx", "bake"],
                subDomainGroup: "Buildx",
                options:
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--label",
                        PropertyName = "Label",
                        CSharpType = "string",
                    },
                ]);
            var tool = Tool(command) with
            {
                CommandGroupAliases =
                [
                    new CliCommandGroupAlias
                    {
                        Alias = "builder",
                        CanonicalCommand = "buildx",
                        ObsoleteMessage = "Use buildx instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var generatedAlias = (await new OptionsClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath)
                    .Equals("ToolBuilderBakeOptions.Generated.cs", StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(generatedAlias).Contains("public new string Label");
                await Assert.That(generatedAlias).Contains("get => base.Label;");
                await Assert.That(generatedAlias).Contains("init => base.Label = value;");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Scopes_Enum_Baselines_To_The_Current_Tool()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        var optionsDirectory = Path.Combine(packageDirectory, "Options");
        var enumsDirectory = Path.Combine(packageDirectory, "Enums");
        Directory.CreateDirectory(optionsDirectory);
        Directory.CreateDirectory(enumsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolRunOptions.Generated.cs"),
                "public record ToolRunOptions : ToolOptions { public ToolLegacy? Mode { get; init; } }");
            await File.WriteAllTextAsync(
                Path.Combine(enumsDirectory, "ToolLegacy.Generated.cs"),
                "public enum ToolLegacy { Value }");
            await File.WriteAllTextAsync(
                Path.Combine(enumsDirectory, "OtherLegacy.Generated.cs"),
                "public enum OtherLegacy { Value }");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                Tool(Command("ToolRunOptions", "ToolOptions", ["run"])),
                root);

            await Assert.That(preserved.CompatibilityEnums.Select(static definition => definition.EnumName))
                .IsEquivalentTo(["ToolLegacy"]);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Forwarding_To_Global_Option()
    {
        var root = Path.Combine(Path.GetTempPath(), $"options-api-{Guid.NewGuid():N}");
        var optionsDirectory = Path.Combine(root, "src", "ModularPipelines.Tool", "Options");
        Directory.CreateDirectory(optionsDirectory);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(optionsDirectory, "ToolRunOptions.Generated.cs"),
                "public record ToolRunOptions;");
            var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
            {
                CompatibilityProperties =
                [
                    new CliCompatibilityProperty
                    {
                        PropertyName = "LegacyEndpoint",
                        CSharpType = "string?",
                        ForwardToPropertyName = "Endpoint",
                        ObsoleteMessage = "Use Endpoint instead.",
                    },
                ],
            };
            var tool = Tool(command) with
            {
                GlobalOptions =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--endpoint",
                        PropertyName = "NewEndpoint",
                        CSharpType = "string?",
                    },
                ],
                GlobalCompatibilityProperties =
                [
                    new CliCompatibilityProperty
                    {
                        PropertyName = "Endpoint",
                        CSharpType = "string?",
                        ForwardToPropertyName = "NewEndpoint",
                        ObsoleteMessage = "Use NewEndpoint instead.",
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);
            var alias = preserved.Commands.Single().CompatibilityProperties.Single();

            await Assert.That(alias.ForwardToPropertyName).IsEqualTo("NewEndpoint");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task OptionsClassGenerator_Rejects_Alias_Enum_Nullability_Changes()
    {
        const string aliasClassName = "ToolBuilderBakeOptions";
        var command = Command(
            "ToolBuildxBakeOptions",
            "ToolOptions",
            ["buildx", "bake"],
            subDomainGroup: "Buildx") with
        {
            AliasCompatibilityProperties = new Dictionary<string, IReadOnlyList<CliAliasCompatibilityProperty>>
            {
                [aliasClassName] =
                [
                    new CliAliasCompatibilityProperty
                    {
                        PropertyName = "Progress",
                        AliasCSharpType = "ToolBuilderBakeProgress?",
                        CanonicalCSharpType = "ToolBuildxBakeProgress",
                        ObsoleteMessage = "Use the canonical property instead.",
                    },
                ],
            },
        };
        var tool = Tool(command) with
        {
            CommandGroupAliases =
            [
                new CliCommandGroupAlias
                {
                    Alias = "builder",
                    CanonicalCommand = "buildx",
                    ObsoleteMessage = "Use buildx instead.",
                },
            ],
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new OptionsClassGenerator().GenerateAsync(tool));

        await Assert.That(exception.Message)
            .Contains("alias property Progress because its nullability changed");
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rekeys_Documentation_Examples_After_Required_Rename()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "CurrentName",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
            DocumentationExampleValues = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["CurrentName"] = "\"example\"",
            },
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("StableName", "string", argumentPosition: 0, isRequired: true)]);

        await Assert.That(preserved.DocumentationExampleValues.Keys).IsEquivalentTo(["StableName"]);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Resolves_Required_Rename_Collisions()
    {
        var command = Command("ToolAddOptions", "ToolOptions", ["add"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "CurrentName",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                },
            ],
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--stable-name",
                    PropertyName = "StableName",
                    CSharpType = "string?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("StableName", "string", argumentPosition: 0, isRequired: true)]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.PositionalArguments.Single().PropertyName)
                .IsEqualTo("StableName");
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("StableNameOption");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Removed_Flat_Nested_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolSourceEditOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"source\", \"edit\")] "
                + "public record ToolSourceEditOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class Tool { "
                + "public Task SourceEditAsync(ToolSourceEditOptions? options = null) => Task.CompletedTask; }");
            var current = Command(
                "ToolSourceListOptions",
                "ToolOptions",
                ["source", "list"],
                subDomainGroup: "source");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var generated = (await new ServiceInterfaceGenerator().GenerateAsync(preserved))
                .Single().Content;

            using (Assert.Multiple())
            {
                await Assert.That(preserved.Commands.Select(static command => command.ClassName))
                    .Contains("ToolSourceEditOptions");
                await Assert.That(generated)
                    .Contains("SourceEditAsync(ToolSourceEditOptions? options = null");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Leaves_Nested_Named_Facades_Nested()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolSourceEditOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"source\", \"edit\")] "
                + "public record ToolSourceEditOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolSource.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolSource { "
                + "public Task EditAsync(ToolSourceEditOptions? options = null) => Task.CompletedTask; }");
            var current = Command(
                "ToolSourceEditOptions",
                "ToolOptions",
                ["source", "edit"],
                subDomainGroup: "source");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var rootFacade = (await new ServiceInterfaceGenerator().GenerateAsync(preserved))
                .Single().Content;
            var nestedFacade = (await new SubDomainClassGenerator().GenerateAsync(preserved))
                .Single(file => Path.GetFileName(file.RelativePath)
                    .Equals("ToolSource.Generated.cs", StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(rootFacade).DoesNotContain("SourceEditAsync(");
                await Assert.That(nestedFacade).Contains("EditAsync(");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Live_Class_Name_Collisions()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolOldAOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"a\")] "
                + "public record ToolOldAOptions : ToolOptions;");
            var tool = Tool(
                Command("ToolNewAOptions", "ToolOptions", ["a"]),
                Command("ToolOldAOptions", "ToolOptions", ["b"]));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                Task.FromResult(GeneratedApiCompatibilityPreserver.Preserve(tool, root)));

            await Assert.That(exception!.Message).Contains("ToolOldAOptions (tool, tool)");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Live_Command_Class_Name_By_Command_Path()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolBundleOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"bundle\")] "
                + "public record ToolBundleOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class Tool { "
                + "public Task BundleAsync(ToolBundleOptions? options = null) => Task.CompletedTask; }");
            var tool = Tool(Command(
                "ToolBundleBundleOptions",
                "ToolOptions",
                ["bundle"]));

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(tool, root);

            await Assert.That(preserved.Commands).HasSingleItem();
            await Assert.That(preserved.Commands.Single().ClassName).IsEqualTo("ToolBundleOptions");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Unique_Command_Path_Across_Parent_Changes()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolOldGroupOptions.Generated.cs"),
                "using ModularPipelines.Attributes; [CliSubCommand(\"group\")] "
                + "public record ToolOldGroupOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolOldGroupRunOptions.Generated.cs"),
                "using ModularPipelines.Attributes; [CliSubCommand(\"group\", \"run\")] "
                + "public record ToolOldGroupRunOptions : ToolOldGroupOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroup.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolGroup { "
                + "public Task RunAsync(ToolOldGroupRunOptions? options = null) => Task.CompletedTask; }");
            var current = Command(
                "ToolNewGroupRunOptions",
                "ToolNewGroupOptions",
                ["group", "run"],
                subDomainGroup: "group");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var command = preserved.Commands.Single(candidate => candidate.CommandParts.Length == 2);

            using (Assert.Multiple())
            {
                await Assert.That(command.ClassName).IsEqualTo("ToolOldGroupRunOptions");
                await Assert.That(command.ParentClassName).IsEqualTo("ToolOldGroupOptions");
                await Assert.That(command.PreserveNamedFacade).IsTrue();
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Distinguishes_Positional_Argument_Phases()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Prefix",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                    Phase = CommandLinePhase.Passthrough,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty(
                    "CommandOptions",
                    "IEnumerable<string>?",
                    argumentPosition: 0,
                    phase: CommandLinePhase.EarlyOperand),
            ]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Prefix");
            await Assert.That(preserved.CompatibilityProperties.Single().PropertyName)
                .IsEqualTo("CommandOptions");
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardToPropertyName)
                .IsNull();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Removed_Nested_Root_Facade()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolPinInstalled_formulaOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"pin\", \"installed_formula\")] "
                + "public record ToolPinInstalled_formulaOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class Tool { "
                + "public Task PinInstalled_formulaAsync(ToolPinInstalled_formulaOptions? options = null) "
                + "=> Task.CompletedTask; }");
            var current = Command("ToolPinOptions", "ToolOptions", ["pin"]) with
            {
                PositionalArguments =
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "InstalledFormula",
                        CSharpType = "IEnumerable<string>",
                        IsRequired = true,
                        PositionIndex = 0,
                        Phase = CommandLinePhase.Passthrough,
                    },
                ],
            };

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(current), root);
            var restored = preserved.Commands.Single(command =>
                command.ClassName == "ToolPinInstalled_formulaOptions");
            var generated = (await new ServiceImplementationGenerator().GenerateAsync(preserved))
                .Single(file => file.RelativePath.EndsWith("Tool.Generated.cs", StringComparison.Ordinal))
                .Content;

            using (Assert.Multiple())
            {
                await Assert.That(restored.SubDomainGroup).IsNull();
                await Assert.That(restored.PreserveRootNamedFacade).IsTrue();
                await Assert.That(generated).Contains("PinInstalled_formulaAsync(");
                await Assert.That(generated)
                    .Contains("ToolPinInstalled_formulaOptions? options = null");
                await Assert.That(generated).Contains("[Obsolete(\"Use PinAsync instead.\")]");
                await Assert.That(generated)
                    .DoesNotContain("[Obsolete(\"Use PinInstalledFormulaAsync instead.\")]");
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Keeps_Literal_Execute_When_It_Gains_Children()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolRemoteExecuteOptions.Generated.cs"),
                "using ModularPipelines.Attributes; "
                + "[CliSubCommand(\"remote\", \"execute\")] "
                + "public record ToolRemoteExecuteOptions : ToolOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolRemote.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; public class ToolRemote { "
                + "public Task ExecuteAsync(ToolRemoteExecuteOptions? options = null) => Task.CompletedTask; }");
            var execute = Command(
                "ToolRemoteExecuteOptions",
                "ToolOptions",
                ["remote", "execute"],
                subDomainGroup: "Remote");
            var nested = Command(
                "ToolRemoteExecuteNestedOptions",
                "ToolOptions",
                ["remote", "execute", "nested"],
                subDomainGroup: "Remote");

            var preserved = GeneratedApiCompatibilityPreserver.Preserve(Tool(execute, nested), root);
            var literalExecute = preserved.Commands.Single(command =>
                command.ClassName == "ToolRemoteExecuteOptions");

            using (Assert.Multiple())
            {
                await Assert.That(literalExecute.PreserveNamedFacade).IsTrue();
                await Assert.That(literalExecute.PreserveExecuteFacade).IsFalse();
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Allows_Explicit_Rendering_Phase_Migration()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Input",
                    CSharpType = "string?",
                    PositionIndex = 0,
                    Phase = CommandLinePhase.Passthrough,
                    AllowRenderingPhaseMigrationFromBaseline = true,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Input",
                "string?",
                argumentPosition: 1,
                phase: CommandLinePhase.EarlyOperand)]);

        var argument = preserved.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
            await Assert.That(argument.PositionIndex).IsEqualTo(0);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Optional_Value_Property_Types()
    {
        var command = Command("ToolBranchOptions", "ToolOptions", ["branch"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--color",
                    PropertyName = "Color",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--abbrev",
                    PropertyName = "Abbrev",
                    CSharpType = "int?",
                    ValueArity = CliOptionValueArity.Optional,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--depth",
                    PropertyName = "Depth",
                    CSharpType = "int?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty("Color", "string?", switchName: "--color[=<when>]"),
                BaselineProperty("Abbrev", "int?", switchName: "--abbrev[=<n>]"),
                BaselineProperty("Depth", "int?", switchName: "--depth=<n>"),
            ]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved)))
            .Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Select(option => option.PropertyName))
                .IsEquivalentTo(["ColorOption", "AbbrevOption", "Depth"]);
            await Assert.That(generated).Contains("public CliOptionValue? ColorOption");
            await Assert.That(generated).Contains("public string? Color");
            await Assert.That(generated).Contains("get => ColorOption?.Value;");
            await Assert.That(generated).Contains("public int? Abbrev");
            await Assert.That(generated).Contains("int.TryParse(AbbrevOption?.Value");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "Color",
            "ColorOption",
            CliCompatibilityForwardingKind.NullableStringToCliOptionValue);
        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "Abbrev",
            "AbbrevOption",
            CliCompatibilityForwardingKind.NullableInt32ToCliOptionValue);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Numeric_Property_When_Cli_Value_Becomes_Textual()
    {
        var command = Command("ToolDeployOptions", "ToolOptions", ["deploy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--service-account",
                    PropertyName = "ServiceAccount",
                    CSharpType = "string?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("ServiceAccount", "int?", switchName: "--service-account")]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved)))
            .Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("ServiceAccountValue");
            await Assert.That(preserved.Options.Single().CSharpType).IsEqualTo("string?");
            await Assert.That(generated).Contains("public string? ServiceAccountValue");
            await Assert.That(generated).Contains("public int? ServiceAccount");
            await Assert.That(generated).Contains("int.TryParse(ServiceAccountValue");
            await Assert.That(generated).Contains(
                "set => ServiceAccountValue = value?.ToString(global::System.Globalization.CultureInfo.InvariantCulture);");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "ServiceAccount",
            "ServiceAccountValue",
            CliCompatibilityForwardingKind.NullableInt32ToString);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Preserves_Numeric_Property_When_Cli_Value_Becomes_Textual_Collection()
    {
        var command = Command("ToolDeployOptions", "ToolOptions", ["deploy"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--service-account",
                    PropertyName = "ServiceAccount",
                    CSharpType = "IEnumerable<string>?",
                    AcceptsMultipleValues = true,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty("ServiceAccount", "int?", switchName: "--service-account")]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved)))
            .Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName)
                .IsEqualTo("ServiceAccountValues");
            await Assert.That(generated).Contains("public IEnumerable<string>? ServiceAccountValues");
            await Assert.That(generated).Contains("public int? ServiceAccount");
            await Assert.That(generated).Contains("int.TryParse(ServiceAccountValues?.FirstOrDefault()");
            await Assert.That(generated).Contains(
                "set => ServiceAccountValues = value is null ? null : [value.Value.ToString(global::System.Globalization.CultureInfo.InvariantCulture)];");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "ServiceAccount",
            "ServiceAccountValues",
            CliCompatibilityForwardingKind.NullableInt32ToStringCollection);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Optional_Value_Target_On_Second_Run()
    {
        var command = Command("ToolBranchOptions", "ToolOptions", ["branch"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--color",
                    PropertyName = "Color",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [
                BaselineProperty(
                    "ColorOption",
                    "CliOptionValue?",
                    switchName: "--color",
                    valueArity: CliOptionValueArity.Optional),
                BaselineProperty(
                    "Color",
                    "string?",
                    isCompatibility: true,
                    forwardToPropertyName: "ColorOption",
                    forwardingKind: CliCompatibilityForwardingKind.NullableStringToCliOptionValue),
            ]);

        using (Assert.Multiple())
        {
            await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo("ColorOption");
            await Assert.That(preserved.CompatibilityProperties.Single().PropertyName).IsEqualTo("Color");
        }

        await AssertCompatibilityForwardingRoundTrips(
            preserved,
            "Color",
            "ColorOption",
            CliCompatibilityForwardingKind.NullableStringToCliOptionValue);
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_NonOptional_Forwarded_Alias_Targets()
    {
        var cases = new[]
        {
            (
                TargetName: "Output",
                TargetType: "string?",
                AliasName: "LegacyOutput",
                AliasType: "string?",
                Kind: CliCompatibilityForwardingKind.Direct),
            (
                TargetName: "Outputs",
                TargetType: "IEnumerable<string>?",
                AliasName: "Output",
                AliasType: "string?",
                Kind: CliCompatibilityForwardingKind.ScalarToCollection),
        };

        foreach (var (TargetName, TargetType, AliasName, AliasType, Kind) in cases)
        {
            var command = Command("ToolPushOptions", "ToolOptions", ["push"]) with
            {
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--output",
                        PropertyName = AliasName,
                        CSharpType = TargetType,
                        AcceptsMultipleValues = Kind == CliCompatibilityForwardingKind.ScalarToCollection,
                    },
                ],
            };
            var preserved = GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [
                    BaselineProperty(TargetName, TargetType, switchName: "--output"),
                    BaselineProperty(
                        AliasName,
                        AliasType,
                        isCompatibility: true,
                        forwardToPropertyName: TargetName,
                        forwardingKind: Kind),
                ]);
            var alias = preserved.CompatibilityProperties.Single(property =>
                property.PropertyName == AliasName);

            using (Assert.Multiple())
            {
                await Assert.That(preserved.Options.Single().PropertyName).IsEqualTo(TargetName);
                await Assert.That(alias.ForwardToPropertyName).IsEqualTo(TargetName);
                await Assert.That(alias.ForwardingKind).IsEqualTo(Kind);
            }
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Defaults_Historical_Positional_Phase_Before_Matching()
    {
        var command = Command("ToolRunOptions", "ToolOptions", ["run"]) with
        {
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Prefix",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                    Phase = CommandLinePhase.EarlyOperand,
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.Preserve(
            command,
            [BaselineProperty(
                "Package",
                "string",
                argumentPosition: 0,
                isRequired: true,
                omitPhase: true)]);
        var arguments = preserved.PositionalArguments.ToDictionary(
            static argument => argument.PropertyName,
            StringComparer.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(arguments.Keys).IsEquivalentTo(["Package", "Prefix"]);
            await Assert.That(arguments["Package"].Phase).IsEqualTo(CommandLinePhase.Passthrough);
            await Assert.That(arguments["Prefix"].Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Retains_Removed_Global_Options()
    {
        var preserved = GeneratedApiCompatibilityPreserver.PreserveGlobalOptions(
            Tool(Command("ToolRunOptions", "ToolOptions", ["run"])),
            [BaselineProperty("LegacyFlag", "bool?", switchName: "--legacy-flag")]);

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(preserved)).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public virtual bool? LegacyFlag { get; set; }");
            await Assert.That(generated).Contains("LegacyFlag is no longer supported");
            await Assert.That(generated).DoesNotContain("CliFlag(\"--legacy-flag\")");
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Supplemental_Global_Option_Types()
    {
        var tool = Tool(Command("ToolRunOptions", "ToolOptions", ["run"])) with
        {
            SupplementalGlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--progress",
                    PropertyName = "Progress",
                    CSharpType = "string?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.PreserveGlobalOptions(
            tool,
            [BaselineProperty("Progress", "CliOptionValue?", switchName: "--progress")]);
        var progress = preserved.GlobalOptions.Single();

        using (Assert.Multiple())
        {
            await Assert.That(progress.CSharpType).IsEqualTo("CliOptionValue?");
            await Assert.That(preserved.SupplementalGlobalOptions).IsEmpty();
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Restores_Global_Case_Variant_Alias_Dispatch()
    {
        var tool = Tool(Command("ToolRunOptions", "ToolOptions", ["run"])) with
        {
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--generate-changeset-created-values",
                    PropertyName = "GenerateChangeSetCreatedValues",
                    CSharpType = "bool?",
                },
            ],
        };

        var preserved = GeneratedApiCompatibilityPreserver.PreserveGlobalOptions(
            tool,
            [
                BaselineProperty("GenerateChangeSetCreatedValues", "bool?", isCompatibility: true),
                BaselineProperty("GenerateChangesetCreatedValues", "bool?", isCompatibility: true),
            ]);
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(preserved)).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.GlobalCompatibilityProperties.Single().ForwardToPropertyName)
                .IsEqualTo("GenerateChangeSetCreatedValues");
            await Assert.That(generated).Contains("get => GenerateChangesetCreatedValues;");
            await Assert.That(generated).Contains("set => GenerateChangesetCreatedValues = value;");
            await Assert.That(generated)
                .Contains("public virtual bool? GenerateChangesetCreatedValues { get; set; }");
        }
    }

    [Test]
    public async Task Global_Compatibility_Aliases_Follow_Inherited_Renames_And_Preserve_Dispatch()
    {
        var tool = Tool(Command("ToolRunOptions", "ToolOptions", ["run"])) with
        {
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "IEnumerable<string>?",
                },
            ],
            GlobalCompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "LegacyArguments",
                    CSharpType = "IEnumerable<string>?",
                    ForwardToPropertyName = "Arguments",
                    ObsoleteMessage = "Use Arguments instead.",
                },
            ],
        };

        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(resolved)).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(generated).Contains("public virtual IEnumerable<string>? CliArguments");
            await Assert.That(generated).Contains("get => LegacyArguments;");
            await Assert.That(generated).Contains("set => LegacyArguments = value;");
            await Assert.That(generated)
                .Contains("public virtual IEnumerable<string>? LegacyArguments { get; set; }");
        }
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

    #endregion

    #region EnsureNoDuplicateFilePaths

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

    #endregion

    #region Nullable parameter emission

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
    public async Task GenerateServiceMethod_Does_Not_Construct_Required_Optional_Options()
    {
        var command = Command(
            "ToolExecuteOptions",
            "ToolOptions",
            options:
            [
                new CliOptionDefinition
                {
                    SwitchName = "--name",
                    PropertyName = "Name",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ]) with
        {
            PreserveOptionalOptionsParameter = true,
        };
        var sb = new StringBuilder();

        GeneratorUtils.GenerateServiceMethod(sb, "Execute", command);

        var generated = sb.ToString();
        await Assert.That(generated).Contains("ToolExecuteOptions? options = null");
        await Assert.That(generated).Contains("options ?? throw new ArgumentNullException(nameof(options))");
        await Assert.That(generated).DoesNotContain("new ToolExecuteOptions()");
    }

    [Test]
    public async Task GenerateServiceMethod_Emits_Obsolete_Forwarding_Alias()
    {
        const string obsoleteMessage = "Use \"CreateOrUpdate\".\r\nPath:\tC:\\tool";
        var sb = new StringBuilder();
        var command = Command(
            "ToolCreateOrUpdateOptions",
            "ToolOptions",
            compatibilityMethods:
            [
                new CliCompatibilityMethod
                {
                    MethodName = "Create_or_update",
                    ObsoleteMessage = obsoleteMessage,
                },
            ]);

        GeneratorUtils.GenerateServiceMethod(sb, "CreateOrUpdate", command);

        var generated = sb.ToString();
        var expectedObsoleteMessage = obsoleteMessage.Replace(
            "CreateOrUpdate",
            "CreateOrUpdateAsync",
            StringComparison.Ordinal);

        await Assert.That(generated)
            .Contains($"[Obsolete({GeneratorUtils.FormatStringLiteral(expectedObsoleteMessage)})]");
        await Assert.That(generated).DoesNotContain("CreateOrUpdateAsyncAsync");
        await Assert.That(generated).Contains("Task<CommandResult> Create_or_updateAsync(");
        await Assert.That(generated).Contains(
            "return await CreateOrUpdateAsync(options, executionOptions, cancellationToken);");
    }

    [Test]
    public async Task ServiceInterfaceGenerator_Delegates_Current_Member_To_Obsolete_Compatibility_Signature()
    {
        const string obsoleteMessage = "Use \"CreateOrUpdate\".\r\nPath:\tC:\\tool";
        var tool = Tool(Command(
            "ToolCreateOrUpdateOptions",
            "ToolOptions",
            ["create_or_update"],
            compatibilityMethods:
            [
                new CliCompatibilityMethod
                {
                    MethodName = "Create_or_update",
                    ObsoleteMessage = obsoleteMessage,
                },
            ]));

        var generated = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;
        var expectedObsoleteMessage = obsoleteMessage.Replace(
            "CreateOrUpdate",
            "CreateOrUpdateAsync",
            StringComparison.Ordinal);

        await Assert.That(generated)
            .Contains($"[Obsolete({GeneratorUtils.FormatStringLiteral(expectedObsoleteMessage)})]");
        await Assert.That(generated).Contains("Task<CommandResult> Create_or_updateAsync(");
        await Assert.That(generated).Contains("    #pragma warning disable CS0618");
        await Assert.That(generated)
            .Contains("    => Create_or_updateAsync(options, executionOptions, cancellationToken);");
        await Assert.That(generated).Contains("    #pragma warning restore CS0618");
        await Assert.That(generated)
            .Contains("    => throw new System.NotSupportedException();");
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

    #endregion

    #region Method name casing

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
    public async Task Az_Compatibility_Preserves_Known_Snake_Case_Methods()
    {
        var automationMethods = AzCliCompatibility.GetMethods(
            ["security", "automation", "create_or_update"]);
        var suppressionRuleMethods = AzCliCompatibility.GetMethods(
            ["security", "alerts-suppression-rule", "upsert_scope"]);

        await Assert.That(automationMethods.Single().MethodName).IsEqualTo("Create_or_update");
        await Assert.That(suppressionRuleMethods.Single().MethodName).IsEqualTo("Upsert_scope");
    }

    #endregion

    #region XML documentation path normalization

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

    #endregion

    #region Secrets

    [Test]
    public async Task IsSecretOption_Detects_Passphrase()
    {
        var result = GeneratorUtils.IsSecretOption("SshPassphrase", isFlag: false);

        await Assert.That(result).IsTrue();
    }

    #endregion

    #region Enum deduplication

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

    #endregion

    #region Duplicate command detection

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

    #endregion

    #region Root command collision filter

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

    #endregion

    #region GeneratedCode attribute version

    [Test]
    public async Task GeneratedCodeAttribute_Contains_A_Version()
    {
        await Assert.That(GeneratorUtils.GeneratedCodeAttribute)
            .Contains($"\"{GeneratorUtils.GeneratorVersion}\"");
    }

    #endregion
}
