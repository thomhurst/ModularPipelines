using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Generates immutable assembly metadata for Modular Pipelines integration registrations.
/// </summary>
[Generator]
public sealed class ModularPipelinesIntegrationGenerator : IIncrementalGenerator
{
    private const string IntegrationAttributeFullName =
        "ModularPipelines.Attributes.ModularPipelinesIntegrationAttribute";

    private const string ServiceCollectionFullName =
        "Microsoft.Extensions.DependencyInjection.IServiceCollection";

    private const string PipelineContextFullName =
        "ModularPipelines.Context.IPipelineContext";

    private const string AssemblyMetadataAttributeFullName =
        "System.Reflection.AssemblyMetadataAttribute";

    private const string ToolPropertyMetadataPrefix =
        "ModularPipelines.ToolProperty:";

    private const string ToolTypeIdentityMetadataPrefix =
        "ModularPipelines.ToolTypeIdentity:";

    private static readonly ImmutableHashSet<string> ShadowedToolPropertyNames =
    [
        "Equals",
        "Get",
        "GetHashCode",
        "GetType",
        "ToString",
    ];

    private static readonly DiagnosticDescriptor InvalidIntegrationMethod =
        GeneratorDiagnostics.InvalidIntegrationMethod;

    private static readonly DiagnosticDescriptor UnsupportedToolsLanguageVersion =
        GeneratorDiagnostics.UnsupportedToolsLanguageVersion;

    private static readonly DiagnosticDescriptor ConflictingToolProperty =
        GeneratorDiagnostics.ConflictingToolProperty;

