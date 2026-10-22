using Microsoft.CodeAnalysis;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

internal static class ModuleDependencySelectors
{
    internal static DependencyDeclarationKind? GetDeclarationKind(
        INamedTypeSymbol module,
        INamedTypeSymbol dependency,
        Compilation compilation)
    {
        // Selectors exclude the declaring module, even when their predicate matches it.
        if (SymbolEqualityComparer.Default.Equals(module, dependency))
        {
            return null;
        }

        DependencyDeclarationKind? result = null;
        foreach (var attribute in module.AllInterfaces.SelectMany(iface => iface.GetAttributes()).Concat(GetApplicableAttributes(module, compilation)))
        {
            if (attribute.AttributeClass is not { } selector
                || !selector.InheritsFrom(compilation, "ModularPipelines.DependsOnBaseAttribute"))
            {
                continue;
            }

            var kind = GetSelectorKind(attribute, dependency, compilation);
            if (kind == DependencyDeclarationKind.Required)
            {
                return kind;
            }

            result ??= kind;
        }

        return result;
    }

    private static DependencyDeclarationKind? GetSelectorKind(
        AttributeData attribute,
        INamedTypeSymbol dependency,
        Compilation compilation)
    {
        var selector = attribute.AttributeClass!;
        if (selector.IsAnyType(compilation,
                "ModularPipelines.DependsOnAllModulesInheritingFromAttribute",
                "ModularPipelines.DependsOnAllModulesInheritingFromAttribute`1"))
        {
            var target = selector.IsGenericType ? selector.TypeArguments[0]
                : attribute.ConstructorArguments.FirstOrDefault().Value as ITypeSymbol;
            return target is null ? DependencyDeclarationKind.Unknown
                : MatchKind(IsAssignableTo(dependency, target, compilation));
        }

        if (selector.IsType(compilation, "ModularPipelines.DependsOnModulesWithAttributeAttribute`1"))
        {
            var target = selector.TypeArguments[0];
            foreach (var candidateAttribute in GetApplicableAttributes(dependency, compilation))
            {
                if (candidateAttribute.AttributeClass is { } attributeClass
                    && IsAssignableTo(attributeClass, target, compilation) is true)
                {
                    return DependencyDeclarationKind.Required;
                }
            }

            return ContainsTypeParameters(target) ? DependencyDeclarationKind.Unknown : null;
        }

        // Tags/categories can be supplied or overridden by Configure at runtime.
        // Custom selectors can execute arbitrary code; never infer their result.
        return DependencyDeclarationKind.Unknown;
    }

    private static DependencyDeclarationKind? MatchKind(bool? matches) => matches switch
    {
        true => DependencyDeclarationKind.Required,
        false => null,
        null => DependencyDeclarationKind.Unknown,
    };

    private static bool? IsAssignableTo(ITypeSymbol candidate, ITypeSymbol target, Compilation compilation)
    {
        if (target is INamedTypeSymbol { IsUnboundGenericType: true })
        {
            for (var current = candidate; current is not null; current = current.BaseType)
            {
                if (SymbolEqualityComparer.Default.Equals(current.OriginalDefinition, target.OriginalDefinition))
                {
                    return true;
                }
            }

            if (candidate.AllInterfaces.Any(iface => SymbolEqualityComparer.Default.Equals(iface.OriginalDefinition, target.OriginalDefinition)))
            {
                return true;
            }
        }
        else
        {
            var conversion = compilation.ClassifyCommonConversion(candidate, target);
            if (conversion.IsIdentity || (conversion.IsImplicit && conversion.IsReference))
            {
                return true;
            }
        }

        return ContainsTypeParameters(candidate) || ContainsTypeParameters(target) ? null : false;
    }

    private static bool ContainsTypeParameters(ITypeSymbol type) => type switch
    {
        ITypeParameterSymbol => true,
        IArrayTypeSymbol array => ContainsTypeParameters(array.ElementType),
        INamedTypeSymbol named => named.TypeArguments.Any(ContainsTypeParameters)
            || (named.ContainingType is { } containingType && ContainsTypeParameters(containingType)),
        _ => false,
    };

    private static IEnumerable<AttributeData> GetApplicableAttributes(INamedTypeSymbol dependency, Compilation compilation)
    {
        for (var current = dependency; current is not null; current = current.BaseType)
        {
            foreach (var attribute in current.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(current, dependency)
                    || IsInherited(attribute.AttributeClass, compilation))
                {
                    yield return attribute;
                }
            }
        }
    }

    private static bool IsInherited(INamedTypeSymbol? attributeClass, Compilation compilation)
    {
        for (var current = attributeClass; current is not null; current = current.BaseType)
        {
            var usage = current.GetAttributes().FirstOrDefault(attribute =>
                attribute.AttributeClass.IsType(compilation, "System.AttributeUsageAttribute"));
            if (usage is not null)
            {
                return !usage.NamedArguments.Any(argument => argument.Key == "Inherited" && argument.Value.Value is false);
            }
        }

        return true;
    }
}
