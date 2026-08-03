using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a single CLI option or flag.
/// </summary>
public record CliOptionDefinition
{
    private const string CollectionProbeTypeName = "CollectionShapeProbe.Probe";
    private static readonly ConcurrentDictionary<string, bool> CollectionShapes = new(StringComparer.Ordinal);
    private static readonly Lazy<CSharpCompilation> CollectionProbeCompilation = new(CreateCollectionProbeCompilation);

    /// <summary>
    /// The CLI switch name (e.g., "--output", "-o").
    /// </summary>
    public required string SwitchName { get; init; }

    /// <summary>
    /// Short form if available (e.g., "-o" for "--output").
    /// </summary>
    public string? ShortForm { get; init; }

    /// <summary>
    /// Whether generated attributes should render the short form when available.
    /// </summary>
    public bool PreferShortForm { get; init; }

    /// <summary>
    /// Generated C# property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// C# type (e.g., "string?", "bool?", "int?").
    /// </summary>
    public required string CSharpType { get; init; }

    /// <summary>
    /// C# type emitted for the generated property.
    /// </summary>
    public string PropertyType => (ValueArity, UsesCollectionShape) switch
    {
        (CliOptionValueArity.Optional, true) => "IEnumerable<CliOptionValue>?",
        (CliOptionValueArity.Optional, false) => "CliOptionValue?",
        _ => CSharpType,
    };

    private bool UsesCollectionShape =>
        AcceptsMultipleValues
        || GroupValues
        || IsCollectionType(CSharpType);

    private static bool IsCollectionType(string cSharpType) =>
        CollectionShapes.GetOrAdd(cSharpType, static typeName => IsEnumerableType(typeName));

    private static bool IsEnumerableType(string cSharpType)
    {
        var source = $$"""
            #nullable enable
            using System;
            using System.Collections;
            using System.Collections.Concurrent;
            using System.Collections.Frozen;
            using System.Collections.Generic;
            using System.Collections.Immutable;
            using System.Collections.ObjectModel;

            namespace CollectionShapeProbe;

            internal sealed class Probe
            {
                internal {{cSharpType}} Value { get; } = default!;
            }
            """;
        var compilation = CollectionProbeCompilation.Value.AddSyntaxTrees(
            CSharpSyntaxTree.ParseText(source));
        var propertyType = compilation.GetTypeByMetadataName(CollectionProbeTypeName)?
            .GetMembers("Value")
            .OfType<IPropertySymbol>()
            .SingleOrDefault()?
            .Type;
        if (propertyType is INamedTypeSymbol
            {
                OriginalDefinition.SpecialType: SpecialType.System_Nullable_T,
                TypeArguments.Length: 1,
            } nullableType)
        {
            propertyType = nullableType.TypeArguments[0];
        }

        if (propertyType is null || propertyType.SpecialType == SpecialType.System_String)
        {
            return false;
        }

        return propertyType is IArrayTypeSymbol
               || propertyType.SpecialType == SpecialType.System_Collections_IEnumerable
               || propertyType.AllInterfaces.Any(
                   interfaceType => interfaceType.SpecialType == SpecialType.System_Collections_IEnumerable);
    }

    private static PortableExecutableReference[] GetPlatformReferences()
    {
        if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string trustedPlatformAssemblies)
        {
            return
            [
                .. trustedPlatformAssemblies
                .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Where(File.Exists)
                .Select(path => MetadataReference.CreateFromFile(path)),
            ];
        }

        return
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ImmutableArray<>).Assembly.Location),
        ];
    }

    private static CSharpCompilation CreateCollectionProbeCompilation() =>
        CSharpCompilation.Create(
            "CollectionShapeProbe",
            references: GetPlatformReferences(),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Documentation URL for options whose metadata does not come from CLI help.
    /// </summary>
    public string? DocumentationUrl { get; init; }

    /// <summary>
    /// Optional edition, license, plugin, or environment availability note.
    /// </summary>
    public string? Availability { get; init; }

    /// <summary>
    /// Whether this is a boolean flag (no value).
    /// </summary>
    public bool IsFlag { get; init; }

    /// <summary>
    /// Whether a value is required, optional, or absent.
    /// </summary>
    public CliOptionValueArity ValueArity { get; init; } = CliOptionValueArity.Required;

    /// <summary>
    /// Semantic rendering phase for this option.
    /// </summary>
    public CommandLinePhase Phase { get; init; } = CommandLinePhase.Normal;

    /// <summary>
    /// Whether the option is required.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether the option can be specified multiple times.
    /// </summary>
    public bool AcceptsMultipleValues { get; init; }

    /// <summary>
    /// Whether collection values render after one option occurrence.
    /// </summary>
    public bool GroupValues { get; init; }

    /// <summary>
    /// Whether this is a key-value pair option.
    /// </summary>
    public bool IsKeyValue { get; init; }

    /// <summary>
    /// Whether generated code needs the ModularPipelines.Models namespace for this option type.
    /// </summary>
    public bool RequiresModelsNamespace
        => IsKeyValue
           || ValueArity == CliOptionValueArity.Optional
           || CSharpType.Contains("CliValuePair", StringComparison.Ordinal);

    /// <summary>
    /// Whether the value is numeric.
    /// </summary>
    public bool IsNumeric { get; init; }

    /// <summary>
    /// Whether this option contains a secret value that should be obfuscated in logs.
    /// Automatically detected for options with "Secret", "Password", "Token", "Key", or "Credential" in the name.
    /// </summary>
    public bool IsSecret { get; init; }

    /// <summary>
    /// For key-value options, the keys whose values contain secrets.
    /// </summary>
    public IReadOnlyList<string> SecretValueKeys { get; init; } = [];

    /// <summary>
    /// The value separator (space, equals, etc.).
    /// </summary>
    public string ValueSeparator { get; init; } = " ";

    /// <summary>
    /// If this option has constrained values, the enum definition for it.
    /// </summary>
    public CliEnumDefinition? EnumDefinition { get; init; }

    /// <summary>
    /// Validation constraints (e.g., min/max for numeric values).
    /// </summary>
    public CliValidationConstraints? ValidationConstraints { get; init; }
}

/// <summary>
/// Validation constraints for an option.
/// </summary>
public record CliValidationConstraints
{
    /// <summary>
    /// Minimum value for numeric options.
    /// </summary>
    public int? MinValue { get; init; }

    /// <summary>
    /// Maximum value for numeric options.
    /// </summary>
    public int? MaxValue { get; init; }

    /// <summary>
    /// Regex pattern for string validation.
    /// </summary>
    public string? Pattern { get; init; }
}
