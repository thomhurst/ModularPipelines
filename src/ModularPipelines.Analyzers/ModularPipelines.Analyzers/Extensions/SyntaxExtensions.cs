using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModularPipelines.Analyzers.Extensions;

[ExcludeFromCodeCoverage]
internal static class SyntaxExtensions
{
    private const string AttributeSuffix = "Attribute";

    internal static ImmutableHashSet<string> GetUsingAliasNames(
        this Compilation compilation,
        CancellationToken cancellationToken)
    {
        var aliases = ImmutableHashSet.CreateBuilder<string>(StringComparer.Ordinal);

        foreach (var directive in compilation.SyntaxTrees
                     .SelectMany(tree => tree.GetRoot(cancellationToken)
                .DescendantNodes(static node =>
                    node is CompilationUnitSyntax or BaseNamespaceDeclarationSyntax)
                .OfType<UsingDirectiveSyntax>()))
        {
            if (directive.Alias?.Name.Identifier.ValueText is not { } alias)
            {
                continue;
            }

            aliases.Add(alias);
            if (alias.EndsWith(AttributeSuffix, StringComparison.Ordinal))
            {
                aliases.Add(alias.Substring(0, alias.Length - AttributeSuffix.Length));
            }
        }

        return aliases.ToImmutable();
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
