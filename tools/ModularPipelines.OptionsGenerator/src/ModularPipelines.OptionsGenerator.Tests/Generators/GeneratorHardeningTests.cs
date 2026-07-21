using System.Text;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

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
        IReadOnlyList<CliEnumDefinition>? enums = null) =>
        new()
        {
            FullCommand = "tool",
            CommandParts = commandParts ?? [],
            ClassName = className,
            ParentClassName = parentClassName,
            ToolNamespacePrefix = "Tool",
            Options = [],
            SubDomainGroup = subDomainGroup,
            CommandGroupIdentifierOverride = commandGroupIdentifierOverride,
            Enums = enums ?? [],
        };

    private static CliToolDefinition Tool(params CliCommandDefinition[] commands) =>
        new()
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = commands,
        };

    #region NormalizeCommandClassNames

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

        await Assert.That(subDomainFiles.Single().RelativePath).EndsWith("ToolApplicationSet.Generated.cs");
        await Assert.That(subDomainFiles.Single().Content).Contains("ToolApplicationSetOptions? options = null");
        await Assert.That(interfaceFiles.Single().Content).Contains("ToolApplicationSet ApplicationSet { get; }");
        await Assert.That(interfaceFiles.Single().Content).DoesNotContain("Appset(");
        await Assert.That(implementationFiles.Single().Content).Contains("ToolApplicationSet ApplicationSet { get; }");
        await Assert.That(registrationFiles.Single().Content).Contains("TryAddScoped<ToolApplicationSet>()");
        await Assert.That(GeneratorUtils.GetNonCollidingRootCommands(tool)).IsEmpty();
    }

    [Test]
    public async Task SubDomain_Generators_Preserve_Legacy_Casing_Without_Override()
    {
        var tool = Tool(Command(
            "ToolWorkspaceAddOnsGetOptions",
            "ToolOptions",
            ["workspace-add-ons", "get"],
            subDomainGroup: "WorkspaceAddOns"));

        var subDomainFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var interfaceFiles = await new ServiceInterfaceGenerator().GenerateAsync(tool);

        await Assert.That(subDomainFiles.Single().RelativePath).EndsWith("ToolWorkspaceaddons.Generated.cs");
        await Assert.That(interfaceFiles.Single().Content).Contains("ToolWorkspaceaddons Workspaceaddons { get; }");
    }

    #endregion

    #region Compatibility properties

    [Test]
    public async Task OptionsClassGenerator_Emits_NonCli_Compatibility_Properties()
    {
        var command = Command("ToolBuildOptions", "ToolOptions") with
        {
            CompatibilityProperties =
            [
                new CliCompatibilityProperty
                {
                    PropertyName = "OldName",
                    CSharpType = "bool?",
                    ForwardToPropertyName = "NewName",
                    ObsoleteMessage = "Use NewName instead.",
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

        await Assert.That(generated).Contains("[Obsolete(\"Use NewName instead.\")]");
        await Assert.That(generated).Contains("get => NewName;");
        await Assert.That(generated).Contains("set => NewName = value;");
        await Assert.That(generated).Contains("public bool? RemovedFlag { get; set; }");
        await Assert.That(generated).DoesNotContain("CliFlag(\"--removed-flag\")");
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
            Command("ToolNetworkOptions", "ToolOptions", ["network"]),
            Command("ToolNetwork2Options", "ToolOptions", ["Network"]));

        await Assert.That(() => new SubDomainClassGenerator().GenerateAsync(tool))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("network");
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
