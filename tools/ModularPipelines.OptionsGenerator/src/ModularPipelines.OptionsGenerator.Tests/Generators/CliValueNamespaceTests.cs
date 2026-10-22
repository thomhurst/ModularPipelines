using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

[NotInParallel(nameof(CliValueNamespaceTests))]
public class CliValueNamespaceTests
{
    [Test]
    [MatrixDataSource]
    public async Task Snapshot_Element_Recognition_Accepts_Both_Contract_Namespaces(
        [Matrix("KeyValue", "CliValuePair", "CliOptionValue", "CliValueGroup")] string elementName,
        [Matrix("", "global::ModularPipelines.", "global::ModularPipelines.Models.")] string qualifier)
    {
        await Assert.That(CliOptionDefinition.IsSnapshotElementType($"IEnumerable<{qualifier}{elementName}>?", elementName)).IsTrue();
    }

    [Test]
    [MatrixDataSource]
    public async Task Generated_Options_Work_With_Either_Contract_Namespace(
        [Matrix(false, true)] bool rootNamespace,
        [Matrix("objects", "non-generic", "keys", "pairs", "optional", "groups")] string shape)
    {
        await VerifyConsumer(rootNamespace, shape, positional: false);
    }

    [Test]
    [MatrixDataSource]
    public async Task Generated_Operands_Work_With_Either_Contract_Namespace(
        [Matrix(false, true)] bool rootNamespace,
        [Matrix("objects", "keys")] string shape)
    {
        await VerifyConsumer(rootNamespace, shape, positional: true);
    }

