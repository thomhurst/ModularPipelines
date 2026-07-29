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
    internal const string ModuleNamespace = "ModularPipelines.Modules";
    internal const string GenericModuleMetadataName = "Module`1";
    internal const string DependsOnAttributeFullName = "ModularPipelines.Attributes.DependsOnAttribute";
    internal const string GenericDependsOnAttributeMetadataName = "DependsOnAttribute`1";
    internal const string PipelineBuilderExtensionsFullName =
        "ModularPipelines.Extensions.PipelineBuilderExtensions";

    private static readonly DiagnosticDescriptor SkippedModuleRuntimeMetadata =
        GeneratorDiagnostics.SkippedModuleRuntimeMetadata;

    private static readonly DiagnosticDescriptor ExternalClosedGenericModuleRuntimeMetadata =
        GeneratorDiagnostics.ExternalClosedGenericModuleRuntimeMetadata;

    private static readonly DiagnosticDescriptor GenericModuleRegistrationRuntimeMetadata =
        GeneratorDiagnostics.GenericModuleRegistrationRuntimeMetadata;

    private static readonly DiagnosticDescriptor PartialModuleRuntimeMetadata =
        GeneratorDiagnostics.PartialModuleRuntimeMetadata;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var modules = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsCandidate(node),
                static (generatorContext, _) => GetModuleMetadata(generatorContext))
            .SelectMany(static (metadata, _) => metadata);

        var registeredClosedGenericModules = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsModuleRegistrationCandidate(node),
                static (generatorContext, _) => GetRegisteredModuleMetadata(generatorContext))
            .SelectMany(static (metadata, _) => metadata);

        var genericModuleRegistrations = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsModuleRegistrationCandidate(node),
                static (generatorContext, _) => GetGenericModuleRegistration(generatorContext))
            .Where(static registration => registration is not null);

        var allModules = modules
            .Collect()
            .Combine(registeredClosedGenericModules.Collect())
            .Select(static (input, _) => input.Left.AddRange(input.Right));

        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(allModules),
            static (sourceContext, input) =>
            {
                if (input.Left.GetTypeByMetadataName(ModuleInterfaceFullName) is not null)
                {
                    foreach (var skipped in input.Right
                                 .Where(static module => !module.CanEmit)
                                 .GroupBy(static module => module.TypeName, StringComparer.Ordinal)
                                 .Select(static group => group.First()))
                    {
                        sourceContext.ReportDiagnostic(Diagnostic.Create(
                            skipped.IsExternalRegistration
                                ? ExternalClosedGenericModuleRuntimeMetadata
                                : SkippedModuleRuntimeMetadata,
                            skipped.Location,
                            skipped.TypeName));
                    }

                    foreach (var partial in input.Right
                                 .Where(static module => module.IsPartial)
                                 .GroupBy(static module => module.TypeName, StringComparer.Ordinal)
                                 .Select(static group => group.First()))
                    {
                        sourceContext.ReportDiagnostic(Diagnostic.Create(
                            PartialModuleRuntimeMetadata,
                            partial.Location,
                            partial.TypeName));
                    }

                    sourceContext.AddSource(
                        "ModularPipelines.ModuleMetadata.g.cs",
                        Generate(input.Left.AssemblyName, input.Right));
                }
            });

        context.RegisterSourceOutput(
            genericModuleRegistrations,
            static (sourceContext, registration) =>
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    GenericModuleRegistrationRuntimeMetadata,
                    registration!.Location,
                    registration.TypeParameterName)));
    }

    internal static bool IsModuleRegistrationCandidate(SyntaxNode node)
    {
        if (node is not InvocationExpressionSyntax invocation)
        {
            return false;
        }

        var genericName = invocation.Expression switch
        {
            GenericNameSyntax directName => directName,
            MemberAccessExpressionSyntax { Name: GenericNameSyntax memberName } => memberName,
            _ => null,
        };

        return genericName is
        {
            Identifier.ValueText: "AddModule",
            TypeArgumentList.Arguments.Count: 1,
        };
    }

    internal static INamedTypeSymbol? GetRegisteredClosedGenericModule(
        GeneratorSyntaxContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocation
            || context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method
            || (method.ReducedFrom ?? method).ContainingType.ToDisplayString()
            != PipelineBuilderExtensionsFullName
            || method.TypeArguments.Length != 1
            || method.TypeArguments[0] is not INamedTypeSymbol type
            || !type.IsGenericType
            || type.IsUnboundGenericType
            || type.IsAbstract
            || !ImplementsModule(type, context.SemanticModel.Compilation))
        {
            return null;
        }

        return type;
    }

    internal static ImmutableArray<INamedTypeSymbol> GetClosedGenericModuleDependencies(
        INamedTypeSymbol type,
        Compilation compilation)
    {
        return
        [
            .. GetDependencyAttributes(type)
                .Select(attribute =>
                    TryGetDependency(attribute, out var dependencyType, out _)
                        ? dependencyType as INamedTypeSymbol
                        : null)
                .OfType<INamedTypeSymbol>()
                .Where(dependency =>
                    dependency.IsGenericType
                    && !dependency.IsUnboundGenericType
                    && ImplementsModule(dependency, compilation))
                .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default),
        ];
    }

    private static GenericModuleRegistrationInfo? GetGenericModuleRegistration(
        GeneratorSyntaxContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocation
            || context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method
            || (method.ReducedFrom ?? method).ContainingType.ToDisplayString()
            != PipelineBuilderExtensionsFullName
            || method.TypeArguments.Length != 1
            || method.TypeArguments[0] is not ITypeParameterSymbol typeParameter)
        {
            return null;
        }

        return new GenericModuleRegistrationInfo(
            typeParameter.Name,
            invocation.GetLocation());
    }

    private static bool IsCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { BaseList.Types.Count: > 0 }
               || (node is RecordDeclarationSyntax { BaseList.Types.Count: > 0 } record
                   && record.ClassOrStructKeyword.ValueText != "struct");
    }

    private static ImmutableArray<ModuleMetadataInfo> GetModuleMetadata(
        GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol type
            || type.IsAbstract
            || type.IsGenericType
            || !ImplementsModule(type, context.SemanticModel.Compilation))
        {
            return [];
        }

        return CreateModuleMetadataGraph(type, context.SemanticModel.Compilation);
    }

    private static ImmutableArray<ModuleMetadataInfo> GetRegisteredModuleMetadata(
        GeneratorSyntaxContext context)
    {
        var type = GetRegisteredClosedGenericModule(context);
        if (type is null)
        {
            return [];
        }

        var invocation = (InvocationExpressionSyntax) context.Node;
        if (!SymbolEqualityComparer.Default.Equals(
                type.ContainingAssembly,
                context.SemanticModel.Compilation.Assembly))
        {
            return
            [
                new ModuleMetadataInfo(
                    type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    GetModuleResultTypeName(type),
                    false,
                    invocation.GetLocation(),
                    [],
                    false,
                    false,
                    true),
            ];
        }

        return CreateModuleMetadataGraph(type, context.SemanticModel.Compilation);
    }

    private static ImmutableArray<ModuleMetadataInfo> CreateModuleMetadataGraph(
        INamedTypeSymbol root,
        Compilation compilation)
    {
        var metadata = ImmutableArray.CreateBuilder<ModuleMetadataInfo>();
        var pending = new Stack<INamedTypeSymbol>();
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        pending.Push(root);

        while (pending.Count > 0)
        {
            var type = pending.Pop();
            if (!visited.Add(type))
            {
                continue;
            }

            metadata.Add(CreateModuleMetadata(type, compilation));
            foreach (var dependency in GetClosedGenericModuleDependencies(type, compilation))
            {
                if (SymbolEqualityComparer.Default.Equals(
                        dependency.ContainingAssembly,
                        compilation.Assembly))
                {
                    pending.Push(dependency);
                    continue;
                }

                metadata.Add(new ModuleMetadataInfo(
                    dependency.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    GetModuleResultTypeName(dependency),
                    false,
                    GetDependencyLocation(type, dependency),
                    [],
                    false,
                    false,
                    true));
            }
        }

        return metadata.ToImmutable();
    }

    private static Location? GetDependencyLocation(
        INamedTypeSymbol type,
        INamedTypeSymbol dependency)
    {
        foreach (var attribute in GetDependencyAttributes(type))
        {
            if (TryGetDependency(attribute, out var candidate, out _)
                && SymbolEqualityComparer.Default.Equals(candidate, dependency))
            {
                return attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                       ?? type.Locations.FirstOrDefault(static location => location.IsInSource);
            }
        }

        return type.Locations.FirstOrDefault(static location => location.IsInSource);
    }

    private static ModuleMetadataInfo CreateModuleMetadata(
        INamedTypeSymbol type,
        Compilation compilation)
    {
        var currentAssembly = compilation.Assembly;
        var dependencies = ImmutableArray.CreateBuilder<DependencyMetadataInfo>();
        var isPartial = HasPartialDeclaration(type);
        var dependenciesComplete = !isPartial;

        foreach (var attribute in GetDependencyAttributes(type))
        {
            if (!TryGetDependencyMetadata(
                    attribute,
                    compilation,
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
            GetModuleResultTypeName(type),
            IsTypeAccessible(type, currentAssembly),
            type.Locations.FirstOrDefault(static location => location.IsInSource),
            [.. dependencies
                .OrderBy(static dependency => dependency.TypeName, StringComparer.Ordinal)
                .ThenBy(static dependency => dependency.Optional)],
            dependenciesComplete,
            isPartial,
            false);
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
            GetModuleResultTypeName(namedDependency),
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

    private static string? GetModuleResultTypeName(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.OriginalDefinition.MetadataName == GenericModuleMetadataName
                && current.OriginalDefinition.ContainingNamespace.ToDisplayString() == ModuleNamespace)
            {
                return current.TypeArguments[0]
                    .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            }
        }

        return null;
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
            .Where(dependency => !emittedModuleNames.Contains(dependency.TypeName))
            .GroupBy(static dependency => dependency.TypeName, StringComparer.Ordinal)
            .Select(static group => group.First())
            .OrderBy(static dependency => dependency.TypeName, StringComparer.Ordinal)
            .ToArray();

        // Generators cannot observe modules emitted by other generators in the same
        // compilation. Assembly-wide discovery therefore remains incomplete and uses
        // the documented reflection fallback; explicitly registered source modules use
        // the trim-safe registrations below.
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
            sb.AppendLine($"                global::ModularPipelines.Engine.GeneratedModuleMetadata.CreateRegistration<{GetRegistrationTypeArguments(module.TypeName, module.ResultTypeName)}>(");
            sb.AppendLine("                    new global::ModularPipelines.Engine.ModuleDependencyMetadata[]");
            sb.AppendLine("                    {");

            foreach (var dependency in module.Dependencies)
            {
                sb.AppendLine($"                        new(typeof({dependency.TypeName}), {BooleanLiteral(dependency.Optional)}),");
            }

            sb.AppendLine($"                    }}, dependenciesComplete: {BooleanLiteral(module.DependenciesComplete)}),");
        }

        foreach (var dependency in closedGenericDependencies)
        {
            sb.AppendLine($"                global::ModularPipelines.Engine.GeneratedModuleMetadata.CreateRegistration<{GetRegistrationTypeArguments(dependency.TypeName, dependency.ResultTypeName)}>(");
            sb.AppendLine("                    global::System.Array.Empty<global::ModularPipelines.Engine.ModuleDependencyMetadata>(),");
            sb.AppendLine("                    dependenciesComplete: false),");
        }

        sb.AppendLine($"            }}, isComplete: {BooleanLiteral(isComplete)});");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static string BooleanLiteral(bool value) => value ? "true" : "false";

    private static string GetRegistrationTypeArguments(string moduleTypeName, string? resultTypeName)
    {
        return resultTypeName is null
            ? moduleTypeName
            : $"{moduleTypeName}, {resultTypeName}";
    }

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
        string? ResultTypeName,
        bool CanEmit,
        Location? Location,
        ImmutableArray<DependencyMetadataInfo> Dependencies,
        bool DependenciesComplete,
        bool IsPartial,
        bool IsExternalRegistration);

    private sealed record DependencyMetadataInfo(
        string TypeName,
        string? ResultTypeName,
        bool Optional,
        bool EmitActivationRegistration);

    private sealed record GenericModuleRegistrationInfo(
        string TypeParameterName,
        Location Location);
}
