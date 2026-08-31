using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class GlobalOptionsBaseGeneratorTests
{
    [Test]
    public async Task Generate_Marks_Base_And_Emits_Global_Options()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "liquibase",
            NamespacePrefix = "Liquibase",
            TargetNamespace = "ModularPipelines.Liquibase",
            OutputDirectory = "src/ModularPipelines.Liquibase",
            Commands = [],
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--search-path",
                    PropertyName = "SearchPath",
                    CSharpType = "string?",
                    ValueSeparator = "=",
                },
            ],
        };

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains("[CliGlobalOptions]");
        await Assert.That(generated).Contains("[CliOption(\"--search-path\", Format = OptionFormat.EqualsSeparated)]");
        await Assert.That(generated).Contains("public virtual string? SearchPath { get; set; }");
    }

    [Test]
    public async Task Generate_Can_Keep_Inherited_Options_After_Subcommands()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [],
            GlobalOptionsBeforeSubcommands = false,
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--change-reference",
                    PropertyName = "ChangeReference",
                    CSharpType = "string?",
                },
            ],
        };

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
        await Assert.That(generated).Contains("// Global options intentionally follow subcommands.");
        await Assert.That(generated).Contains("public virtual string? ChangeReference { get; set; }");
    }

    [Test]
    public async Task Generate_Uses_Supplemental_Options_Through_The_Normal_Global_Path()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [],
            SupplementalGlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--license-key",
                    PropertyName = "LicenseKey",
                    CSharpType = "string?",
                    Description = "Enables licensed features.",
                    DocumentationUrl = "https://example.test/license",
                    Availability = "Secure edition",
                    IsSecret = true,
                    ValueSeparator = "=",
                },
            ],
        };

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            $"using ModularPipelines.Secrets;{Environment.NewLine}using System.CodeDom.Compiler;");
        await Assert.That(generated).Contains("[SecretValue]");
        await Assert.That(generated)
            .Contains("[CliOption(\"--license-key\", Format = OptionFormat.EqualsSeparated)]");
        await Assert.That(generated).Contains("public virtual string? LicenseKey { get; set; }");
        await Assert.That(generated).Contains("Availability: Secure edition.");
        await Assert.That(generated).Contains("Documentation: https://example.test/license");
    }

    [Test]
    public async Task Generate_Uses_CliOptionValue_For_Optional_Value_Arity()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [],
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--run-tests",
                    PropertyName = "RunTests",
                    CSharpType = "string?",
                    ValueArity = CliOptionValueArity.Optional,
                    ValidationConstraints = new CliValidationConstraints { Pattern = "^[a-z]+$" },
                },
            ],
        };

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains("using ModularPipelines.Models;");
        await Assert.That(generated).Contains("[CliOptionValueRegularExpression(\"^[a-z]+$\")]");
        await Assert.That(generated).Contains("public virtual CliOptionValue? RunTests { get; set; }");
    }

    [Test]
    public async Task Generate_Preserves_Keyed_Secret_Masking_For_Global_Options()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [],
            GlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--property",
                    PropertyName = "Property",
                    CSharpType = "IReadOnlyList<KeyValue>?",
                    IsSecret = true,
                    IsKeyValue = true,
                    SecretValueKeys = ["token", "password"],
                },
            ],
        };

        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains("[SecretValue(\"token\", \"password\")]");
    }
}
