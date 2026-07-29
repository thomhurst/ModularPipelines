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

    private static readonly DiagnosticDescriptor InvalidIntegrationMethod =
        GeneratorDiagnostics.InvalidIntegrationMethod;

    private static readonly DiagnosticDescriptor UnsupportedToolsLanguageVersion = new(
        id: "MPGEN003",
        title: "Discoverable tool properties require C# 14",
        messageFormat:
            "Tool accessor '{0}' cannot generate a context.Tools property because language version "
            + "'{1}' does not support extension members; use C# 14 or preview",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ConflictingToolProperty = new(
        id: "MPGEN004",
        title: "Conflicting discoverable tool property",
        messageFormat: "Tool property '{0}' has conflicting return types: {1}",
        category: "ModularPipelines.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
                IntegrationAttributeFullName,
                static (node, _) => node is MethodDeclarationSyntax,
                static (generatorContext, _) => GetCandidate(generatorContext));
        var referencedToolProperties = context.CompilationProvider.Select(
            static (compilation, _) => GetReferencedToolProperties(compilation));

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
            GetToolProperties(method.ContainingType),
            GetGeneratedTypeName(context.SemanticModel.Compilation.AssemblyName),
            method.ToDisplayString(),
            location);
    }

    private static EquatableArray<ToolProperty> GetToolProperties(INamedTypeSymbol type)
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
                && method.ReturnType.IsReferenceType)
            .Select(static method => new ToolProperty(
                method.Name,
                method.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                method.ToDisplayString(),
                method.Locations.FirstOrDefault()))
            .ToImmutableArray();
    }

    private static EquatableArray<ReferencedToolProperty> GetReferencedToolProperties(
        Compilation compilation)
    {
        return compilation.References
            .Select(compilation.GetAssemblyOrModuleSymbol)
            .OfType<IAssemblySymbol>()
            .SelectMany(static assembly => assembly.GetAttributes())
            .Where(static attribute =>
                attribute.AttributeClass?.ToDisplayString() == AssemblyMetadataAttributeFullName
                && attribute.ConstructorArguments.Length == 2
                && attribute.ConstructorArguments[0].Value is string key
                && key.StartsWith(ToolPropertyMetadataPrefix, StringComparison.Ordinal)
                && attribute.ConstructorArguments[1].Value is string)
            .Select(static attribute => new ReferencedToolProperty(
                ((string) attribute.ConstructorArguments[0].Value!)
                    .Substring(ToolPropertyMetadataPrefix.Length),
                (string) attribute.ConstructorArguments[1].Value!))
            .Distinct()
            .OrderBy(static property => property.Name, StringComparer.Ordinal)
            .ThenBy(static property => property.TypeName, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static string GetGeneratedTypeName(string? assemblyName)
    {
        var name = assemblyName ?? "Integration";
        var builder = new StringBuilder(name.Length + 15);

        if (name.Length == 0 || !IsIdentifierStart(name[0]))
        {
            builder.Append('_');
        }

        foreach (var character in name)
        {
            builder.Append(IsIdentifierPart(character) ? character : '_');
        }

        return builder.Append("ToolsExtensions").ToString();
    }

    private static bool IsIdentifierStart(char character) =>
        character == '_' || char.IsLetter(character);

    private static bool IsIdentifierPart(char character) =>
        character == '_' || char.IsLetterOrDigit(character);

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

    private static void Generate(
        SourceProductionContext context,
        ImmutableArray<IntegrationCandidate> candidates,
        ParseOptions parseOptions,
        EquatableArray<ReferencedToolProperty> referencedToolProperties)
    {
        foreach (var candidate in candidates)
        {
            if (candidate.Registration is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    InvalidIntegrationMethod,
                    candidate.Location,
                    candidate.MethodName));
            }
        }

        var uniqueRegistrations = candidates
            .Select(static candidate => candidate.Registration)
            .OfType<IntegrationRegistration>()
            .Distinct()
            .OrderBy(static registration => registration.TypeName, StringComparer.Ordinal)
            .ThenBy(static registration => registration.MethodName, StringComparer.Ordinal)
            .ToArray();

        var toolProperties = candidates
            .SelectMany(static candidate => candidate.ToolProperties)
            .ToArray();
        var allToolProperties = toolProperties
            .Concat(referencedToolProperties.Select(static property => new ToolProperty(
                property.Name,
                property.TypeName,
                MethodName: property.Name,
                Location: null)))
            .ToArray();
        var conflictingPropertyNames = ReportToolPropertyConflicts(context, allToolProperties);
        var uniqueToolProperties = toolProperties
            .Where(property => !conflictingPropertyNames.Contains(property.Name))
            .GroupBy(static property => new
            {
                property.Name,
                property.TypeName,
            })
            .Select(static group => group.First())
            .OrderBy(static property => property.Name, StringComparer.Ordinal)
            .ThenBy(static property => property.TypeName, StringComparer.Ordinal)
            .ToArray();

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
        if (uniqueToolProperties.Length > 0 && SupportsExtensionMembers(parseOptions))
        {
            foreach (var property in uniqueToolProperties)
            {
                builder.AppendLine(
                    "[assembly: global::System.Reflection.AssemblyMetadataAttribute("
                    + $"{Literal(ToolPropertyMetadataPrefix + property.Name)}, "
                    + $"{Literal(property.TypeName)})]");
            }
        }

        builder.AppendLine();
        builder.AppendLine("namespace ModularPipelines.Generated");
        builder.AppendLine("{");
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
        builder.AppendLine("}");

        if (uniqueToolProperties.Length > 0 && !SupportsExtensionMembers(parseOptions))
        {
            var firstProperty = uniqueToolProperties[0];
            context.ReportDiagnostic(Diagnostic.Create(
                UnsupportedToolsLanguageVersion,
                firstProperty.Location,
                firstProperty.MethodName,
                GetLanguageVersionDisplay(parseOptions)));
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
                    $"        public {property.TypeName} {property.Name} "
                    + $"=> tools.Get<{property.TypeName}>();");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");
            builder.AppendLine("}");
        }

        context.AddSource("ModularPipelines.IntegrationRegistration.g.cs", builder.ToString());
    }

    private static ImmutableHashSet<string> ReportToolPropertyConflicts(
        SourceProductionContext context,
        ToolProperty[] toolProperties)
    {
        var conflicts = toolProperties
            .GroupBy(static property => property.Name, StringComparer.Ordinal)
            .Where(static group =>
                group.Select(static property => property.TypeName)
                    .Distinct(StringComparer.Ordinal)
                    .Skip(1)
                    .Any())
            .ToArray();

        foreach (var conflict in conflicts)
        {
            var types = string.Join(
                ", ",
                conflict.Select(static property => property.TypeName)
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(static typeName => typeName, StringComparer.Ordinal));
            foreach (var property in conflict)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    ConflictingToolProperty,
                    property.Location,
                    property.Name,
                    types));
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

    private sealed record IntegrationCandidate(
        IntegrationRegistration? Registration,
        EquatableArray<ToolProperty> ToolProperties,
        string GeneratedTypeName,
        string MethodName,
        Location Location);

    private sealed record IntegrationRegistration(string TypeName, string MethodName);

    private sealed record ToolProperty(
        string Name,
        string TypeName,
        string MethodName,
        Location? Location);

    private sealed record ReferencedToolProperty(
        string Name,
        string TypeName);
}
