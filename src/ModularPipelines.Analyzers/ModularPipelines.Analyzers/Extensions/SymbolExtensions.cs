using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SymbolExtensions
{
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

    internal static bool IsDependsOnAttributeFor(this AttributeData attributeData, INamedTypeSymbol namedTypeSymbol)
    {
        var attributeClassName = attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (string.IsNullOrEmpty(attributeClassName))
        {
            return false;
        }

        if (!attributeClassName!.StartsWith("global::ModularPipelines.Attributes.DependsOnAttribute"))
        {
            return false;
        }

        if (attributeData.AttributeClass!.IsGenericType)
        {
            return SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass.TypeArguments.First(), namedTypeSymbol);
        }

        return attributeData.ConstructorArguments.Any(x =>
        {
            var argumentValue = x.Value;

            if (argumentValue is INamedTypeSymbol argumentNamedTypeSymbol)
            {
                return SymbolEqualityComparer.Default.Equals(argumentNamedTypeSymbol, namedTypeSymbol);
            }

            return false;
        });
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
}