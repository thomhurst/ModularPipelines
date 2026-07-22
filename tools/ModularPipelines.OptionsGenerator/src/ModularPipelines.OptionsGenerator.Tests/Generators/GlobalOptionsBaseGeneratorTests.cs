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
}
