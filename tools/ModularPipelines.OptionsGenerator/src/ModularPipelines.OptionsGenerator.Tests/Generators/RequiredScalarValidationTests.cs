using System.Reflection;
using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Constructor_Documentation_Matches_Primary_Explicit_And_Alternate_Records()
    {
        var scalar = new CliOptionDefinition
        {
            SwitchName = "--value",
            PropertyName = "Value",
            CSharpType = "int?",
            IsRequired = true,
            Description = "A <value> & description.",
        };
        var assembly = Compile(
            await GenerateScalar(scalar, "PrimaryOptions"),
            await GenerateScalar(scalar with { CSharpType = "string?" }, "ExplicitOptions"),
            await GenerateScalar(scalar with { CSharpType = "string?" }, "AlternateOptions", alternateInput: true));
        await Assert.That(assembly.GetType("ModularPipelines.Tool.Options.PrimaryOptions")).IsNotNull();
        await Assert.That(assembly.GetType("ModularPipelines.Tool.Options.ExplicitOptions")).IsNotNull();
        await Assert.That(assembly.GetType("ModularPipelines.Tool.Options.AlternateOptions")).IsNotNull();
    }

    [Test]
    public async Task Alternate_Factories_Require_A_Selector_After_Mutation_Or_Copy()
    {
        var scalar = new CliOptionDefinition
        {
            SwitchName = "--name",
            PropertyName = "Value",
            CSharpType = "string?",
            IsRequired = true,
        };
        var assembly = Compile(
            await GenerateScalar(scalar, "AlternateOptions", alternateInput: true, includeSkeleton: true),
            await GenerateScalar(scalar, "SkeletonOnlyOptions", includeSkeleton: true),
            """
            namespace ModularPipelines.Tool.Options;
            public static class Consumer
            {
                public static AlternateOptions ClearInput(AlternateOptions options) =>
                    options with { CliInputJson = null };
            }
            """);
        var options = assembly.GetType("ModularPipelines.Tool.Options.AlternateOptions")!;
        static ValidationResult[] Validate(object instance) => instance is IValidatableObject validatable
            ? [.. validatable.Validate(new ValidationContext(instance))]
            : [];

        var json = options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!;
        await Assert.That(Validate(json)).IsEmpty();
        var copy = assembly.GetType("ModularPipelines.Tool.Options.Consumer")!
            .GetMethod("ClearInput")!.Invoke(null, [json])!;
        await Assert.That(Validate(copy)).Count().IsEqualTo(1);
        await Assert.That(Validate(json)).IsEmpty();
        options.GetProperty("CliInputJson")!.SetValue(json, null);
        await Assert.That(Validate(json)).Count().IsEqualTo(1);
        options.GetProperty("GenerateCliSkeleton")!.SetValue(json, "input");
        await Assert.That(Validate(json)).IsEmpty();

        foreach (var mode in new[] { "input", "yaml-input" })
        {
            var skeleton = options.GetMethod("ForCliSkeleton")!.Invoke(null, [mode])!;
            await Assert.That(Validate(skeleton)).IsEmpty();
            foreach (var invalid in new string?[] { null, "output", "" })
            {
                options.GetProperty("GenerateCliSkeleton")!.SetValue(skeleton, invalid);
                await Assert.That(Validate(skeleton)).Count().IsEqualTo(1);
            }
        }

        var ordinary = options.GetConstructors().Single().Invoke(["name"]);
        await Assert.That(Validate(ordinary)).IsEmpty();
        options.GetProperty("GenerateCliSkeleton")!.SetValue(ordinary, "output");
        await Assert.That(Validate(ordinary)).IsEmpty();

        var skeletonOnlyOptions = assembly.GetType("ModularPipelines.Tool.Options.SkeletonOnlyOptions")!;
        var skeletonOnly = skeletonOnlyOptions.GetMethod("ForCliSkeleton")!.Invoke(null, ["input"])!;
        await Assert.That(Validate(skeletonOnly)).IsEmpty();
        skeletonOnlyOptions.GetProperty("GenerateCliSkeleton")!.SetValue(skeletonOnly, null);
        await Assert.That(Validate(skeletonOnly)).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Alternate_Inputs_Bypass_Required_Groups_Only_While_Selected()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [new()
            {
                FullCommand = "tool run", CommandParts = ["run"], ClassName = "AlternateOptions",
                ParentClassName = "ToolOptions", ToolNamespacePrefix = "Tool",
                Options = [
                    new() { SwitchName = "--name", PropertyName = "Name", CSharpType = "string?", IsRequired = true },
                    new() { SwitchName = "--value", PropertyName = "Value", CSharpType = "string?" },
                    new() { SwitchName = "--file", PropertyName = "File", CSharpType = "string?" },
                    new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" },
                    new() { SwitchName = "--generate-cli-skeleton", PropertyName = "GenerateCliSkeleton", CSharpType = "string?" },
                ],
                RequiredAlternativeGroups = [new()
                {
                    Members = [
                        new() { PropertyName = "Value", OptionSwitch = "--value" },
                        new() { PropertyName = "File", OptionSwitch = "--file" },
                    ],
                }],
            }],
        };
        var options = Compile((await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content)
            .GetType("ModularPipelines.Tool.Options.AlternateOptions")!;
        static ValidationResult[] Validate(object instance) =>
            [.. ((IValidatableObject) instance).Validate(new ValidationContext(instance))];

        var normal = options.GetConstructors().Single().Invoke(["name"]);
        await Assert.That(Validate(normal)).Count().IsEqualTo(1);
        options.GetProperty("Value")!.SetValue(normal, "value");
        await Assert.That(Validate(normal)).IsEmpty();

        var json = options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!;
        await Assert.That(Validate(json)).IsEmpty();
        options.GetProperty("CliInputJson")!.SetValue(json, null);
        await Assert.That(Validate(json)).Count().IsEqualTo(1);
        foreach (var mode in new[] { "input", "yaml-input" })
        {
            var skeleton = options.GetMethod("ForCliSkeleton")!.Invoke(null, [mode])!;
            await Assert.That(Validate(skeleton)).IsEmpty();
            options.GetProperty("GenerateCliSkeleton")!.SetValue(skeleton, "output");
            await Assert.That(Validate(skeleton)).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments("IEnumerable<string>")]
    [Arguments("List<string>")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>")]
    public async Task Constructors_Allow_Omitted_Operands_With_Nullable_Contracts(string collectionType)
    {
        List<string> sources = [];
        foreach (var explicitConstructor in new[] { false, true })
        {
            var tool = new CliToolDefinition
            {
                ToolName = "tool",
                NamespacePrefix = "Tool",
                TargetNamespace = "ModularPipelines.Tool",
                OutputDirectory = "src/ModularPipelines.Tool",
                Commands = [new()
                {
                    FullCommand = "tool run",
                    CommandParts = ["run"],
                    ClassName = explicitConstructor ? "ExplicitOptions" : "PrimaryOptions",
                    ParentClassName = "ToolOptions",
                    ToolNamespacePrefix = "Tool",
                    Options = explicitConstructor
                        ? [new() { SwitchName = "--required", PropertyName = "Required", CSharpType = "string?", IsRequired = true }]
                        : [],
                    PositionalArguments = [
                        new() { PropertyName = "Name", CSharpType = "string", PositionIndex = 0, IsRequired = true, IsValidationRequired = false },
                        new() { PropertyName = "Values", CSharpType = collectionType, PositionIndex = 1, IsRequired = true, IsValidationRequired = false, IsVariadic = true },
                    ],
                }],
            };
            sources.Add((await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content);
        }

        var assembly = Compile([.. sources]);
        var nullability = new NullabilityInfoContext();
        foreach (var className in new[] { "PrimaryOptions", "ExplicitOptions" })
        {
            var options = assembly.GetType($"ModularPipelines.Tool.Options.{className}")!;
            var constructor = options.GetConstructors().Single();
            object?[] arguments = className == "ExplicitOptions" ? ["required", null, null] : [null, null];
            var instance = constructor.Invoke(arguments);
            var outputs = new object?[arguments.Length];
            var deconstruct = options.GetMethod("Deconstruct")!;
            deconstruct.Invoke(instance, outputs);
            foreach (var name in new[] { "Name", "Values" })
            {
                var property = options.GetProperty(name)!;
                await Assert.That(property.GetValue(instance)).IsNull();
                await Assert.That(nullability.Create(property).ReadState).IsEqualTo(NullabilityState.Nullable);
                var parameter = constructor.GetParameters().Single(parameter => parameter.Name == name);
                await Assert.That(nullability.Create(parameter).ReadState).IsEqualTo(NullabilityState.Nullable);
                var output = deconstruct.GetParameters().Single(parameter => parameter.Name == name);
                await Assert.That(outputs[output.Position]).IsNull();
                await Assert.That(nullability.Create(output).WriteState).IsEqualTo(NullabilityState.Nullable);
            }

            arguments[^2] = "node";
            var mutableValues = new List<string>();
            arguments[^1] = collectionType.Contains("ImmutableArray", StringComparison.Ordinal)
                ? System.Collections.Immutable.ImmutableArray<string>.Empty
                : mutableValues;
            instance = constructor.Invoke(arguments);
            await Assert.That(options.GetProperty("Name")!.GetValue(instance)).IsEqualTo("node");
            await Assert.That((IEnumerable<string>) options.GetProperty("Values")!.GetValue(instance)!).IsEmpty();

            mutableValues.Add("value");
            arguments[^1] = collectionType.Contains("ImmutableArray", StringComparison.Ordinal)
                ? System.Collections.Immutable.ImmutableArray.Create("value")
                : mutableValues;
            instance = constructor.Invoke(arguments);
            mutableValues.Clear();
            var retained = (IEnumerable<string>) options.GetProperty("Values")!.GetValue(instance)!;
            await Assert.That(retained).IsEquivalentTo(["value"]);
            await Assert.That(retained).IsEquivalentTo(["value"]);

            if (collectionType == "IEnumerable<string>")
            {
                arguments[^1] = new SinglePassValues().Cast<string>();
                instance = constructor.Invoke(arguments);
                retained = (IEnumerable<string>) options.GetProperty("Values")!.GetValue(instance)!;
                await Assert.That(retained).IsEquivalentTo(["first", "second"]);
                await Assert.That(retained).IsEquivalentTo(["first", "second"]);
            }
        }
    }

    [Test]
    public async Task Scalar_Only_Constructors_Validate_Required_References()
    {
        var scalar = new CliOptionDefinition
        {
            SwitchName = "--value",
            PropertyName = "Value",
            CSharpType = "string?",
            IsRequired = true,
        };
        var assembly = Compile(
            await GenerateScalar(scalar, "StringOptions"),
            await GenerateScalar(scalar with { CSharpType = "PrivatePackage.Token?" }, "ReferenceOptions"),
            await GenerateScalar(scalar with { CSharpType = "PrivatePackage.ValueToken?" }, "ValueOptions"));

        foreach (var className in new[] { "StringOptions", "ReferenceOptions" })
        {
            var options = assembly.GetType($"ModularPipelines.Tool.Options.{className}")!;
            var constructor = options.GetConstructors().Single();
            var exception = await Assert.That(() => constructor.Invoke([null]))
                .Throws<TargetInvocationException>();
            await Assert.That(exception!.InnerException).IsTypeOf<ArgumentNullException>();

            var supplied = className == "StringOptions"
                ? "value"
                : Activator.CreateInstance(assembly.GetType("PrivatePackage.Token")!)!;
            var instance = constructor.Invoke([supplied]);
            await Assert.That(options.GetProperty("Value")!.GetValue(instance)).IsEqualTo(supplied);
            var outputs = new object?[1];
            options.GetMethod("Deconstruct")!.Invoke(instance, outputs);
            await Assert.That(outputs[0]).IsEqualTo(supplied);
        }

        var valueOptions = assembly.GetType("ModularPipelines.Tool.Options.ValueOptions")!;
        var value = Activator.CreateInstance(assembly.GetType("PrivatePackage.ValueToken")!);
        var valueInstance = valueOptions.GetConstructors().Single().Invoke([value]);
        await Assert.That(valueOptions.GetProperty("Value")!.GetValue(valueInstance)).IsEqualTo(value);
    }

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
        CliOptionDefinition scalar, string className, bool includeCollection = false, bool alternateInput = false,
        bool includeSkeleton = false)
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

        if (includeSkeleton)
        {
            options.Add(new() { SwitchName = "--generate-cli-skeleton", PropertyName = "GenerateCliSkeleton", CSharpType = "string?" });
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
