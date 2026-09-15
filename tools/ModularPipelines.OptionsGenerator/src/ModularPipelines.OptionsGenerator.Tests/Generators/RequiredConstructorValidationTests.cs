using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] SingleValue = ["value"];
    private static readonly MetadataReference[] CompilationReferences =
        [.. ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(static path => MetadataReference.CreateFromFile(path))];

    [Test]
    [Arguments(false, false, false)]
    [Arguments(false, true, false)]
    [Arguments(true, false, false)]
    [Arguments(true, true, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, true)]
    [Arguments(true, false, true)]
    [Arguments(true, true, true)]
    public async Task Required_Properties_Win_Collisions_Regardless_Of_Source_Order(
        bool requiredFirst, bool alternateInput, bool sameCase)
    {
        List<CliOptionDefinition> definitions =
        [
            new() { SwitchName = "--optional-name", PropertyName = sameCase ? "Name" : "NAME", CSharpType = "string?" },
            new() { SwitchName = "--required-name", PropertyName = "Name", CSharpType = "string?", IsRequired = true },
        ];
        if (requiredFirst)
        {
            definitions.Reverse();
        }

        if (alternateInput)
        {
            definitions.Add(new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" });
        }

        var options = Compile(await Generate(definitions)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var parameter = options.GetConstructors().Single().GetParameters().Single();
        var property = options.GetProperty(parameter.Name!)!;
        var attribute = property.CustomAttributes.Single(value => value.AttributeType.Name == "CliOptionAttribute");
        await Assert.That(attribute.ConstructorArguments.Single().Value).IsEqualTo("--required-name");
        await Assert.That(property.SetMethod!.IsPrivate).IsTrue();
        await Assert.That(new NullabilityInfoContext().Create(property).ReadState)
            .IsEqualTo(alternateInput ? NullabilityState.Nullable : NullabilityState.NotNull);
        var instance = Activator.CreateInstance(options, ["name"]);
        await Assert.That(property.GetValue(instance)).IsEqualTo("name");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Required_Flags_Require_An_Emitted_Switch(bool negatable, bool alternateInput)
    {
        List<CliOptionDefinition> definitions =
        [
            new() { SwitchName = "--enabled", PropertyName = "Enabled", CSharpType = "bool?", IsFlag = true, IsRequired = true,
                NegatedSwitchName = negatable ? "--no-enabled" : null },
        ];
        if (alternateInput)
        {
            definitions.Add(new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" });
        }

        var options = Compile(await Generate(definitions)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        await Assert.That(options.GetConstructors().Single().GetParameters().Single().ParameterType).IsEqualTo(typeof(bool));
        var enabled = Activator.CreateInstance(options, [true]);
        await Assert.That((bool) options.GetProperty("Enabled")!.GetValue(enabled)!).IsTrue();
        if (negatable)
        {
            var disabled = Activator.CreateInstance(options, [false]);
            await Assert.That((bool) options.GetProperty("Enabled")!.GetValue(disabled)!).IsFalse();
        }
        else
        {
            var exception = await Assert.That(() => Activator.CreateInstance(options, [false]))
                .Throws<TargetInvocationException>();
            await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
            await Assert.That(((ArgumentException) exception.InnerException!).ParamName).IsEqualTo("Enabled");
        }

        if (alternateInput)
        {
            var instance = options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!;
            var validation = (System.ComponentModel.DataAnnotations.IValidatableObject) instance;
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Required_Validation_Follows_Parameter_Order(bool scalarFirst)
    {
        var options = Compile(await Generate("IEnumerable<string>?", alternateInput: false, scalarFirst))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var exception = await Assert.That(() => Activator.CreateInstance(options, [null, null]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentNullException>();
        await Assert.That(((ArgumentNullException) exception.InnerException!).ParamName)
            .IsEqualTo(scalarFirst ? "Name" : "Values");
    }

    [Test]
    [Arguments("IReadOnlyList<KeyValue>?")]
    [Arguments("List<KeyValue>?")]
    [Arguments("HashSet<KeyValue>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<KeyValue>?")]
    public async Task Required_Domain_Collections_Compile_And_Retain_Values(string collectionType)
    {
        var assembly = Compile(await Generate(collectionType, alternateInput: false, isCollection: null));
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var elementType = typeof(ModularPipelines.Models.KeyValue);
        var values = Array.CreateInstance(elementType, 1);
        values.SetValue(new ModularPipelines.Models.KeyValue("key", "value"), 0);
        var listType = typeof(List<>).MakeGenericType(elementType);
        var supplied = (System.Collections.IList) Activator.CreateInstance(listType, [values])!;
        object argument = supplied;
        if (collectionType.StartsWith("HashSet", StringComparison.Ordinal))
        {
            argument = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(elementType), [values])!;
        }
        else if (collectionType.Contains("ImmutableArray", StringComparison.Ordinal))
        {
            argument = typeof(System.Collections.Immutable.ImmutableArray).GetMethods()
                .Single(method => method.Name == "CreateRange" && method.IsGenericMethodDefinition
                                  && method.GetParameters() is [{ ParameterType.IsGenericType: true } parameter]
                                  && parameter.ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .MakeGenericMethod(elementType).Invoke(null, [values])!;
        }

        var instance = Activator.CreateInstance(options, [argument, "name"]);
        supplied.Clear();
        var retained = (System.Collections.IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<object>().ToArray()).IsEquivalentTo(values.Cast<object>());
        var exception = await Assert.That(() => Activator.CreateInstance(options, [null, "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
    }

    [Test]
    public async Task Required_Domain_List_Without_Collection_Metadata_Rejects_Empty_And_AllNull_Input()
    {
        var assembly = Compile(await Generate("IReadOnlyList<KeyValue>?", alternateInput: false, isCollection: null));
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var elementType = typeof(ModularPipelines.Models.KeyValue);

        foreach (var count in new[] { 0, 1 })
        {
            var values = Array.CreateInstance(elementType, count);
            var exception = await Assert.That(() => Activator.CreateInstance(options, [values, "name"]))
                .Throws<TargetInvocationException>();
            await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
            await Assert.That(((ArgumentException) exception.InnerException!).ParamName).IsEqualTo("Values");
        }
    }

    [Test]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.Immutable.IImmutableList<string>?")]
    public async Task Required_Immutable_Collections_Reject_Default_Arrays(string collectionType)
    {
        var options = Compile(await Generate(collectionType, alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        object values = default(System.Collections.Immutable.ImmutableArray<string>);
        var exception = await Assert.That(() => Activator.CreateInstance(options, [values, "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
        await Assert.That(((ArgumentException) exception.InnerException!).ParamName).IsEqualTo("Values");

        var valid = System.Collections.Immutable.ImmutableArray.Create("value");
        var instance = Activator.CreateInstance(options, [valid, "name"]);
        var retained = (IEnumerable<string>) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained).IsEquivalentTo(["value"]);
    }

    [Test]
    public async Task Required_NonGeneric_Collections_Preserve_SinglePass_Values()
    {
        var options = Compile(await Generate("System.Collections.IEnumerable?", alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options, [new SinglePassValues(), "name"]);
        var retained = (System.Collections.IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<string>().ToArray()).IsEquivalentTo(["first", "second"]);
        await Assert.That(retained.Cast<string>().ToArray()).IsEquivalentTo(["first", "second"]);
    }

    private sealed class SinglePassValues : System.Collections.IEnumerable
    {
        private readonly System.Collections.IEnumerator _values = new[] { "first", "second" }.GetEnumerator();

        public System.Collections.IEnumerator GetEnumerator() => _values;
    }

    [Test]
    public async Task Required_Set_Snapshots_Preserve_Comparer_And_Values()
    {
        var options = Compile(await Generate("ISet<string>?", alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var first = new string('x', 1);
        var second = new string('x', 1);
        var supplied = new HashSet<string>(ReferenceEqualityComparer.Instance) { first, second };
        var instance = Activator.CreateInstance(options, [supplied, "name"]);
        var retained = (HashSet<string>) options.GetProperty("Values")!.GetValue(instance)!;
        supplied.Clear();
        await Assert.That(retained.Count).IsEqualTo(2);
        await Assert.That(retained.Comparer).IsSameReferenceAs(ReferenceEqualityComparer.Instance);

        var exception = await Assert.That(() => Activator.CreateInstance(options, [new SortedSet<string> { "value" }, "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();
    }

    [Test]
    [Arguments("IEnumerable<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Required_Collections_Reject_Only_Null_Elements(string collectionType)
    {
        var options = Compile(await Generate(collectionType, alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        static object CreateValues(string?[] values) => values;

        var exception = await Assert.That(() => Activator.CreateInstance(options, [CreateValues([null]), "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();

        var instance = Activator.CreateInstance(options, [CreateValues([null, "value"]), "name"]);
        var retained = (System.Collections.IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.OfType<string>().ToArray()).IsEquivalentTo(["value"]);
    }

    [Test]
    [Arguments("List<string>?")]
    [Arguments("HashSet<string>?")]
    [Arguments("ISet<string>?")]
    [Arguments("IReadOnlySet<string>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.Immutable.IImmutableList<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    [Arguments("System.Collections.ArrayList?")]
    public async Task Required_Collections_Preserve_Their_Declared_Type(string collectionType)
    {
        var options = Compile(await Generate(collectionType, alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        object CreateValues(string[] values) => collectionType switch
        {
            "List<string>?" => values.ToList(),
            "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(values),
            "System.Collections.Immutable.IImmutableList<string>?" => System.Collections.Immutable.ImmutableList.CreateRange(values),
            "System.Collections.IEnumerable?" or "System.Collections.ArrayList?" => new System.Collections.ArrayList(values),
            _ => values.ToHashSet(),
        };

        var emptyException = await Assert.That(() => Activator.CreateInstance(options, [CreateValues([]), "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(emptyException!.InnerException).IsTypeOf<ArgumentException>();

        var supplied = CreateValues(["value"]);
        var instance = Activator.CreateInstance(options, [supplied, "name"]);
        var retained = options.GetProperty("Values")!.GetValue(instance)!;
        if (supplied is System.Collections.IList { IsReadOnly: false, IsFixedSize: false } list)
        {
            list.Clear();
        }
        else if (supplied is ISet<string> set)
        {
            set.Clear();
        }

        await Assert.That(options.GetProperty("Values")!.PropertyType.IsInstanceOfType(retained)).IsTrue();
        await Assert.That(((System.Collections.IEnumerable) retained).Cast<string>().ToArray()).IsEquivalentTo(["value"]);
    }

    [Test]
    [Arguments("IEnumerable<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Cli_Json_Factories_Reject_Blank_Alternate_Input(string collectionType)
    {
        var generated = await Generate(collectionType, alternateInput: true);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var factory = options.GetMethod("FromCliInputJson")!;

        var exception = await Assert.That(() => factory.Invoke(null, [null]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentNullException>();
        await Assert.That(((ArgumentNullException) exception.InnerException!).ParamName).IsEqualTo("cliInputJson");

        foreach (var input in new[] { "", " \t\r\n" })
        {
            var blankException = await Assert.That(() => factory.Invoke(null, [input]))
                .Throws<TargetInvocationException>();
            await Assert.That(blankException!.InnerException).IsTypeOf<ArgumentException>();
            await Assert.That(((ArgumentException) blankException.InnerException!).ParamName).IsEqualTo("cliInputJson");
        }

        foreach (var input in new[] { "{}", "file://parameters.json" })
        {
            var instance = factory.Invoke(null, [input])!;
            await Assert.That(options.GetProperty("CliInputJson")!.GetValue(instance)).IsEqualTo(input);
            await Assert.That(options.GetProperty("Name")!.GetValue(instance)).IsNull();
            var validation = (System.ComponentModel.DataAnnotations.IValidatableObject) instance;
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
            foreach (var blank in new string?[] { null, "", " \t\r\n" })
            {
                options.GetProperty("CliInputJson")!.SetValue(instance, blank);
                await Assert.That(validation.Validate(new(instance))).IsNotEmpty();
            }
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Unsupported_Required_Collections_Fail_Generation(bool alternateInput)
    {
        var exception = await Assert.That(async () => { await Generate("CustomValues?", alternateInput); })
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("cannot safely retain a reusable snapshot");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Explicit_Constructors_Reject_Null_Strings(bool alternateInput)
    {
        var generated = await Generate("IEnumerable<string>?", alternateInput);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

        var nullException = await Assert.That(() => Activator.CreateInstance(options, [SingleValue, null]))
            .Throws<TargetInvocationException>();
        await Assert.That(nullException!.InnerException).IsTypeOf<ArgumentNullException>();
        if (alternateInput)
        {
            var instance = options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!;
            await Assert.That(options.GetProperty("Name")!.GetValue(instance)).IsNull();
            await Assert.That(options.GetMethod("Deconstruct")).IsNull();
        }
    }

    private static async Task<string> Generate(string collectionType, bool alternateInput, bool scalarFirst = false, bool? isCollection = true)
    {
        List<CliOptionDefinition> options =
        [
            new() { SwitchName = "--values", PropertyName = "Values", CSharpType = collectionType, IsRequired = true, IsCollection = isCollection },
            new() { SwitchName = "--name", PropertyName = "Name", CSharpType = "string?", IsRequired = true },
        ];
        if (scalarFirst)
        {
            options.Reverse();
        }

        if (alternateInput)
        {
            options.Add(new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" });
        }

        return await Generate(options);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Duplicate_Operands_Keep_Alternate_Input_Validation_Consistent(bool firstRequired, bool secondRequired)
    {
        var generated = await Generate(
        [
            new() { SwitchName = "--name", PropertyName = "Name", CSharpType = "string?", IsRequired = true },
            new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" },
        ],
        [
            new() { PropertyName = "Path", CSharpType = "string?", PositionIndex = 0, IsRequired = firstRequired },
            new() { PropertyName = "Path", CSharpType = "string?", PositionIndex = 1, IsRequired = secondRequired },
        ]);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var supportsAlternateInput = !firstRequired && !secondRequired;
        await Assert.That(typeof(System.ComponentModel.DataAnnotations.IValidatableObject).IsAssignableFrom(options))
            .IsEqualTo(supportsAlternateInput);
        await Assert.That(options.GetMethod("FromCliInputJson") is not null).IsEqualTo(supportsAlternateInput);
        var instance = Activator.CreateInstance(options, supportsAlternateInput ? ["name"] : ["name", "path"]);
        await Assert.That(options.GetProperty("Path")!.GetValue(instance)).IsEqualTo(supportsAlternateInput ? null : "path");
    }

    private static async Task<string> Generate(List<CliOptionDefinition> options, IReadOnlyList<CliPositionalArgument>? positionalArguments = null, IReadOnlyList<CliRequiredAlternativeGroup>? alternativeGroups = null)
    {
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [new() { FullCommand = "tool run", CommandParts = ["run"], ClassName = "ToolRunOptions", ParentClassName = "ToolOptions", ToolNamespacePrefix = "Tool", Options = options, PositionalArguments = positionalArguments ?? [], RequiredAlternativeGroups = alternativeGroups ?? [] }],
        };
        return (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
    }

    private static Assembly Compile(params string[] generated)
    {
        const string support = """
            global using System.Collections.Generic;
            global using System.Linq;
            namespace ModularPipelines.Attributes
            {
                public sealed class CliOptionAttribute(string name) : System.Attribute;
                public sealed class CliSubCommandAttribute(params string[] parts) : System.Attribute;
                public sealed class CliArgumentAttribute(int position) : System.Attribute
                {
                    public CommandLinePhase Phase { get; set; }
                    public bool IsVariadic { get; set; }
                    public bool Required { get; set; }
                }
                public sealed class CliFlagAttribute(string name) : System.Attribute
                {
                    public string? NegatedName { get; set; }
                }
            }
            namespace PrivatePackage
            {
                public sealed class Token;
                public readonly record struct ValueToken(int Value);
            }
            namespace ModularPipelines.Tool.Options
            {
                public record ToolOptions;
            }
            """;
        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString("N"),
            [CSharpSyntaxTree.ParseText(support, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview)), .. generated.Select(static source => CSharpSyntaxTree.ParseText(source,
                CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview).WithDocumentationMode(DocumentationMode.Diagnose)))], CompilationReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        var invalidDocumentation = result.Diagnostics.Where(static diagnostic =>
            diagnostic.Id is "CS1572" or "CS1573" or "CS1587").ToArray();
        if (!result.Success || invalidDocumentation.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        return System.Reflection.Assembly.Load(stream.ToArray());
    }
}
