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
    private const int RuntimeMetadataSchemaVersion = 1;

    internal const string CommandLineToolOptionsFullName = "ModularPipelines.Options.CommandLineToolOptions";
    internal const string OptionsNamespace = "Microsoft.Extensions.Options";
    internal const string DependencyInjectionNamespace = "Microsoft.Extensions.DependencyInjection";
    internal const string CliOptionAttributeFullName = "ModularPipelines.Attributes.CliOptionAttribute";
    internal const string CliFlagAttributeFullName = "ModularPipelines.Attributes.CliFlagAttribute";
    internal const string CliArgumentAttributeFullName = "ModularPipelines.Attributes.CliArgumentAttribute";
    internal const string CliGlobalOptionsAttributeFullName = "ModularPipelines.Attributes.CliGlobalOptionsAttribute";
    internal const string SecretValueAttributeFullName = "ModularPipelines.Attributes.SecretValueAttribute";
    internal const string ExperimentalAttributeFullName = "System.Diagnostics.CodeAnalysis.ExperimentalAttribute";
    internal const string IncompleteRuntimeMetadataAttributeFullName =
        "ModularPipelines.Generated.IncompleteRuntimeMetadataAttribute";

    private static readonly DiagnosticDescriptor IncompleteCommandMetadata =
        GeneratorDiagnostics.IncompleteCommandMetadata;

    private static readonly DiagnosticDescriptor IncompleteSecretMetadata =
        GeneratorDiagnostics.IncompleteSecretMetadata;

    private static readonly DiagnosticDescriptor SkippedRuntimeMetadata =
        GeneratorDiagnostics.SkippedRuntimeMetadata;

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
            .Where(static metadataName => metadataName is not null)
            .Select(static (metadataName, _) => metadataName!);
        var collectedOptionsTypeUsages = optionsTypeUsages.Collect();

        var externalTypeCandidates = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Combine(collectedOptionsTypeUsages)
            .Select(static (input, _) => GetExternalTypeCandidates(
                input.Left.Left,
                input.Left.Right,
                input.Right));
        var hasRuntimeReference = context.CompilationProvider.Select(
            static (compilation, _) => compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName) is not null);
        var sourceCandidates = typeCandidates.Collect()
            .Combine(secretCandidates.Collect())
            .Select(static (input, _) => input.Left.AddRange(input.Right));
        var candidates = sourceCandidates
            .Combine(externalTypeCandidates)
            .Select(static (input, _) => input.Left.AddRange(input.Right))
            .Combine(hasRuntimeReference);
        var generationInputs = candidates.Combine(collectedOptionsTypeUsages);
        context.RegisterSourceOutput(generationInputs, static (sourceContext, input) =>
        {
            if (!input.Left.Right)
            {
                return;
            }

            var candidates = input.Left.Left;
            var optionsTypeMetadataNames = new HashSet<string>(input.Right, StringComparer.Ordinal);
            var ambiguousMetadataNames = new HashSet<string>(candidates
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
                var representative = collision.FirstOrDefault(static candidate => candidate.Location.IsInSource)
                                     ?? collision.First();
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    SkippedRuntimeMetadata,
                    representative.Location,
                    representative.TypeName));
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
                optionsTypeMetadataNames);
            sourceContext.AddSource("ModularPipelines.RuntimeMetadata.g.cs", Generate(items));
        });
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

    private static string? GetOptionsTypeUsage(GeneratorSyntaxContext context)
    {
        var symbol = context.SemanticModel.GetSymbolInfo(context.Node).Symbol;
        var optionsTypeSymbol = symbol switch
        {
            INamedTypeSymbol constructedType when IsOptionsTypeUsage(constructedType) =>
                constructedType.TypeArguments[0],
            IMethodSymbol method => GetRegisteredOptionsType(method),
            _ => null,
        };
        if (optionsTypeSymbol is not INamedTypeSymbol optionsType
            || optionsType.TypeKind == TypeKind.Error
            || optionsType.ContainingNamespace is null)
        {
            return null;
        }

        return GetMetadataName(optionsType);
    }

    private static bool IsOptionsTypeUsageCandidate(SyntaxNode node) =>
        node is GenericNameSyntax
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
            && GetInvokedMethodName(invocation) is "AddSingleton" or "AddScoped" or "AddTransient");

    private static string? GetInvokedMethodName(InvocationExpressionSyntax invocation) =>
        invocation.Expression switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText,
            _ => null,
        };

    private static bool IsOptionsTypeUsage(INamedTypeSymbol type)
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

    private static bool IsOptionsRegistrationMethod(IMethodSymbol method)
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
        if (definition.ContainingNamespace?.ToDisplayString() != DependencyInjectionNamespace
            || definition.ContainingType.MetadataName != "ServiceCollectionServiceExtensions"
            || definition.Name is not ("AddSingleton" or "AddScoped" or "AddTransient"))
        {
            return null;
        }

        return method.TypeArguments
            .OfType<INamedTypeSymbol>()
            .FirstOrDefault(IsOptionsTypeUsage)
            ?.TypeArguments[0];
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
        bool isExternal = false)
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
                canRegisterSecretCoverage,
                isExternal);
        }

        var commandMetadata = isCommandOptions
            ? GetCommandProperties(type, compilation.Assembly)
            : PropertyCollection.Empty;
        var secretMetadata = GetSecretProperties(type, compilation.Assembly);
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
        bool canRegisterSecretCoverage,
        bool isExternal)
    {
        var hasSecretAttributes = hasKnownSecretAttribute
            || GetSecretProperties(type, currentAssembly).HasAttributes;
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
                CanRegisterCommandMetadata: false,
                CanRegisterSecretCoverage: canRegisterSecretCoverage,
                UseTypeForEmptySecretCoverage: isExternal,
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
                CanRegisterCommandMetadata: !hasPartialDeclaration,
                CanRegisterSecretCoverage: !hasPartialDeclaration,
                UseTypeForEmptySecretCoverage: isExternal,
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

        var attribute = FindAttribute(property, IsCommandAttribute);
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
                    property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
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
                isGlobalOption,
                property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        }

        if (attributeName == CliFlagAttributeFullName)
        {
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
                isGlobalOption,
                property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
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
            isGlobalOption,
            property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
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

    private static string Generate(ImmutableArray<TypeMetadata> items)
    {
        var uniqueItems = items
            .GroupBy(item => item.MetadataName, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(item => item.MetadataName, StringComparer.Ordinal)
            .ToList();
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        foreach (var item in uniqueItems.Where(static item => !item.CanRegisterSecretCoverage))
        {
            sb.AppendLine(
                $"[assembly: global::{IncompleteRuntimeMetadataAttributeFullName}({Literal(item.MetadataName)})]");
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
        sb.AppendLine("namespace ModularPipelines.Generated;");
        sb.AppendLine();
        sb.AppendLine("[global::System.AttributeUsage(global::System.AttributeTargets.Assembly, AllowMultiple = true)]");
        sb.AppendLine("internal sealed class IncompleteRuntimeMetadataAttribute : global::System.Attribute");
        sb.AppendLine("{");
        sb.AppendLine("    internal IncompleteRuntimeMetadataAttribute(string metadataName)");
        sb.AppendLine("    {");
        sb.AppendLine("        MetadataName = metadataName;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    internal string MetadataName { get; }");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("internal static class RuntimeMetadataRegistration");
        sb.AppendLine("{");
        sb.AppendLine($"    public const int SchemaVersion = {RuntimeMetadataSchemaVersion};");
        sb.AppendLine();
        sb.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        sb.AppendLine("    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");
        sb.AppendLine("        var assembly = global::System.Reflection.Assembly.GetExecutingAssembly();");
        sb.AppendLine("        global::ModularPipelines.Helpers.Internal.GeneratedCommandMetadata.RegisterAssembly(assembly);");
        sb.AppendLine("        global::ModularPipelines.Engine.GeneratedSecretMetadata.RegisterAssembly(assembly);");
        AppendTypeNameRegistration(
            sb,
            "RegisterCoveredTypeNames",
            uniqueItems.Where(item => item.IsCommandOptions),
            "global::ModularPipelines.Helpers.Internal.GeneratedCommandMetadata");
        AppendTypeNameRegistration(
            sb,
            "RegisterCoveredTypeNames",
            uniqueItems.Where(item => item.CanRegisterSecretCoverage
                                      && item.SecretMetadata.IsComplete
                                      && item.SecretMetadata.Properties.Count == 0
                                      && !item.UseTypeForEmptySecretCoverage));
        AppendTypeNameRegistration(
            sb,
            "RegisterIncompleteTypeNames",
            uniqueItems.Where(item => !item.CanRegisterSecretCoverage));
        foreach (var item in uniqueItems)
        {
            if (item.IsCommandOptions
                && item.CanRegisterCommandMetadata
                && item.CommandMetadata.IsComplete)
            {
                AppendCommandRegistration(sb, item);
            }

            if (item.SecretMetadata.IsComplete)
            {
                AppendSecretRegistration(sb, item);
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static void AppendTypeNameRegistration(
        StringBuilder sb,
        string methodName,
        IEnumerable<TypeMetadata> items,
        string registryType = "global::ModularPipelines.Engine.GeneratedSecretMetadata")
    {
        var metadataNames = items.Select(item => item.MetadataName).ToList();
        if (metadataNames.Count == 0)
        {
            return;
        }

        sb.AppendLine($"        {registryType}.{methodName}(");
        sb.AppendLine("            assembly,");
        sb.AppendLine("            new string[]");
        sb.AppendLine("            {");
        foreach (var metadataName in metadataNames)
        {
            sb.AppendLine($"                {Literal(metadataName)},");
        }

        sb.AppendLine("            });");
    }

    private static void ReportIncompleteMetadata(
        SourceProductionContext context,
        IReadOnlyCollection<TypeMetadataCandidate> candidates,
        IReadOnlyCollection<string> optionsTypeMetadataNames)
    {
        foreach (var candidate in candidates
                     .Where(static candidate => candidate.Metadata is not null)
                     .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                     .Select(static group => group.First()))
        {
            var item = candidate.Metadata!;
            if (!item.CanRegisterSecretCoverage
                && optionsTypeMetadataNames.Contains(item.MetadataName))
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
        sb.AppendLine($"        global::ModularPipelines.Helpers.Internal.GeneratedCommandMetadata.{registrationMethod}(");
        if (item.IsExternal)
        {
            sb.AppendLine("            assembly,");
        }

        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Helpers.Internal.PropertyCommandLinePart[]");
        sb.AppendLine("            {");

        foreach (var property in item.CommandMetadata.Properties)
        {
            var getter = $"static instance => (({property.AccessorTypeName})instance).@{property.Name}";
            switch (property.Kind)
            {
                case PropertyKind.Argument:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.ArgumentPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliArgumentAttribute({property.FirstInt})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                        PrependOptionTerminator = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Required = {BooleanLiteral(property.IsRequired)},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)} }},");
                    break;
                case PropertyKind.Flag:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.FlagPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliFlagAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)} }},");
                    break;
                case PropertyKind.Option:
                    sb.AppendLine("                new global::ModularPipelines.Helpers.Internal.OptionPart(");
                    sb.AppendLine($"                    {Literal(property.Name)}, {getter},");
                    sb.AppendLine($"                    new global::ModularPipelines.Attributes.CliOptionAttribute({Literal(property.PrimaryValue!)})");
                    sb.AppendLine("                    {");
                    sb.AppendLine($"                        ShortForm = {NullableLiteral(property.ShortForm)},");
                    sb.AppendLine($"                        PreferShortForm = {BooleanLiteral(property.BooleanValue)},");
                    sb.AppendLine($"                        Format = (global::ModularPipelines.Attributes.OptionFormat){property.FirstInt},");
                    sb.AppendLine($"                        ValueArity = (global::ModularPipelines.Attributes.CliOptionValueArity){property.ValueArity},");
                    sb.AppendLine($"                        GroupValues = {BooleanLiteral(property.GroupValues)},");
                    sb.AppendLine($"                        Phase = global::ModularPipelines.Attributes.CommandLinePhase.{property.Phase},");
                    sb.AppendLine($"                    }}) {{ IsGlobalOption = {BooleanLiteral(property.IsGlobalOption)} }},");
                    break;
            }
        }

        sb.AppendLine("            });");
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
                sb.AppendLine($"        global::ModularPipelines.Engine.GeneratedSecretMetadata.RegisterExternal(assembly, typeof({item.TypeName}));");
            }

            return;
        }

        var registrationMethod = item.IsExternal ? "RegisterExternal" : "Register";
        sb.AppendLine($"        global::ModularPipelines.Engine.GeneratedSecretMetadata.{registrationMethod}(");
        if (item.IsExternal)
        {
            sb.AppendLine("            assembly,");
        }

        sb.AppendLine($"            typeof({item.TypeName}),");
        sb.AppendLine("            new global::ModularPipelines.Engine.SecretPropertyAccessor[]");
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

    private static ImmutableArray<TypeMetadataCandidate> GetExternalTypeCandidates(
        Compilation compilation,
        AnalyzerConfigOptionsProvider optionsProvider,
        ImmutableArray<string> optionsTypeMetadataNames)
    {
        if (compilation.GetTypeByMetadataName(CommandLineToolOptionsFullName)?.ContainingAssembly is not { } runtimeAssembly)
        {
            return [];
        }

        var includeAllRuntimeMetadata = IsTrimOrAotEnabled(optionsProvider);
        var usedOptionsTypes = new HashSet<string>(optionsTypeMetadataNames, StringComparer.Ordinal);
        return compilation.SourceModule.ReferencedAssemblySymbols
            .SelectMany(assembly => GetExternalTypeCandidates(
                assembly,
                runtimeAssembly,
                compilation,
                includeAllRuntimeMetadata,
                usedOptionsTypes))
            .ToImmutableArray();
    }

    private static IEnumerable<TypeMetadataCandidate> GetExternalTypeCandidates(
        IAssemblySymbol assembly,
        IAssemblySymbol runtimeAssembly,
        Compilation compilation,
        bool includeAllRuntimeMetadata,
        HashSet<string> usedOptionsTypes)
    {
        if (!RequiresExternalMetadata(assembly, runtimeAssembly))
        {
            return [];
        }

        var incompleteTypeNames = GetIncompleteTypeNames(assembly);
        if (!includeAllRuntimeMetadata && !incompleteTypeNames.Overlaps(usedOptionsTypes))
        {
            return [];
        }

        return GetTypes(assembly.GlobalNamespace)
            .Select(type => GetExternalTypeCandidate(
                type,
                compilation,
                includeAllRuntimeMetadata,
                usedOptionsTypes,
                incompleteTypeNames))
            .OfType<TypeMetadataCandidate>();
    }

    private static TypeMetadataCandidate? GetExternalTypeCandidate(
        INamedTypeSymbol type,
        Compilation compilation,
        bool includeAllRuntimeMetadata,
        ISet<string> usedOptionsTypes,
        ISet<string> incompleteTypeNames)
    {
        var metadataName = GetMetadataName(type);
        if (usedOptionsTypes.Contains(metadataName)
            && (includeAllRuntimeMetadata || incompleteTypeNames.Contains(metadataName)))
        {
            return GetExternalOptionsUsageCandidate(
                type,
                compilation,
                incompleteTypeNames.Contains(metadataName));
        }

        return includeAllRuntimeMetadata
            ? GetExternalTypeCandidate(type, compilation)
            : null;
    }

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
        var hasSecretAttributes = GetSecretProperties(type, compilation.Assembly).HasAttributes;
        if ((!isCommandOptions && !hasSecretAttributes)
            || (type.IsAbstract && type.IsGenericType))
        {
            return null;
        }

        return GetTypeCandidate(type, compilation, hasSecretAttributes, isExternal: true);
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
        IsAttribute(attribute, SecretValueAttributeFullName);

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
        bool CanRegisterCommandMetadata,
        bool CanRegisterSecretCoverage,
        bool UseTypeForEmptySecretCoverage,
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
        bool IsGlobalOption,
        string AccessorTypeName);

    private enum PropertyKind
    {
        Argument,
        Flag,
        Option,
        Secret,
    }
}
