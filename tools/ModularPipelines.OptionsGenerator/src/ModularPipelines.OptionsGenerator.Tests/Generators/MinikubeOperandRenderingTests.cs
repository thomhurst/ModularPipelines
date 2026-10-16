using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("image", "pull")]
    [Arguments("image", "push")]
    [Arguments("image", "tag")]
    [Arguments("cache", "add")]
    [Arguments("cache", "delete")]
    public async Task Minikube_Omitted_Operands_Compile_And_Render_In_Order(string group, string action)
    {
        var parsed = await MinikubeMetadataTests.ParseFixture(group, action);
        var options = Compile(await Generate([], parsed.PositionalArguments))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] images = ["registry.example/source:tag", "registry.example/target:tag"];
        var instance = action == "tag"
            ? Activator.CreateInstance(options, [images[0], images[1]])!
            : Activator.CreateInstance(options)!;
        if (action != "tag")
        {
            options.GetProperty("Images")!.SetValue(instance, images);
        }

        var parts = parsed.PositionalArguments.Select(argument =>
        {
            var property = options.GetProperty(argument.PropertyName)!;
            var attribute = property.CustomAttributes.Single(item => item.AttributeType.Name == nameof(CliArgumentAttribute));
            var index = (int) attribute.ConstructorArguments.Single().Value!;
            var phase = (CommandLinePhase) attribute.NamedArguments.Single(item => item.MemberName == nameof(CliArgumentAttribute.Phase)).TypedValue.Value!;
            var required = attribute.NamedArguments.FirstOrDefault(item => item.MemberName == nameof(CliArgumentAttribute.Required)).TypedValue.Value is true;
            return new ArgumentPart(property.Name, property.GetValue, new CliArgumentAttribute(index) { Phase = phase, Required = required });
        }).ToArray<PropertyCommandLinePart>();

        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(images, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        if (action != "tag")
        {
            var omitted = Activator.CreateInstance(options)!;
            await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, omitted)).IsEmpty();
        }
    }
}
