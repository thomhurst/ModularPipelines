using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Generates direct attribute factories for statically known module types.
/// </summary>
[Generator]
public sealed class ModuleEventMetadataGenerator : IIncrementalGenerator
{
    internal const string ModuleBaseFullName = "ModularPipelines.Modules.Module`1";

    private const string AttributeUsageFullName = "System.AttributeUsageAttribute";

    private static readonly DiagnosticDescriptor IncompleteModuleEventMetadata =
        GeneratorDiagnostics.IncompleteModuleEventMetadata;

    private static readonly DiagnosticDescriptor SkippedModuleEventMetadata =
        GeneratorDiagnostics.SkippedModuleEventMetadata;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeCandidates = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => IsTypeCandidate(node),
                static (generatorContext, _) => GetTypeCandidates(generatorContext))
            .SelectMany(static (candidates, _) => candidates)
            .WithComparer(ModuleEventMetadataCandidateComparer.Instance);

        var registeredClosedGenericCandidates = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => ModuleMetadataGenerator.IsModuleRegistrationCandidate(node),
                static (generatorContext, _) => GetRegisteredModuleCandidate(generatorContext))
            .Where(static candidate => candidate is not null)
            .Select(static (candidate, _) => candidate!)
            .WithComparer(ModuleEventMetadataCandidateComparer.Instance);

        var allCandidates = typeCandidates
            .Collect()
            .Combine(registeredClosedGenericCandidates.Collect())
            .Select(static (input, _) => input.Left.AddRange(input.Right));

        context.RegisterSourceOutput(allCandidates, static (sourceContext, candidates) =>
        {
            foreach (var skipped in candidates
                         .Where(static candidate => candidate.Metadata is null)
                         .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                         .Select(static group => group.First()))
            {
                sourceContext.ReportDiagnostic(Diagnostic.Create(
                    SkippedModuleEventMetadata,
                    skipped.Location,
                    skipped.TypeName));
            }

            var metadata = candidates
                .Select(static candidate => candidate.Metadata)
                .OfType<ModuleEventMetadata>()
                .ToImmutableArray();
            if (!metadata.IsEmpty)
            {
                foreach (var candidate in candidates
                             .Where(static candidate =>
                                 candidate.Metadata is { IsComplete: false })
                             .GroupBy(static candidate => candidate.TypeName, StringComparer.Ordinal)
                             .Select(static group => group.First()))
                {
                    sourceContext.ReportDiagnostic(Diagnostic.Create(
                        IncompleteModuleEventMetadata,
                        candidate.Location,
                        candidate.TypeName));
                }

                sourceContext.AddSource(
                    "ModularPipelines.ModuleEventMetadata.g.cs",
                    Generate(metadata));
            }
        });
    }

    internal static bool IsTypeCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { BaseList.Types.Count: > 0 }
            || (node is RecordDeclarationSyntax { BaseList.Types.Count: > 0 } record
                && !record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword));
    }

    private static ImmutableArray<ModuleEventMetadataCandidate> GetTypeCandidates(
        GeneratorSyntaxContext context)
    {
        var compilation = context.SemanticModel.Compilation;
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol type
            || type.IsAbstract
            || !InheritsFromModule(type, compilation))
        {
            return [];
        }

        var candidates = ImmutableArray.CreateBuilder<ModuleEventMetadataCandidate>();
        candidates.Add(CreateModuleCandidate(
            type,
            compilation,
            type.Locations.FirstOrDefault() ?? Location.None,
            allowConstructedGeneric: false));

        AddClosedGenericDependencyCandidates(
            context,
            type,
            compilation,
            candidates);

        return candidates.ToImmutable();
    }

    private static ModuleEventMetadataCandidate? GetRegisteredModuleCandidate(
        GeneratorSyntaxContext context)
    {
        var type = ModuleMetadataGenerator.GetRegisteredClosedGenericModule(context);
        return type is null
            ? null
            : CreateModuleCandidate(
                type,
                context.SemanticModel.Compilation,
                context.Node.GetLocation(),
                allowConstructedGeneric: true);
    }

    private static void AddClosedGenericDependencyCandidates(
        GeneratorSyntaxContext context,
        INamedTypeSymbol type,
        Compilation compilation,
        ImmutableArray<ModuleEventMetadataCandidate>.Builder candidates)
    {
        var pending = new Stack<INamedTypeSymbol>(
            ModuleMetadataGenerator.GetClosedGenericModuleDependencies(type, compilation));
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        while (pending.Count > 0)
        {
            var dependency = pending.Pop();
            if (!visited.Add(dependency))
            {
                continue;
            }

            candidates.Add(CreateModuleCandidate(
                dependency,
                compilation,
                context.Node.GetLocation(),
                allowConstructedGeneric: true));
            foreach (var transitiveDependency in ModuleMetadataGenerator
                         .GetClosedGenericModuleDependencies(dependency, compilation))
            {
                pending.Push(transitiveDependency);
            }
        }
    }

    private static ModuleEventMetadataCandidate CreateModuleCandidate(
        INamedTypeSymbol type,
        Compilation compilation,
        Location location,
        bool allowConstructedGeneric)
    {
        var typeName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (!IsTypeAccessible(type, compilation.Assembly))
        {
            return new ModuleEventMetadataCandidate(typeName, location, Metadata: null);
        }

        if (type.IsGenericType && !allowConstructedGeneric)
        {
            return new ModuleEventMetadataCandidate(typeName, location, Metadata: null);
        }

        var attributeMetadata = GetAttributeMetadata(type, compilation.Assembly);
        return new ModuleEventMetadataCandidate(typeName, location, new ModuleEventMetadata(
            typeName,
            attributeMetadata.Expressions,
            attributeMetadata.IsComplete));
    }

    private static AttributeMetadata GetAttributeMetadata(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        var attributes = ImmutableArray.CreateBuilder<string>();
        var seenSingleUseAttributes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var isComplete = true;

        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var attribute in current.GetAttributes())
            {
                var candidate = GetAttributeCandidate(
                    attribute,
                    current,
                    type,
                    currentAssembly,
                    seenSingleUseAttributes);
                if (!candidate.IsComplete)
                {
                    isComplete = false;
                }

                if (candidate.Expression is not null)
                {
                    attributes.Add(candidate.Expression);
                }
            }
        }

        return new AttributeMetadata(attributes.ToImmutable(), isComplete);
    }

    private static AttributeCandidate GetAttributeCandidate(
        AttributeData attribute,
        INamedTypeSymbol declaringType,
        INamedTypeSymbol moduleType,
        IAssemblySymbol currentAssembly,
        HashSet<INamedTypeSymbol> seenSingleUseAttributes)
    {
        if (attribute.AttributeClass is not { } attributeType)
        {
            return new AttributeCandidate(null, IsComplete: false);
        }

        var usage = GetAttributeUsage(attributeType);
        if (!SymbolEqualityComparer.Default.Equals(declaringType, moduleType) && !usage.Inherited)
        {
            return new AttributeCandidate(null, IsComplete: true);
        }

        if (!usage.AllowMultiple && !seenSingleUseAttributes.Add(attributeType))
        {
            return new AttributeCandidate(null, IsComplete: true);
        }

        var expression = CreateAttributeExpression(attribute, currentAssembly);
        return new AttributeCandidate(expression, IsComplete: expression is not null);
    }

    private static AttributeUsageMetadata GetAttributeUsage(INamedTypeSymbol attributeType)
    {
        AttributeData? usage = null;
        for (var current = attributeType; current is not null && usage is null; current = current.BaseType)
        {
            usage = current.GetAttributes().FirstOrDefault(attribute =>
                attribute.AttributeClass?.ToDisplayString() == AttributeUsageFullName);
        }

        if (usage is null)
        {
            return new AttributeUsageMetadata(AllowMultiple: false, Inherited: true);
        }

        var allowMultiple = GetNamedBoolean(usage, nameof(AttributeUsageAttribute.AllowMultiple), false);
        var inherited = GetNamedBoolean(usage, nameof(AttributeUsageAttribute.Inherited), true);
        return new AttributeUsageMetadata(allowMultiple, inherited);
    }

    private static bool GetNamedBoolean(AttributeData attribute, string name, bool defaultValue)
    {
        foreach (var argument in attribute.NamedArguments)
        {
            if (argument.Key == name && argument.Value.Value is bool value)
            {
                return value;
            }
        }

        return defaultValue;
    }

    private static string? CreateAttributeExpression(
        AttributeData attribute,
        IAssemblySymbol currentAssembly)
    {
        var attributeType = GetAccessibleAttributeType(attribute, currentAssembly);
        if (attributeType is null)
        {
            return null;
        }

        var constructorArguments = FormatArguments(attribute.ConstructorArguments, currentAssembly);
        var namedArguments = FormatNamedArguments(attribute, attributeType, currentAssembly);
        if (constructorArguments is null || namedArguments is null)
        {
            return null;
        }

        return BuildAttributeExpression(attributeType, constructorArguments, namedArguments);
    }

    private static INamedTypeSymbol? GetAccessibleAttributeType(
        AttributeData attribute,
        IAssemblySymbol currentAssembly)
    {
        if (attribute.AttributeClass is not { } attributeType
            || attribute.AttributeConstructor is not { } constructor
            || !IsTypeAccessible(attributeType, currentAssembly)
            || !IsAccessible(
                constructor.DeclaredAccessibility,
                constructor.ContainingAssembly,
                currentAssembly))
        {
            return null;
        }

        return attributeType;
    }

    private static List<string>? FormatArguments(
        ImmutableArray<TypedConstant> arguments,
        IAssemblySymbol currentAssembly)
    {
        var expressions = new List<string>();
        foreach (var argument in arguments)
        {
            var expression = FormatTypedConstant(argument, currentAssembly);
            if (expression is null)
            {
                return null;
            }

            expressions.Add(expression);
        }

        return expressions;
    }

    private static List<string>? FormatNamedArguments(
        AttributeData attribute,
        INamedTypeSymbol attributeType,
        IAssemblySymbol currentAssembly)
    {
        var namedArguments = new List<string>();
        foreach (var argument in attribute.NamedArguments)
        {
            var member = FindNamedArgumentMember(attributeType, argument.Key);
            var expression = FormatTypedConstant(argument.Value, currentAssembly);
            if (member is null
                || expression is null
                || !IsAccessible(member.DeclaredAccessibility, member.ContainingAssembly, currentAssembly))
            {
                return null;
            }

            namedArguments.Add($"@{argument.Key} = {expression}");
        }

        return namedArguments;
    }

    private static ISymbol? FindNamedArgumentMember(INamedTypeSymbol attributeType, string name)
    {
        for (var current = attributeType; current is not null; current = current.BaseType)
        {
            var member = current.GetMembers(name).FirstOrDefault();
            if (member is not null)
            {
                return member;
            }
        }

        return null;
    }

    private static string BuildAttributeExpression(
        INamedTypeSymbol attributeType,
        List<string> constructorArguments,
        List<string> namedArguments)
    {
        var typeName = attributeType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var result = $"new {typeName}({string.Join(", ", constructorArguments)})";
        if (namedArguments.Count > 0)
        {
            result += $" {{ {string.Join(", ", namedArguments)} }}";
        }

        return result;
    }

    private static string? FormatTypedConstant(
        TypedConstant constant,
        IAssemblySymbol currentAssembly)
    {
        if (constant.IsNull)
        {
            return "null";
        }

        if (constant.Type is not null
            && !IsTypeReferenceAccessible(constant.Type, currentAssembly))
        {
            return null;
        }

        return constant.Kind switch
        {
            TypedConstantKind.Array => FormatArrayConstant(constant, currentAssembly),
            TypedConstantKind.Type => FormatTypeConstant(constant, currentAssembly),
            _ when constant.Type?.TypeKind == TypeKind.Enum
                => FormatEnumConstant(constant),
            _ => FormatPrimitive(constant.Value),
        };
    }

    private static string? FormatArrayConstant(
        TypedConstant constant,
        IAssemblySymbol currentAssembly)
    {
        if (constant.Type is not IArrayTypeSymbol arrayType)
        {
            return null;
        }

        var values = FormatArguments(constant.Values, currentAssembly);
        if (values is null)
        {
            return null;
        }

        var elementType = arrayType.ElementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return $"new {elementType}[] {{ {string.Join(", ", values)} }}";
    }

    private static string? FormatTypeConstant(
        TypedConstant constant,
        IAssemblySymbol currentAssembly) =>
        constant.Value is ITypeSymbol type
        && IsTypeReferenceAccessible(type, currentAssembly)
            ? $"typeof({type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"
            : null;

    private static string FormatEnumConstant(TypedConstant constant) =>
        $"({constant.Type!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"
        + FormatEnumValue(constant.Value!);

    private static bool IsTypeReferenceAccessible(
        ITypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        return type switch
        {
            IArrayTypeSymbol arrayType
                => IsTypeReferenceAccessible(arrayType.ElementType, currentAssembly),
            IPointerTypeSymbol pointerType
                => IsTypeReferenceAccessible(pointerType.PointedAtType, currentAssembly),
            INamedTypeSymbol namedType => IsTypeAccessible(namedType, currentAssembly),
            ITypeParameterSymbol => false,
            _ => true,
        };
    }

    private static string? FormatPrimitive(object? value)
    {
        return value switch
        {
            null => "null",
            string text => SymbolDisplay.FormatLiteral(text, quote: true),
            char character => SymbolDisplay.FormatLiteral(character, quote: true),
            bool boolean => boolean ? "true" : "false",
            byte number => $"(byte){number.ToString(CultureInfo.InvariantCulture)}",
            sbyte number => $"(sbyte){number.ToString(CultureInfo.InvariantCulture)}",
            short number => $"(short){number.ToString(CultureInfo.InvariantCulture)}",
            ushort number => $"(ushort){number.ToString(CultureInfo.InvariantCulture)}",
            int number => number.ToString(CultureInfo.InvariantCulture),
            uint number => $"{number.ToString(CultureInfo.InvariantCulture)}U",
            long number => $"{number.ToString(CultureInfo.InvariantCulture)}L",
            ulong number => $"{number.ToString(CultureInfo.InvariantCulture)}UL",
            float number when float.IsNaN(number) => "global::System.Single.NaN",
            float number when float.IsPositiveInfinity(number) => "global::System.Single.PositiveInfinity",
            float number when float.IsNegativeInfinity(number) => "global::System.Single.NegativeInfinity",
            float number => $"{number.ToString("R", CultureInfo.InvariantCulture)}F",
            double number when double.IsNaN(number) => "global::System.Double.NaN",
            double number when double.IsPositiveInfinity(number) => "global::System.Double.PositiveInfinity",
            double number when double.IsNegativeInfinity(number) => "global::System.Double.NegativeInfinity",
            double number => number.ToString("R", CultureInfo.InvariantCulture),
            _ => null,
        };
    }

    private static string FormatEnumValue(object value)
    {
        return value switch
        {
            byte number => number.ToString(CultureInfo.InvariantCulture),
            sbyte number => number.ToString(CultureInfo.InvariantCulture),
            short number => number.ToString(CultureInfo.InvariantCulture),
            ushort number => number.ToString(CultureInfo.InvariantCulture),
            int number => number.ToString(CultureInfo.InvariantCulture),
            uint number => $"{number.ToString(CultureInfo.InvariantCulture)}U",
            long number => $"{number.ToString(CultureInfo.InvariantCulture)}L",
            ulong number => $"{number.ToString(CultureInfo.InvariantCulture)}UL",
            _ => throw new InvalidOperationException($"Unsupported enum value type {value.GetType()}."),
        };
    }

    private static bool InheritsFromModule(INamedTypeSymbol type, Compilation compilation)
    {
        var moduleBaseType = compilation.GetTypeByMetadataName(ModuleBaseFullName);
        if (moduleBaseType is null)
        {
            return false;
        }

        for (var current = type.BaseType; current is not null; current = current.BaseType)
        {
            if (current.IsGenericType
                && SymbolEqualityComparer.Default.Equals(current.OriginalDefinition, moduleBaseType))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTypeAccessible(INamedTypeSymbol type, IAssemblySymbol currentAssembly)
    {
        return IsTypeDeclarationAccessible(type, currentAssembly)
               && type.TypeArguments.All(typeArgument =>
                   IsTypeReferenceAccessible(typeArgument, currentAssembly));
    }

    private static bool IsTypeDeclarationAccessible(
        INamedTypeSymbol type,
        IAssemblySymbol currentAssembly)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (!IsAccessible(current.DeclaredAccessibility, current.ContainingAssembly, currentAssembly))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsAccessible(
        Accessibility accessibility,
        IAssemblySymbol containingAssembly,
        IAssemblySymbol currentAssembly)
    {
        return accessibility == Accessibility.Public
            || ((accessibility == Accessibility.Internal
                    || accessibility == Accessibility.ProtectedOrInternal)
                && (SymbolEqualityComparer.Default.Equals(containingAssembly, currentAssembly)
                    || containingAssembly.GivesAccessTo(currentAssembly)));
    }

    private static string Generate(ImmutableArray<ModuleEventMetadata> items)
    {
        var modules = items
            .GroupBy(item => item.TypeName, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(item => item.TypeName, StringComparer.Ordinal);
        var builder = new StringBuilder();

        builder.AppendLine("// <auto-generated/>");
        builder.AppendLine("#nullable enable");
        builder.AppendLine("#pragma warning disable CS0618");
        builder.AppendLine();
        builder.AppendLine("namespace ModularPipelines.Generated;");
        builder.AppendLine();
        builder.AppendLine("internal static class ModuleEventMetadataRegistration");
        builder.AppendLine("{");
        builder.AppendLine("    [global::System.Runtime.CompilerServices.ModuleInitializer]");
        builder.AppendLine("    internal static void Register()");
        builder.AppendLine("    {");

        foreach (var module in modules)
        {
            builder.AppendLine("        global::ModularPipelines.Generated.GeneratedModuleEventMetadata.Register(");
            builder.AppendLine($"            typeof({module.TypeName}),");
            if (module.Attributes.IsEmpty)
            {
                builder.AppendLine("            static () => global::System.Array.Empty<global::System.Attribute>(),");
            }
            else
            {
                builder.AppendLine("            static () => new global::System.Attribute[]");
                builder.AppendLine("            {");
                foreach (var attribute in module.Attributes)
                {
                    builder.AppendLine($"                {attribute},");
                }

                builder.AppendLine("            },");
            }

            builder.AppendLine($"            isComplete: {(module.IsComplete ? "true" : "false")});");
        }

        builder.AppendLine("    }");
        builder.AppendLine("}");
        return builder.ToString();
    }

    private sealed record ModuleEventMetadata(
        string TypeName,
        EquatableArray<string> Attributes,
        bool IsComplete);

    private sealed record ModuleEventMetadataCandidate(
        string TypeName,
        Location Location,
        ModuleEventMetadata? Metadata);

    private sealed class ModuleEventMetadataCandidateComparer
        : IEqualityComparer<ModuleEventMetadataCandidate>
    {
        public static ModuleEventMetadataCandidateComparer Instance { get; } = new();

        public bool Equals(
            ModuleEventMetadataCandidate? x,
            ModuleEventMetadataCandidate? y) =>
            ReferenceEquals(x, y)
            || (x is not null
                && y is not null
                && StringComparer.Ordinal.Equals(x.TypeName, y.TypeName)
                && EqualityComparer<ModuleEventMetadata?>.Default.Equals(x.Metadata, y.Metadata)
                && (!RequiresDiagnostic(x) || x.Location.Equals(y.Location)));

        public int GetHashCode(ModuleEventMetadataCandidate obj)
        {
            var hashCode = (StringComparer.Ordinal.GetHashCode(obj.TypeName) * 397)
                           ^ (obj.Metadata?.GetHashCode() ?? 0);
            return RequiresDiagnostic(obj)
                ? (hashCode * 397) ^ obj.Location.GetHashCode()
                : hashCode;
        }

        private static bool RequiresDiagnostic(ModuleEventMetadataCandidate candidate) =>
            candidate.Metadata is null or { IsComplete: false };
    }

    private sealed record AttributeMetadata(
        EquatableArray<string> Expressions,
        bool IsComplete);

    private sealed record AttributeCandidate(
        string? Expression,
        bool IsComplete);

    private sealed record AttributeUsageMetadata(bool AllowMultiple, bool Inherited);
}
