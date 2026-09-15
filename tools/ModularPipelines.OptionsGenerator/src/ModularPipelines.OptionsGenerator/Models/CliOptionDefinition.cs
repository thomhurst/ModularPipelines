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
    /// <summary>Option-local prose for shape validation, excluding inherited group documentation.</summary>
    internal string? ValueShapeDescription { get; init; }

    private const string CollectionProbeTypeName = "CollectionShapeProbe.Probe";
    private static readonly ConcurrentDictionary<string, CollectionShapeResolution> CollectionShapes = new(StringComparer.Ordinal);
    private static readonly Lazy<CSharpCompilation> CollectionProbeCompilation = new(CreateCollectionProbeCompilation);

    /// <summary>
    /// The CLI switch name (e.g., "--output", "-o").
    /// </summary>
    public required string SwitchName { get; init; }

    /// <summary>
    /// Paired switch emitted when a nullable boolean flag is explicitly false.
    /// </summary>
    public string? NegatedSwitchName { get; init; }

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

    private bool UsesCollectionShape
    {
        get
        {
            if (AcceptsMultipleValues || GroupValues)
            {
                return true;
            }

            return IsCollectionType(CSharpType, IsCollection);
        }
    }

    internal static bool IsCollectionType(string cSharpType, bool? unresolvedOverride = null) =>
        TryGetCollectionShape(cSharpType, out var isCollection)
            ? isCollection
            : unresolvedOverride == true;

    internal static bool TryGetCollectionShape(string cSharpType, out bool isCollection)
    {
        var resolution = GetCollectionShape(cSharpType);
        isCollection = resolution.IsCollection;
        return resolution.IsResolved;
    }

    internal static bool MayBeReferenceType(string cSharpType)
    {
        var resolution = GetCollectionShape(cSharpType);
        return !resolution.IsResolved || resolution.IsReferenceType;
    }

    internal static string GetCollectionSnapshotExpression(
        string cSharpType, string valueExpression, bool retainUnsupportedCollections = false, string typedSnapshotPrefix = "__Snapshot", bool preserveValuePairs = true,
        CliOptionValueArity valueArity = CliOptionValueArity.Required)
    {
        var shape = GetCollectionShape(cSharpType);
        if (retainUnsupportedCollections)
        {
            if (!shape.IsResolved)
            {
                throw new InvalidOperationException(
                    $"Alternative collection type '{cSharpType}' cannot safely retain a reusable snapshot because its shape is unresolved. Use a supported collection contract.");
            }

            // Optional properties must continue accepting every implementation allowed by
            // their declared contract. Retain it when no assignable safe copy is available.
            var snapshotExpression = (valueArity, preserveValuePairs) switch
            {
                (CliOptionValueArity.Optional, _) => shape.OptionalDeclaredSnapshotExpression,
                (_, true) => shape.OptionalValuePairSnapshotExpression,
                (_, false) => shape.OptionalSnapshotExpression,
            };
            return snapshotExpression?.Replace("{0}", valueExpression, StringComparison.Ordinal)
                       .Replace("{1}", typedSnapshotPrefix, StringComparison.Ordinal)
                   ?? valueExpression;
        }

        return shape.SnapshotExpression?.Replace("{0}", valueExpression, StringComparison.Ordinal)
            ?? throw new InvalidOperationException(
                $"Required collection type '{cSharpType}' cannot safely retain a reusable snapshot. Use a supported collection contract.");
    }

    internal static string? GetTypedSnapshotCollectionType(string cSharpType) =>
        GetCollectionShape(cSharpType).TypedSnapshotCollectionType;

    internal static string GetSnapshotElementTypeName(string cSharpType) =>
        GetCollectionShape(cSharpType).ElementTypeName
        ?? throw new InvalidOperationException($"Collection element type for '{cSharpType}' is unresolved.");

    internal static bool IsSnapshotElementType(string cSharpType, string elementName) =>
        MatchesRendererElementType(GetCollectionShape(cSharpType).ElementTypeIdentity, elementName);

    private static CollectionShapeResolution GetCollectionShape(string cSharpType) =>
        CollectionShapes.GetOrAdd(cSharpType.TrimEnd('?'), static typeName => ResolveCollectionShape(typeName));

    internal static int FindIndexBySwitch(
        IReadOnlyList<CliOptionDefinition> options,
        string optionSwitch) =>
        Enumerable.Range(0, options.Count).FirstOrDefault(index =>
            options[index].GetSwitchNames().Contains(optionSwitch, StringComparer.Ordinal),
            -1);

    internal IEnumerable<string> GetSwitchNames()
    {
        yield return SwitchName;
        if (ShortForm is not null)
        {
            yield return ShortForm;
        }

        if (NegatedSwitchName is not null)
        {
            yield return NegatedSwitchName;
        }
    }

    private static CollectionShapeResolution ResolveCollectionShape(string cSharpType)
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

        if (propertyType is null || propertyType.TypeKind == TypeKind.Error)
        {
            return default;
        }

        if (propertyType.SpecialType == SpecialType.System_String)
        {
            return new CollectionShapeResolution(IsResolved: true, IsCollection: false, IsReferenceType: true);
        }

        var isCollection = propertyType is IArrayTypeSymbol
                           || propertyType.SpecialType == SpecialType.System_Collections_IEnumerable
                           || propertyType.AllInterfaces.Any(
                               interfaceType => interfaceType.SpecialType == SpecialType.System_Collections_IEnumerable);
        var enumerableType = propertyType.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T
            ? (INamedTypeSymbol) propertyType
            : propertyType.AllInterfaces.FirstOrDefault(
                interfaceType => interfaceType.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T);
        var elementType = enumerableType?.TypeArguments[0] ?? compilation.GetSpecialType(SpecialType.System_Object);
        var isArrayAssignable = isCollection && compilation.ClassifyConversion(
            compilation.CreateArrayTypeSymbol(elementType), propertyType).IsImplicit;
        var snapshotExpression = isCollection
            ? GetSnapshotExpression(compilation, propertyType, elementType, isArrayAssignable)
            : null;
        var typedSnapshotCollectionType = GetTypedSnapshotCollectionType(compilation, propertyType, elementType, isArrayAssignable);
        return new CollectionShapeResolution(IsResolved: true, IsCollection: isCollection,
            IsReferenceType: propertyType.IsReferenceType,
            SnapshotExpression: snapshotExpression,
            OptionalSnapshotExpression: isCollection
                ? GetOptionalSnapshotExpression(compilation, propertyType, elementType, isArrayAssignable, typedSnapshotCollectionType, preserveValuePairs: false)
                : null,
            OptionalValuePairSnapshotExpression: isCollection
                ? GetOptionalSnapshotExpression(compilation, propertyType, elementType, isArrayAssignable, typedSnapshotCollectionType, preserveValuePairs: true)
                : null,
            TypedSnapshotCollectionType: typedSnapshotCollectionType,
            ElementTypeName: elementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            ElementTypeIdentity: GetElementTypeIdentity(elementType),
            OptionalDeclaredSnapshotExpression: isCollection
                ? GetSnapshotExpression(compilation, propertyType, elementType, isArrayAssignable, retainUnsupportedCollections: true)
                : null);
    }

    private static string? GetTypedSnapshotCollectionType(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType, bool isArrayAssignable)
    {
        if (SymbolEqualityComparer.Default.Equals(propertyType.OriginalDefinition,
                compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlySet`1")))
        {
            return "IReadOnlySet";
        }

        if (elementType.SpecialType != SpecialType.System_Object)
        {
            return propertyType.OriginalDefinition.SpecialType switch
            {
                SpecialType.System_Collections_Generic_IEnumerable_T => "IEnumerable",
                SpecialType.System_Collections_Generic_IReadOnlyCollection_T => "IReadOnlyCollection",
                SpecialType.System_Collections_Generic_IReadOnlyList_T => "IReadOnlyList",
                _ => null,
            };
        }

        if (IsMutableObjectCollection(compilation, propertyType, elementType))
        {
            return "List";
        }

        if (isArrayAssignable)
        {
            return null;
        }

        var setType = compilation.GetTypeByMetadataName("System.Collections.Generic.HashSet`1")?.Construct(elementType);
        return setType is not null && compilation.ClassifyConversion(setType, propertyType).IsImplicit ? "HashSet" : null;
    }

    private static bool IsMutableObjectCollection(CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType)
    {
        if (elementType.SpecialType != SpecialType.System_Object)
        {
            return false;
        }

        if (IsMutableCollectionInterface(compilation, propertyType))
        {
            return true;
        }

        var objectListType = compilation.GetTypeByMetadataName("System.Collections.Generic.List`1")?.Construct(elementType);
        return SymbolEqualityComparer.Default.Equals(propertyType, objectListType);
    }

    private static string GetOptionalSnapshotExpression(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType, bool isArrayAssignable, string? typedSnapshotCollectionType, bool preserveValuePairs)
    {
        var snapshot = GetSnapshotExpression(compilation, propertyType, elementType, isArrayAssignable, retainUnsupportedCollections: true) ?? "{0}";
        var supportsTypedSnapshot = typedSnapshotCollectionType is not null || (isArrayAssignable && elementType.SpecialType == SpecialType.System_Object);
        snapshot = supportsTypedSnapshot
            ? GetTypedSnapshotExpression(propertyType, elementType, typedSnapshotCollectionType, "KeyValue", "keyValues", snapshot)
            : RetainIncompatibleTypedView(elementType, "KeyValue", snapshot);

        // Scalar character rendering precedes ordinary collection rendering, while
        // option value pairs take precedence over both in CommandArgumentBuilder.
        snapshot = $"(object){{0}} is global::System.Collections.Generic.IEnumerable<char> ? {{0}} : ({snapshot})";
        if (!preserveValuePairs)
        {
            return snapshot;
        }

        return supportsTypedSnapshot
            ? GetTypedSnapshotExpression(propertyType, elementType, typedSnapshotCollectionType, "CliValuePair", "valuePairs", snapshot)
            : RetainIncompatibleTypedView(elementType, "CliValuePair", snapshot);
    }

    private static string RetainIncompatibleTypedView(ITypeSymbol declaredElementType, string elementName, string fallback)
    {
        if (MatchesRendererElementType(GetElementTypeIdentity(declaredElementType), elementName))
        {
            return fallback;
        }

        // A snapshot of the declared elements cannot represent this different runtime
        // view. Preserve the implementation and its mutation behavior without enumerating it.
        return $"(object){{0}} is global::System.Collections.Generic.IEnumerable<global::ModularPipelines.Models.{elementName}> ? {{0}} : ({fallback})";
    }

    private static string GetElementTypeIdentity(ITypeSymbol declaredElementType)
    {
        // Unresolved domain names can be bound as Nullable<T> in the probe even
        // though the generated property uses the real reference type.
        if (declaredElementType is INamedTypeSymbol
            {
                OriginalDefinition.SpecialType: SpecialType.System_Nullable_T,
                TypeArguments.Length: 1,
            } nullableType)
        {
            declaredElementType = nullableType.TypeArguments[0];
        }

        return declaredElementType.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    // Generated options import ModularPipelines.Models, including domain names the probe cannot resolve.
    private static bool MatchesRendererElementType(string? declaredName, string elementName) =>
        declaredName == elementName || declaredName == $"global::ModularPipelines.Models.{elementName}";

    private static string GetTypedSnapshotExpression(
        ITypeSymbol propertyType, ITypeSymbol declaredElementType, string? collectionType, string elementName, string variableName, string fallback)
    {
        if (MatchesRendererElementType(GetElementTypeIdentity(declaredElementType), elementName))
        {
            return fallback;
        }

        var elementType = $"global::ModularPipelines.Models.{elementName}";
        if (collectionType == "HashSet")
        {
            // A custom set can expose a rendering view unrelated to its membership.
            // Its comparer can reject or deduplicate those values, and mutations can
            // affect that view arbitrarily. Retain the implementation rather than
            // replacing its contracts with a generated set adapter.
            return $"(object){{0}} is global::System.Collections.Generic.IEnumerable<{elementType}> ? {{0}} : ({fallback})";
        }

        var values = $"default(global::System.Collections.Immutable.ImmutableArray<{elementType}>).Equals((object){variableName}) ? global::System.Array.Empty<{elementType}>() : {variableName}";
        var propertyName = propertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var snapshot = collectionType is not null
            ? $"new {{1}}{elementName}({values})"
            : $"({propertyName})(object)global::System.Linq.Enumerable.ToArray({values})";
        if (collectionType is "IEnumerable" or "IReadOnlyCollection" or "IReadOnlyList" or "IReadOnlySet")
        {
            snapshot = $"new {{1}}{elementName}({{0}}, {values})";
        }

        return $"(object){{0}} is global::System.Collections.Generic.IEnumerable<{elementType}> {variableName} ? {snapshot} : ({fallback})";
    }

    private static string? GetSnapshotExpression(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType, bool isArrayAssignable,
        bool retainUnsupportedCollections = false)
    {
        var needsMutableSnapshot = retainUnsupportedCollections && IsMutableCollectionInterface(compilation, propertyType);
        if (isArrayAssignable && !needsMutableSnapshot)
        {
            return GetArraySnapshotExpression(compilation, propertyType, elementType, retainUnsupportedCollections);
        }

        var elementName = elementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var values = $"global::System.Linq.Enumerable.Cast<{elementName}>({{0}})";
        foreach (var metadataName in new[]
                 {
                     "System.Collections.Generic.List`1",
                     "System.Collections.Generic.HashSet`1",
                     "System.Collections.Immutable.ImmutableArray`1",
                 })
        {
            var snapshotType = compilation.GetTypeByMetadataName(metadataName)?.Construct(elementType);
            if (snapshotType is null || !compilation.ClassifyConversion(snapshotType, propertyType).IsImplicit)
            {
                continue;
            }

            return GetConstructedSnapshotExpression(compilation, propertyType, elementType,
                snapshotType, metadataName, retainUnsupportedCollections);
        }

        var arrayList = compilation.GetTypeByMetadataName("System.Collections.ArrayList");
        return arrayList is not null && compilation.ClassifyConversion(arrayList, propertyType).IsImplicit
            ? $"new global::System.Collections.ArrayList(global::System.Linq.Enumerable.ToArray({values}))"
            : null;
    }

    private static string GetConstructedSnapshotExpression(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType,
        INamedTypeSymbol snapshotType, string metadataName, bool retainUnsupportedCollections)
    {
        var values = $"global::System.Linq.Enumerable.Cast<{elementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>({{0}})";
        var snapshotName = snapshotType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (metadataName == "System.Collections.Generic.HashSet`1")
        {
            // A set may use identity or another non-default equality contract.
            // Preserve that comparer instead of silently collapsing distinct CLI values.
            return retainUnsupportedCollections
                ? $"{{0}} is {snapshotName} sourceSet ? new {snapshotName}({values}, sourceSet.Comparer) : {{0}}"
                : $"new {snapshotName}({values}, {{0}} is {snapshotName} sourceSet ? sourceSet.Comparer : throw new global::System.ArgumentException(\"Required set must be a HashSet so its comparer can be preserved.\"))";
        }

        if (metadataName == "System.Collections.Immutable.ImmutableArray`1")
        {
            var defaultValue = retainUnsupportedCollections
                ? $"{snapshotName}.Empty"
                : "throw new global::System.ArgumentException(\"Required collection must contain at least one value.\", nameof({0}))";
            return $"{{0}} is {snapshotName} {{ IsDefault: true }} ? {defaultValue} : global::System.Collections.Immutable.ImmutableArray.CreateRange({values})";
        }

        var snapshot = $"new {snapshotName}({values})";
        return retainUnsupportedCollections
            ? GetDefaultImmutableArraySafeSnapshot(compilation, propertyType, elementType, snapshot, $"new {snapshotName}()")
            : snapshot;
    }

    private static bool IsMutableCollectionInterface(CSharpCompilation compilation, ITypeSymbol propertyType) =>
        propertyType.TypeKind == TypeKind.Interface
        && (propertyType.OriginalDefinition.SpecialType is SpecialType.System_Collections_Generic_ICollection_T
            or SpecialType.System_Collections_Generic_IList_T
            || SymbolEqualityComparer.Default.Equals(propertyType, compilation.GetTypeByMetadataName("System.Collections.IList")));

    private static string GetArraySnapshotExpression(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType, bool retainUnsupportedCollections)
    {
        var elementName = elementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var values = $"global::System.Linq.Enumerable.Cast<{elementName}>({{0}})";
        var snapshot = $"global::System.Linq.Enumerable.ToArray({values})";
        return retainUnsupportedCollections
            ? GetDefaultImmutableArraySafeSnapshot(compilation, propertyType, elementType, snapshot, $"global::System.Array.Empty<{elementName}>()")
            : snapshot;
    }

    private static string GetDefaultImmutableArraySafeSnapshot(
        CSharpCompilation compilation, ITypeSymbol propertyType, ITypeSymbol elementType, string snapshot, string emptySnapshot)
    {
        var immutableArrayType = compilation.GetTypeByMetadataName("System.Collections.Immutable.ImmutableArray`1")?.Construct(elementType);
        if (immutableArrayType is not null
            && compilation.ClassifyConversion(immutableArrayType, propertyType).IsImplicit)
        {
            var immutableArrayName = immutableArrayType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            // Object equality compares backing-array identity across element types, so a
            // default ImmutableArray<string> is also recognized through IEnumerable<object>.
            return $"default({immutableArrayName}).Equals((object){{0}}) ? {emptySnapshot} : {snapshot}";
        }

        return snapshot;
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

    private readonly record struct CollectionShapeResolution(
        bool IsResolved, bool IsCollection, bool IsReferenceType,
        string? SnapshotExpression = null, string? OptionalSnapshotExpression = null,
        string? OptionalValuePairSnapshotExpression = null, string? TypedSnapshotCollectionType = null,
        string? ElementTypeName = null, string? ElementTypeIdentity = null,
        string? OptionalDeclaredSnapshotExpression = null);

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
    /// Separator used to join collection elements into one option value.
    /// </summary>
    public string? CollectionSeparator { get; init; }

    /// <summary>
    /// Whether an optional value type unavailable to the generator has collection semantics.
    /// </summary>
    public bool? IsCollection { get; init; }

    /// <summary>
    /// Whether this is a key-value pair option.
    /// </summary>
    public bool IsKeyValue { get; init; }

    /// <summary>
    /// Whether the CLI declares this option as one structured value, even when
    /// its nested fields contain collection-shaped values.
    /// </summary>
    internal bool IsStructuredValue { get; init; }

    /// <summary>
    /// Whether an explicit CLI type declares one scalar value, taking precedence over
    /// collection-shaped prose about results or the contents of that value.
    /// </summary>
    internal bool IsScalarValue { get; init; }

    /// <summary>
    /// Whether the scraper synthesized this option from another option's negation syntax.
    /// Explicit declarations take precedence and must retain their own constraint identity.
    /// </summary>
    internal bool IsGeneratedNegation { get; init; }

    /// <summary>
    /// Whether generated code needs the ModularPipelines.Models namespace for this option type.
    /// </summary>
    public bool RequiresModelsNamespace
        => IsKeyValue
           || ValueArity == CliOptionValueArity.Optional
           || TypeRequiresModelsNamespace(CSharpType);

    internal static bool TypeRequiresModelsNamespace(string cSharpType) =>
        cSharpType.Contains("KeyValue", StringComparison.Ordinal)
        || cSharpType.Contains("CliValuePair", StringComparison.Ordinal)
        || cSharpType.Contains("CliOptionValue", StringComparison.Ordinal);

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
