using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class EnumDefinitionValidatorTests
{
    [Test]
    public async Task Duplicate_Current_Cli_Values_Are_Rejected()
    {
        var tool = ToolWithValues(
            new CliEnumValue { MemberName = "First", CliValue = "same" },
            new CliEnumValue { MemberName = "Second", CliValue = "same" });

        await Assert.That(() => EnumDefinitionValidator.Validate(tool))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("duplicate CLI value 'same'");
    }

    [Test]
    public async Task Duplicate_Current_Member_Names_Are_Rejected()
    {
        var tool = ToolWithValues(
            new CliEnumValue { MemberName = "Same", CliValue = "first" },
            new CliEnumValue { MemberName = "Same", CliValue = "second" });

        await Assert.That(() => EnumDefinitionValidator.Validate(tool))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("duplicate member 'Same'");
    }

    private static CliToolDefinition ToolWithValues(params CliEnumValue[] values) => new()
    {
        ToolName = "tool",
        NamespacePrefix = "Tool",
        TargetNamespace = "Tool",
        OutputDirectory = "src/Tool",
        Commands =
        [
            new CliCommandDefinition
            {
                FullCommand = "tool run",
                CommandParts = ["run"],
                ClassName = "ToolRunOptions",
                ParentClassName = "ToolOptions",
                ToolNamespacePrefix = "Tool",
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--value",
                        PropertyName = "Value",
                        CSharpType = "ToolRunValue?",
                        EnumDefinition = new CliEnumDefinition
                        {
                            EnumName = "ToolRunValue",
                            Values = values,
                        },
                    },
                ],
            },
        ],
    };
}
