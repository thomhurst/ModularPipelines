using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Generated_Group_Alternatives_Preserve_Typed_Values(bool optional)
    {
        var definition = await AzArgumentShapeTests.ParseFixture("aro-create", "--assign-platform-wi");
        definition = definition with { ValueArity = optional ? CliOptionValueArity.Optional : CliOptionValueArity.Required };
        var generated = await Generate([definition], alternativeGroups:
            [new() { Members = [new() { PropertyName = definition.PropertyName, OptionSwitch = definition.SwitchName }] }]);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty(definition.PropertyName)!;
        var validation = (System.ComponentModel.DataAnnotations.IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        property.SetValue(instance, new CliValueGroup[] { new(optional ? [] : ["operator", "identity"]) });
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var part = new OptionPart(property.Name, property.GetValue,
            new CliOptionAttribute(definition.SwitchName) { GroupValues = true, ValueArity = definition.ValueArity });
        var expected = optional ? new[] { definition.SwitchName } : [definition.SwitchName, "operator", "identity"];
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo(expected,
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Azure_Append_Groups_Render_Separate_Identity_Assignments()
    {
        var definition = await AzArgumentShapeTests.ParseFixture("aro-create", "--assign-platform-wi");
        var options = Compile(await Generate([definition])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty(definition.PropertyName)!;
        property.SetValue(instance, new CliValueGroup[]
        {
            new(["operator-one", "/subscriptions/example/identity one"]),
            new(["operator-two", "/subscriptions/example/identity two"]),
        });
        var attribute = property.CustomAttributes.Single(attribute => attribute.AttributeType.Name == nameof(CliOptionAttribute));
        await Assert.That(attribute.NamedArguments.Any(argument => argument.MemberName == nameof(CliOptionAttribute.GroupValues)
            && Equals(argument.TypedValue.Value, true))).IsTrue();
        var part = new OptionPart(property.Name, property.GetValue, new CliOptionAttribute(definition.SwitchName) { GroupValues = true });
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo(
            new[] { definition.SwitchName, "operator-one", "/subscriptions/example/identity one", definition.SwitchName,
                "operator-two", "/subscriptions/example/identity two" }, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("aks-create", "--node-vm-size", "Standard_DS2_v2")]
    [Arguments("aks-nodepool-update", "--max-unavailable", "5%")]
    public async Task AzureGeneratedScalarsRenderText(string fixture, string name, string value)
    {
        var definition = await AzArgumentShapeTests.ParseFixture(fixture, name);
        var options = Compile(await Generate([definition])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty(definition.PropertyName)!;
        property.SetValue(instance, value);
        var part = new OptionPart(property.Name, property.GetValue, new CliOptionAttribute(name));
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo([name, value], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [MatrixDataSource]
    public async Task AzureGeneratedOptionalGroupsRenderOmittedBareAndValues(
        [Matrix("--enable-azure-container-storage", "--parameters", "--variables", "--fields")] string name,
        [Matrix("omitted", "empty", "bare", "values")] string payload)
    {
        var fixture = name switch
        {
            "--enable-azure-container-storage" => "aks-create",
            "--fields" => "boards-work-item-create",
            _ => "pipelines-run",
        };
        var definition = await AzArgumentShapeTests.ParseFixture(fixture, name);
        var options = Compile(await Generate([definition])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty(definition.PropertyName)!;
        CliOptionValue[]? values = payload switch
        {
            "empty" => [],
            "bare" => [CliOptionValue.Bare],
            "values" => ["first=value", "second=other"],
            _ => null,
        };
        property.SetValue(instance, values);
        var attribute = property.CustomAttributes.Single(attribute => attribute.AttributeType.Name == nameof(CliOptionAttribute));
        var grouped = (bool) attribute.NamedArguments.Single(argument => argument.MemberName == nameof(CliOptionAttribute.GroupValues)).TypedValue.Value!;
        var arity = (CliOptionValueArity) attribute.NamedArguments.Single(argument => argument.MemberName == nameof(CliOptionAttribute.ValueArity)).TypedValue.Value!;
        var part = new OptionPart(property.Name, property.GetValue, new CliOptionAttribute(name) { GroupValues = grouped, ValueArity = arity });
        string[] expected = payload switch
        {
            "bare" => [name],
            "values" => [name, "first=value", "second=other"],
            _ => [],
        };
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo(expected, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