    private static readonly DiagnosticDescriptor ShadowedToolProperty =
        GeneratorDiagnostics.ShadowedToolProperty;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
                IntegrationAttributeFullName,
                static (node, _) => node is MethodDeclarationSyntax,
                static (generatorContext, _) => GetCandidate(generatorContext))
            .WithComparer(IntegrationCandidateComparer.Instance);
        var referencedToolProperties = context.MetadataReferencesProvider
            .Collect()
            .Select(static (references, _) => GetReferencedToolProperties(references));

        context.RegisterSourceOutput(
            candidates
                .Collect()
                .Combine(context.ParseOptionsProvider)
                .Combine(referencedToolProperties),
            static (sourceContext, input) => Generate(
                sourceContext,
                input.Left.Left,
                input.Left.Right,
                input.Right));
    }

    private static IntegrationCandidate GetCandidate(
        GeneratorAttributeSyntaxContext context)
    {
        var method = (IMethodSymbol) context.TargetSymbol;
        var location = method.Locations.FirstOrDefault() ?? context.TargetNode.GetLocation();

        if (!method.IsStatic
            || method.IsGenericMethod
            || method.Parameters.Length != 1
            || method.Parameters[0].RefKind != RefKind.None
            || method.Parameters[0].Type.ToDisplayString() != ServiceCollectionFullName
            || (!method.ReturnsVoid
                && method.ReturnType.ToDisplayString() != ServiceCollectionFullName)
            || method.DeclaredAccessibility is not (
                Accessibility.Public
                or Accessibility.Internal
                or Accessibility.ProtectedOrInternal)
            || !IsAccessibleFromGeneratedRegistrar(method.ContainingType))
        {
            return new IntegrationCandidate(
                Registration: null,
                EquatableArray<ToolProperty>.Empty,
                GetGeneratedTypeName(context.SemanticModel.Compilation.AssemblyName),
                method.ToDisplayString(),
                location);
        }

        return new IntegrationCandidate(
            new IntegrationRegistration(
                method.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                method.Name),
            GetToolProperties(method.ContainingType, context.SemanticModel.Compilation),
            GetGeneratedTypeName(context.SemanticModel.Compilation.AssemblyName),
            method.ToDisplayString(),
            location);
    }

    private static EquatableArray<ToolProperty> GetToolProperties(
        INamedTypeSymbol type,
        Compilation compilation)
    {
        return type.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(static method =>
                method.IsStatic
                && method.IsExtensionMethod
                && !method.IsGenericMethod
                && method.DeclaredAccessibility == Accessibility.Public
                && method.Parameters.Length == 1
                && method.Parameters[0].RefKind == RefKind.None
                && method.Parameters[0].Type.ToDisplayString() == PipelineContextFullName
                && method.ReturnType.IsReferenceType
                && IsPubliclyAccessible(method.ReturnType))
            .Select(method => new ToolProperty(
                method.Name,
                method.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                GetTypeIdentity(method.ReturnType, compilation),
                method.ToDisplayString(),
                method.Locations.FirstOrDefault(),
                method.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
            .ToImmutableArray();
    }

    private static string GetTypeIdentity(ITypeSymbol type, Compilation compilation)
    {
        if (type.TypeKind == TypeKind.Dynamic)
        {
            return GetTypeIdentity(
                compilation.GetSpecialType(SpecialType.System_Object),
                compilation);
        }

        if (type is IArrayTypeSymbol arrayType)
        {
            return $"{GetTypeIdentity(arrayType.ElementType, compilation)}[{new string(',', arrayType.Rank - 1)}]";
        }

        if (type is not INamedTypeSymbol namedType)
        {
            return type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        var definition = namedType.OriginalDefinition;
        var typeNames = new Stack<string>();
        for (var current = definition; current is not null; current = current.ContainingType)
        {
            typeNames.Push(current.MetadataName);
        }

        var namespaceName = definition.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : $"{definition.ContainingNamespace.ToDisplayString()}.";
        var identity =
            $"{definition.ContainingAssembly.Identity.Name}:{namespaceName}{string.Join("+", typeNames)}";
        var typeArguments = GetAllTypeArguments(namedType).ToArray();
        return typeArguments.Length == 0
            ? identity
            : $"{identity}[{string.Join(",", typeArguments.Select(argument => GetTypeIdentity(argument, compilation)))}]";
    }

    private static IEnumerable<ITypeSymbol> GetAllTypeArguments(INamedTypeSymbol type)
    {
        if (type.ContainingType is { } containingType)
        {
            foreach (var typeArgument in GetAllTypeArguments(containingType))
            {
                yield return typeArgument;
            }
        }

        foreach (var typeArgument in type.TypeArguments)
        {
            yield return typeArgument;
        }
    }

    private static EquatableArray<ReferencedToolProperty> GetReferencedToolProperties(
        ImmutableArray<MetadataReference> references)
    {
        var compilation = CSharpCompilation.Create(
            "ModularPipelines.ReferencedToolProperties",
            references: references);
        return compilation.References
            .Select(compilation.GetAssemblyOrModuleSymbol)
            .OfType<IAssemblySymbol>()
            .SelectMany(static assembly => assembly.GetAttributes()
                .Where(static attribute =>
                    attribute.AttributeClass?.ToDisplayString() == AssemblyMetadataAttributeFullName
                    && attribute.ConstructorArguments.Length == 2
                    && attribute.ConstructorArguments[0].Value is string key
                    && key.StartsWith(ToolPropertyMetadataPrefix, StringComparison.Ordinal)
                    && attribute.ConstructorArguments[1].Value is string)
                .Select(attribute => new ReferencedToolProperty(
                    ((string) attribute.ConstructorArguments[0].Value!)
                        .Substring(ToolPropertyMetadataPrefix.Length),
                    (string) attribute.ConstructorArguments[1].Value!,
                    assembly.Identity.ToString())))
            .Distinct()
            .OrderBy(static property => property.Name, StringComparer.Ordinal)
            .ThenBy(static property => property.TypeName, StringComparer.Ordinal)
            .ThenBy(static property => property.SourceId, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static string GetGeneratedTypeName(string? assemblyName)
    {
        return GeneratedTypeName.FromAssembly(
            assemblyName,
            "Integration",
            "ToolsExtensions");
    }

    private static bool IsAccessibleFromGeneratedRegistrar(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.IsFileLocal
                || current.IsGenericType
                || current.DeclaredAccessibility is not (
                    Accessibility.Public
                    or Accessibility.Internal
                    or Accessibility.ProtectedOrInternal))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsPubliclyAccessible(ITypeSymbol type)
    {
        if (type is IArrayTypeSymbol arrayType)
        {
            return IsPubliclyAccessible(arrayType.ElementType);
        }

        if (type is not INamedTypeSymbol namedType)
        {
            return type.TypeKind == TypeKind.Dynamic;
        }

        for (var current = namedType; current is not null; current = current.ContainingType)
        {
            if (current.DeclaredAccessibility != Accessibility.Public)
            {
                return false;
            }
        }

        return namedType.TypeArguments.All(IsPubliclyAccessible);
    }

    private static void Generate(
        SourceProductionContext context,
        ImmutableArray<IntegrationCandidate> candidates,
        ParseOptions parseOptions,
        EquatableArray<ReferencedToolProperty> referencedToolProperties)
    {
        ReportInvalidIntegrationMethods(context, candidates);

        var uniqueRegistrations = candidates
            .Select(static candidate => candidate.Registration)
            .OfType<IntegrationRegistration>()
            .Distinct()
            .OrderBy(static registration => registration.TypeName, StringComparer.Ordinal)
            .ThenBy(static registration => registration.MethodName, StringComparer.Ordinal)
            .ToArray();

        var supportsExtensionMembers = SupportsExtensionMembers(parseOptions);
        var uniqueToolProperties = GetUniqueToolProperties(
            context,
            candidates,
            referencedToolProperties,
            supportsExtensionMembers);
        var generatesExtensionMembers =
            uniqueToolProperties.Length > 0 && supportsExtensionMembers;

        if (uniqueRegistrations.Length == 0)
        {
            return;
        }

        var builder = new StringBuilder();
        builder.AppendLine("// <auto-generated/>");
        builder.AppendLine("#nullable enable");
        builder.AppendLine();
        builder.AppendLine(
            "[assembly: global::ModularPipelines.Attributes.ModularPipelinesContextAttribute("
            + "typeof(global::ModularPipelines.Generated.ModularPipelinesContextRegistration))]");
        foreach (var property in uniqueToolProperties)
        {
            builder.AppendLine(
                "[assembly: global::System.Reflection.AssemblyMetadataAttribute("
                + $"{Literal(ToolPropertyMetadataPrefix + property.Name)}, "
                + $"{Literal(property.TypeName)})]");
            builder.AppendLine(
                "[assembly: global::System.Reflection.AssemblyMetadataAttribute("
                + $"{Literal(ToolTypeIdentityMetadataPrefix + property.Name)}, "
                + $"{Literal(property.TypeIdentity)})]");
        }

        builder.AppendLine();
        if (generatesExtensionMembers)
        {
            builder.AppendLine("namespace ModularPipelines.Generated");
            builder.AppendLine("{");
        }
        else
        {
            builder.AppendLine("namespace ModularPipelines.Generated;");
        }

        builder.AppendLine();
        builder.AppendLine("internal static class ModularPipelinesContextRegistration");
        builder.AppendLine("{");
        builder.AppendLine(
            "    internal static void Register("
            + "global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)");
        builder.AppendLine("    {");

        foreach (var registration in uniqueRegistrations)
        {
            builder.AppendLine(
                $"        {registration.TypeName}.{registration.MethodName}(services);");
        }

        builder.AppendLine("    }");
        builder.AppendLine("}");
        if (generatesExtensionMembers)
        {
            builder.AppendLine("}");
        }

        if (uniqueToolProperties.Length > 0 && !supportsExtensionMembers)
        {
            var firstProperty = uniqueToolProperties[0];
            context.ReportDiagnostic(Diagnostic.Create(
                UnsupportedToolsLanguageVersion,
                firstProperty.Location,
                firstProperty.MethodName,
                GetLanguageVersionDisplay(parseOptions),
                EscapeIdentifier(firstProperty.Name)));
        }
        else if (uniqueToolProperties.Length > 0)
        {
            builder.AppendLine();
            builder.AppendLine("namespace ModularPipelines.Context");
            builder.AppendLine("{");
            builder.AppendLine(
                $"public static class {candidates[0].GeneratedTypeName}");
            builder.AppendLine("{");
            builder.AppendLine(
                "    extension(global::ModularPipelines.Context.IToolsContext tools)");
            builder.AppendLine("    {");

            foreach (var property in uniqueToolProperties)
            {
                builder.AppendLine(
                    $"        public {property.TypeName} {EscapeIdentifier(property.Name)} "
                    + $"=> tools.Get<{property.TypeName}>();");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");
            builder.AppendLine("}");
        }

        context.AddSource("ModularPipelines.IntegrationRegistration.g.cs", builder.ToString());
    }

    private static void ReportInvalidIntegrationMethods(
        SourceProductionContext context,
        ImmutableArray<IntegrationCandidate> candidates)
    {
        foreach (var candidate in candidates.Where(
                     static candidate => candidate.Registration is null))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                InvalidIntegrationMethod,
                candidate.Location,
                candidate.MethodName));
        }
    }

    private static ToolProperty[] GetUniqueToolProperties(
        SourceProductionContext context,
        ImmutableArray<IntegrationCandidate> candidates,
        EquatableArray<ReferencedToolProperty> referencedToolProperties,
        bool supportsExtensionMembers)
    {
        var toolProperties = candidates
            .SelectMany(static candidate => candidate.ToolProperties)
            .ToArray();
        if (supportsExtensionMembers)
        {
            ReportShadowedToolProperties(context, toolProperties);
            toolProperties =
            [
                .. toolProperties.Where(
                    static property => !ShadowedToolPropertyNames.Contains(property.Name)),
            ];
        }

        var allToolProperties = toolProperties
            .Concat(referencedToolProperties.Select(static property => new ToolProperty(
                property.Name,
                property.TypeName,
                TypeIdentity: property.TypeName,
                MethodName: property.Name,
                Location: null,
                property.SourceId)))
            .ToArray();
        var conflictingPropertyNames = supportsExtensionMembers
            ? ReportToolPropertyConflicts(context, allToolProperties)
            : [];

        return
        [
            .. toolProperties
                .Where(property => !conflictingPropertyNames.Contains(property.Name))
                .GroupBy(static property => new
                {
                    property.Name,
                    property.TypeName,
                })
                .Select(static group => group.First())
                .OrderBy(static property => property.Name, StringComparer.Ordinal)
                .ThenBy(static property => property.TypeName, StringComparer.Ordinal),
        ];
    }

    private static void ReportShadowedToolProperties(
        SourceProductionContext context,
        ToolProperty[] toolProperties)
    {
        foreach (var property in toolProperties.Where(
                     static property => ShadowedToolPropertyNames.Contains(property.Name)))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                ShadowedToolProperty,
                property.Location,
                property.MethodName,
                property.Name));
        }
    }

    private static ImmutableHashSet<string> ReportToolPropertyConflicts(
        SourceProductionContext context,
        ToolProperty[] toolProperties)
    {
        var conflicts = toolProperties
            .GroupBy(static property => new
            {
                property.Name,
                property.TypeName,
                property.SourceId,
            })
            .Select(static group => group.First())
            .GroupBy(static property => property.Name, StringComparer.Ordinal)
            .Where(static group => group.Skip(1).Any())
            .ToArray();

        foreach (var conflict in conflicts)
        {
            var declarations = string.Join(
                ", ",
                conflict.Select(static property =>
                        $"{property.TypeName} ({property.SourceId})")
                    .OrderBy(static declaration => declaration, StringComparer.Ordinal));
            foreach (var property in conflict)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    ConflictingToolProperty,
                    property.Location,
                    property.Name,
                    declarations));
            }
        }

        return conflicts
            .Select(static group => group.Key)
            .ToImmutableHashSet(StringComparer.Ordinal);
    }

    private static bool SupportsExtensionMembers(ParseOptions parseOptions)
    {
        return parseOptions is CSharpParseOptions csharpParseOptions
            && (int) LanguageVersionFacts.MapSpecifiedToEffectiveVersion(
                csharpParseOptions.SpecifiedLanguageVersion) >= 1400;
    }

    private static string GetLanguageVersionDisplay(ParseOptions parseOptions)
    {
        return parseOptions is CSharpParseOptions csharpParseOptions
            ? csharpParseOptions.SpecifiedLanguageVersion.ToDisplayString()
            : parseOptions.Language;
    }

    private static string Literal(string value) =>
        global::Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(value, quote: true);

    private static string EscapeIdentifier(string identifier) =>
        SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None
        || SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None
            ? $"@{identifier}"
            : identifier;

    private sealed record IntegrationCandidate(
        IntegrationRegistration? Registration,
        EquatableArray<ToolProperty> ToolProperties,
        string GeneratedTypeName,
        string MethodName,
        Location Location);

    private sealed class IntegrationCandidateComparer : IEqualityComparer<IntegrationCandidate>
    {
        public static IntegrationCandidateComparer Instance { get; } = new();

        public bool Equals(IntegrationCandidate? x, IntegrationCandidate? y) =>
            ReferenceEquals(x, y)
            || (x is not null
                && y is not null
                && EqualityComparer<IntegrationRegistration?>.Default.Equals(
                    x.Registration,
                    y.Registration)
                && x.ToolProperties.Equals(y.ToolProperties)
                && StringComparer.Ordinal.Equals(x.GeneratedTypeName, y.GeneratedTypeName)
                && StringComparer.Ordinal.Equals(x.MethodName, y.MethodName)
                && (x.Registration is not null || x.Location.Equals(y.Location)));

        public int GetHashCode(IntegrationCandidate obj)
        {
            var hashCode = obj.Registration?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ obj.ToolProperties.GetHashCode();
            hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.GeneratedTypeName);
            hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.MethodName);
            return obj.Registration is null
                ? (hashCode * 397) ^ obj.Location.GetHashCode()
                : hashCode;
        }
    }

    private sealed record IntegrationRegistration(string TypeName, string MethodName);

    private sealed record ToolProperty(
        string Name,
        string TypeName,
        string TypeIdentity,
        string MethodName,
        Location? Location,
        string SourceId);

    private sealed record ReferencedToolProperty(
        string Name,
        string TypeName,
        string SourceId);
}
