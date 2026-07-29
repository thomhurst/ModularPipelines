using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
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

    private static readonly DiagnosticDescriptor InvalidIntegrationMethod =
        GeneratorDiagnostics.InvalidIntegrationMethod;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
                IntegrationAttributeFullName,
                static (node, _) => node is MethodDeclarationSyntax,
                static (generatorContext, _) => GetCandidate(generatorContext));

        context.RegisterSourceOutput(
            candidates.Collect(),
            static (sourceContext, items) => Generate(sourceContext, items));
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
                && !method.ReturnsVoid)
            .Select(static method => new ToolProperty(
                method.Name,
                method.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
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
        ImmutableArray<IntegrationCandidate> candidates)
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
            .Distinct()
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

        if (toolProperties.Length > 0)
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

            foreach (var property in toolProperties)
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

    private sealed record IntegrationCandidate(
        IntegrationRegistration? Registration,
        EquatableArray<ToolProperty> ToolProperties,
        string GeneratedTypeName,
        string MethodName,
        Location Location);

    private sealed record IntegrationRegistration(string TypeName, string MethodName);

    private sealed record ToolProperty(string Name, string TypeName);
}
