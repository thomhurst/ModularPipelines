using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Generates module discovery, DI registration, and static dependency metadata.
/// </summary>
[Generator]
public sealed class ModuleMetadataGenerator : IIncrementalGenerator
{
    internal const string ModuleInterfaceFullName = "ModularPipelines.Modules.IModule";
    internal const string DependsOnAttributeFullName = "ModularPipelines.Attributes.DependsOnAttribute";
    internal const string GenericDependsOnAttributeMetadataName = "DependsOnAttribute`1";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var modules = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsCandidate(node),
                static (generatorContext, _) => GetModuleMetadata(generatorContext))
            .Where(static metadata => metadata is not null)
            .Select(static (metadata, _) => metadata!);

        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(modules.Collect()),
            static (sourceContext, input) =>
            {
                if (input.Left.GetTypeByMetadataName(ModuleInterfaceFullName) is not null)
                {
                    sourceContext.AddSource(
                        "ModularPipelines.ModuleMetadata.g.cs",
                        Generate(input.Left.AssemblyName, input.Right));
                }
            });
    }

    private static bool IsCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { BaseList.Types.Count: > 0 }
               || (node is RecordDeclarationSyntax { BaseList.Types.Count: > 0 } record
                   && record.ClassOrStructKeyword.ValueText != "struct");
    }

    private static ModuleMetadataInfo? GetModuleMetadata(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol type
            || type.IsAbstract
            || type.IsGenericType
            || !ImplementsModule(type, context.SemanticModel.Compilation))
        {
            return null;
        }

        var currentAssembly = context.SemanticModel.Compilation.Assembly;
        var dependencies = ImmutableArray.CreateBuilder<DependencyMetadataInfo>();
        var dependenciesComplete = !HasPartialDeclaration(type);

        foreach (var attribute in GetDependencyAttributes(type))
        {
            if (!TryGetDependencyMetadata(
                    attribute,
                    context.SemanticModel.Compilation,
                    currentAssembly,
                    out var dependency,
                    out var dependencyComplete))
            {
                dependenciesComplete = false;
                continue;
            }

            dependencies.Add(dependency);
            dependenciesComplete &= dependencyComplete;
        }

        return new ModuleMetadataInfo(
            type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            IsTypeAccessible(type, currentAssembly),
            [.. dependencies
                .OrderBy(static dependency => dependency.TypeName, StringComparer.Ordinal)
                .ThenBy(static dependency => dependency.Optional)],
            dependenciesComplete);
    }

    private static bool HasPartialDeclaration(INamedTypeSymbol type)
    {
        return type.DeclaringSyntaxReferences.Any(static reference =>
            reference.GetSyntax() is TypeDeclarationSyntax declaration
            && declaration.Modifiers.Any(SyntaxKind.PartialKeyword));
    }

    private static bool TryGetDependencyMetadata(
        AttributeData attribute,
        Compilation compilation,
        IAssemblySymbol currentAssembly,
        out DependencyMetadataInfo dependency,
        out bool dependencyComplete)
    {
        dependency = default!;
        dependencyComplete = false;
        if (!TryGetDependency(attribute, out var dependencyType, out var optional)
            || dependencyType is not INamedTypeSymbol namedDependency
            || namedDependency.IsUnboundGenericType
            || !ImplementsModule(namedDependency, compilation)
            || !IsTypeReferenceAccessible(namedDependency, currentAssembly))
        {
            return false;
        }

        var isClosedGeneric = namedDependency.IsGenericType
                              && !namedDependency.IsUnboundGenericType;
        var canEmitActivationRegistration = isClosedGeneric
                                            && SymbolEqualityComparer.Default.Equals(
                                                namedDependency.ContainingAssembly,
                                                currentAssembly);
        dependency = new DependencyMetadataInfo(
            namedDependency.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            optional,
            canEmitActivationRegistration);
        dependencyComplete = !isClosedGeneric || canEmitActivationRegistration;
        return true;
    }

    private static IEnumerable<AttributeData> GetDependencyAttributes(INamedTypeSymbol type)
    {
        foreach (var interfaceType in type.AllInterfaces)
        {
            foreach (var attribute in interfaceType.GetAttributes().Where(IsDependsOnAttribute))
            {
                yield return attribute;
            }
        }

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var attribute in current.GetAttributes().Where(IsDependsOnAttribute))
            {
                yield return attribute;
            }
        }
    }

    private static bool IsDependsOnAttribute(AttributeData attribute)
    {
        for (var current = attribute.AttributeClass; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == DependsOnAttributeFullName)
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryGetDependency(
        AttributeData attribute,
        out ITypeSymbol? dependencyType,
        out bool optional)
    {
        if (!IsBuiltInDependsOnAttribute(attribute.AttributeClass))
        {
            dependencyType = null;
            optional = false;
            return false;
        }

        optional = attribute.NamedArguments
            .FirstOrDefault(static argument => argument.Key == "Optional")
            .Value.Value as bool? ?? false;

        for (var current = attribute.AttributeClass; current is not null; current = current.BaseType)
        {
            if (current.MetadataName == GenericDependsOnAttributeMetadataName
                && current.ContainingNamespace.ToDisplayString() == "ModularPipelines.Attributes")
            {
                dependencyType = current.TypeArguments[0];
                return true;
            }
        }

        dependencyType = attribute.ConstructorArguments
            .FirstOrDefault(static argument => argument.Kind == TypedConstantKind.Type)
            .Value as ITypeSymbol;
        return dependencyType is not null;
    }

    private static bool IsBuiltInDependsOnAttribute(INamedTypeSymbol? attributeType)
    {
        return attributeType is not null
               && (attributeType.ToDisplayString() == DependsOnAttributeFullName
                   || (attributeType.OriginalDefinition.MetadataName
                           == GenericDependsOnAttributeMetadataName
                       && attributeType.ContainingNamespace.ToDisplayString()
                           == "ModularPipelines.Attributes"));
    }

    private static bool ImplementsModule(INamedTypeSymbol type, Compilation compilation)
    {
        var moduleInterface = compilation.GetTypeByMetadataName(ModuleInterfaceFullName);
        if (moduleInterface is null)
        {
            return false;
        }

        return SymbolEqualityComparer.Default.Equals(type, moduleInterface)
               || type.AllInterfaces.Any(candidate =>
                   SymbolEqualityComparer.Default.Equals(candidate, moduleInterface));
    }

    private static bool IsTypeAccessible(INamedTypeSymbol type, IAssemblySymbol currentAssembly)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.IsFileLocal)
            {
                return false;
            }

            if (!IsAccessible(current, currentAssembly))
            {
                return false;
            }
        }

        return type.TypeArguments.All(typeArgument =>
            IsTypeReferenceAccessible(typeArgument, currentAssembly));
    }

    private static bool IsTypeReferenceAccessible(
        ITypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        return type switch
        {
            IArrayTypeSymbol arrayType =>
                IsTypeReferenceAccessible(arrayType.ElementType, currentAssembly),
            IPointerTypeSymbol pointerType =>
                IsTypeReferenceAccessible(pointerType.PointedAtType, currentAssembly),
            INamedTypeSymbol namedType => IsTypeAccessible(namedType, currentAssembly),
            ITypeParameterSymbol => false,
            _ => true,
        };
    }

    private static bool IsAccessible(INamedTypeSymbol type, IAssemblySymbol currentAssembly)
    {
        return type.DeclaredAccessibility == Accessibility.Public
               || (type.DeclaredAccessibility is (
                       Accessibility.Internal or Accessibility.ProtectedOrInternal)
                   && (SymbolEqualityComparer.Default.Equals(
                           type.ContainingAssembly,
                           currentAssembly)
                       || type.ContainingAssembly.GivesAccessTo(currentAssembly)));
    }

    private static string Generate(string? assemblyName, ImmutableArray<ModuleMetadataInfo> items)
    {
        var modules = items
            .GroupBy(static item => item.TypeName, StringComparer.Ordinal)
            .Select(static group => group.First())
            .OrderBy(static item => item.TypeName, StringComparer.Ordinal)
            .ToArray();
        var emittedModules = modules.Where(static module => module.CanEmit).ToArray();
        var emittedModuleNames = emittedModules
            .Select(static module => module.TypeName)
            .ToImmutableHashSet(StringComparer.Ordinal);
        var closedGenericDependencies = modules
            .SelectMany(static module => module.Dependencies)
            .Where(static dependency => dependency.EmitActivationRegistration)
            .Select(static dependency => dependency.TypeName)
            .Where(typeName => !emittedModuleNames.Contains(typeName))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static typeName => typeName, StringComparer.Ordinal)
            .ToArray();

        // Generators cannot observe modules emitted by other generators in the same
        // compilation, so assembly discovery must retain its reflection fallback.
        // TODO(#3228): Revisit completeness when final trim/AOT certification can
        // account for every generator participating in the compilation.
        const bool isComplete = false;
        var registrationTypeName = $"ModuleMetadataRegistration_{GetStableIdentifier(assemblyName)}";
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine($"internal static class {registrationTypeName}");
        sb.AppendLine("{");
        sb.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");
        sb.AppendLine("        global::ModularPipelines.Engine.GeneratedModuleMetadata.Register(");
        sb.AppendLine($"            typeof({registrationTypeName}).Assembly,");
        sb.AppendLine("            new global::ModularPipelines.Engine.GeneratedModuleRegistration[]");
        sb.AppendLine("            {");

        foreach (var module in emittedModules)
        {
            sb.AppendLine($"                global::ModularPipelines.Engine.GeneratedModuleMetadata.CreateRegistration<{module.TypeName}>(");
            sb.AppendLine("                    new global::ModularPipelines.Engine.ModuleDependencyMetadata[]");
            sb.AppendLine("                    {");

            foreach (var dependency in module.Dependencies)
            {
                sb.AppendLine($"                        new(typeof({dependency.TypeName}), {BooleanLiteral(dependency.Optional)}),");
            }

            sb.AppendLine($"                    }}, dependenciesComplete: {BooleanLiteral(module.DependenciesComplete)}),");
        }

        foreach (var typeName in closedGenericDependencies)
        {
            sb.AppendLine($"                global::ModularPipelines.Engine.GeneratedModuleMetadata.CreateRegistration<{typeName}>(");
            sb.AppendLine("                    global::System.Array.Empty<global::ModularPipelines.Engine.ModuleDependencyMetadata>(),");
            sb.AppendLine("                    dependenciesComplete: false),");
        }

        sb.AppendLine($"            }}, isComplete: {BooleanLiteral(isComplete)});");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static string BooleanLiteral(bool value) => value ? "true" : "false";

    private static string GetStableIdentifier(string? value)
    {
        var text = value ?? "Assembly";
        var identifier = new StringBuilder(text.Length + 9);
        var hash = 2166136261;

        foreach (var character in text)
        {
            identifier.Append(char.IsLetterOrDigit(character) ? character : '_');
            hash = unchecked((hash ^ character) * 16777619);
        }

        return $"{identifier}_{hash:X8}";
    }

    private sealed record ModuleMetadataInfo(
        string TypeName,
        bool CanEmit,
        ImmutableArray<DependencyMetadataInfo> Dependencies,
        bool DependenciesComplete);

    private sealed record DependencyMetadataInfo(
        string TypeName,
        bool Optional,
        bool EmitActivationRegistration);
}
