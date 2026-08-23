using System.Text;
using ModularPipelines.Attributes;
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

    private static GeneratedApiProperty BaselineProperty(
        string propertyName,
        string cSharpType,
        string? switchName = null,
        int? argumentPosition = null,
        bool isRequired = false,
        bool isCompatibility = false,
        string? forwardToPropertyName = null,
        bool useInitAccessor = false) =>
        new(
            propertyName,
            cSharpType,
            switchName,
            argumentPosition,
            isRequired,
            isCompatibility,
            forwardToPropertyName,
            null,
            useInitAccessor);

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
        await Assert.That(interfaceFiles.Single().Content).Contains("IToolApplicationSet ApplicationSet { get; }");
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
        await Assert.That(interfaceFiles.Single().Content).Contains("IToolWorkspaceAddOns WorkspaceAddOns { get; }");
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
        await Assert.That(interfaceFiles.Single().Content).Contains("IToolGroup Group { get; }");
        await Assert.That(optionFiles.Single(file =>
                file.RelativePath.EndsWith("ToolGroupOptions.Generated.cs")).Content)
            .Contains("[CliSubCommand(\"group\")]");
        await Assert.That(optionFiles.Single(file =>
                file.RelativePath.EndsWith("ToolGroupChildOptions.Generated.cs")).Content)
            .Contains("[CliSubCommand(\"group\", \"child\")]");
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
                subDomainGroup: "ClusterInfo")) with
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
            .Contains("IKubernetesClusterInfo ClusterInfo { get; }");
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
                .Contains("ToolParentImageTools ImageTools { get; }");
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
    public async Task ApiCompatibilityPreserver_Rejects_Renamed_Scalar_To_Collection_Changes()
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

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("CommandOptions", "string?", switchName: "--command-options")]));

        await Assert.That(exception.Message)
            .Contains("changed type from string? to IEnumerable<string>? while being renamed");
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
        var generated = (await new OptionsClassGenerator().GenerateAsync(Tool(preserved))).Single().Content;

        using (Assert.Multiple())
        {
            await Assert.That(preserved.CompatibilityProperties.Single().ForwardingKind)
                .IsEqualTo(CliCompatibilityForwardingKind.NullableStringToRequiredString);
            await Assert.That(generated).Contains("public string? Args");
            await Assert.That(generated).Contains("get => Subcommand;");
            await Assert.That(generated).Contains("set => Subcommand = value ?? string.Empty;");
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
    public async Task ApiCompatibilityPreserver_Rejects_Reused_Compatibility_Property_With_Cli_Identity()
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

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("RemovedFlag", "bool?", isCompatibility: true)]));

        await Assert.That(exception.Message)
            .Contains("ToolCopyOptions.RemovedFlag changed CLI switch or argument position");
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
    public async Task ApiCompatibilityPreserver_Rejects_Optional_Member_Becoming_Required()
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

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("Name", "string", argumentPosition: 0)]));

        await Assert.That(exception.Message)
            .Contains("Name changed from optional to required and would remove its public setter");
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
    public async Task ApiCompatibilityPreserver_Rejects_Old_Deconstruct_Arity_For_New_Required_Members()
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

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("Source", "string", switchName: "--source", isRequired: true)]));

        await Assert.That(exception.Message).Contains("newly required member(s) Destination have no baseline value");
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
    public async Task ApiCompatibilityPreserver_Rejects_Removed_Required_Members()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                Command("ToolAddOptions", "ToolOptions", ["add"]),
                [BaselineProperty(
                    "Package",
                    "string",
                    argumentPosition: 0,
                    isRequired: true)]));

        await Assert.That(exception.Message)
            .Contains("ToolAddOptions.Package positional argument was removed");
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
    public async Task ApiCompatibilityPreserver_Rejects_Deconstruct_Preservation_For_New_Required_Members()
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

            var exception = Assert.Throws<InvalidOperationException>(() =>
                GeneratedApiCompatibilityPreserver.Preserve(Tool(command), root));

            await Assert.That(exception.Message).Contains("newly required member(s) Force have no baseline value");
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
    public async Task ApiCompatibilityPreserver_Rejects_Removed_Command_Facades()
    {
        var root = Path.Combine(Path.GetTempPath(), $"service-api-{Guid.NewGuid():N}");
        var packageDirectory = Path.Combine(root, "src", "ModularPipelines.Tool");
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Options"));
        Directory.CreateDirectory(Path.Combine(packageDirectory, "Services"));
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolRemovedOptions.Generated.cs"),
                "public record ToolRemovedOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "Tool.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class Tool { public Task RemovedAsync(ToolRemovedOptions? options = null) => Task.CompletedTask; }");

            var exception = Assert.Throws<InvalidOperationException>(() =>
                GeneratedApiCompatibilityPreserver.Preserve(
                    Tool(Command("ToolCurrentOptions", "ToolOptions", ["current"])),
                    root));

            await Assert.That(exception.Message)
                .Contains("ToolRemovedOptions command disappeared from generated facade");
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
                "public record ToolAddOptions;");
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
                "public record ToolGroupChildOptions;");
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
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Options", "ToolGroupKubeconfigOptions.Generated.cs"),
                "public record ToolGroupKubeconfigOptions;");
            await File.WriteAllTextAsync(
                Path.Combine(packageDirectory, "Services", "ToolGroup.Generated.cs"),
                "namespace ModularPipelines.Tool.Services; "
                + "public class ToolGroup { public Task KubeconfigAsync(ToolGroupKubeconfigOptions? options = null) => Task.CompletedTask; }");
            var command = Command(
                "ToolGroupKubeConfigOptions",
                "ToolOptions",
                ["group", "kubeconfig"],
                subDomainGroup: "group");

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
    public async Task ApiCompatibilityPreserver_Rejects_Alias_Constructors_For_New_Required_Members()
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

            var exception = Assert.Throws<InvalidOperationException>(() =>
                GeneratedApiCompatibilityPreserver.Preserve(tool, root));

            await Assert.That(exception.Message).Contains("newly required member(s) Destination have no baseline value");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task ApiCompatibilityPreserver_Rejects_Parameterless_Alias_Constructor_For_New_Required_Members()
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

            var exception = Assert.Throws<InvalidOperationException>(() =>
                GeneratedApiCompatibilityPreserver.Preserve(tool, root));

            await Assert.That(exception.Message).Contains("newly required member(s) Source have no baseline value");
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
                "public record ToolRunOptions;");
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
    public async Task ApiCompatibilityPreserver_Rejects_Required_Rename_Collisions()
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

        var exception = Assert.Throws<InvalidOperationException>(() =>
            GeneratedApiCompatibilityPreserver.Preserve(
                command,
                [BaselineProperty("StableName", "string", argumentPosition: 0, isRequired: true)]));

        await Assert.That(exception.Message).Contains("would duplicate a member name");
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
    public async Task Global_Compatibility_Targets_Follow_Inherited_Property_Renames()
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
            await Assert.That(generated).Contains("get => CliArguments;");
            await Assert.That(generated).Contains("set => CliArguments = value;");
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

        await Assert.That(generated).Contains("[property: SecretValue, CliArgument(0");
        await Assert.That(generated).Contains($"[SecretValue]{Environment.NewLine}    [CliArgument(1");
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
    public async Task Case_Variant_Enum_Names_Fail_The_Duplicate_Path_Check()
    {
        CliEnumDefinition EnumDef(string name) => new()
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
