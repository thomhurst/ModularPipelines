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
    public const string DiagnosticId = "MP0005";

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(startContext =>
        {
            var edges = new ConcurrentBag<DependencyEdge>();
            var usingAliasNames = startContext.Compilation.GetUsingAliasNames(
                startContext.CancellationToken);

            startContext.RegisterSyntaxNodeAction(
                syntaxContext => CollectDependencyEdge(
                    syntaxContext,
                    edges,
                    usingAliasNames),
                SyntaxKind.Attribute);
            startContext.RegisterCompilationEndAction(
                compilationContext => ReportCircularDependencies(compilationContext, edges));
        });
    }

    private static void CollectDependencyEdge(
        SyntaxNodeAnalysisContext context,
        ConcurrentBag<DependencyEdge> edges,
        ImmutableHashSet<string> usingAliasNames)
    {
        if (!TryGetDependencyType(context, usingAliasNames, out var dependencyType) ||
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
        ImmutableHashSet<string> usingAliasNames,
        out INamedTypeSymbol? dependencyType)
    {
        dependencyType = null;

        if (context.Node is not AttributeSyntax attributeSyntax
            || !attributeSyntax.CouldBeDependsOnAttribute(usingAliasNames))
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

        var graphs = BuildDependencyGraphs(edges);
        var components = FindStronglyConnectedComponents(
            graphs.Effective,
            context.CancellationToken);
        var cyclicEffectiveDependents = FindCyclicEffectiveDependents(
            graphs,
            components,
            context.CancellationToken);

        foreach (var edge in edges)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var dependencyType = Normalize(edge.DependencyType);
            var declarationType = Normalize(edge.DependentType);

            if (!cyclicEffectiveDependents.TryGetValue(
                    declarationType,
                    out var cyclicDependencies)
                || !cyclicDependencies.TryGetValue(
                    dependencyType,
                    out var effectiveDependentType))
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

    private static DependencyGraphs BuildDependencyGraphs(
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

        return new DependencyGraphs(directGraph, effectiveGraph);
    }

    private static Dictionary<
        INamedTypeSymbol,
        Dictionary<INamedTypeSymbol, INamedTypeSymbol>> FindCyclicEffectiveDependents(
        DependencyGraphs graphs,
        Dictionary<INamedTypeSymbol, int> components,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<
            INamedTypeSymbol,
            Dictionary<INamedTypeSymbol, INamedTypeSymbol>>(
            SymbolEqualityComparer.Default);
        var receivers = graphs.Effective.Keys
            .OrderBy(
                type => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                StringComparer.Ordinal);

        foreach (var receiver in receivers)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var declaration in GetAttributeDeclarations(
                         receiver,
                         graphs.Direct))
            {
                foreach (var dependency in graphs.Direct[declaration])
                {
                    if (components[receiver] != components[dependency])
                    {
                        continue;
                    }

                    if (!result.TryGetValue(declaration, out var cyclicDependencies))
                    {
                        cyclicDependencies =
                            new Dictionary<INamedTypeSymbol, INamedTypeSymbol>(
                                SymbolEqualityComparer.Default);
                        result.Add(declaration, cyclicDependencies);
                    }

                    if (!cyclicDependencies.ContainsKey(dependency))
                    {
                        cyclicDependencies.Add(dependency, receiver);
                    }
                }
            }
        }

        return result;
    }

    private static IEnumerable<INamedTypeSymbol> GetAttributeDeclarations(
        INamedTypeSymbol type,
        Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> directGraph)
    {
        var yielded = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        if (directGraph.ContainsKey(type) && yielded.Add(type))
        {
            yield return type;
        }

        foreach (var interfaceType in type.AllInterfaces)
        {
            var declaration = Normalize(interfaceType);

            if (directGraph.ContainsKey(declaration) && yielded.Add(declaration))
            {
                yield return declaration;
            }
        }

        for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            var declaration = Normalize(baseType);

            if (directGraph.ContainsKey(declaration) && yielded.Add(declaration))
            {
                yield return declaration;
            }
        }
    }

    private static Dictionary<INamedTypeSymbol, int> FindStronglyConnectedComponents(
        IReadOnlyDictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph,
        CancellationToken cancellationToken)
    {
        var finishOrder = CreateFinishOrder(graph, cancellationToken);
        var reverseGraph = CreateReverseGraph(graph);

        return AssignComponents(finishOrder, reverseGraph, cancellationToken);
    }

    private static List<INamedTypeSymbol> CreateFinishOrder(
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

        return finishOrder;
    }

    private static Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> CreateReverseGraph(
        IReadOnlyDictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph)
    {
        var reverseGraph = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(
            SymbolEqualityComparer.Default);

        foreach (var node in graph.Keys)
        {
            reverseGraph.Add(node, []);
        }

        foreach (var entry in graph)
        {
            foreach (var dependency in entry.Value)
            {
                reverseGraph[dependency].Add(entry.Key);
            }
        }

        return reverseGraph;
    }

    private static Dictionary<INamedTypeSymbol, int> AssignComponents(
        List<INamedTypeSymbol> finishOrder,
        Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> reverseGraph,
        CancellationToken cancellationToken)
    {
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

    private sealed class DependencyGraphs(
        Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> direct,
        Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> effective)
    {
        public Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> Direct { get; } = direct;

        public Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> Effective { get; } = effective;
    }
}
