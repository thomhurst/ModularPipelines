using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("string", true, false)]
    [Arguments("object", true, false)]
    [Arguments("IEnumerable<string>", true, false)]
    [Arguments("IEnumerable<string>", true, true)]
    [Arguments("string[]", true, false)]
    [Arguments("string[]", true, true)]
    [Arguments("int", false, false)]
    [Arguments("bool", false, false)]
    [Arguments("System.DateTime", false, false)]
    public async Task Required_Override_Uses_Compatible_Global_Storage(string propertyType, bool sharesStorage, bool rejectBlankCollectionValues)
    {
        var global = new CliOptionDefinition
        {
            SwitchName = "--project",
            ShortForm = "-p",
            PropertyName = "Project",
            CSharpType = propertyType + "?",
            ValueSeparator = "=",
        };
        var local = global with { ShortForm = null, IsRequired = true, Description = "The command-specific project selector.", RejectBlankCollectionValues = rejectBlankCollectionValues };
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [local],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            GlobalOptionsBeforeSubcommands = false,
            GlobalOptions = [global],
            Commands = [command],
        };
        var baseSource = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        var source = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(source).Contains("The command-specific project selector.");
        await Assert.That(source).DoesNotContain("ShortForm = \"-p\"");
        var assembly = Compile(baseSource, source);
        var baseType = assembly.GetType("ModularPipelines.Tool.Options.ToolOptions")!;
        var derivedType = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var baseProperty = baseType.GetProperty("Project")!;
        var derivedProperty = derivedType.GetProperty("Project", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
        object Value(int index) => propertyType switch
        {
            "IEnumerable<string>" or "string[]" => new[] { index.ToString() },
            "int" => index,
            "bool" => index % 2 == 0,
            "System.DateTime" => new DateTime(2026, 1, index),
            _ => index.ToString(),
        };

        var instance = Activator.CreateInstance(derivedType, [Value(1)]);
        if (sharesStorage)
        {
            await Assert.That(baseProperty.GetValue(instance)).IsEqualTo(derivedProperty.GetValue(instance));
            if (propertyType == "string")
            {
                await Assert.That(baseProperty.GetValue(instance)).IsEqualTo("1");
            }

            await Assert.That(new NullabilityInfoContext().Create(derivedProperty).ReadState).IsEqualTo(NullabilityState.NotNull);
            baseProperty.SetValue(instance, Value(2));
            await Assert.That(derivedProperty.GetValue(instance)).IsEqualTo(baseProperty.GetValue(instance));
            if (propertyType == "string")
            {
                await Assert.That(derivedProperty.GetValue(instance)).IsEqualTo("2");
                var metadata = derivedProperty.CustomAttributes.Single(attribute => attribute.AttributeType.Name == nameof(CliOptionAttribute));
                var format = (OptionFormat) metadata.NamedArguments.Single(argument => argument.MemberName == nameof(CliOptionAttribute.Format)).TypedValue.Value!;
                var part = new OptionPart(derivedProperty.Name, derivedProperty.GetValue,
                    new CliOptionAttribute((string) metadata.ConstructorArguments.Single().Value!) { Format = format });
                await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance!)).IsEquivalentTo(["--project=2"]);
            }

            derivedProperty.SetValue(instance, Value(3));
            await Assert.That(baseProperty.GetValue(instance)).IsEqualTo(derivedProperty.GetValue(instance));
            if (rejectBlankCollectionValues)
            {
                var snapshotMethod = derivedType.GetMethod(OptionsClassGenerator.GetGlobalCollectionSnapshotMethodName("Project"),
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
                var nullability = new NullabilityInfoContext();
                await Assert.That(nullability.Create(snapshotMethod.ReturnParameter).ReadState).IsEqualTo(NullabilityState.Nullable);
                await Assert.That(nullability.Create(snapshotMethod.GetParameters().Single()).ReadState).IsEqualTo(NullabilityState.Nullable);
            }

            await Assert.That(() => Activator.CreateInstance(derivedType, [null]))
                .Throws<TargetInvocationException>().WithInnerException<ArgumentNullException>();
        }
        else
        {
            await Assert.That(baseProperty.GetValue(instance)).IsNull();
            baseProperty.SetValue(instance, Value(2));
            await Assert.That(derivedProperty.GetValue(instance)).IsEqualTo(Value(1));
            derivedProperty.SetValue(instance, Value(3));
            await Assert.That(baseProperty.GetValue(instance)).IsEqualTo(Value(2));
        }
    }
}
