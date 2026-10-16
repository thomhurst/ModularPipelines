using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] TerraformVariableValues = ["foo=hello world", "bar=two"];

    [Test]
    [Arguments("false")]
    [Arguments("true")]
    [Arguments("unset")]
    public async Task Terraform_Generated_Options_Render_Boolean_Values_And_Repeated_Variables(string value)
    {
        var command = await TerraformPlanningOptionsTests.ParseFixture("apply");
        var definitions = command.Options.Where(option => option.SwitchName is "-lock" or "-input" or "-var").ToList();
        var type = Compile(await Generate(definitions)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(type)!;
        var parts = definitions.Select(definition =>
        {
            var property = type.GetProperty(definition.PropertyName)!;
            if (definition.SwitchName == "-var")
            {
                property.SetValue(instance, TerraformVariableValues);
            }
            else if (value != "unset")
            {
                property.SetValue(instance, bool.Parse(value));
            }

            var attribute = property.CustomAttributes.Single(item => item.AttributeType.Name == nameof(CliOptionAttribute));
            var name = (string) attribute.ConstructorArguments.Single().Value!;
            var format = (OptionFormat) attribute.NamedArguments.Single(item => item.MemberName == nameof(CliOptionAttribute.Format)).TypedValue.Value!;
            return new OptionPart(property.Name, property.GetValue, new CliOptionAttribute(name) { Format = format });
        }).ToArray<PropertyCommandLinePart>();

        string[] expected = value == "unset"
            ? ["-var=foo=hello world", "-var=bar=two"]
            : [$"-lock={value}", $"-input={value}", "-var=foo=hello world", "-var=bar=two"];
        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(expected, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
