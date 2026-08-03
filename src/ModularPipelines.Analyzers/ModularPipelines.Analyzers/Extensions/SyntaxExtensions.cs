using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SyntaxExtensions
{
    internal static ImmutableHashSet<string> GetUsingAliasNames(
        this Compilation compilation,
        CancellationToken cancellationToken)
    {
        return compilation.SyntaxTrees
            .SelectMany(tree => tree.GetRoot(cancellationToken)
                .DescendantNodes(static node =>
                    node is CompilationUnitSyntax or BaseNamespaceDeclarationSyntax)
                .OfType<UsingDirectiveSyntax>())
            .Select(static directive => directive.Alias?.Name.Identifier.ValueText)
            .OfType<string>()
            .ToImmutableHashSet(StringComparer.Ordinal);
    }

    internal static bool CouldBeDependsOnAttribute(
        this AttributeSyntax attribute,
        ImmutableHashSet<string> usingAliasNames)
    {
        var name = attribute.Name switch
        {
            SimpleNameSyntax simpleName => simpleName.Identifier.ValueText,
            QualifiedNameSyntax qualifiedName => qualifiedName.Right.Identifier.ValueText,
            AliasQualifiedNameSyntax aliasQualifiedName => aliasQualifiedName.Name.Identifier.ValueText,
            _ => string.Empty,
        };

        return name.Contains("DependsOn", StringComparison.Ordinal)
               || usingAliasNames.Contains(name);
    }
}
