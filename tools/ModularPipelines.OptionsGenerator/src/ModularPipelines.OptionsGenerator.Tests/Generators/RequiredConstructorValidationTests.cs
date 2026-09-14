using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class RequiredConstructorValidationTests
{
    private static readonly string[] SingleValue = ["value"];
    private static readonly MetadataReference[] CompilationReferences =
        [.. ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(static path => MetadataReference.CreateFromFile(path))];

    [Test]
    [Arguments("IEnumerable<string>?")]
    [Arguments("CustomValues?")]
    public async Task Required_Collections_Reject_Only_Null_Elements(string collectionType)
    {
        var options = Compile(await Generate(collectionType, alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        object CreateValues(string?[] values) => collectionType == "CustomValues?"
            ? Activator.CreateInstance(options.Assembly.GetType("ModularPipelines.Tool.Options.CustomValues")!, [values])!
            : values;

        var exception = await Assert.That(() => Activator.CreateInstance(options, [CreateValues([null]), "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();

        var instance = Activator.CreateInstance(options, [CreateValues([null, "value"]), "name"]);
        var retained = (IEnumerable<string?>) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.OfType<string>().ToArray()).IsEquivalentTo(["value"]);
    }

    [Test]
    [Arguments("List<string>?")]
    [Arguments("HashSet<string>?")]
    [Arguments("ISet<string>?")]
    [Arguments("IReadOnlySet<string>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Required_Collections_Preserve_Their_Declared_Type(string collectionType)
    {
        var options = Compile(await Generate(collectionType, alternateInput: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        object CreateValues(string[] values) => collectionType switch
        {
            "List<string>?" => values.ToList(),
            "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(values),
            "System.Collections.IEnumerable?" => new System.Collections.ArrayList(values),
            _ => values.ToHashSet(),
        };

        var emptyException = await Assert.That(() => Activator.CreateInstance(options, [CreateValues([]), "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(emptyException!.InnerException).IsTypeOf<ArgumentException>();

        var supplied = CreateValues(["value"]);
        var instance = Activator.CreateInstance(options, [supplied, "name"]);
        var retained = options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.GetType()).IsEqualTo(supplied.GetType());
        await Assert.That(((System.Collections.IEnumerable) retained).Cast<string>().ToArray()).IsEquivalentTo(["value"]);
    }

    [Test]
    [Arguments("IEnumerable<string>?")]
    [Arguments("CustomValues?")]
    public async Task Cli_Json_Factories_Reject_Null_Alternate_Input(string collectionType)
    {
        var generated = await Generate(collectionType, alternateInput: true);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var factory = options.GetMethod("FromCliInputJson")!;

        var exception = await Assert.That(() => factory.Invoke(null, [null]))
            .Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentNullException>();
        await Assert.That(((ArgumentNullException) exception.InnerException!).ParamName).IsEqualTo("cliInputJson");

        foreach (var input in new[] { "{}", "file://parameters.json" })
        {
            var instance = factory.Invoke(null, [input])!;
            await Assert.That(options.GetProperty("CliInputJson")!.GetValue(instance)).IsEqualTo(input);
            await Assert.That(options.GetProperty("Name")!.GetValue(instance)).IsNull();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Required_Custom_Collections_Reject_Null_And_Empty_Values(bool alternateInput)
    {
        var generated = await Generate("CustomValues?", alternateInput);
        var assembly = Compile(generated);
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var values = assembly.GetType("ModularPipelines.Tool.Options.CustomValues")!;
        var empty = Activator.CreateInstance(values, [Array.Empty<string>()]);
        var populated = Activator.CreateInstance(values, [SingleValue]);

        var nullException = await Assert.That(() => Activator.CreateInstance(options, [null, "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(nullException!.InnerException).IsTypeOf<ArgumentNullException>();
        var emptyException = await Assert.That(() => Activator.CreateInstance(options, [empty, "name"]))
            .Throws<TargetInvocationException>();
        await Assert.That(emptyException!.InnerException).IsTypeOf<ArgumentException>();
        var instance = Activator.CreateInstance(options, [populated, "name"]);
        await Assert.That(options.GetProperty("Values")!.GetValue(instance)).IsSameReferenceAs(populated);
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

    private static async Task<string> Generate(string collectionType, bool alternateInput)
    {
        List<CliOptionDefinition> options =
        [
            new() { SwitchName = "--values", PropertyName = "Values", CSharpType = collectionType, IsRequired = true, IsCollection = true },
            new() { SwitchName = "--name", PropertyName = "Name", CSharpType = "string?", IsRequired = true },
        ];
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
            Commands = [new() { FullCommand = "tool run", CommandParts = ["run"], ClassName = "ToolRunOptions", ParentClassName = "ToolOptions", ToolNamespacePrefix = "Tool", Options = options }],
        };
        return (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
    }

    private static Assembly Compile(string generated)
    {
        const string support = """
            global using System.Collections.Generic;
            namespace ModularPipelines.Attributes
            {
                public sealed class CliOptionAttribute(string name) : System.Attribute;
                public sealed class CliSubCommandAttribute(params string[] parts) : System.Attribute;
            }
            namespace ModularPipelines.Tool.Options
            {
                public record ToolOptions;
                public sealed class CustomValues(string[] values) : IEnumerable<string>
                {
                    public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)values).GetEnumerator();
                    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }
            """;
        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString("N"),
            [CSharpSyntaxTree.ParseText(support), CSharpSyntaxTree.ParseText(generated)], CompilationReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        return System.Reflection.Assembly.Load(stream.ToArray());
    }
}
