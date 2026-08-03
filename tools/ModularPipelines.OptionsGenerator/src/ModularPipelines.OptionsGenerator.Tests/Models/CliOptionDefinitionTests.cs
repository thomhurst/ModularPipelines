using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Models;

public class CliOptionDefinitionTests
{
    #region Default Values Tests

    [Test]
    public async Task ValueSeparator_Defaults_To_Space()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.ValueSeparator).IsEqualTo(" ");
    }

    [Test]
    public async Task IsFlag_Defaults_To_False()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task IsRequired_Defaults_To_False()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.IsRequired).IsFalse();
    }

    [Test]
    public async Task AcceptsMultipleValues_Defaults_To_False()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.AcceptsMultipleValues).IsFalse();
    }

    #endregion

    #region Optional Properties Tests

    [Test]
    public async Task ShortForm_Is_Nullable()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.ShortForm).IsNull();
    }

    [Test]
    public async Task Description_Is_Nullable()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.Description).IsNull();
    }

    [Test]
    public async Task EnumDefinition_Is_Nullable()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.EnumDefinition).IsNull();
    }

    [Test]
    public async Task ValidationConstraints_Is_Nullable()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option.ValidationConstraints).IsNull();
    }

    #endregion

    #region Record Equality Tests

    [Test]
    public async Task Records_With_Same_Values_Are_Equal()
    {
        var option1 = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            Description = "Output file",
        };

        var option2 = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            Description = "Output file",
        };

        await Assert.That(option1).IsEqualTo(option2);
    }

    [Test]
    public async Task Records_With_Different_SwitchName_Are_Not_Equal()
    {
        var option1 = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        var option2 = new CliOptionDefinition
        {
            SwitchName = "--input",
            PropertyName = "Output",
            CSharpType = "string?",
        };

        await Assert.That(option1).IsNotEqualTo(option2);
    }

    #endregion
}
