using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("set")]
    [Arguments("get")]
    [Arguments("unset")]
    [Arguments("param-persist delete")]
    public async Task Azure_Config_Positionals_Compile_And_Render_Without_Switches(string command)
    {
        var parsed = await AzPositionalArgumentTests.ParseFixture(command);
        var argument = parsed.PositionalArguments.Single();
        var options = Compile(await Generate([], parsed.PositionalArguments))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] values = ["core.no_color"];
        if (command == "set")
        {
            values = ["defaults.location=westus2", "defaults.group=group with spaces"];
        }
        else if (argument.IsVariadic)
        {
            values = ["core.no_color", "core.only_show_errors"];
        }
        object supplied = argument.IsVariadic ? values : values.Single();
        var instance = Activator.CreateInstance(options, argument.IsRequired ? [supplied] : [])!;
        var property = options.GetProperty(argument.PropertyName)!;
        if (!argument.IsRequired)
        {
            property.SetValue(instance, supplied);
        }

        var attribute = property.CustomAttributes.Single(item => item.AttributeType.Name == nameof(CliArgumentAttribute));
        var required = attribute.NamedArguments.FirstOrDefault(item => item.MemberName == nameof(CliArgumentAttribute.Required)).TypedValue.Value is true;
        var phase = (CommandLinePhase) attribute.NamedArguments.Single(item => item.MemberName == nameof(CliArgumentAttribute.Phase)).TypedValue.Value!;
        var part = new ArgumentPart(property.Name, property.GetValue, new CliArgumentAttribute(0) { Required = required, Phase = phase });
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo(values,
            TUnit.Assertions.Enums.CollectionOrdering.Matching);

        if (argument.IsRequired)
        {
            var exception = await Assert.That(() => Activator.CreateInstance(options, [Array.Empty<string>()]))
                .Throws<TargetInvocationException>();
            await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
        }
    }

    [Test]
    [Arguments("get")]
    [Arguments("param-persist delete")]
    public async Task Azure_Config_Optional_Positionals_Permit_Omission(string command)
    {
        var parsed = await AzPositionalArgumentTests.ParseFixture(command);
        var argument = parsed.PositionalArguments.Single();
        var options = Compile(await Generate([.. parsed.Options.Where(option => option.SwitchName == "--all")], parsed.PositionalArguments))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        List<PropertyCommandLinePart> parts =
        [
            new ArgumentPart(argument.PropertyName, options.GetProperty(argument.PropertyName)!.GetValue,
                new CliArgumentAttribute(0) { Phase = argument.Phase }),
        ];
        if (command == "param-persist delete")
        {
            var all = options.GetProperty("All")!;
            all.SetValue(instance, true);
            parts.Add(new FlagPart(all.Name, all.GetValue, new CliFlagAttribute("--all")));
        }

        string[] expected = command == "get" ? [] : ["--all"];
        await Assert.That(new CommandArgumentBuilder().BuildArguments([.. parts], instance)).IsEquivalentTo(expected,
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
