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
            .Contains("[CliOption(\"--library-path\", ShortForm = \"-L\", PreferShortForm = true, AllowMultiple = true)]");
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
                    PlaceholderName = "SERVICE",
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
    public async Task OptionsClassGenerator_Marks_Inherited_Name_Collisions_As_New()
    {
        var command = Command("ToolRunOptions", "ToolOptions") with
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

        await Assert.That(generated).Contains("public new string? Tool { get; set; }");
        await Assert.That(generated).Contains("public new IEnumerable<string>? CommandParts { get; set; }");
        await Assert.That(generated).Contains("public new bool? Arguments { get; set; }");
        await Assert.That(generated).Contains("public new IEnumerable<string>? RunSettings { get; set; }");
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

        await Assert.That(generated).Contains("[property: CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)] IEnumerable<string> Image");
        await Assert.That(generated).DoesNotContain("public string? Image { get; set; }");
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
    public async Task ServiceInterfaceGenerator_Emits_Obsolete_Compatibility_Signature()
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

        await Assert.That(result).DoesNotContain("runneradmin");
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
