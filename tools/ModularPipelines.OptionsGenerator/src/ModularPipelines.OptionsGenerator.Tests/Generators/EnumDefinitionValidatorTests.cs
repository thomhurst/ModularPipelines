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

    [Test]
    public async Task Duplicate_Explicit_Numeric_Values_Are_Rejected()
    {
        var tool = ToolWithValues(
            new CliEnumValue { MemberName = "First", CliValue = "first", NumericValue = 1 },
            new CliEnumValue { MemberName = "Second", CliValue = "second", NumericValue = 1 });

        await Assert.That(() => EnumDefinitionValidator.Validate(tool))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("duplicate effective numeric value '1'");
    }

    [Test]
    public async Task Duplicate_Implicit_And_Explicit_Numeric_Values_Are_Rejected()
    {
        var tool = ToolWithValues(
            new CliEnumValue { MemberName = "First", CliValue = "first", NumericValue = 1 },
            new CliEnumValue { MemberName = "Second", CliValue = "second" },
            new CliEnumValue { MemberName = "Third", CliValue = "third", NumericValue = 2 });

        await Assert.That(() => EnumDefinitionValidator.Validate(tool))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("duplicate effective numeric value '2'");
    }

    [Test]
    public async Task Implicit_Numeric_Value_After_Int_MaxValue_Is_Rejected()
    {
        var tool = ToolWithValues(
            new CliEnumValue
            {
                MemberName = "Maximum",
                CliValue = "maximum",
                NumericValue = int.MaxValue,
            },
            new CliEnumValue { MemberName = "Overflow", CliValue = "overflow" });

        await Assert.That(() => EnumDefinitionValidator.Validate(tool))
            .Throws<InvalidOperationException>()
            .WithMessageContaining($"implicit numeric value after '{int.MaxValue}'");
    }

    [Test]
    public void Explicit_Numeric_Value_After_Int_MaxValue_Is_Allowed()
    {
        var tool = ToolWithValues(
            new CliEnumValue
            {
                MemberName = "Maximum",
                CliValue = "maximum",
                NumericValue = int.MaxValue,
            },
            new CliEnumValue { MemberName = "Reset", CliValue = "reset", NumericValue = 1 });

        EnumDefinitionValidator.Validate(tool);
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
