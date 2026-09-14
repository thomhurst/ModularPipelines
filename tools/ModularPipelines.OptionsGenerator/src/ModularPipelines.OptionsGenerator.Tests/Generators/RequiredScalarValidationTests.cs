using System.Reflection;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Required_Negated_Flags_Preserve_Renderer_And_Deconstruction_Contracts()
    {
        var flag = new CliOptionDefinition
        {
            SwitchName = "--enabled",
            NegatedSwitchName = "--no-enabled",
            PropertyName = "Value",
            CSharpType = "bool?",
            IsFlag = true,
            IsRequired = true,
        };
        var assembly = Compile(
            await GenerateScalar(flag, "FlagOptions"),
            await GenerateScalar(flag, "CollectionFlagOptions", includeCollection: true),
            await GenerateScalar(flag, "AlternateFlagOptions", alternateInput: true));

        foreach (var className in new[] { "FlagOptions", "CollectionFlagOptions", "AlternateFlagOptions" })
        {
            var options = assembly.GetType($"ModularPipelines.Tool.Options.{className}")!;
            var property = options.GetProperty("Value")!;
            await Assert.That(property.PropertyType).IsEqualTo(typeof(bool?));
            var constructor = options.GetConstructors().Single();
            await Assert.That(constructor.GetParameters()[0].ParameterType).IsEqualTo(typeof(bool));

            foreach (var value in new[] { true, false })
            {
                object?[] arguments = className == "CollectionFlagOptions" ? [value, SingleValue] : [value];
                var instance = constructor.Invoke(arguments);
                await Assert.That(property.GetValue(instance)).IsEqualTo(value);
                if (className != "AlternateFlagOptions")
                {
                    var deconstruct = options.GetMethod("Deconstruct")!;
                    var outputs = new object?[arguments.Length];
                    deconstruct.Invoke(instance, outputs);
                    await Assert.That(outputs[0]).IsEqualTo(value);
                }
            }
        }
    }

    [Test]
    public async Task Explicit_Constructors_Validate_Target_Only_Scalars()
    {
        var scalar = new CliOptionDefinition
        {
            SwitchName = "--token",
            PropertyName = "Value",
            CSharpType = "PrivatePackage.Token?",
            IsRequired = true,
        };
        var assembly = Compile(
            await GenerateScalar(scalar, "ReferenceOptions", includeCollection: true),
            await GenerateScalar(scalar, "AlternateReferenceOptions", alternateInput: true),
            await GenerateScalar(scalar with { CSharpType = "PrivatePackage.ValueToken?" }, "ValueOptions", includeCollection: true));

        foreach (var className in new[] { "ReferenceOptions", "AlternateReferenceOptions" })
        {
            var options = assembly.GetType($"ModularPipelines.Tool.Options.{className}")!;
            object?[] arguments = className == "ReferenceOptions" ? [null, SingleValue] : [null];
            var exception = await Assert.That(() => options.GetConstructors().Single().Invoke(arguments))
                .Throws<TargetInvocationException>();
            await Assert.That(exception!.InnerException).IsTypeOf<ArgumentNullException>();
        }

        var valueOptions = assembly.GetType("ModularPipelines.Tool.Options.ValueOptions")!;
        var token = Activator.CreateInstance(assembly.GetType("PrivatePackage.ValueToken")!);
        var instance = valueOptions.GetConstructors().Single().Invoke([token, SingleValue]);
        await Assert.That(valueOptions.GetProperty("Value")!.GetValue(instance)).IsEqualTo(token);
    }

    private static async Task<string> GenerateScalar(
        CliOptionDefinition scalar, string className, bool includeCollection = false, bool alternateInput = false)
    {
        List<CliOptionDefinition> options = [scalar];
        if (includeCollection)
        {
            options.Add(new() { SwitchName = "--values", PropertyName = "Values", CSharpType = "IEnumerable<string>?", IsRequired = true });
        }

        if (alternateInput)
        {
            options.Add(new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" });
        }

        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [new() { FullCommand = "tool run", CommandParts = ["run"], ClassName = className, ParentClassName = "ToolOptions", ToolNamespacePrefix = "Tool", Options = options }],
        };
        return (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
    }
}
