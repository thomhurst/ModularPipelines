using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class RuntimeMetadataIsolationTests
{
    private const string Infrastructure = """
        [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Consumer")]

        namespace ModularPipelines.Options
        {
            public abstract record CommandLineToolOptions;
        }

        namespace ModularPipelines.Attributes
        {
            public enum CommandLinePhase { Normal }

            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class CliFlagAttribute : System.Attribute
            {
                public CliFlagAttribute(string name) { }
                public string? ShortForm { get; set; }
                public string? NegatedName { get; set; }
                public bool PreferShortForm { get; set; }
                public CommandLinePhase Phase { get; set; }
            }
        }

        namespace ModularPipelines.Secrets
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class SecretValueAttribute : System.Attribute;
        }

        namespace ModularPipelines.Generated
        {
            public abstract class PropertyCommandLinePart;

            public sealed class FlagPart : PropertyCommandLinePart
            {
                public FlagPart(string name, System.Func<object, object?> getter,
                    ModularPipelines.Attributes.CliFlagAttribute attribute) { }
                public bool IsGlobalOption { get; set; }
                public bool IsSupportedPropertyType { get; set; }
            }

            public sealed class SecretPropertyAccessor
            {
                public SecretPropertyAccessor(string name, System.Func<object, object?> getter, string[] keys) { }
            }

            public static class GeneratedCommandMetadata
            {
                public static void RegisterAssembly(System.Reflection.Assembly assembly, bool requiresGeneratedMetadata) { }
                public static void RegisterCoveredTypeNames(System.Reflection.Assembly assembly, string[] names) { }
                public static void Register(System.Type type, PropertyCommandLinePart[] parts, int schemaVersion) { }
            }

            public static class GeneratedSecretMetadata
            {
                public static void RegisterAssembly(System.Reflection.Assembly assembly, bool requiresGeneratedMetadata) { }
                public static void RegisterCoveredTypeNames(System.Reflection.Assembly assembly, string[] names) { }
                public static void Register(System.Type type, SecretPropertyAccessor[] parts, int schemaVersion) { }
            }
        }
        """;

    [Test]
    [Arguments(2, 1)]
    [Arguments(33, 33)]
    public async Task Friend_Assembly_Can_Generate_Shared_Metadata_Without_Type_Conflicts(
        int derivedCount,
        int propertyCount)
    {
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path))
            .ToArray();
        var producer = Generate(CSharpCompilation.Create(
            "ModularPipelines",
            [CSharpSyntaxTree.ParseText(Infrastructure), CSharpSyntaxTree.ParseText(CreateOptions("Producer", derivedCount, propertyCount))],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable)));
        using var producerStream = new MemoryStream();
        var producerEmit = producer.Emit(producerStream);
        await Assert.That(producerEmit.Diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
            .IsEmpty();
        await Assert.That(producerEmit.Success).IsTrue();
        var producerReference = MetadataReference.CreateFromImage(producerStream.ToArray());
        var consumer = Generate(CSharpCompilation.Create(
            "Consumer",
            [CSharpSyntaxTree.ParseText(CreateOptions("Consumer", derivedCount, propertyCount))],
            [.. references, producerReference],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable)));
        using var consumerStream = new MemoryStream();
        var consumerEmit = consumer.Emit(consumerStream);

        using (Assert.Multiple())
        {
            await Assert.That(consumerEmit.Diagnostics.Where(static diagnostic => diagnostic.Id == "CS0436"))
                .IsEmpty();
            await Assert.That(consumerEmit.Diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
                .IsEmpty();
            await Assert.That(consumerEmit.Success).IsTrue();
        }
    }

    private static Compilation Generate(CSharpCompilation compilation)
    {
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new CommandOptionsGenerator());
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out var diagnostics);
        if (diagnostics.Any(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, diagnostics));
        }

        return output;
    }

    private static string CreateOptions(string namespaceName, int derivedCount, int propertyCount)
    {
        var properties = string.Join(Environment.NewLine, Enumerable.Range(0, propertyCount).Select(index => $$"""
            [ModularPipelines.Attributes.CliFlag("--flag{{index}}")]
            [ModularPipelines.Secrets.SecretValue]
            public bool Flag{{index}} { get; set; }
            """));
        var derivedTypes = string.Join(Environment.NewLine, Enumerable.Range(0, derivedCount).Select(index =>
            $"public sealed record Options{index} : BaseOptions;"));
        return $$"""
            namespace {{namespaceName}};
            public abstract record BaseOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                {{properties}}
            }
            {{derivedTypes}}
            """;
    }
}
