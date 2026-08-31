using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Generates direct property-access metadata for CLI options and secrets.
/// </summary>
[Generator]
public sealed class CommandOptionsGenerator : IIncrementalGenerator
{
    private const int RuntimeRegistrationChunkSize = 32;
    private const int RuntimeMetadataSchemaVersion = 2;
    private const int CommandMetadataSchemaVersion = 4;
    private const string CliOptionValueFullName = "ModularPipelines.Models.CliOptionValue";
    private const string CliValuePairFullName = "ModularPipelines.Models.CliValuePair";

    internal const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";
    internal const string OptionsNamespace = "Microsoft.Extensions.Options";
    internal const string DependencyInjectionNamespace = "Microsoft.Extensions.DependencyInjection";
    internal const string DependencyInjectionExtensionsNamespace =
        "Microsoft.Extensions.DependencyInjection.Extensions";
    internal const string CliOptionAttributeFullName = "ModularPipelines.Attributes.CliOptionAttribute";
    internal const string CliFlagAttributeFullName = "ModularPipelines.Attributes.CliFlagAttribute";
    internal const string CliArgumentAttributeFullName = "ModularPipelines.Attributes.CliArgumentAttribute";
    internal const string CliGlobalOptionsAttributeFullName = "ModularPipelines.Attributes.CliGlobalOptionsAttribute";
    internal const string SecretValueAttributeFullName = "ModularPipelines.Secrets.SecretValueAttribute";
    internal const string ExperimentalAttributeFullName = "System.Diagnostics.CodeAnalysis.ExperimentalAttribute";
    internal const string IncompleteRuntimeMetadataAttributeFullName =
        "ModularPipelines.Generated.IncompleteRuntimeMetadataAttribute";
    internal const string RuntimeMetadataRegistrationFullName =
        "ModularPipelines.Generated.RuntimeMetadataRegistration";

    private static readonly DiagnosticDescriptor IncompleteCommandMetadata =
        GeneratorDiagnostics.IncompleteCommandMetadata;

    private static readonly DiagnosticDescriptor IncompleteSecretMetadata =
        GeneratorDiagnostics.IncompleteSecretMetadata;

    private static readonly DiagnosticDescriptor SkippedRuntimeMetadata =
        GeneratorDiagnostics.SkippedRuntimeMetadata;

