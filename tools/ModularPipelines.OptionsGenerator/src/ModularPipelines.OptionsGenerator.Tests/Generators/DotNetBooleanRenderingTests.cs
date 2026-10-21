using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;
using TUnit.Assertions.Enums;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("build", "--nologo")]
    [Arguments("build", "-nologo, --no-logo")]
    [Arguments("clean", "--nologo")]
    [Arguments("clean", "-nologo, --no-logo")]
    [Arguments("pack", "--nologo")]
    [Arguments("pack", "-nologo, --no-logo")]
    [Arguments("publish", "--nologo")]
    [Arguments("publish", "-nologo, --no-logo")]
    public async Task DotNet_True_Default_NoLogo_Compiles_And_Renders_Explicit_Booleans(
        string command, string declaration)
    {
        var definition = await DotNetCliScraperTests.ParseNoLogoForRendering(command, declaration);
        var options = Compile(await Generate([definition])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("NoLogo")!;
        await Assert.That(property.PropertyType).IsEqualTo(typeof(bool?));

        var attribute = property.CustomAttributes.Single(item => item.AttributeType.Name == nameof(CliOptionAttribute));
        var switchName = (string) attribute.ConstructorArguments.Single().Value!;
        await Assert.That(switchName).IsEqualTo("--nologo");
        await Assert.That(attribute.NamedArguments.Any(item => item.MemberName == nameof(CliOptionAttribute.ShortForm))).IsFalse();
        PropertyCommandLinePart[] parts = [new OptionPart(property.Name, property.GetValue, new CliOptionAttribute(switchName))];
        var renderer = new CommandArgumentBuilder();

        await Assert.That(renderer.BuildArguments(parts, instance)).IsEmpty();
        property.SetValue(instance, true);
        await Assert.That(renderer.BuildArguments(parts, instance))
            .IsEquivalentTo(["--nologo", "true"], CollectionOrdering.Matching);
        property.SetValue(instance, false);
        await Assert.That(renderer.BuildArguments(parts, instance))
            .IsEquivalentTo(["--nologo", "false"], CollectionOrdering.Matching);
    }
}
