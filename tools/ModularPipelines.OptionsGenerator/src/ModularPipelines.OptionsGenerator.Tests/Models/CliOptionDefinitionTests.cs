using ModularPipelines.Attributes;
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

    [Test]
    public async Task PositionalArgument_Phase_Defaults_To_EarlyOperand()
    {
        var argument = new CliPositionalArgument
        {
            PropertyName = "Input",
            CSharpType = "string",
        };

        await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
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

    [Test]
    public async Task PropertyType_Uses_CliOptionValue_For_Optional_Value_Arity()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--run-tests",
            PropertyName = "RunTests",
            CSharpType = "string?",
            ValueArity = CliOptionValueArity.Optional,
        };

        await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        await Assert.That(option.RequiresModelsNamespace).IsTrue();
    }

    [Test]
    public async Task PropertyType_Uses_CliOptionValue_Collection_For_Repeatable_Optional_Value_Arity()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--attach-debugger",
            PropertyName = "AttachDebugger",
            CSharpType = "IEnumerable<string>?",
            ValueArity = CliOptionValueArity.Optional,
            AcceptsMultipleValues = true,
        };

        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
        await Assert.That(option.RequiresModelsNamespace).IsTrue();
    }

    [Test]
    public async Task RequiresModelsNamespace_When_CSharpType_Uses_KeyValue_Without_Metadata()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--labels",
            PropertyName = "Labels",
            CSharpType = "IReadOnlyList<KeyValue>?",
        };

        await Assert.That(option.RequiresModelsNamespace).IsTrue();
    }

    [Test]
    public async Task PropertyType_Preserves_Grouped_Optional_Collection_Shape()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "string[]?",
            ValueArity = CliOptionValueArity.Optional,
            GroupValues = true,
        };

        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
    }

    [Test]
    public async Task PropertyType_Preserves_Declared_Optional_Collection_Shape()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "IReadOnlyList<string>?",
            ValueArity = CliOptionValueArity.Optional,
        };

        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
    }

    [Test]
    [Arguments("Queue<string>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.ObjectModel.ReadOnlyDictionary<string, string>?")]
    public async Task PropertyType_Preserves_Framework_Optional_Collection_Shapes(string cSharpType)
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = cSharpType,
            ValueArity = CliOptionValueArity.Optional,
        };

        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
    }

    [Test]
    [Arguments(true, "IEnumerable<CliOptionValue>?")]
    [Arguments(false, "CliOptionValue?")]
    public async Task PropertyType_Uses_Declared_Custom_Optional_Collection_Shape(
        bool isCollection,
        string expectedType)
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "PrivatePackage.CustomValues?",
            ValueArity = CliOptionValueArity.Optional,
            IsCollection = isCollection,
        };

        await Assert.That(option.PropertyType).IsEqualTo(expectedType);
    }
}
