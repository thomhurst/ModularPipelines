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
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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

        var graph = BuildGraph(edges);

        foreach (var edge in edges)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (!CanReach(
                    edge.DependencyType,
                    edge.DependentType,
                    graph,
                    context.CancellationToken))
            {
                continue;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                Rule,
                edge.Location,
                edge.DependencyType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                edge.DependentType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }
    }

    private static Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> BuildGraph(
        IEnumerable<DependencyEdge> edges)
    {
        var graph = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(
            SymbolEqualityComparer.Default);

        foreach (var edge in edges)
        {
            if (!graph.TryGetValue(edge.DependentType, out var dependencies))
            {
                dependencies = [];
                graph.Add(edge.DependentType, dependencies);
            }

            dependencies.Add(edge.DependencyType);
        }

        return graph;
    }

    private static bool CanReach(
        INamedTypeSymbol start,
        INamedTypeSymbol target,
        IReadOnlyDictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph,
        CancellationToken cancellationToken)
    {
        var pending = new Stack<INamedTypeSymbol>();
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        pending.Push(start);

        while (pending.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var current = pending.Pop();

            if (SymbolEqualityComparer.Default.Equals(current, target))
            {
                return true;
            }

            if (!visited.Add(current))
            {
                continue;
            }

            foreach (var dependency in GetDependencies(current, graph))
            {
                pending.Push(dependency);
            }
        }

        return false;
    }

    private static IEnumerable<INamedTypeSymbol> GetDependencies(
        INamedTypeSymbol type,
        IReadOnlyDictionary<INamedTypeSymbol, List<INamedTypeSymbol>> graph)
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
            if (!graph.TryGetValue(interfaceType, out var interfaceDependencies))
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
            if (!graph.TryGetValue(baseType, out var baseDependencies))
            {
                continue;
            }

            foreach (var dependency in baseDependencies)
            {
                yield return dependency;
            }
        }
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
