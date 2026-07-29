using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ModularPipelines.Analyzers.Extensions;

namespace ModularPipelines.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class ConflictingDependsOnAttributeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MPDEP002";

    public static DiagnosticDescriptor Rule { get; } = DiagnosticDescriptorFactory.Create(
        DiagnosticId,
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerTitle),
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerMessageFormat),
        nameof(Resources.ConflictingDependsOnAttributeAnalyzerDescription));

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(startContext =>
        {
            var edges = new ConcurrentBag<DependencyEdge>();

            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectDependencyEdge(syntaxContext, edges),
                SyntaxKind.Attribute);
            startContext.RegisterCompilationEndAction(
                compilationContext => ReportCircularDependencies(compilationContext, edges));
        });
    }

    private static void CollectDependencyEdge(
        SyntaxNodeAnalysisContext context,
        ConcurrentBag<DependencyEdge> edges)
    {
        if (!TryGetDependencyType(context, out var dependencyType) ||
            dependencyType is null)
        {
            return;
        }

        var typeDeclaration = context.Node.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        var dependentType = typeDeclaration is null
            ? null
            : context.SemanticModel.GetDeclaredSymbol(
                typeDeclaration,
                context.CancellationToken);

        if (dependentType is null)
        {
            return;
        }

        edges.Add(new DependencyEdge(dependentType, dependencyType, context.Node.GetLocation()));
    }

    private static bool TryGetDependencyType(
        SyntaxNodeAnalysisContext context,
        out INamedTypeSymbol? dependencyType)
    {
        dependencyType = null;

        if (context.Node is not AttributeSyntax attributeSyntax)
        {
            return false;
        }

        var attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;

        if (attributeSymbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }

        var attributeType = methodSymbol.ContainingType;

        if (!attributeType.IsDependsOnAttribute(context.Compilation))
        {
            return false;
        }

        dependencyType = attributeType.GetDependsOnTypeArgument(
            attributeSyntax,
            context.SemanticModel) as INamedTypeSymbol;

        return dependencyType is not null;
    }

    private static void ReportCircularDependencies(
        CompilationAnalysisContext context,
        ConcurrentBag<DependencyEdge> collectedEdges)
    {
        var edges = collectedEdges
            .OrderBy(edge => edge.Location.SourceTree?.FilePath, StringComparer.Ordinal)
            .ThenBy(edge => edge.Location.SourceSpan.Start)
            .ToArray();

        if (edges.Length == 0)
        {
            return;
        }

        var graph = BuildEffectiveGraph(edges);
        var components = FindStronglyConnectedComponents(
            graph,
            context.CancellationToken);

        foreach (var edge in edges)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var dependencyType = Normalize(edge.DependencyType);
            var declarationType = Normalize(edge.DependentType);
            var effectiveDependentType =
                components[declarationType] == components[dependencyType]
                    ? declarationType
                    : graph.Keys
                        .Where(type => !SymbolEqualityComparer.Default.Equals(
                            type,
                            declarationType))
                        .Where(type => ReceivesAttributesFrom(type, declarationType))
                        .Where(type => graph[type].Contains(
                            dependencyType,
                            SymbolEqualityComparer.Default))
                        .Where(type => components[type] == components[dependencyType])
                        .OrderBy(
                            type => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                            StringComparer.Ordinal)
                        .FirstOrDefault();

            if (effectiveDependentType is null)
            {
                continue;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                Rule,
                edge.Location,
                edge.DependencyType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                effectiveDependentType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> BuildEffectiveGraph(
        IEnumerable<DependencyEdge> edges)
    {
        var directGraph = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(
            SymbolEqualityComparer.Default);
        var nodes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        foreach (var edge in edges)
        {
            var dependentType = Normalize(edge.DependentType);
            var dependencyType = Normalize(edge.DependencyType);

            nodes.Add(dependentType);
            nodes.Add(dependencyType);

            if (!directGraph.TryGetValue(dependentType, out var dependencies))
            {
                dependencies = [];
                directGraph.Add(dependentType, dependencies);
            }

            dependencies.Add(dependencyType);
        }

        var effectiveGraph = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(
            SymbolEqualityComparer.Default);

        foreach (var node in nodes)
        {
            effectiveGraph[node] =
            [
                .. GetDependencies(node, directGraph)
                    .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default),
            ];
        }

        return effectiveGraph;
    }

    private static Dictionary<INamedTypeSymbol, int> FindStronglyConnectedComponents(
        IReadOnlyDictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph,
        CancellationToken cancellationToken)
    {
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var finishOrder = new List<INamedTypeSymbol>(graph.Count);

        foreach (var node in graph.Keys)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (visited.Contains(node))
            {
                continue;
            }

            var traversal = new Stack<(INamedTypeSymbol Node, bool IsExpanded)>();
            traversal.Push((node, false));

            while (traversal.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var (current, isExpanded) = traversal.Pop();

                if (isExpanded)
                {
                    finishOrder.Add(current);
                    continue;
                }

                if (!visited.Add(current))
                {
                    continue;
                }

                traversal.Push((current, true));

                foreach (var dependency in graph[current])
                {
                    if (!visited.Contains(dependency))
                    {
                        traversal.Push((dependency, false));
                    }
                }
            }
        }

        var reverseGraph = graph.Keys.ToDictionary(
            node => node,
            _ => new List<INamedTypeSymbol>(),
            SymbolEqualityComparer.Default);

        foreach (var entry in graph)
        {
            foreach (var dependency in entry.Value)
            {
                reverseGraph[dependency].Add(entry.Key);
            }
        }

        var components = new Dictionary<INamedTypeSymbol, int>(SymbolEqualityComparer.Default);
        var nextComponent = 0;

        for (var index = finishOrder.Count - 1; index >= 0; index--)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var node = finishOrder[index];

            if (components.ContainsKey(node))
            {
                continue;
            }

            var traversal = new Stack<INamedTypeSymbol>();
            traversal.Push(node);
            components[node] = nextComponent;

            while (traversal.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = traversal.Pop();

                foreach (var dependent in reverseGraph[current])
                {
                    if (!components.ContainsKey(dependent))
                    {
                        components.Add(dependent, nextComponent);
                        traversal.Push(dependent);
                    }
                }
            }

            nextComponent += 1;
        }

        return components;
    }

    private static IEnumerable<INamedTypeSymbol> GetDependencies(
        INamedTypeSymbol type,
        Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph)
    {
        if (graph.TryGetValue(type, out var directDependencies))
        {
            foreach (var dependency in directDependencies)
            {
                yield return dependency;
            }
        }

        foreach (var interfaceType in type.AllInterfaces)
        {
            if (!graph.TryGetValue(
                    Normalize(interfaceType),
                    out var interfaceDependencies))
            {
                continue;
            }

            foreach (var dependency in interfaceDependencies)
            {
                yield return dependency;
            }
        }

        for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (!graph.TryGetValue(
                    Normalize(baseType),
                    out var baseDependencies))
            {
                continue;
            }

            foreach (var dependency in baseDependencies)
            {
                yield return dependency;
            }
        }
    }

    private static bool ReceivesAttributesFrom(
        INamedTypeSymbol type,
        INamedTypeSymbol declarationType)
    {
        if (SymbolEqualityComparer.Default.Equals(type, declarationType))
        {
            return true;
        }

        if (type.AllInterfaces.Any(interfaceType =>
                SymbolEqualityComparer.Default.Equals(
                    Normalize(interfaceType),
                    declarationType)))
        {
            return true;
        }

        for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(
                    Normalize(baseType),
                    declarationType))
            {
                return true;
            }
        }

        return false;
    }

    private static INamedTypeSymbol Normalize(INamedTypeSymbol type)
    {
        return type.OriginalDefinition;
    }

    private sealed class DependencyEdge(
        INamedTypeSymbol dependentType,
        INamedTypeSymbol dependencyType,
        Location location)
    {
        public INamedTypeSymbol DependentType { get; } = dependentType;

        public INamedTypeSymbol DependencyType { get; } = dependencyType;

        public Location Location { get; } = location;
    }
}
