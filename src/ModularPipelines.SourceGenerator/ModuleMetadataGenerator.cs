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
    internal const string ModuleNamespace = "ModularPipelines";
    internal const string GenericModuleMetadataName = "Module`1";
    internal const string DependsOnAttributeFullName = "ModularPipelines.DependsOnAttribute";
    internal const string GenericDependsOnAttributeMetadataName = "DependsOnAttribute`1";
    internal const string DependsOnAttributeNamespace = "ModularPipelines";
    internal const string SelectorDependencyAttributeFullName =
        "ModularPipelines.DependsOnAllModulesInheritingFromAttribute";

    internal const string PredicateDependencyAttributeFullName =
        "ModularPipelines.DependsOnBaseAttribute";

    internal const string PipelineBuilderExtensionsFullName =
        "ModularPipelines.PipelineBuilderExtensions";

    private static readonly DiagnosticDescriptor SkippedModuleRuntimeMetadata =
        GeneratorDiagnostics.SkippedModuleRuntimeMetadata;

    private static readonly DiagnosticDescriptor ExternalClosedGenericModuleRuntimeMetadata =
        GeneratorDiagnostics.ExternalClosedGenericModuleRuntimeMetadata;

    private static readonly DiagnosticDescriptor GenericModuleRegistrationRuntimeMetadata =
        GeneratorDiagnostics.GenericModuleRegistrationRuntimeMetadata;

    private static readonly DiagnosticDescriptor PartialModuleRuntimeMetadata =
        GeneratorDiagnostics.PartialModuleRuntimeMetadata;

    private static readonly DiagnosticDescriptor NonConcreteModuleRegistrationRuntimeMetadata =
        GeneratorDiagnostics.NonConcreteModuleRegistrationRuntimeMetadata;

    private static readonly DiagnosticDescriptor SelectorDependencyRuntimeMetadata =
        GeneratorDiagnostics.SelectorDependencyRuntimeMetadata;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var modules = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsCandidate(node),
                static (generatorContext, _) => GetModuleMetadata(generatorContext))
            .SelectMany(static (metadata, _) => metadata)
            .WithComparer(ModuleMetadataInfoComparer.Instance);

        var moduleRegistrations = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsModuleRegistrationCandidate(node),
                static (generatorContext, _) => GetModuleRegistration(generatorContext))
            .Where(static registration => registration is not null)
            .Select(static (registration, _) => registration!);

        var registeredClosedGenericModules = moduleRegistrations
            .SelectMany(static (registration, _) =>
                registration is ClosedGenericModuleRegistration closedGeneric
                    ? closedGeneric.Metadata
                    : ImmutableArray<ModuleMetadataInfo>.Empty)
            .WithComparer(ModuleMetadataInfoComparer.Instance);

        var genericModuleRegistrations = moduleRegistrations
            .Where(static registration => registration is GenericModuleRegistration)
            .Select(static (registration, _) => (GenericModuleRegistration) registration);

        var nonConcreteModuleRegistrations = moduleRegistrations
            .Where(static registration => registration is NonConcreteModuleRegistration)
            .Select(static (registration, _) => (NonConcreteModuleRegistration) registration);

        var allModules = modules
            .Collect()
            .Combine(registeredClosedGenericModules.Collect())
            .Select(static (input, _) => input.Left.AddRange(input.Right));
        var compilationMetadata = context.CompilationProvider.Select(
            static (compilation, _) => new CompilationMetadata(
                compilation.AssemblyName,
                compilation.GetTypeByMetadataName(ModuleInterfaceFullName) is not null));

        context.RegisterSourceOutput(
            compilationMetadata.Combine(allModules),
            static (sourceContext, input) =>
                EmitModuleMetadata(sourceContext, input.Left, input.Right));

        context.RegisterSourceOutput(
            genericModuleRegistrations,
            static (sourceContext, registration) =>
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    GenericModuleRegistrationRuntimeMetadata,
                    registration.Location,
                    registration.TypeParameterName)));

        context.RegisterSourceOutput(
            nonConcreteModuleRegistrations,
            static (sourceContext, registration) =>
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    NonConcreteModuleRegistrationRuntimeMetadata,
                    registration.Location,
                    registration.TypeName)));
    }

    private static void EmitModuleMetadata(
        SourceProductionContext sourceContext,
        CompilationMetadata compilation,
        ImmutableArray<ModuleMetadataInfo> modules)
    {
        if (!compilation.HasModuleInterface)
        {
            return;
        }

        foreach (var skipped in modules
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

        foreach (var partial in modules
                     .Where(static module => module.IsPartial)
                     .GroupBy(static module => module.TypeName, StringComparer.Ordinal)
                     .Select(static group => group.First()))
        {
            sourceContext.ReportDiagnostic(Diagnostic.Create(
                PartialModuleRuntimeMetadata,
                partial.Location,
                partial.TypeName));
        }

        foreach (var selector in modules
                     .Where(static module => module.SelectorDependencyLocation is not null)
                     .GroupBy(static module => module.TypeName, StringComparer.Ordinal)
                     .Select(static group => group.First()))
        {
            sourceContext.ReportDiagnostic(Diagnostic.Create(
                SelectorDependencyRuntimeMetadata,
                selector.SelectorDependencyLocation,
                selector.TypeName));
        }

        var source = Generate(compilation.AssemblyName, modules);
        if (source is not null)
        {
            sourceContext.AddSource(
                "ModularPipelines.ModuleMetadata.g.cs",
                source);
        }
    }

    internal static bool IsModuleRegistrationCandidate(SyntaxNode node)
    {
        if (node is not InvocationExpressionSyntax invocation)
        {
            return false;
        }

        var name = invocation.Expression switch
        {
            SimpleNameSyntax directName => directName,
            MemberAccessExpressionSyntax { Name: var memberName } => memberName,
            MemberBindingExpressionSyntax { Name: var bindingName } => bindingName,
            _ => null,
        };

        return name?.Identifier.ValueText == "AddModule";
    }

    internal static INamedTypeSymbol? GetRegisteredClosedGenericModule(
        GeneratorSyntaxContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocation
            || context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method
            || !IsModuleRegistrationMethod(method)
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

    private static ModuleRegistration? GetModuleRegistration(
        GeneratorSyntaxContext context)
    {
        if (context.Node is not InvocationExpressionSyntax invocation
            || GetRegistrationMethod(context, invocation) is not { } method)
        {
            return null;
        }

        return method.TypeArguments[0] switch
        {
            ITypeParameterSymbol typeParameter => new GenericModuleRegistration(
                typeParameter.Name,
                invocation.GetLocation()),
            INamedTypeSymbol type => GetNamedModuleRegistration(
                type,
                invocation,
                context.SemanticModel.Compilation),
            _ => null,
        };
    }

    private static IMethodSymbol? GetRegistrationMethod(
        GeneratorSyntaxContext context,
        InvocationExpressionSyntax invocation)
    {
        if (context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method
            || !IsModuleRegistrationMethod(method)
            || method.TypeArguments.Length != 1)
        {
            return null;
        }

        return method;
    }

    private static ModuleRegistration? GetNamedModuleRegistration(
        INamedTypeSymbol type,
        InvocationExpressionSyntax invocation,
        Compilation compilation)
    {
        if (!ImplementsModule(type, compilation) || type.IsUnboundGenericType)
        {
            return null;
        }

        if (type.IsAbstract || type.TypeKind == TypeKind.Interface)
        {
            return new NonConcreteModuleRegistration(
                type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                invocation.GetLocation());
        }

        return type.IsGenericType
            ? new ClosedGenericModuleRegistration(
                GetRegisteredModuleMetadata(
                    type,
                    invocation,
                    compilation))
            : null;
    }

    private static bool IsModuleRegistrationMethod(IMethodSymbol method)
    {
        var containingType = (method.ReducedFrom ?? method).ContainingType.OriginalDefinition;
        return containingType.ToDisplayString() == PipelineBuilderExtensionsFullName;
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
        INamedTypeSymbol type,
        InvocationExpressionSyntax invocation,
        Compilation compilation)
    {
        if (!SymbolEqualityComparer.Default.Equals(
                type.ContainingAssembly,
                compilation.Assembly))
        {
            return
            [
                new ModuleMetadataInfo(
                    type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    GetModuleResultTypeName(type),
                    false,
                    invocation.GetLocation(),
                    ImmutableArray<DependencyMetadataInfo>.Empty,
                    false,
                    false,
                    true,
                    null),
            ];
        }

        return CreateModuleMetadataGraph(type, compilation);
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
                    ImmutableArray<DependencyMetadataInfo>.Empty,
                    false,
                    false,
                    true,
                    null));
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
            dependencies
                .GroupBy(static dependency => (dependency.TypeName, dependency.Optional))
                .Select(static group => group.First())
                .OrderBy(static dependency => dependency.TypeName, StringComparer.Ordinal)
                .ThenBy(static dependency => dependency.Optional)
                .ToImmutableArray(),
            dependenciesComplete,
            isPartial,
            false,
            GetSelectorDependencyLocation(type));
    }

    private static Location? GetSelectorDependencyLocation(INamedTypeSymbol type)
    {
        foreach (var interfaceType in type.AllInterfaces)
        {
            var attribute = interfaceType.GetAttributes()
                .FirstOrDefault(IsSelectorDependencyAttribute);
            if (attribute is not null)
            {
                return GetAttributeLocation(attribute, type);
            }
        }

        for (var current = type; current is not null; current = current.BaseType)
        {
            var attribute = current.GetAttributes().FirstOrDefault(IsSelectorDependencyAttribute);
            if (attribute is not null)
            {
                return GetAttributeLocation(attribute, type);
            }
        }

        return null;
    }

    private static Location? GetAttributeLocation(AttributeData attribute, INamedTypeSymbol type)
    {
        return attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()
               ?? type.Locations.FirstOrDefault(static location => location.IsInSource);
    }

    private static bool IsSelectorDependencyAttribute(AttributeData attribute)
    {
        if (IsDependsOnAttribute(attribute)
            && !IsBuiltInDependsOnAttribute(attribute.AttributeClass))
        {
            return true;
        }

        for (var current = attribute.AttributeClass; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() is
                SelectorDependencyAttributeFullName
                or PredicateDependencyAttributeFullName)
            {
                return true;
            }
        }

        return false;
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
                && current.ContainingNamespace.ToDisplayString() == DependsOnAttributeNamespace)
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
                           == DependsOnAttributeNamespace));
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

    private static string? Generate(string? assemblyName, ImmutableArray<ModuleMetadataInfo> items)
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

        if (emittedModules.Length == 0 && closedGenericDependencies.Length == 0)
        {
            return null;
        }

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
        sb.AppendLine("        global::ModularPipelines.Generated.GeneratedModuleMetadata.Register(");
        sb.AppendLine($"            typeof({registrationTypeName}).Assembly,");
        sb.AppendLine("            new global::ModularPipelines.Generated.GeneratedModuleRegistration[]");
        sb.AppendLine("            {");

        foreach (var module in emittedModules)
        {
            sb.AppendLine($"                global::ModularPipelines.Generated.GeneratedModuleMetadata.CreateRegistration<{GetRegistrationTypeArguments(module.TypeName, module.ResultTypeName)}>(");
            sb.AppendLine("                    new global::ModularPipelines.Generated.ModuleDependencyMetadata[]");
            sb.AppendLine("                    {");

            foreach (var dependency in module.Dependencies)
            {
                sb.AppendLine($"                        new(typeof({dependency.TypeName}), {BooleanLiteral(dependency.Optional)}),");
            }

            sb.AppendLine($"                    }}, dependenciesComplete: {BooleanLiteral(module.DependenciesComplete)}),");
        }

        foreach (var dependency in closedGenericDependencies)
        {
            sb.AppendLine($"                global::ModularPipelines.Generated.GeneratedModuleMetadata.CreateRegistration<{GetRegistrationTypeArguments(dependency.TypeName, dependency.ResultTypeName)}>(");
            sb.AppendLine("                    global::System.Array.Empty<global::ModularPipelines.Generated.ModuleDependencyMetadata>(),");
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
        EquatableArray<DependencyMetadataInfo> Dependencies,
        bool DependenciesComplete,
        bool IsPartial,
        bool IsExternalRegistration,
        Location? SelectorDependencyLocation);

    private sealed class ModuleMetadataInfoComparer : IEqualityComparer<ModuleMetadataInfo>
    {
        public static ModuleMetadataInfoComparer Instance { get; } = new();

        public bool Equals(ModuleMetadataInfo? x, ModuleMetadataInfo? y) =>
            ReferenceEquals(x, y)
            || (x is not null
                && y is not null
                && StringComparer.Ordinal.Equals(x.TypeName, y.TypeName)
                && StringComparer.Ordinal.Equals(x.ResultTypeName, y.ResultTypeName)
                && x.CanEmit == y.CanEmit
                && x.Dependencies.Equals(y.Dependencies)
                && x.DependenciesComplete == y.DependenciesComplete
                && x.IsPartial == y.IsPartial
                && x.IsExternalRegistration == y.IsExternalRegistration
                && LocationsEqualWhenRequired(x, y));

        public int GetHashCode(ModuleMetadataInfo obj)
        {
            var hashCode = StringComparer.Ordinal.GetHashCode(obj.TypeName);
            hashCode = (hashCode * 397) ^ (obj.ResultTypeName is null
                ? 0
                : StringComparer.Ordinal.GetHashCode(obj.ResultTypeName));
            hashCode = (hashCode * 397) ^ obj.CanEmit.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.Dependencies.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DependenciesComplete.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.IsPartial.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.IsExternalRegistration.GetHashCode();
            if (!obj.CanEmit || obj.IsPartial)
            {
                hashCode = (hashCode * 397) ^ (obj.Location?.GetHashCode() ?? 0);
            }

            return obj.SelectorDependencyLocation is null
                ? hashCode
                : (hashCode * 397) ^ obj.SelectorDependencyLocation.GetHashCode();
        }

        private static bool LocationsEqualWhenRequired(
            ModuleMetadataInfo x,
            ModuleMetadataInfo y) =>
            ((x.CanEmit && !x.IsPartial) || Equals(x.Location, y.Location))
            && Equals(x.SelectorDependencyLocation, y.SelectorDependencyLocation);
    }

    private sealed record CompilationMetadata(string? AssemblyName, bool HasModuleInterface);

    private sealed record DependencyMetadataInfo(
        string TypeName,
        string? ResultTypeName,
        bool Optional,
        bool EmitActivationRegistration);

    private abstract record ModuleRegistration;

    private sealed record ClosedGenericModuleRegistration(
        EquatableArray<ModuleMetadataInfo> Metadata) : ModuleRegistration;

    private sealed record GenericModuleRegistration(
        string TypeParameterName,
        Location Location) : ModuleRegistration;

    private sealed record NonConcreteModuleRegistration(
        string TypeName,
        Location Location) : ModuleRegistration;
}
