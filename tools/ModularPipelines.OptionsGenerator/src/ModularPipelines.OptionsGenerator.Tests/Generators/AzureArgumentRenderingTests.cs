using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
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
