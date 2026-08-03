using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SyntaxExtensions
{
    internal static bool CouldBeDependsOnAttribute(this AttributeSyntax attribute)
    {
        var name = attribute.Name switch
        {
            SimpleNameSyntax simpleName => simpleName.Identifier.ValueText,
            QualifiedNameSyntax qualifiedName => qualifiedName.Right.Identifier.ValueText,
            AliasQualifiedNameSyntax aliasQualifiedName => aliasQualifiedName.Name.Identifier.ValueText,
            _ => string.Empty,
        };

        return name.Contains("DependsOn", StringComparison.Ordinal);
    }
}