    private static readonly DiagnosticDescriptor IncompatibleRuntimeMetadata =
        GeneratorDiagnostics.IncompatibleRuntimeMetadata;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeCandidates = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsTypeCandidate(node),
                static (generatorContext, _) => GetTypeCandidate(generatorContext))
            .Where(static item => item is not null)
            .Select(static (item, _) => item!)
            .WithComparer(TypeMetadataCandidateComparer.Instance);

        var secretCandidates = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SecretValueAttributeFullName,
                static (_, _) => true,
                static (generatorContext, _) => GetTypeCandidate(generatorContext))
            .Where(static item => item is not null)
            .Select(static (item, _) => item!)
            .WithComparer(TypeMetadataCandidateComparer.Instance);
        var optionsTypeUsages = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsOptionsTypeUsageCandidate(node),
                static (generatorContext, _) => GetOptionsTypeUsage(generatorContext))
            .Where(static usage => usage is not null)
            .Select(static (usage, _) => usage!);
        var collectedOptionsTypeUsages = optionsTypeUsages
            .Where(static usage => usage.TypeIdentity is not null)
            .Select(static (usage, _) => usage.TypeIdentity!)
            .Collect();

        var externalTypeCandidates = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Combine(collectedOptionsTypeUsages)
            .Select(static (input, _) => GetExternalTypeCandidates(
                input.Left.Left,
                input.Left.Right,
                input.Right));
        context.RegisterSourceOutput(
            externalTypeCandidates,
            static (sourceContext, result) => ReportIncompatibleRuntimeMetadata(
                sourceContext,
                result.IncompatibleAssemblies));
        var compatibleExternalTypeCandidates = externalTypeCandidates.Select(
            static (result, _) => result.Candidates);
        var coveredExternalAssemblyIdentities = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(static (input, _) => GetCoveredExternalAssemblyIdentities(
                input.Left,
                input.Right));
        var runtimeMetadataConfiguration = coveredExternalAssemblyIdentities
            .Combine(context.AnalyzerConfigOptionsProvider.Select(
                static (optionsProvider, _) => IsTrimOrAotEnabled(optionsProvider)));
        var hasRuntimeReference = context.CompilationProvider.Select(
            static (compilation, _) => compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName) is not null);
        context.RegisterSourceOutput(
            optionsTypeUsages
                .Where(static usage => usage.TypeParameterName is not null)
                .Combine(hasRuntimeReference),
            static (sourceContext, input) => ReportGenericOptionsUsage(sourceContext, input));
        var sourceCandidates = typeCandidates.Collect()
            .Combine(secretCandidates.Collect())
            .Select(static (input, _) => input.Left.AddRange(input.Right));
        var candidates = sourceCandidates
            .Combine(compatibleExternalTypeCandidates)
            .Select(static (input, _) => input.Left.AddRange(input.Right))
            .Combine(hasRuntimeReference);
        var generationInputs = candidates
            .Combine(collectedOptionsTypeUsages)
            .Combine(runtimeMetadataConfiguration);
        context.RegisterSourceOutput(
            generationInputs,
            static (sourceContext, input) => GenerateSource(sourceContext, input));
    }

    private static void ReportGenericOptionsUsage(
        SourceProductionContext sourceContext,
        (OptionsTypeUsage Usage, bool HasRuntimeReference) input)
    {
        if (!input.HasRuntimeReference)
        {
            return;
        }

        sourceContext.ReportDiagnostic(Diagnostic.Create(
            SkippedRuntimeMetadata,
            input.Usage.Location,
            input.Usage.TypeParameterName));
    }

    private static void GenerateSource(
        SourceProductionContext sourceContext,
        (((ImmutableArray<TypeMetadataCandidate> Candidates, bool HasRuntimeReference) Candidates,
            ImmutableArray<OptionsTypeIdentity> OptionsTypes) Generation,
            (ImmutableArray<string> CoveredExternalAssemblyIdentities, bool RequiresGeneratedMetadata) Configuration) input)
    {
        if (!input.Generation.Candidates.HasRuntimeReference)
        {
            return;
        }

        var candidates = input.Generation.Candidates.Candidates;
        var optionsTypes = new HashSet<OptionsTypeIdentity>(input.Generation.OptionsTypes);
        var ambiguousMetadataNames = new HashSet<string>(candidates
            .Where(RequiresDirectTypeReference)
            .GroupBy(static candidate => candidate.MetadataName, StringComparer.Ordinal)
            .Where(static group => group
                .Select(static candidate => candidate.AssemblyIdentity)
                .Distinct(StringComparer.Ordinal)
                .Skip(1)
                .Any())
            .Select(static group => group.Key), StringComparer.Ordinal);
        foreach (var collision in candidates
                     .Where(candidate => ambiguousMetadataNames.Contains(candidate.MetadataName))
                     .GroupBy(static candidate => candidate.MetadataName, StringComparer.Ordinal))
        {
            foreach (var candidate in collision
                         .GroupBy(static candidate => candidate.AssemblyIdentity, StringComparer.Ordinal)
                         .Select(static group => group.First()))
            {
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    SkippedRuntimeMetadata,
                    candidate.Location,
                    candidate.TypeName));
            }
        }

        var unambiguousCandidates = candidates
            .Where(candidate => !ambiguousMetadataNames.Contains(candidate.MetadataName))
            .ToArray();
        foreach (var skipped in unambiguousCandidates
                     .Where(static candidate => candidate.Metadata is null)
                     .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                     .Select(static group => group.First()))
        {
            sourceContext.ReportDiagnostic(Diagnostic.Create(
                SkippedRuntimeMetadata,
                skipped.Location,
                skipped.TypeName));
        }

        var items = unambiguousCandidates
            .Select(static candidate => candidate.Metadata)
            .OfType<TypeMetadata>()
            .ToImmutableArray();
        ReportIncompleteMetadata(
            sourceContext,
            unambiguousCandidates,
            optionsTypes);
        foreach (var (hintName, source) in Generate(
                     items,
                     input.Configuration.CoveredExternalAssemblyIdentities,
                     input.Configuration.RequiresGeneratedMetadata))
        {
            sourceContext.AddSource(hintName, source);
        }
    }

    internal static bool IsTypeCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax
            || node is StructDeclarationSyntax
            || node is RecordDeclarationSyntax
            || node is EnumDeclarationSyntax
            || node is DelegateDeclarationSyntax;
    }

    private static TypeMetadataCandidate? GetTypeCandidate(GeneratorSyntaxContext context)
    {
        return GetTypeCandidate(
            context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol,
            context.SemanticModel.Compilation,
            hasKnownSecretAttribute: false);
    }

    private static OptionsTypeUsage? GetOptionsTypeUsage(GeneratorSyntaxContext context)
    {
        var optionsTypeSymbol = GetOptionsTypeUsageSymbol(context);
        if (optionsTypeSymbol is INamedTypeSymbol
            {
                TypeKind: not TypeKind.Error,
                ContainingNamespace: not null,
            } optionsType
            && IsConcreteOptionsRegistrationUsage(context))
        {
            return new OptionsTypeUsage(
                new OptionsTypeIdentity(
                    GetMetadataName(optionsType),
                    optionsType.ContainingAssembly.Identity.ToString()),
                TypeParameterName: null,
                context.Node.GetLocation());
        }

        var enclosingNamespace = context.SemanticModel
            .GetEnclosingSymbol(context.Node.SpanStart)?
            .ContainingNamespace?
            .ToDisplayString();
        return enclosingNamespace != OptionsNamespace
               && optionsTypeSymbol is ITypeParameterSymbol typeParameter
               && IsGenericOptionsRegistrationUsage(context)
            ? new OptionsTypeUsage(
                TypeIdentity: null,
                typeParameter.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                context.Node.GetLocation())
            : null;
    }

    private static bool IsGenericOptionsRegistrationUsage(GeneratorSyntaxContext context)
    {
        var symbol = context.SemanticModel.GetSymbolInfo(context.Node).Symbol;
        return symbol switch
        {
            INamedTypeSymbol type => IsOptionsBuilder(type)
                || IsServiceTypeRegistrationUsage(context),
            IMethodSymbol method => GetRegisteredOptionsType(context, method) is ITypeParameterSymbol,
            _ => false,
        };
    }

    private static bool IsConcreteOptionsRegistrationUsage(GeneratorSyntaxContext context)
    {
        var symbol = context.SemanticModel.GetSymbolInfo(context.Node).Symbol;
        return symbol switch
        {
            INamedTypeSymbol type => IsOptionsBuilder(type)
                                     || IsServiceTypeRegistrationUsage(context),
            IMethodSymbol method => GetRegisteredOptionsType(context, method) is INamedTypeSymbol,
            _ => false,
        };
    }

    private static bool IsOptionsBuilder(INamedTypeSymbol type) =>
        type.OriginalDefinition is
        {
            MetadataName: "OptionsBuilder`1",
            ContainingNamespace: { } containingNamespace,
        } && containingNamespace.ToDisplayString() == OptionsNamespace;

    private static bool IsServiceTypeRegistrationUsage(GeneratorSyntaxContext context)
    {
        var typeOfExpression = context.Node.FirstAncestorOrSelf<TypeOfExpressionSyntax>();
        if (typeOfExpression?.Parent is not ArgumentSyntax argument
            || argument.Parent is not BaseArgumentListSyntax argumentList
            || argumentList.Parent is not ExpressionSyntax descriptorCreation
            || context.SemanticModel.GetSymbolInfo(descriptorCreation).Symbol is not IMethodSymbol method
            || !IsServiceTypeCarrier(method))
        {
            return false;
        }

        var argumentIndex = argumentList.Arguments.IndexOf(argument);
        var parameter = argument.NameColon is { Name.Identifier.ValueText: { } parameterName }
            ? method.Parameters.FirstOrDefault(candidate => candidate.Name == parameterName)
            : argumentIndex >= 0 && argumentIndex < method.Parameters.Length
                ? method.Parameters[argumentIndex]
                : null;
        return parameter?.Name == "serviceType";
    }

    internal static bool IsServiceTypeCarrier(IMethodSymbol method)
    {
        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        return IsServiceCollectionRegistration(definition)
               || IsTryAddRegistration(definition)
               || IsServiceDescriptorRegistration(definition)
               || (definition.ContainingNamespace?.ToDisplayString() == DependencyInjectionNamespace
                   && definition.ContainingType.MetadataName == "ServiceDescriptor"
                   && definition.MethodKind == MethodKind.Constructor);
    }

    private static ITypeSymbol? GetOptionsTypeUsageSymbol(GeneratorSyntaxContext context)
    {
        var symbol = context.SemanticModel.GetSymbolInfo(context.Node).Symbol;
        return symbol switch
        {
            INamedTypeSymbol constructedType when IsOptionsTypeUsage(constructedType) =>
                constructedType.TypeArguments[0],
            IMethodSymbol method => GetRegisteredOptionsType(context, method),
            _ => null,
        };
    }

    private static ITypeSymbol? GetRegisteredOptionsType(
        GeneratorSyntaxContext context,
        IMethodSymbol method) =>
        GetRegisteredOptionsType(method)
        ?? GetServiceTypeArgumentOptionsType(context, method);

    private static ITypeSymbol? GetServiceTypeArgumentOptionsType(
        GeneratorSyntaxContext context,
        IMethodSymbol method)
    {
        if (!IsServiceTypeCarrier(method))
        {
            return null;
        }

        var argumentList = context.Node switch
        {
            InvocationExpressionSyntax invocation => invocation.ArgumentList,
            BaseObjectCreationExpressionSyntax objectCreation => objectCreation.ArgumentList,
            _ => null,
        };
        if (argumentList is null)
        {
            return null;
        }

        for (var index = 0; index < argumentList.Arguments.Count; index++)
        {
            var argument = argumentList.Arguments[index];
            if (ResolveArgumentParameter(method, argument, index)?.Name != "serviceType")
            {
                continue;
            }

            var optionsType = GetTypeOfOptionsArgument(context, argument);
            if (optionsType is not null)
            {
                return optionsType;
            }
        }

        return null;
    }

    private static IParameterSymbol? ResolveArgumentParameter(
        IMethodSymbol method,
        ArgumentSyntax argument,
        int index)
    {
        if (argument.NameColon is { Name.Identifier.ValueText: { } parameterName })
        {
            return method.Parameters.FirstOrDefault(candidate => candidate.Name == parameterName);
        }

        return index < method.Parameters.Length ? method.Parameters[index] : null;
    }

    private static ITypeSymbol? GetTypeOfOptionsArgument(
        GeneratorSyntaxContext context,
        ArgumentSyntax argument)
    {
        if (argument.Expression is not TypeOfExpressionSyntax typeOfExpression
            || typeOfExpression.Type.DescendantNodesAndSelf().Any(IsOptionsTypeUsageCandidate)
            || context.SemanticModel.GetTypeInfo(typeOfExpression.Type).Type
                is not INamedTypeSymbol serviceType
            || !IsOptionsTypeUsage(serviceType))
        {
            return null;
        }

        return serviceType.TypeArguments[0];
    }

    private static bool IsOptionsTypeUsageCandidate(SyntaxNode node) =>
        IsServiceDescriptorObjectCreationCandidate(node)
        || node is GenericNameSyntax
        {
            Identifier.ValueText: "IOptions"
                or "IOptionsMonitor"
                or "IOptionsSnapshot"
                or "IConfigureOptions"
                or "IPostConfigureOptions"
                or "IValidateOptions"
                or "IConfigureNamedOptions"
                or "OptionsBuilder"
                or "Configure"
                or "ConfigureAll"
                or "PostConfigure"
                or "PostConfigureAll"
                or "AddOptions"
                or "AddOptionsWithValidateOnStart",
            TypeArgumentList.Arguments.Count: > 0,
        }
        || (node is InvocationExpressionSyntax invocation
            && IsServiceRegistrationMethodName(GetInvokedMethodName(invocation)));

    private static bool IsServiceDescriptorObjectCreationCandidate(SyntaxNode node) =>
        node is ImplicitObjectCreationExpressionSyntax
        {
            ArgumentList.Arguments.Count: > 0,
        }
            or ObjectCreationExpressionSyntax
        {
            Type: IdentifierNameSyntax { Identifier.ValueText: "ServiceDescriptor" }
                    or QualifiedNameSyntax { Right.Identifier.ValueText: "ServiceDescriptor" }
                    or AliasQualifiedNameSyntax { Name.Identifier.ValueText: "ServiceDescriptor" },
        };

    private static bool IsServiceRegistrationMethodName(string? methodName) =>
        IsServiceCollectionRegistrationMethodName(methodName)
        || IsTryAddRegistrationMethodName(methodName)
        || IsServiceDescriptorRegistrationMethodName(methodName);

    private static bool IsServiceCollectionRegistrationMethodName(string? methodName) => methodName is
        "AddSingleton"
        or "AddScoped"
        or "AddTransient"
        or "AddKeyedSingleton"
        or "AddKeyedScoped"
        or "AddKeyedTransient";

    private static bool IsTryAddRegistrationMethodName(string? methodName) => methodName is
        "TryAddSingleton"
        or "TryAddScoped"
        or "TryAddTransient"
        or "TryAddKeyedSingleton"
        or "TryAddKeyedScoped"
        or "TryAddKeyedTransient";

    private static bool IsServiceDescriptorRegistrationMethodName(string? methodName) => methodName is
        "Singleton"
        or "Scoped"
        or "Transient"
        or "KeyedSingleton"
        or "KeyedScoped"
        or "KeyedTransient"
        or "Describe"
        or "DescribeKeyed";

    private static string? GetInvokedMethodName(InvocationExpressionSyntax invocation) =>
        invocation.Expression switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText,
            _ => null,
        };

    internal static bool IsOptionsTypeUsage(INamedTypeSymbol type)
    {
        var definition = type.OriginalDefinition;
        return type.TypeArguments.Length == 1
               && definition.ContainingNamespace?.ToDisplayString() == OptionsNamespace
               && definition.MetadataName is
                   "IOptions`1"
                   or "IOptionsMonitor`1"
                   or "IOptionsSnapshot`1"
                   or "IConfigureOptions`1"
                   or "IPostConfigureOptions`1"
                   or "IValidateOptions`1"
                   or "IConfigureNamedOptions`1"
                   or "OptionsBuilder`1";
    }

    internal static bool IsOptionsRegistrationMethod(IMethodSymbol method)
    {
        if (method.TypeArguments.Length == 0)
        {
            return false;
        }

        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (definition.ContainingNamespace?.ToDisplayString() != DependencyInjectionNamespace)
        {
            return false;
        }

        return definition.ContainingType.MetadataName switch
        {
            "OptionsServiceCollectionExtensions" => definition.Name is
                "Configure"
                or "ConfigureAll"
                or "PostConfigure"
                or "PostConfigureAll"
                or "AddOptions"
                or "AddOptionsWithValidateOnStart",
            "OptionsConfigurationServiceCollectionExtensions" => definition.Name == "Configure",
            _ => false,
        };
    }

    private static ITypeSymbol? GetRegisteredOptionsType(IMethodSymbol method)
    {
        if (IsOptionsRegistrationMethod(method))
        {
            return method.TypeArguments[0];
        }

        var definition = (method.ReducedFrom ?? method).OriginalDefinition;
        if (!IsServiceCollectionRegistration(definition)
            && !IsTryAddRegistration(definition)
            && !IsServiceDescriptorRegistration(definition))
        {
            return null;
        }

        return method.TypeArguments
            .OfType<INamedTypeSymbol>()
            .FirstOrDefault(IsOptionsTypeUsage)
            ?.TypeArguments[0];
    }

    private static bool IsServiceCollectionRegistration(IMethodSymbol method)
    {
        return method.ContainingNamespace?.ToDisplayString() == DependencyInjectionNamespace
               && method.ContainingType.MetadataName == "ServiceCollectionServiceExtensions"
               && IsServiceCollectionRegistrationMethodName(method.Name);
    }

    private static bool IsTryAddRegistration(IMethodSymbol method)
    {
        return method.ContainingNamespace?.ToDisplayString() == DependencyInjectionExtensionsNamespace
               && method.ContainingType.MetadataName == "ServiceCollectionDescriptorExtensions"
               && IsTryAddRegistrationMethodName(method.Name);
    }

    private static bool IsServiceDescriptorRegistration(IMethodSymbol method)
    {
        return method.ContainingNamespace?.ToDisplayString() == DependencyInjectionNamespace
               && method.ContainingType.MetadataName == "ServiceDescriptor"
               && IsServiceDescriptorRegistrationMethodName(method.Name);
    }

    private static TypeMetadataCandidate? GetTypeCandidate(GeneratorAttributeSyntaxContext context)
    {
        return GetTypeCandidate(
            context.TargetSymbol.ContainingType,
            context.SemanticModel.Compilation,
            hasKnownSecretAttribute: true);
    }

    private static TypeMetadataCandidate? GetTypeCandidate(
        INamedTypeSymbol? type,
        Compilation compilation,
        bool hasKnownSecretAttribute,
        bool isExternal = false,
        PropertyCollection? precomputedSecretMetadata = null)
    {
        if (type is null)
        {
            return null;
        }

        var isCommandOptions = InheritsFrom(type, CommandLineToolOptionsFullName);
        if (isCommandOptions && type is { IsAbstract: true, IsGenericType: true })
        {
            return null;
        }

        var typeName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var location = type.Locations.FirstOrDefault() ?? Location.None;
        var hasPartialDeclaration = HasPartialDeclarationInHierarchy(type);
        var canRegisterSecretCoverage = !hasPartialDeclaration;
        var canReferenceType = IsTypeAccessible(type, compilation.Assembly)
                               && !HasGenericTypeInHierarchy(type)
                               && !HasObsoleteErrorInHierarchy(type);
        if (!canReferenceType)
        {
            return GetInaccessibleTypeCandidate(
                type,
                compilation.Assembly,
                typeName,
                location,
                isCommandOptions,
                hasKnownSecretAttribute,
                precomputedSecretMetadata,
                canRegisterSecretCoverage,
                isExternal);
        }

        var commandMetadata = isCommandOptions
            ? GetCommandProperties(type, compilation.Assembly)
            : PropertyCollection.Empty;
        var secretMetadata = precomputedSecretMetadata
                             ?? GetSecretProperties(type, compilation.Assembly);
        return GetAccessibleTypeCandidate(
            type,
            typeName,
            location,
            isCommandOptions,
            hasPartialDeclaration,
            commandMetadata,
            secretMetadata,
            isExternal);
    }

    private static TypeMetadataCandidate GetInaccessibleTypeCandidate(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly,
        string typeName,
        Location location,
        bool isCommandOptions,
        bool hasKnownSecretAttribute,
        PropertyCollection? precomputedSecretMetadata,
        bool canRegisterSecretCoverage,
        bool isExternal)
    {
        var hasSecretAttributes = precomputedSecretMetadata?.HasAttributes
                                  ?? (hasKnownSecretAttribute
                                      || GetSecretProperties(type, currentAssembly).HasAttributes);
        if (isCommandOptions || hasSecretAttributes)
        {
            return new TypeMetadataCandidate(
                typeName,
                GetMetadataName(type),
                type.ContainingAssembly.Identity.ToString(),
                location,
                Metadata: null);
        }

        return new TypeMetadataCandidate(
            typeName,
            GetMetadataName(type),
            type.ContainingAssembly.Identity.ToString(),
            location,
            new TypeMetadata(
                typeName,
                GetMetadataName(type),
                type.ContainingAssembly.Identity.ToString(),
                CanRegisterCommandMetadata: false,
                CanRegisterSecretCoverage: canRegisterSecretCoverage,
                UseTypeForEmptySecretCoverage: false,
                UseExternalTypeNameForEmptySecretCoverage: isExternal,
                IsExternal: isExternal,
                IsCommandOptions: false,
                PropertyCollection.Empty,
                PropertyCollection.Empty,
                EquatableArray<string>.Empty));
    }

    private static TypeMetadataCandidate GetAccessibleTypeCandidate(
        INamedTypeSymbol type,
        string typeName,
        Location location,
        bool isCommandOptions,
        bool hasPartialDeclaration,
        PropertyCollection commandMetadata,
        PropertyCollection secretMetadata,
        bool isExternal)
    {
        var metadata = hasPartialDeclaration
                       && (isCommandOptions || secretMetadata.HasAttributes)
            ? null
            : new TypeMetadata(
                typeName,
                GetMetadataName(type),
                type.ContainingAssembly.Identity.ToString(),
                CanRegisterCommandMetadata: !hasPartialDeclaration,
                CanRegisterSecretCoverage: !hasPartialDeclaration,
                UseTypeForEmptySecretCoverage: isExternal,
                UseExternalTypeNameForEmptySecretCoverage: false,
                IsExternal: isExternal,
                isCommandOptions,
                commandMetadata,
                secretMetadata,
                GetExperimentalDiagnosticIds(type));
        return new TypeMetadataCandidate(
            typeName,
            GetMetadataName(type),
            type.ContainingAssembly.Identity.ToString(),
            location,
            metadata);
    }

    private static PropertyCollection GetCommandProperties(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        var properties = new Dictionary<string, PropertyMetadata>(StringComparer.Ordinal);
        var seenPropertyNames = new HashSet<string>(StringComparer.Ordinal);
        var incompleteProperties = ImmutableArray.CreateBuilder<string>();
        var isComplete = true;
        var hasAttributes = false;

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var property in current.GetMembers().OfType<IPropertySymbol>())
            {
                if (!TryGetCommandProperty(
                        property,
                        currentAssembly,
                        seenPropertyNames,
                        out var propertyMetadata,
                        out var isIncomplete))
                {
                    continue;
                }

                hasAttributes = true;
                if (isIncomplete)
                {
                    isComplete = false;
                    incompleteProperties.Add(property.Name);
                    continue;
                }

                properties.Add(property.Name, propertyMetadata);
            }
        }

        return new PropertyCollection(
            properties.Values.ToImmutableArray(),
            incompleteProperties.ToImmutable(),
            isComplete,
            hasAttributes);
    }

    private static bool TryGetCommandProperty(
        IPropertySymbol property,
        IAssemblySymbol currentAssembly,
        ISet<string> seenPropertyNames,
        out PropertyMetadata propertyMetadata,
        out bool isIncomplete)
    {
        propertyMetadata = null!;
        isIncomplete = false;
        if (property.IsStatic || property.GetMethod is null || !seenPropertyNames.Add(property.Name))
        {
            return false;
        }

        var attribute = FindCommandAttribute(property, out var hasConflictingAttributes);
        if (hasConflictingAttributes)
        {
            isIncomplete = true;
            return true;
        }

        if (attribute is null)
        {
            return false;
        }

        if (!IsPropertyAccessible(property, currentAssembly) || HasObsoleteError(property))
        {
            isIncomplete = true;
            return true;
        }

        propertyMetadata = CreatePropertyMetadata(property, attribute);
        isIncomplete = propertyMetadata is
        {
            Kind: PropertyKind.Flag or PropertyKind.Option,
            PrimaryValue: null,
        };
        return true;
    }

    private static PropertyCollection GetSecretProperties(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        var properties = ImmutableArray.CreateBuilder<PropertyMetadata>();
        var seenPropertySlots = new HashSet<IPropertySymbol>(SymbolEqualityComparer.Default);
        var incompleteProperties = ImmutableArray.CreateBuilder<string>();
        var isComplete = true;
        var hasAttributes = false;

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var property in current.GetMembers().OfType<IPropertySymbol>())
            {
                if (property.IsStatic || property.GetMethod is null)
                {
                    continue;
                }

                var secretAttribute = FindAttribute(property, IsSecretAttribute);
                if (secretAttribute is null)
                {
                    continue;
                }

                if (!seenPropertySlots.Add(GetPropertySlot(property)))
                {
                    continue;
                }

                hasAttributes = true;
                if (!IsPropertyAccessible(property, currentAssembly)
                    || HasObsoleteError(property))
                {
                    isComplete = false;
                    incompleteProperties.Add(property.Name);
                    continue;
                }

                properties.Add(new PropertyMetadata(
                    property.Name,
                    PropertyKind.Secret,
                    null,
                    null,
                    false,
                    false,
                    false,
                    0,
                    "Normal",
                    0,
                    GetConstructorStrings(secretAttribute),
                    false,
                    false,
                    0,
                    property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    false,
                    false));
            }
        }

        return new PropertyCollection(
            properties.ToImmutable(),
            incompleteProperties.ToImmutable(),
            isComplete,
            hasAttributes);
    }

    private static AttributeData? FindAttribute(
        IPropertySymbol property,
        Func<AttributeData, bool> predicate)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            var attribute = current.GetAttributes().FirstOrDefault(predicate);
            if (attribute is not null)
            {
                return attribute;
            }
        }

        return null;
    }

    private static AttributeData? FindCommandAttribute(
        IPropertySymbol property,
        out bool hasConflictingAttributes)
    {
        AttributeData? commandAttribute = null;
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            var attributes = current.GetAttributes()
                .Where(IsCommandAttribute)
                .Take(2)
                .ToArray();
            if (attributes.Length > 1 || (attributes.Length == 1 && commandAttribute is not null))
            {
                hasConflictingAttributes = true;
                return null;
            }

            if (attributes.Length == 1)
            {
                commandAttribute = attributes[0];
            }
        }

        hasConflictingAttributes = false;
        return commandAttribute;
    }

    private static IPropertySymbol GetPropertySlot(IPropertySymbol property)
    {
        while (property.OverriddenProperty is { } overriddenProperty)
        {
            property = overriddenProperty;
        }

        return property;
    }

    private static PropertyMetadata CreatePropertyMetadata(IPropertySymbol property, AttributeData attribute)
    {
        var isGlobalOption = IsGlobalOption(property);
        var attributeName = attribute.AttributeClass?.ToDisplayString();
        if (attributeName == CliArgumentAttributeFullName)
        {
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Argument,
                null,
                null,
                GetNamedBool(attribute, "PrependOptionTerminator"),
                false,
                GetNamedBool(attribute, "Required"),
                GetConstructorInt(attribute),
                GetNamedEnumMemberName(attribute, "Phase", "Passthrough"),
                0,
                EquatableArray<string>.Empty,
                GetNamedBool(attribute, "PrependOptionTerminatorIfValueStartsWithDash"),
                isGlobalOption,
                0,
                property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                false,
                attribute.ConstructorArguments.Length > 0,
                GetNamedBool(attribute, "RepeatOptionTerminator"));
        }

        if (attributeName == CliFlagAttributeFullName)
        {
            var negatedName = GetNamedString(attribute, "NegatedName");
            return new PropertyMetadata(
                property.Name,
                PropertyKind.Flag,
                GetConstructorString(attribute),
                GetNamedString(attribute, "ShortForm"),
                GetNamedBool(attribute, "PreferShortForm"),
                false,
                false,
                0,
                GetNamedEnumMemberName(attribute, "Phase", "Normal"),
                0,
                EquatableArray<string>.Empty,
                false,
                isGlobalOption,
                0,
                property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                IsSupportedFlagType(property.Type)
                && (negatedName is null || IsNullableBooleanType(property.Type)),
                false,
                NegatedName: negatedName);
        }

        return new PropertyMetadata(
            property.Name,
            PropertyKind.Option,
            GetConstructorString(attribute),
            GetNamedString(attribute, "ShortForm"),
            GetNamedBool(attribute, "PreferShortForm"),
            GetNamedBool(attribute, "GroupValues"),
            false,
            GetNamedInt(attribute, "Format"),
            GetNamedEnumMemberName(attribute, "Phase", "Normal"),
            GetNamedInt(attribute, "ValueArity"),
            EquatableArray<string>.Empty,
            false,
            isGlobalOption,
            GetManualOperandCount(property.Type),
            property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            IsSupportedOptionalValueType(property.Type),
            false,
            CollectionSeparator: GetNamedString(attribute, "CollectionSeparator"));
    }

    private static bool IsSupportedFlagType(ITypeSymbol propertyType)
    {
        propertyType = UnwrapNullable(propertyType);
        return propertyType.SpecialType is SpecialType.System_Boolean or SpecialType.System_Int32;
    }

    private static bool IsNullableBooleanType(ITypeSymbol propertyType) =>
        propertyType is INamedTypeSymbol namedType
        && namedType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
        && namedType.TypeArguments.Length == 1
        && namedType.TypeArguments[0].SpecialType == SpecialType.System_Boolean;

    private static bool IsSupportedOptionalValueType(ITypeSymbol propertyType)
    {
        propertyType = UnwrapNullable(propertyType);
        return IsType(propertyType, CliOptionValueFullName)
               || IsEnumerableOf(propertyType, CliOptionValueFullName);
    }

    private static bool IsEnumerableOf(ITypeSymbol propertyType, string elementTypeName)
    {
        if (propertyType is IArrayTypeSymbol arrayType)
        {
            return IsType(arrayType.ElementType, elementTypeName);
        }

        return propertyType is INamedTypeSymbol namedType
               && namedType.AllInterfaces
                   .Append(namedType)
                   .Any(type => type.OriginalDefinition.SpecialType
                                == SpecialType.System_Collections_Generic_IEnumerable_T
                                && IsType(type.TypeArguments[0], elementTypeName));
    }

    private static bool IsType(ITypeSymbol type, string typeName) =>
        typeName == "string"
            ? type.SpecialType == SpecialType.System_String
            : type.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString() == typeName;

    private static ITypeSymbol UnwrapNullable(ITypeSymbol propertyType) =>
        propertyType is INamedTypeSymbol nullableType
        && nullableType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
        && nullableType.TypeArguments.Length == 1
            ? nullableType.TypeArguments[0]
            : propertyType;

    private static int GetManualOperandCount(ITypeSymbol propertyType)
    {
        if (propertyType is INamedTypeSymbol nullableType
            && nullableType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T
            && nullableType.TypeArguments.Length == 1)
        {
            propertyType = nullableType.TypeArguments[0];
        }

        if (IsCliValuePair(propertyType))
        {
            return 2;
        }

        if (propertyType is IArrayTypeSymbol arrayType
            && IsCliValuePair(arrayType.ElementType))
        {
            return 2;
        }

        return propertyType is INamedTypeSymbol namedType
               && namedType.AllInterfaces
                   .Append(namedType)
                   .Any(type => type.OriginalDefinition.SpecialType
                                == SpecialType.System_Collections_Generic_IEnumerable_T
                                && IsCliValuePair(type.TypeArguments[0]))
            ? 2
            : 1;
    }

    private static bool IsCliValuePair(ITypeSymbol type)
    {
        for (var current = type.WithNullableAnnotation(NullableAnnotation.None) as INamedTypeSymbol;
             current is not null;
             current = current.BaseType)
        {
            if (current.ToDisplayString() == CliValuePairFullName)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsGlobalOption(IPropertySymbol property)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            if (current.ContainingType.GetAttributes()
                .Any(candidate => IsAttribute(candidate, CliGlobalOptionsAttributeFullName)))
            {
                return true;
            }
        }

        return false;
    }

    private static List<(string HintName, string Source)> Generate(
        ImmutableArray<TypeMetadata> items,
        ImmutableArray<string> coveredExternalAssemblyIdentities,
        bool requiresGeneratedMetadata)
    {
        var uniqueItems = items
            .GroupBy(GetRegistrationIdentity, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(item => item.MetadataName, StringComparer.Ordinal)
            .ThenBy(item => item.AssemblyIdentity, StringComparer.Ordinal)
            .ToList();

        if (uniqueItems.Count(RequiresCommandRegistrationChunking) <= RuntimeRegistrationChunkSize)
        {
            return
            [
                ("ModularPipelines.RuntimeMetadata.g.cs", GenerateSingleSource(
                    uniqueItems,
                    coveredExternalAssemblyIdentities,
                    requiresGeneratedMetadata)),
            ];
        }

        return GenerateChunkedSources(
            uniqueItems,
            coveredExternalAssemblyIdentities,
            requiresGeneratedMetadata);
    }

    private static string GenerateSingleSource(
        IReadOnlyList<TypeMetadata> uniqueItems,
        ImmutableArray<string> coveredExternalAssemblyIdentities,
        bool requiresGeneratedMetadata)
    {
        var sb = new StringBuilder();

        AppendGeneratedFilePreamble(sb, uniqueItems);
        AppendRuntimeMetadataRegistration(
            sb,
            uniqueItems,
            coveredExternalAssemblyIdentities,
            requiresGeneratedMetadata);
        return sb.ToString();
    }

    private static List<(string HintName, string Source)> GenerateChunkedSources(
        IReadOnlyList<TypeMetadata> uniqueItems,
        ImmutableArray<string> coveredExternalAssemblyIdentities,
        bool requiresGeneratedMetadata)
    {
        var registrationItems = uniqueItems
            .Where(RequiresRuntimeRegistrationChunk)
            .ToArray();
        var chunks = new List<IReadOnlyList<TypeMetadata>>();
        for (var offset = 0; offset < registrationItems.Length; offset += RuntimeRegistrationChunkSize)
        {
            chunks.Add(
            [
                .. registrationItems
                    .Skip(offset)
                    .Take(RuntimeRegistrationChunkSize),
            ]);
        }

        var sources = new List<(string HintName, string Source)>(chunks.Count + 1);
        var registrationSource = new StringBuilder();
        AppendGeneratedFilePreamble(registrationSource, uniqueItems);
        AppendChunkedRuntimeMetadataRegistration(
            registrationSource,
            uniqueItems,
            coveredExternalAssemblyIdentities,
            requiresGeneratedMetadata,
            chunks.Count);
        sources.Add(("ModularPipelines.RuntimeMetadata.g.cs", registrationSource.ToString()));

        for (var index = 0; index < chunks.Count; index++)
        {
            var chunkSource = new StringBuilder();
            AppendGeneratedFilePreamble(
                chunkSource,
                chunks[index],
                includeIncompleteMetadataAttributes: false);
            AppendRuntimeMetadataChunk(chunkSource, chunks[index], index);
            sources.Add((
                $"ModularPipelines.RuntimeMetadata.Chunk{index:D4}.g.cs",
                chunkSource.ToString()));
        }

        return sources;
    }

    private static string GetRegistrationIdentity(TypeMetadata item) =>
        item.UseExternalTypeNameForEmptySecretCoverage
            ? $"{item.AssemblyIdentity}\0{item.MetadataName}"
            : item.MetadataName;

    private static void AppendGeneratedFilePreamble(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems,
        bool includeIncompleteMetadataAttributes = true)
    {
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        if (includeIncompleteMetadataAttributes)
        {
            foreach (var item in uniqueItems.Where(HasIncompleteSecretMetadata))
            {
                sb.AppendLine(
                    $"[assembly: global::{IncompleteRuntimeMetadataAttributeFullName}({Literal(item.MetadataName)})]");
            }
        }

        var suppressedDiagnostics = uniqueItems
            .SelectMany(static item => item.ExperimentalDiagnosticIds)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static diagnosticId => diagnosticId, StringComparer.Ordinal);
        sb.Append("#pragma warning disable CS0612, CS0618");
        foreach (var diagnosticId in suppressedDiagnostics)
        {
            sb.Append($", {diagnosticId}");
        }

        sb.AppendLine(" // Generated metadata must register obsolete and experimental types.");
        sb.AppendLine();
    }

    private static void AppendRuntimeMetadataRegistration(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems,
        ImmutableArray<string> coveredExternalAssemblyIdentities,
        bool requiresGeneratedMetadata)
    {
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class RuntimeMetadataRegistration");
        sb.AppendLine("{");
        sb.AppendLine($"    public const int SchemaVersion = {RuntimeMetadataSchemaVersion};");
        sb.AppendLine($"    public const int CommandSchemaVersion = {CommandMetadataSchemaVersion};");
        sb.AppendLine();
        AppendCommandMetadataDependencies(sb, uniqueItems);

        sb.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        sb.AppendLine("    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");
        sb.AppendLine("        var assembly = global::System.Reflection.Assembly.GetExecutingAssembly();");
        var requiresGeneratedMetadataLiteral = requiresGeneratedMetadata ? "true" : "false";
        AppendAssemblyRegistration(
            sb,
            "global::ModularPipelines.Generated.GeneratedCommandMetadata",
            requiresGeneratedMetadataLiteral);
        AppendAssemblyRegistration(
            sb,
            "global::ModularPipelines.Generated.GeneratedSecretMetadata",
            requiresGeneratedMetadataLiteral);
        AppendStringRegistration(
            sb,
            "RegisterCoveredExternalAssemblyIdentities",
            coveredExternalAssemblyIdentities);
        AppendCoverageRegistrations(sb, uniqueItems);
        AppendRuntimeTypeRegistrations(sb, uniqueItems);

        sb.AppendLine("    }");
        sb.AppendLine("}");
    }

    private static void AppendChunkedRuntimeMetadataRegistration(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems,
        ImmutableArray<string> coveredExternalAssemblyIdentities,
        bool requiresGeneratedMetadata,
        int chunkCount)
    {
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class RuntimeMetadataRegistration");
        sb.AppendLine("{");
        sb.AppendLine($"    public const int SchemaVersion = {RuntimeMetadataSchemaVersion};");
        sb.AppendLine($"    public const int CommandSchemaVersion = {CommandMetadataSchemaVersion};");
        sb.AppendLine();
        sb.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        sb.AppendLine("    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");
        sb.AppendLine("        var assembly = global::System.Reflection.Assembly.GetExecutingAssembly();");
        var requiresGeneratedMetadataLiteral = requiresGeneratedMetadata ? "true" : "false";
        AppendAssemblyRegistration(
            sb,
            "global::ModularPipelines.Generated.GeneratedCommandMetadata",
            requiresGeneratedMetadataLiteral);
        AppendAssemblyRegistration(
            sb,
            "global::ModularPipelines.Generated.GeneratedSecretMetadata",
            requiresGeneratedMetadataLiteral);
        AppendStringRegistration(
            sb,
            "RegisterCoveredExternalAssemblyIdentities",
            coveredExternalAssemblyIdentities);
        AppendCoverageRegistrations(sb, uniqueItems);
        for (var index = 0; index < chunkCount; index++)
        {
            sb.AppendLine(
                $"        RuntimeMetadataRegistrationChunk{index:D4}.Register(assembly);");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
    }

    private static void AppendRuntimeMetadataChunk(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> items,
        int index)
    {
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine($"internal static class RuntimeMetadataRegistrationChunk{index:D4}");
        sb.AppendLine("{");
        AppendCommandMetadataDependencies(sb, items);
        sb.AppendLine("    internal static void Register(global::System.Reflection.Assembly assembly)");
        sb.AppendLine("    {");
        AppendRuntimeTypeRegistrations(sb, items);
        sb.AppendLine("    }");
        sb.AppendLine("}");
    }

    private static void AppendCoverageRegistrations(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems)
    {
        AppendExternalTypeNameRegistrations(
            sb,
            "RegisterCoveredExternalTypeNames",
            uniqueItems.Where(static item => item.UseExternalTypeNameForEmptySecretCoverage));
        AppendTypeNameRegistration(
            sb,
            "RegisterCoveredTypeNames",
            uniqueItems.Where(item => item.IsCommandOptions),
            "global::ModularPipelines.Generated.GeneratedCommandMetadata");
        AppendTypeNameRegistration(
            sb,
            "RegisterCoveredTypeNames",
            uniqueItems.Where(item => item.CanRegisterSecretCoverage
                                      && item.SecretMetadata.IsComplete
                                      && item.SecretMetadata.Properties.Count == 0
                                      && !item.UseTypeForEmptySecretCoverage
                                      && !item.UseExternalTypeNameForEmptySecretCoverage));
        AppendTypeNameRegistration(
            sb,
            "RegisterIncompleteTypeNames",
            uniqueItems.Where(HasIncompleteSecretMetadata));
    }

    private static void AppendCommandMetadataDependencies(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems)
    {
        foreach (var item in uniqueItems)
        {
            if (CanPreserveCommandOptionProperties(item))
            {
                sb.AppendLine(
                    $"    [global::System.Diagnostics.CodeAnalysis.DynamicDependency(" +
                    $"global::System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicProperties | " +
                    $"global::System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicProperties, " +
                    $"typeof({item.TypeName}))]");
            }
        }
    }

    private static void AppendRuntimeTypeRegistrations(
        StringBuilder sb,
        IReadOnlyList<TypeMetadata> uniqueItems)
    {
        foreach (var item in uniqueItems)
        {
            if (CanRegisterCompleteCommandMetadata(item))
            {
                AppendCommandRegistration(sb, item);
            }

            if (item.SecretMetadata.IsComplete)
            {
                AppendSecretRegistration(sb, item);
            }
        }
    }

    private static bool RequiresRuntimeRegistrationChunk(TypeMetadata item) =>
        CanPreserveCommandOptionProperties(item)
        || CanRegisterCompleteCommandMetadata(item)
        || item.SecretMetadata.IsComplete;

    private static bool RequiresCommandRegistrationChunking(TypeMetadata item) =>
        !item.IsExternal
        && item.IsCommandOptions
        && (CanPreserveCommandOptionProperties(item) || CanRegisterCompleteCommandMetadata(item));

    private static void AppendAssemblyRegistration(
        StringBuilder sb,
        string registryType,
        string requiresGeneratedMetadataLiteral)
    {
        sb.AppendLine(
            $"        {registryType}.RegisterAssembly(assembly, requiresGeneratedMetadata: {requiresGeneratedMetadataLiteral});");
    }

    private static void AppendTypeNameRegistration(
        StringBuilder sb,
        string methodName,
        IEnumerable<TypeMetadata> items,
        string registryType = "global::ModularPipelines.Generated.GeneratedSecretMetadata")
    {
        AppendStringRegistration(
            sb,
            methodName,
            items.Select(item => item.MetadataName),
            registryType);
    }

    private static void AppendExternalTypeNameRegistrations(
        StringBuilder sb,
        string methodName,
        IEnumerable<TypeMetadata> items)
    {
        foreach (var assemblyGroup in items.GroupBy(static item => item.AssemblyIdentity, StringComparer.Ordinal))
        {
            sb.AppendLine(
                $"        global::ModularPipelines.Generated.GeneratedSecretMetadata.{methodName}(");
            sb.AppendLine("            assembly,");
            sb.AppendLine($"            {Literal(assemblyGroup.Key)},");
            sb.AppendLine("            new string[]");
            sb.AppendLine("            {");
            foreach (var item in assemblyGroup)
            {
                sb.AppendLine($"                {Literal(item.MetadataName)},");
            }

            sb.AppendLine("            });");
        }
    }

    private static void AppendStringRegistration(
        StringBuilder sb,
        string methodName,
        IEnumerable<string> values,
        string registryType = "global::ModularPipelines.Generated.GeneratedSecretMetadata")
    {
        var valueList = values.ToList();
        if (valueList.Count == 0)
        {
            return;
        }

        sb.AppendLine($"        {registryType}.{methodName}(");
        sb.AppendLine("            assembly,");
        sb.AppendLine("            new string[]");
        sb.AppendLine("            {");
        foreach (var value in valueList)
        {
            sb.AppendLine($"                {Literal(value)},");
        }

        sb.AppendLine("            });");
    }

    private static void ReportIncompleteMetadata(
        SourceProductionContext context,
        IReadOnlyCollection<TypeMetadataCandidate> candidates,
        IReadOnlyCollection<OptionsTypeIdentity> optionsTypes)
    {
        foreach (var candidate in candidates
                     .Where(static candidate => candidate.Metadata is not null)
                     .GroupBy(static candidate => (candidate.TypeName, candidate.AssemblyIdentity))
                     .Select(static group => group.First()))
        {
            var item = candidate.Metadata!;
            var isObservedOptionsType = optionsTypes.Contains(new OptionsTypeIdentity(
                item.MetadataName,
                candidate.AssemblyIdentity));
            if (item.CommandMetadata.IsComplete
                && item.SecretMetadata.IsComplete
                && !item.CanRegisterSecretCoverage
                && isObservedOptionsType)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    SkippedRuntimeMetadata,
                    candidate.Location,
                    item.TypeName));
            }

            if (!item.CommandMetadata.IsComplete)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    IncompleteCommandMetadata,
                    candidate.Location,
                    item.TypeName,
                    string.Join(", ", item.CommandMetadata.IncompletePropertyNames)));
            }

            if (!item.SecretMetadata.IsComplete)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    IncompleteSecretMetadata,
                    candidate.Location,
                    item.TypeName,
                    string.Join(", ", item.SecretMetadata.IncompletePropertyNames)));
            }
        }
    }

    private static void AppendCommandRegistration(StringBuilder sb, TypeMetadata item)
    {
        var registrationMethod = item.IsExternal ? "RegisterExternal" : "Register";
        sb.AppendLine($"        global::ModularPipelines.Generated.GeneratedCommandMetadata.{registrationMethod}(");
        if (item.IsExternal)
        {
            sb.AppendLine("            assembly,");
        }

        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Generated.PropertyCommandLinePart[]");
        sb.AppendLine("            {");

        foreach (var property in item.CommandMetadata.Properties)
        {
            var getter = $"static instance => (({property.AccessorTypeName})instance).@{property.Name}";
            switch (property.Kind)
            {
                case PropertyKind.Argument:
                    sb.AppendLine("                new global::ModularPipelines.Generated.ArgumentPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliArgumentAttribute({property.FirstInt})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                        PrependOptionTerminator = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        RepeatOptionTerminator = {BooleanLiteral(property.RepeatOptionTerminator)},");
                    sb.AppendLine($"                        PrependOptionTerminatorIfValueStartsWithDash = {BooleanLiteral(property.PrependOptionTerminatorIfValueStartsWithDash)},");
                    sb.AppendLine($"                        Required = {BooleanLiteral(property.IsRequired)},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)}, HasExplicitPosition = {BooleanLiteral(property.HasExplicitArgumentPosition)} }},");
                    break;
                case PropertyKind.Flag:
                    sb.AppendLine("                new global::ModularPipelines.Generated.FlagPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliFlagAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        NegatedName = {NullableLiteral(property.NegatedName)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)}, IsSupportedPropertyType = {BooleanLiteral(property.IsSupportedPropertyType)} }},");
                    break;
                case PropertyKind.Option:
                    sb.AppendLine("                new global::ModularPipelines.Generated.OptionPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliOptionAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Format = (global::ModularPipelines.Attributes.OptionFormat){property.FirstInt},");
                    sb.AppendLine($"                        ValueArity = (global::ModularPipelines.Attributes.CliOptionValueArity){property.ValueArity},");
                    sb.AppendLine($"                        GroupValues = {BooleanLiteral(property.GroupValues)},");
                    sb.AppendLine($"                        CollectionSeparator = {NullableLiteral(property.CollectionSeparator)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)}, ManualOperandCount = {property.ManualOperandCount}, IsSupportedPropertyType = {BooleanLiteral(property.IsSupportedPropertyType)} }},");
                    break;
            }
        }

        sb.AppendLine("            },");
        sb.AppendLine($"            {CommandMetadataSchemaVersion});");
    }

    private static void AppendSecretRegistration(StringBuilder sb, TypeMetadata item)
    {
        if (!item.CanRegisterSecretCoverage)
        {
            return;
        }

        if (item.SecretMetadata.Properties.Count == 0)
        {
            if (item.UseTypeForEmptySecretCoverage)
            {
                sb.AppendLine($"        global::ModularPipelines.Generated.GeneratedSecretMetadata.RegisterExternal(assembly, typeof({item.TypeName}), global::System.Array.Empty<global::ModularPipelines.Generated.SecretPropertyAccessor>());");
            }

            return;
        }

        var registrationMethod = item.IsExternal ? "RegisterExternal" : "Register";
        sb.AppendLine($"        global::ModularPipelines.Generated.GeneratedSecretMetadata.{registrationMethod}(");
        if (item.IsExternal)
        {
            sb.AppendLine("            assembly,");
        }

        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Generated.SecretPropertyAccessor[]");
        sb.AppendLine("            {");

        foreach (var property in item.SecretMetadata.Properties)
        {
            sb.AppendLine($"                new({Literal(property.Name)}, static instance => (({property.AccessorTypeName})instance).@{property.Name}, {StringArrayLiteral(property.SecretValueKeys)}),");
        }

        sb.AppendLine("            });");
    }

    private static bool InheritsFrom(INamedTypeSymbol type, string metadataName)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == metadataName)
            {
                return true;
            }
        }

        return false;
    }

    private static ExternalMetadataCandidates GetExternalTypeCandidates(
        Compilation compilation,
        AnalyzerConfigOptionsProvider optionsProvider,
        ImmutableArray<OptionsTypeIdentity> optionsTypes)
    {
        if (compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName)?.ContainingAssembly is not { } runtimeAssembly)
        {
            return ExternalMetadataCandidates.Empty;
        }

        var includeAllRuntimeMetadata = IsTrimOrAotEnabled(optionsProvider);
        var usedOptionsTypes = new HashSet<OptionsTypeIdentity>(optionsTypes);
        var referencedAssemblies = GetReferencedAssemblyClosure(
                compilation.SourceModule.ReferencedAssemblySymbols)
            .ToArray();
        var incompatibleAssemblies = referencedAssemblies
            .Select(assembly => GetIncompatibleMetadataAssembly(assembly, runtimeAssembly))
            .OfType<IncompatibleMetadataAssembly>()
            .ToImmutableArray();
        var incompatibleAssemblyIdentities = new HashSet<string>(
            incompatibleAssemblies.Select(static assembly => assembly.AssemblyIdentity),
            StringComparer.Ordinal);
        var candidates = referencedAssemblies
            .Where(assembly => !incompatibleAssemblyIdentities.Contains(assembly.Identity.ToString()))
            .SelectMany(assembly => GetExternalTypeCandidates(
                assembly,
                runtimeAssembly,
                compilation,
                includeAllRuntimeMetadata,
                usedOptionsTypes))
            .ToImmutableArray();
        return new ExternalMetadataCandidates(candidates, incompatibleAssemblies);
    }

    private static IncompatibleMetadataAssembly? GetIncompatibleMetadataAssembly(
        IAssemblySymbol assembly,
        IAssemblySymbol runtimeAssembly)
    {
        if (SymbolEqualityComparer.Default.Equals(assembly, runtimeAssembly)
            || !RequiresExternalMetadata(assembly, runtimeAssembly))
        {
            return null;
        }

        var registration = assembly.GetTypeByMetadataName(RuntimeMetadataRegistrationFullName);
        if (registration is null)
        {
            return null;
        }

        var runtimeSchemaVersion = GetRuntimeMetadataSchemaVersion(registration);
        var commandSchemaVersion = GetRuntimeMetadataSchemaVersion(
            registration,
            "CommandSchemaVersion");
        return runtimeSchemaVersion == RuntimeMetadataSchemaVersion
               && commandSchemaVersion == CommandMetadataSchemaVersion
            ? null
            : new IncompatibleMetadataAssembly(
                assembly.Identity.ToString(),
                runtimeSchemaVersion,
                commandSchemaVersion);
    }

    private static void ReportIncompatibleRuntimeMetadata(
        SourceProductionContext context,
        ImmutableArray<IncompatibleMetadataAssembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                IncompatibleRuntimeMetadata,
                Location.None,
                assembly.AssemblyIdentity,
                assembly.RuntimeSchemaVersion?.ToString() ?? "missing",
                assembly.CommandSchemaVersion?.ToString() ?? "missing",
                RuntimeMetadataSchemaVersion,
                CommandMetadataSchemaVersion));
        }
    }

    private static ImmutableArray<string> GetCoveredExternalAssemblyIdentities(
        Compilation compilation,
        AnalyzerConfigOptionsProvider optionsProvider)
    {
        if (!IsTrimOrAotEnabled(optionsProvider)
            || compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName)?.ContainingAssembly is not { } runtimeAssembly)
        {
            return [];
        }

        return GetReferencedAssemblyClosure(compilation.SourceModule.ReferencedAssemblySymbols)
            .Where(assembly => !SymbolEqualityComparer.Default.Equals(assembly, runtimeAssembly)
                               && !ReferencesAssemblyDirectly(assembly, runtimeAssembly))
            .Select(assembly => assembly.Identity.ToString())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static identity => identity, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    internal static IEnumerable<IAssemblySymbol> GetReferencedAssemblyClosure(
        IEnumerable<IAssemblySymbol> referencedAssemblies)
    {
        var pendingAssemblies = new Stack<IAssemblySymbol>(referencedAssemblies);
        var visitedAssemblyIdentities = new HashSet<string>(StringComparer.Ordinal);
        while (pendingAssemblies.Count > 0)
        {
            var assembly = pendingAssemblies.Pop();
            if (!visitedAssemblyIdentities.Add(assembly.Identity.ToString()))
            {
                continue;
            }

            yield return assembly;
            foreach (var referencedAssembly in assembly.Modules
                         .SelectMany(static module => module.ReferencedAssemblySymbols))
            {
                pendingAssemblies.Push(referencedAssembly);
            }
        }
    }

    private static IEnumerable<TypeMetadataCandidate> GetExternalTypeCandidates(
        IAssemblySymbol assembly,
        IAssemblySymbol runtimeAssembly,
        Compilation compilation,
        bool includeAllRuntimeMetadata,
        HashSet<OptionsTypeIdentity> usedOptionsTypes)
    {
        if (SymbolEqualityComparer.Default.Equals(assembly, runtimeAssembly))
        {
            return [];
        }

        if (!RequiresExternalMetadata(assembly, runtimeAssembly))
        {
            return [];
        }

        var incompleteTypeNames = GetIncompleteTypeNames(assembly);
        var runtimeMetadataRegistration = assembly.GetTypeByMetadataName(
            RuntimeMetadataRegistrationFullName);
        var runtimeMetadataSchemaVersion = GetRuntimeMetadataSchemaVersion(
            runtimeMetadataRegistration);
        var commandMetadataSchemaVersion = GetRuntimeMetadataSchemaVersion(
            runtimeMetadataRegistration,
            "CommandSchemaVersion");
        var hasCurrentSecretMetadata = runtimeMetadataSchemaVersion == RuntimeMetadataSchemaVersion;
        var hasCurrentCommandMetadata = commandMetadataSchemaVersion == CommandMetadataSchemaVersion;
        return GetTypes(assembly.GlobalNamespace)
            .Select(type => GetExternalTypeCandidate(
                type,
                compilation,
                includeAllRuntimeMetadata,
                usedOptionsTypes,
                incompleteTypeNames,
                hasCurrentSecretMetadata,
                hasCurrentCommandMetadata))
            .OfType<TypeMetadataCandidate>();
    }

    private static TypeMetadataCandidate? GetExternalTypeCandidate(
        INamedTypeSymbol type,
        Compilation compilation,
        bool includeAllRuntimeMetadata,
        ISet<OptionsTypeIdentity> usedOptionsTypes,
        ISet<string> incompleteTypeNames,
        bool hasCurrentSecretMetadata,
        bool hasCurrentCommandMetadata)
    {
        var metadataName = GetMetadataName(type);
        var isObservedOptionsType = IsObservedOptionsType(type, usedOptionsTypes);
        var hasIncompleteMetadata = incompleteTypeNames.Contains(metadataName);
        var requiresRescan = !hasCurrentCommandMetadata || hasIncompleteMetadata;
        if (!requiresRescan)
        {
            return null;
        }

        var candidate = GetExternalTypeCandidate(
            type,
            compilation,
            includeAllRuntimeMetadata,
            isObservedOptionsType,
            hasIncompleteMetadata);

        if (hasCurrentSecretMetadata && !hasIncompleteMetadata)
        {
            candidate = UseExistingSecretMetadata(candidate);
        }

        return candidate;
    }

    private static TypeMetadataCandidate? UseExistingSecretMetadata(
        TypeMetadataCandidate? candidate)
    {
        if (candidate?.Metadata is not { } metadata)
        {
            return candidate;
        }

        return candidate with
        {
            Metadata = metadata with
            {
                CanRegisterSecretCoverage = true,
                UseTypeForEmptySecretCoverage = false,
                UseExternalTypeNameForEmptySecretCoverage = true,
                SecretMetadata = PropertyCollection.Empty,
            },
        };
    }

    private static TypeMetadataCandidate? GetExternalTypeCandidate(
        INamedTypeSymbol type,
        Compilation compilation,
        bool includeAllRuntimeMetadata,
        bool isObservedOptionsType,
        bool hasIncompleteMetadata) =>
        isObservedOptionsType
            ? GetExternalOptionsUsageCandidate(type, compilation, hasIncompleteMetadata)
            : includeAllRuntimeMetadata
                ? GetExternalTypeCandidate(type, compilation)
                : null;

    private static bool IsObservedOptionsType(
        INamedTypeSymbol type,
        ISet<OptionsTypeIdentity> usedOptionsTypes) =>
        usedOptionsTypes.Contains(new OptionsTypeIdentity(
            GetMetadataName(type),
            type.ContainingAssembly.Identity.ToString()));

    private static bool RequiresDirectTypeReference(TypeMetadataCandidate candidate)
    {
        if (candidate.Metadata is not { } metadata)
        {
            return false;
        }

        return CanRegisterCompleteCommandMetadata(metadata)
               || (metadata.SecretMetadata.IsComplete
                   && (metadata.SecretMetadata.Properties.Count > 0
                       || metadata.UseTypeForEmptySecretCoverage));
    }

    private static bool CanPreserveCommandOptionProperties(TypeMetadata metadata) =>
        metadata.IsCommandOptions && metadata.CanRegisterCommandMetadata;

    private static bool CanRegisterCompleteCommandMetadata(TypeMetadata metadata) =>
        CanPreserveCommandOptionProperties(metadata) && metadata.CommandMetadata.IsComplete;

    private static bool HasIncompleteSecretMetadata(TypeMetadata metadata) =>
        !metadata.CanRegisterSecretCoverage || !metadata.SecretMetadata.IsComplete;

    private static int? GetRuntimeMetadataSchemaVersion(
        INamedTypeSymbol? registration,
        string fieldName = "SchemaVersion") =>
        registration?
            .GetMembers(fieldName)
            .OfType<IFieldSymbol>()
            .FirstOrDefault(static field => field.HasConstantValue)?
            .ConstantValue as int?;

    private static TypeMetadataCandidate? GetExternalOptionsUsageCandidate(
        INamedTypeSymbol type,
        Compilation compilation,
        bool isIncomplete)
    {
        var candidate = GetTypeCandidate(
            type,
            compilation,
            hasKnownSecretAttribute: false,
            isExternal: true);
        return isIncomplete && candidate?.Metadata is { } metadata
            ? candidate with
            {
                Metadata = metadata with
                {
                    CanRegisterCommandMetadata = false,
                    CanRegisterSecretCoverage = false,
                },
            }
            : candidate;
    }

    private static HashSet<string> GetIncompleteTypeNames(IAssemblySymbol assembly) =>
        new(
            assembly.GetAttributes()
            .Where(attribute => attribute.AttributeClass?.ToDisplayString()
                                == IncompleteRuntimeMetadataAttributeFullName)
            .Select(GetConstructorString)
            .OfType<string>(),
            StringComparer.Ordinal);

    private static bool RequiresExternalMetadata(IAssemblySymbol assembly, IAssemblySymbol runtimeAssembly) =>
        !SymbolEqualityComparer.Default.Equals(assembly, runtimeAssembly)
        && ReferencesAssembly(
            assembly,
            runtimeAssembly,
            new HashSet<IAssemblySymbol>(SymbolEqualityComparer.Default));

    private static bool ReferencesAssemblyDirectly(
        IAssemblySymbol assembly,
        IAssemblySymbol targetAssembly) =>
        assembly.Modules.Any(module => module.ReferencedAssemblySymbols.Any(referenced =>
            SymbolEqualityComparer.Default.Equals(referenced, targetAssembly)));

    private static bool ReferencesAssembly(
        IAssemblySymbol assembly,
        IAssemblySymbol targetAssembly,
        HashSet<IAssemblySymbol> visitedAssemblies)
    {
        if (!visitedAssemblies.Add(assembly))
        {
            return false;
        }

        return assembly.Modules.Any(module => module.ReferencedAssemblySymbols.Any(referenced =>
            SymbolEqualityComparer.Default.Equals(referenced, targetAssembly)
            || ReferencesAssembly(referenced, targetAssembly, visitedAssemblies)));
    }

    private static TypeMetadataCandidate? GetExternalTypeCandidate(
        INamedTypeSymbol type,
        Compilation compilation)
    {
        var isCommandOptions = InheritsFrom(type, CommandLineToolOptionsFullName);
        var secretMetadata = GetSecretProperties(type, compilation.Assembly);
        var hasSecretAttributes = secretMetadata.HasAttributes;
        var canCoverPlainOptions = type.TypeKind is TypeKind.Class or TypeKind.Struct;
        if ((!isCommandOptions && !hasSecretAttributes && !canCoverPlainOptions)
            || (type.IsAbstract && type.IsGenericType && (isCommandOptions || hasSecretAttributes)))
        {
            return null;
        }

        return GetTypeCandidate(
            type,
            compilation,
            hasSecretAttributes,
            isExternal: true,
            precomputedSecretMetadata: secretMetadata);
    }

    private static IEnumerable<INamedTypeSymbol> GetTypes(INamespaceOrTypeSymbol container)
    {
        foreach (var member in container.GetMembers())
        {
            if (member is INamespaceSymbol namespaceSymbol)
            {
                foreach (var type in GetTypes(namespaceSymbol))
                {
                    yield return type;
                }
            }
            else if (member is INamedTypeSymbol typeSymbol)
            {
                yield return typeSymbol;
                foreach (var nestedType in GetTypes(typeSymbol))
                {
                    yield return nestedType;
                }
            }
        }
    }

    private static bool IsTrimOrAotEnabled(AnalyzerConfigOptionsProvider optionsProvider) =>
        IsEnabled(optionsProvider, "build_property.PublishTrimmed")
        || IsEnabled(optionsProvider, "build_property.PublishAot");

    private static bool IsEnabled(AnalyzerConfigOptionsProvider optionsProvider, string key) =>
        optionsProvider.GlobalOptions.TryGetValue(key, out var value)
        && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

    private static string GetMetadataName(INamedTypeSymbol type)
    {
        var typeNames = new Stack<string>();
        for (var current = type; current is not null; current = current.ContainingType)
        {
            typeNames.Push(current.MetadataName);
        }

        var namespaceName = type.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : type.ContainingNamespace.ToDisplayString() + ".";
        return namespaceName + string.Join("+", typeNames);
    }

    private static bool IsTypeAccessible(INamedTypeSymbol type, IAssemblySymbol currentAssembly)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.IsFileLocal
                || !IsAccessible(current.DeclaredAccessibility, current.ContainingAssembly, currentAssembly))
            {
                return false;
            }
        }

        return true;
    }

    private static bool HasPartialDeclarationInHierarchy(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.DeclaringSyntaxReferences.Any(reference =>
                    reference.GetSyntax() is TypeDeclarationSyntax declaration
                    && declaration.Modifiers.Any(SyntaxKind.PartialKeyword)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasGenericTypeInHierarchy(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.IsGenericType)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasObsoleteErrorInHierarchy(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (HasObsoleteError(current))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasObsoleteError(ISymbol symbol)
    {
        var obsoleteAttribute = symbol.GetAttributes().FirstOrDefault(attribute =>
            attribute.AttributeClass?.ToDisplayString() == "System.ObsoleteAttribute");
        return obsoleteAttribute is not null
               && obsoleteAttribute.ConstructorArguments.Length > 1
               && obsoleteAttribute.ConstructorArguments[1].Value is true;
    }

    private static EquatableArray<string> GetExperimentalDiagnosticIds(INamedTypeSymbol type)
    {
        var typeHierarchy = GetBaseTypes(type).ToArray();
        var properties = typeHierarchy
            .SelectMany(static current => current.GetMembers().OfType<IPropertySymbol>())
            .ToArray();
        var propertyTypes = properties
            .SelectMany(static property => GetReferencedNamedTypes(property.Type))
            .ToArray();
        return type.ContainingAssembly.GetAttributes()
            .Concat(typeHierarchy
                .SelectMany(GetContainingTypes)
                .SelectMany(static current => current.GetAttributes()))
            .Concat(properties.SelectMany(static property => property.GetAttributes()))
            .Concat(propertyTypes
                .SelectMany(GetContainingTypes)
                .SelectMany(static current => current.GetAttributes()))
            .Concat(propertyTypes
                .SelectMany(static propertyType => propertyType.ContainingAssembly?.GetAttributes() ?? []))
            .Where(static attribute => IsAttribute(attribute, ExperimentalAttributeFullName))
            .Select(GetConstructorString)
            .OfType<string>()
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static diagnosticId => diagnosticId, StringComparer.Ordinal)
            .ToImmutableArray();
    }

    private static IEnumerable<INamedTypeSymbol> GetReferencedNamedTypes(ITypeSymbol type)
    {
        var pendingTypes = new Stack<ITypeSymbol>();
        pendingTypes.Push(type);
        while (pendingTypes.Count > 0)
        {
            switch (pendingTypes.Pop())
            {
                case INamedTypeSymbol namedType:
                    yield return namedType;
                    if (namedType.ContainingType is not null)
                    {
                        pendingTypes.Push(namedType.ContainingType);
                    }

                    foreach (var typeArgument in namedType.TypeArguments)
                    {
                        pendingTypes.Push(typeArgument);
                    }

                    break;
                case IArrayTypeSymbol arrayType:
                    pendingTypes.Push(arrayType.ElementType);
                    break;
                case IPointerTypeSymbol pointerType:
                    pendingTypes.Push(pointerType.PointedAtType);
                    break;
            }
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetBaseTypes(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            yield return current;
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetContainingTypes(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            yield return current;
        }
    }

    private static bool IsPropertyAccessible(IPropertySymbol property, IAssemblySymbol currentAssembly)
    {
        return IsAccessible(property.DeclaredAccessibility, property.ContainingAssembly, currentAssembly)
            && IsAccessible(property.GetMethod!.DeclaredAccessibility, property.ContainingAssembly, currentAssembly)
            && IsTypeAccessible(property.ContainingType, currentAssembly);
    }

    private static bool IsAccessible(
        Accessibility accessibility,
        IAssemblySymbol containingAssembly,
        IAssemblySymbol currentAssembly)
    {
        return accessibility == Accessibility.Public
            || ((accessibility == Accessibility.Internal || accessibility == Accessibility.ProtectedOrInternal)
                && (SymbolEqualityComparer.Default.Equals(containingAssembly, currentAssembly)
                    || containingAssembly.GivesAccessTo(currentAssembly)));
    }

    private static bool IsCommandAttribute(AttributeData attribute)
    {
        var name = attribute.AttributeClass?.ToDisplayString();
        return name == CliArgumentAttributeFullName || name == CliFlagAttributeFullName || name == CliOptionAttributeFullName;
    }

    private static bool IsSecretAttribute(AttributeData attribute) =>
        attribute.AttributeClass is { } attributeType
        && GetBaseTypes(attributeType).Any(static type =>
            type.ToDisplayString() == SecretValueAttributeFullName);

    private static bool IsAttribute(AttributeData attribute, string fullName) =>
        attribute.AttributeClass?.ToDisplayString() == fullName;

    private static string? GetConstructorString(AttributeData attribute) =>
        attribute.ConstructorArguments.FirstOrDefault().Value as string;

    private static int GetConstructorInt(AttributeData attribute) =>
        attribute.ConstructorArguments.Length == 0 ? 0 : Convert.ToInt32(attribute.ConstructorArguments[0].Value);

    private static ImmutableArray<string> GetConstructorStrings(AttributeData attribute)
    {
        if (attribute.ConstructorArguments.Length == 0 ||
            attribute.ConstructorArguments[0].Kind != TypedConstantKind.Array)
        {
            return [];
        }

        return
        [
            .. attribute.ConstructorArguments[0].Values
                .Select(value => value.Value)
                .OfType<string>(),
        ];
    }

    private static string? GetNamedString(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as string;

    private static bool GetNamedBool(AttributeData attribute, string name) =>
        attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as bool? ?? false;

    private static int GetNamedInt(AttributeData attribute, string name, int defaultValue = 0)
    {
        var value = attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value;
        return value is null ? defaultValue : Convert.ToInt32(value);
    }

    private static string GetNamedEnumMemberName(
        AttributeData attribute,
        string name,
        string defaultValue)
    {
        var typedConstant = attribute.NamedArguments
            .FirstOrDefault(argument => argument.Key == name)
            .Value;
        if (typedConstant.Value is null || typedConstant.Type is null)
        {
            return defaultValue;
        }

        var numericValue = Convert.ToInt64(typedConstant.Value);
        return typedConstant.Type
                   .GetMembers()
                   .OfType<IFieldSymbol>()
                   .FirstOrDefault(field => field.HasConstantValue
                                            && Convert.ToInt64(field.ConstantValue) == numericValue)
                   ?.Name
               ?? defaultValue;
    }

    private static string BooleanLiteral(bool value) => value ? "true" : "false";

    private static string NullableLiteral(string? value) => value is null ? "null" : Literal(value);

    private static string StringArrayLiteral(EquatableArray<string> values) =>
        values.Count == 0
            ? "global::System.Array.Empty<string>()"
            : $"new string[] {{ {string.Join(", ", values.Select(Literal))} }}";

    private static string Literal(string value) =>
        global::Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(value, quote: true);

    private sealed record TypeMetadata(
        string TypeName,
        string MetadataName,
        string AssemblyIdentity,
        bool CanRegisterCommandMetadata,
        bool CanRegisterSecretCoverage,
        bool UseTypeForEmptySecretCoverage,
        bool UseExternalTypeNameForEmptySecretCoverage,
        bool IsExternal,
        bool IsCommandOptions,
        PropertyCollection CommandMetadata,
        PropertyCollection SecretMetadata,
        EquatableArray<string> ExperimentalDiagnosticIds);

    private sealed record TypeMetadataCandidate(
        string TypeName,
        string MetadataName,
        string AssemblyIdentity,
        Location Location,
        TypeMetadata? Metadata);

    private sealed record OptionsTypeUsage(
        OptionsTypeIdentity? TypeIdentity,
        string? TypeParameterName,
        Location Location);

    private sealed record OptionsTypeIdentity(
        string MetadataName,
        string AssemblyIdentity);

    private sealed record ExternalMetadataCandidates(
        ImmutableArray<TypeMetadataCandidate> Candidates,
        ImmutableArray<IncompatibleMetadataAssembly> IncompatibleAssemblies)
    {
        public static ExternalMetadataCandidates Empty { get; } = new([], []);
    }

    private sealed record IncompatibleMetadataAssembly(
        string AssemblyIdentity,
        int? RuntimeSchemaVersion,
        int? CommandSchemaVersion);

    private sealed class TypeMetadataCandidateComparer : IEqualityComparer<TypeMetadataCandidate>
    {
        public static TypeMetadataCandidateComparer Instance { get; } = new();

        public bool Equals(TypeMetadataCandidate? x, TypeMetadataCandidate? y) =>
            ReferenceEquals(x, y)
            || (x is not null
                && y is not null
                && StringComparer.Ordinal.Equals(x.TypeName, y.TypeName)
                && StringComparer.Ordinal.Equals(x.MetadataName, y.MetadataName)
                && StringComparer.Ordinal.Equals(x.AssemblyIdentity, y.AssemblyIdentity)
                && EqualityComparer<TypeMetadata?>.Default.Equals(x.Metadata, y.Metadata)
                && (!RequiresDiagnostic(x) || x.Location.Equals(y.Location)));

        public int GetHashCode(TypeMetadataCandidate obj)
        {
            var hashCode = (StringComparer.Ordinal.GetHashCode(obj.TypeName) * 397)
                           ^ StringComparer.Ordinal.GetHashCode(obj.MetadataName);
            hashCode = (hashCode * 397)
                           ^ StringComparer.Ordinal.GetHashCode(obj.AssemblyIdentity);
            hashCode = (hashCode * 397)
                           ^ (obj.Metadata?.GetHashCode() ?? 0);
            return RequiresDiagnostic(obj)
                ? (hashCode * 397) ^ obj.Location.GetHashCode()
                : hashCode;
        }

        private static bool RequiresDiagnostic(TypeMetadataCandidate candidate) =>
            candidate.Metadata is null
            || !candidate.Metadata.CanRegisterSecretCoverage
            || !candidate.Metadata.CommandMetadata.IsComplete
            || !candidate.Metadata.SecretMetadata.IsComplete;
    }

    private sealed record PropertyCollection(
        EquatableArray<PropertyMetadata> Properties,
        EquatableArray<string> IncompletePropertyNames,
        bool IsComplete,
        bool HasAttributes)
    {
        public static PropertyCollection Empty { get; } = new(
            EquatableArray<PropertyMetadata>.Empty,
            EquatableArray<string>.Empty,
            true,
            false);
    }

    private sealed record PropertyMetadata(
        string Name,
        PropertyKind Kind,
        string? PrimaryValue,
        string? ShortForm,
        bool BooleanValue,
        bool GroupValues,
        bool IsRequired,
        int FirstInt,
        string Phase,
        int ValueArity,
        EquatableArray<string> SecretValueKeys,
        bool PrependOptionTerminatorIfValueStartsWithDash,
        bool IsGlobalOption,
        int ManualOperandCount,
        string AccessorTypeName,
        bool IsSupportedPropertyType,
        bool HasExplicitArgumentPosition,
        bool RepeatOptionTerminator = false,
        string? CollectionSeparator = null,
        string? NegatedName = null);

    private enum PropertyKind
    {
        Argument,
        Flag,
        Option,
        Secret,
    }
}