    private static async Task VerifyConsumer(bool rootNamespace, string shape, bool positional)
    {
        var elementType = shape switch
        {
            "pairs" => "CliValuePair",
            "optional" => "CliOptionValue",
            "groups" => "CliValueGroup",
            _ => "KeyValue",
        };
        var tool = CreateTool(shape, positional, elementType);
        var sources = new[]
        {
            (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content,
            (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content,
        };
        foreach (var source in sources)
        {
            await Assert.That(source).DoesNotContain("global::ModularPipelines.Models.");
        }

        var harness = CreateHarness(shape, positional, elementType);
        var assembly = CompileConsumer(rootNamespace, [.. sources, rootNamespace
            ? harness.Replace("ModularPipelines.Models", "ModularPipelines", StringComparison.Ordinal)
            : harness]);
        var expectedNamespace = rootNamespace ? "ModularPipelines" : "ModularPipelines.Models";
        var globalProperty = assembly.GetType("External.Tool.Options.ToolOptions")!.GetProperty("GlobalValues")!;
        if (globalProperty.PropertyType.IsGenericType && shape is not "objects")
        {
            await Assert.That(globalProperty.PropertyType.GenericTypeArguments[0].Namespace).IsEqualTo(expectedNamespace);
        }

        var actual = (string[]) assembly.GetType("Consumer")!.GetMethod("Run")!.Invoke(null, null)!;
        string[] expected = shape switch
        {
            "pairs" => ["--values", "key", "value"],
            "optional" => ["--values"],
            "groups" => ["--values", "first", "second"],
            _ => positional ? ["key=value"] : ["--values", "key=value"],
        };
        await Assert.That(actual.SequenceEqual(expected)).IsTrue();
    }

    private static CliToolDefinition CreateTool(string shape, bool positional, string elementType)
    {
        var propertyType = shape switch
        {
            "objects" => "IReadOnlyList<object>?",
            "non-generic" => "System.Collections.IEnumerable?",
            _ => $"IEnumerable<{elementType}>?",
        };
        var optional = shape is "optional" or "groups";
        var valueOption = new CliOptionDefinition
        {
            SwitchName = "--values",
            PropertyName = "Values",
            CSharpType = propertyType,
            ValueArity = optional ? CliOptionValueArity.Optional : CliOptionValueArity.Required,
            GroupValues = shape == "groups",
        };
        return new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "External.Tool",
            OutputDirectory = "unused",
            GlobalOptionsBeforeSubcommands = false,
            GlobalOptions = [valueOption with { SwitchName = "--global-values", PropertyName = "GlobalValues" }],
            Commands =
            [
                new()
                {
                    FullCommand = "tool run", CommandParts = ["run"], ClassName = "ToolRunOptions",
                    ParentClassName = "ToolOptions", ToolNamespacePrefix = "Tool",
                    Options = positional
                        ? [new() { SwitchName = "--fallback", PropertyName = "Fallback", CSharpType = "string?" }]
                        : [valueOption, new() { SwitchName = "--fallback", PropertyName = "Fallback", CSharpType = "string?" }],
                    PositionalArguments = positional
                        ? [new() { PropertyName = "Values", CSharpType = propertyType, PositionIndex = 0 }]
                        : [],
                    RequiredAlternativeGroups =
                    [
                        new()
                        {
                            Members =
                            [
                                new() { PropertyName = "Values", OptionSwitch = positional ? null : "--values",
                                    PositionalArgumentPhase = positional ? CommandLinePhase.EarlyOperand : null,
                                    PositionalArgumentPositionIndex = positional ? 0 : null },
                                new() { PropertyName = "Fallback", OptionSwitch = "--fallback" },
                            ],
                        },
                    ],
                },
            ],
        };
    }

    private static string CreateHarness(string shape, bool positional, string elementType)
    {
        var optional = shape is "optional" or "groups";
        var payload = shape switch
        {
            "pairs" => "new CliValuePair(\"key\", \"value\")",
            "optional" => "CliOptionValue.Bare",
            "groups" => "new CliValueGroup([\"first\", \"second\"])",
            _ => "new KeyValue(\"key\", \"value\")",
        };
        var part = positional
            ? "new ArgumentPart(\"Values\", property.GetValue, new CliArgumentAttribute(0))"
            : $"new OptionPart(\"Values\", property.GetValue, new CliOptionAttribute(\"--values\") {{ ValueArity = CliOptionValueArity.{(optional ? "Optional" : "Required")}, GroupValues = {(shape == "groups" ? "true" : "false")} }})";
        return $$"""
            using ModularPipelines.Models;
            using ModularPipelines.Generated;
            using ModularPipelines.Attributes;
            using ModularPipelines.Helpers.Internal;
            using System.ComponentModel.DataAnnotations;
            public static class Consumer
            {
                public static string[] Run()
                {
                    var instance = new External.Tool.Options.ToolRunOptions();
                    var property = instance.GetType().GetProperty("Values")!;
                    var validation = (IValidatableObject)instance;
                    if (validation.Validate(new(instance)).Count() != 1) throw new Exception("Missing value accepted.");
                    var input = new SingleUse<{{elementType}}>([{{payload}}]);
                    property.SetValue(instance, input);
                    string[] result = [];
                    for (var pass = 0; pass < 2; pass++)
                    {
                        if (validation.Validate(new(instance)).Any()) throw new Exception("Value rejected.");
                        var rendered = new CommandArgumentBuilder().BuildArguments([{{part}}], instance).ToArray();
                        if (pass == 1 && !rendered.SequenceEqual(result)) throw new Exception("Rendering changed.");
                        result = rendered;
                    }
                    if (input.EnumerationCount != 1) throw new Exception("Input was not snapshotted once.");
                    property.SetValue(instance, Array.Empty<{{elementType}}>());
                    if (validation.Validate(new(instance)).Count() != 1) throw new Exception("Empty value accepted.");
                    return result;
                }
            }
            public sealed class SingleUse<T>(T[] values) : IReadOnlyList<T>
            {
                public int EnumerationCount { get; private set; }
                public int Count => values.Length;
                public T this[int index] => values[index];
                public IEnumerator<T> GetEnumerator()
                {
                    if (++EnumerationCount != 1) throw new Exception("Input enumerated twice.");
                    return ((IEnumerable<T>)values).GetEnumerator();
                }
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }
            """;
    }

    private static Assembly CompileConsumer(bool rootNamespace, string[] generated)
    {
        const string support = """
            global using System;
            global using System.Collections.Generic;
            global using System.Linq;
            namespace ModularPipelines.Models { internal sealed class NamespaceMarker; }
            namespace ModularPipelines.Options { public record CommandLineToolOptions; }
            namespace ModularPipelines.Attributes
            {
                public sealed class CliSubCommandAttribute(params string[] parts) : Attribute;
                public sealed class CliToolAttribute(string name) : Attribute;
                public sealed class CliGlobalOptionsAttribute : Attribute;
            }
            """;
        var runtimeSources = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Fixtures", "NamespaceRuntime"))
            .Select(File.ReadAllText)
            .Select(source => rootNamespace ? source.Replace("ModularPipelines.Models", "ModularPipelines", StringComparison.Ordinal) : source);
        // Exclude both host assemblies so no old CLI contract can accidentally satisfy a reference.
        var excluded = new[] { typeof(CliValueNamespaceTests).Assembly.Location, typeof(CliToolDefinition).Assembly.Location };
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Except(excluded, StringComparer.OrdinalIgnoreCase).Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString("N"),
            runtimeSources.Concat([support]).Concat(generated).Select(static source => CSharpSyntaxTree.ParseText(source,
                CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview))), references,
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
