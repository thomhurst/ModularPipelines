using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SymbolExtensions
{
    /// <summary>
    /// Checks if the given type symbol inherits from or is the specified type by metadata name.
    /// Uses proper symbol comparison instead of string comparison.
    /// </summary>
    internal static bool InheritsFrom(this ITypeSymbol? typeSymbol, Compilation compilation, string fullyQualifiedMetadataName)
    {
        if (typeSymbol is null)
        {
            return false;
        }

        var targetType = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
        if (targetType is null)
        {
            return false;
        }

        return typeSymbol.InheritsFrom(targetType);
    }

    /// <summary>
    /// Checks if the given type symbol inherits from or is the specified target type.
    /// </summary>
    internal static bool InheritsFrom(this ITypeSymbol? typeSymbol, INamedTypeSymbol? targetType)
    {
        if (typeSymbol is null || targetType is null)
        {
            return false;
        }

        var current = typeSymbol;
        while (current != null)
        {
            if (SymbolEqualityComparer.Default.Equals(current.OriginalDefinition, targetType.OriginalDefinition))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Checks if the type symbol matches the specified type by metadata name.
    /// Handles both generic and non-generic types using OriginalDefinition for generic type comparison.
    /// </summary>
    internal static bool IsType(this ITypeSymbol? typeSymbol, Compilation compilation, string fullyQualifiedMetadataName)
    {
        if (typeSymbol is null)
        {
            return false;
        }

        var targetType = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
        if (targetType is null)
        {
            return false;
        }

        // Use OriginalDefinition for generic types (e.g., ILogger<T> matches ILogger<>)
        return SymbolEqualityComparer.Default.Equals(typeSymbol.OriginalDefinition, targetType.OriginalDefinition);
    }

    /// <summary>
    /// Checks if the type symbol matches any of the specified types by metadata name.
    /// </summary>
    internal static bool IsAnyType(this ITypeSymbol? typeSymbol, Compilation compilation, params string[] fullyQualifiedMetadataNames)
    {
        return typeSymbol.IsAnyType(compilation, fullyQualifiedMetadataNames.AsSpan());
    }

    /// <summary>
    /// Checks if the type symbol matches any of the specified types by metadata name.
    /// </summary>
    internal static bool IsAnyType(this ITypeSymbol? typeSymbol, Compilation compilation, ReadOnlySpan<string> fullyQualifiedMetadataNames)
    {
        if (typeSymbol is null)
        {
            return false;
        }

        foreach (var metadataName in fullyQualifiedMetadataNames)
        {
            if (typeSymbol.IsType(compilation, metadataName))
            {
                return true;
            }
        }

        return false;
    }

    internal static IEnumerable<AttributeData> GetAllAttributesIncludingBaseAndInterfaces(this INamedTypeSymbol classSymbol)
    {
        foreach (var attributeData in classSymbol.AllInterfaces.SelectMany(x => x.GetAttributes()))
        {
            yield return attributeData;
        }

        while (true)
        {
            foreach (var attributeData in classSymbol.GetAttributes())
            {
                yield return attributeData;
            }

            var baseType = classSymbol.BaseType;

            if (baseType == null)
            {
                yield break;
            }

            classSymbol = baseType;
        }
    }

    internal static IEnumerable<INamedTypeSymbol> GetSelfAndAllBaseTypes(this INamedTypeSymbol? classSymbol)
    {
        while (classSymbol != null)
        {
            yield return classSymbol;
            classSymbol = classSymbol.BaseType;
        }
    }

    internal static bool IsDependsOnAttributeFor(this AttributeData attributeData, Compilation compilation, INamedTypeSymbol namedTypeSymbol)
    {
        var attributeClass = attributeData.AttributeClass;
        if (attributeClass is null)
        {
            return false;
        }

        if (!IsDependsOnAttribute(attributeClass, compilation))
        {
            return false;
        }

        if (attributeClass.IsGenericType)
        {
            return SymbolEqualityComparer.Default.Equals(attributeClass.TypeArguments.First(), namedTypeSymbol);
        }

        return attributeData.ConstructorArguments.Any(x =>
            x.Value is INamedTypeSymbol argumentNamedTypeSymbol &&
            SymbolEqualityComparer.Default.Equals(argumentNamedTypeSymbol, namedTypeSymbol));
    }

    internal static INamedTypeSymbol? GetClassThatNodeIsIn(this SyntaxNodeAnalysisContext context)
    {
        var node = context.Node;

        while (node is not ClassDeclarationSyntax)
        {
            if (node is null)
            {
                return null;
            }

            node = node.Parent;
        }

        return context.SemanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
    }

    /// <summary>
    /// Checks if the given type symbol is the DependsOnAttribute type (generic or non-generic).
    /// Uses proper symbol comparison instead of string comparison.
    /// </summary>
    private static bool IsDependsOnAttribute(INamedTypeSymbol attributeClass, Compilation compilation)
    {
        // Get the non-generic DependsOnAttribute type
        var dependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute");
        if (dependsOnAttributeType is null)
        {
            return false;
        }

        // For generic types, compare the original definition (DependsOnAttribute<T> -> DependsOnAttribute<>)
        var attributeToCompare = attributeClass.IsGenericType
            ? attributeClass.OriginalDefinition
            : attributeClass;

        // Check if it's the non-generic version
        if (SymbolEqualityComparer.Default.Equals(attributeToCompare, dependsOnAttributeType))
        {
            return true;
        }

        // Get and check the generic version (DependsOnAttribute`1)
        var genericDependsOnAttributeType = compilation.GetTypeByMetadataName("ModularPipelines.Attributes.DependsOnAttribute`1");
        return genericDependsOnAttributeType is not null &&
               SymbolEqualityComparer.Default.Equals(attributeToCompare, genericDependsOnAttributeType);
    }
}