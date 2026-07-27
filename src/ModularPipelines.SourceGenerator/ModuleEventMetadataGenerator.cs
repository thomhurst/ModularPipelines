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

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var modules = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax
                {
                    BaseList.Types.Count: > 0,
                },
                static (generatorContext, _) => GetModuleMetadata(generatorContext))
            .Where(static metadata => metadata is not null)
            .Select(static (metadata, _) => metadata!);

        context.RegisterSourceOutput(modules.Collect(), static (sourceContext, metadata) =>
        {
            if (!metadata.IsEmpty)
            {
                sourceContext.AddSource(
                    "ModularPipelines.ModuleEventMetadata.g.cs",
                    Generate(metadata));
            }
        });
    }

    private static ModuleEventMetadata? GetModuleMetadata(GeneratorSyntaxContext context)
    {
        var compilation = context.SemanticModel.Compilation;
        var type = GetEligibleModuleType(context, compilation);
        if (type is null)
        {
            return null;
        }

        var attributeMetadata = GetAttributeMetadata(type, compilation.Assembly);
        return new ModuleEventMetadata(
            type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            attributeMetadata.Expressions,
            attributeMetadata.IsComplete);
    }

    private static INamedTypeSymbol? GetEligibleModuleType(
        GeneratorSyntaxContext context,
        Compilation compilation)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol type
            || type.IsAbstract
            || type.IsGenericType
            || !IsTypeAccessible(type, compilation.Assembly)
            || !InheritsFromModule(type, compilation))
        {
            return null;
        }

        return type;
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
            var member = attributeType.GetMembers(argument.Key).FirstOrDefault();
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

        if (constant.Kind == TypedConstantKind.Array)
        {
            if (constant.Type is not IArrayTypeSymbol arrayType)
            {
                return null;
            }

            var values = new List<string>();
            foreach (var value in constant.Values)
            {
                var expression = FormatTypedConstant(value, currentAssembly);
                if (expression is null)
                {
                    return null;
                }

                values.Add(expression);
            }

            var elementType = arrayType.ElementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return $"new {elementType}[] {{ {string.Join(", ", values)} }}";
        }

        if (constant.Kind == TypedConstantKind.Type && constant.Value is ITypeSymbol type)
        {
            if (!IsTypeReferenceAccessible(type, currentAssembly))
            {
                return null;
            }

            return $"typeof({type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})";
        }

        if (constant.Type?.TypeKind == TypeKind.Enum)
        {
            var enumType = constant.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return $"({enumType}){FormatEnumValue(constant.Value!)}";
        }

        return FormatPrimitive(constant.Value);
    }

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
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (!IsAccessible(current.DeclaredAccessibility, current.ContainingAssembly, currentAssembly))
            {
                return false;
            }
        }

        return type.TypeArguments.All(typeArgument =>
            typeArgument is not INamedTypeSymbol namedType || IsTypeAccessible(namedType, currentAssembly));
    }

    private static bool IsAccessible(
        Accessibility accessibility,
        IAssemblySymbol containingAssembly,
        IAssemblySymbol currentAssembly)
    {
        return accessibility == Accessibility.Public
            || ((accessibility == Accessibility.Internal
                    || accessibility == Accessibility.ProtectedOrInternal)
                && SymbolEqualityComparer.Default.Equals(containingAssembly, currentAssembly));
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
            builder.AppendLine("        global::ModularPipelines.Engine.Attributes.GeneratedModuleEventMetadata.Register(");
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
        ImmutableArray<string> Attributes,
        bool IsComplete);

    private sealed record AttributeMetadata(
        ImmutableArray<string> Expressions,
        bool IsComplete);

    private sealed record AttributeCandidate(
        string? Expression,
        bool IsComplete);

    private sealed record AttributeUsageMetadata(bool AllowMultiple, bool Inherited);
}
