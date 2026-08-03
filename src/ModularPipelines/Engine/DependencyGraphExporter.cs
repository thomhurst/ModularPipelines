using System.Text;
using System.Text.Json;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal sealed class DependencyGraphExporter(
    ModuleRetriever moduleRetriever,
    ModuleDiscoveryPlanner moduleDiscoveryPlanner,
    IDependencyChainProvider dependencyChainProvider,
    IModuleMetadataRegistry metadataRegistry,
    IModuleConditionHandler moduleConditionHandler,
    IServiceProvider serviceProvider,
    ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
    IMediator mediator,
    IIgnoredModuleResultRegistrar ignoredModuleResultRegistrar) :
    IDependencyGraphExporter
{
    public async Task<string> RenderAsync(
        DependencyGraphFormat format,
        CancellationToken cancellationToken = default)
    {
        var graph = await CreateGraphAsync(cancellationToken).ConfigureAwait(false);
        return Render(format, graph);
    }

    public async Task<string> RenderSummaryAsync(
        DependencyGraphFormat format,
        PipelineSummary pipelineSummary,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pipelineSummary);
        var graph = await CreateGraphAsync(pipelineSummary, cancellationToken).ConfigureAwait(false);
        return Render(format, graph);
    }

    private static string Render(
        DependencyGraphFormat format,
        DependencyGraphDocument graph)
    {
        return format switch
        {
            DependencyGraphFormat.Mermaid => RenderMermaid(graph),
            DependencyGraphFormat.Dot => RenderDot(graph),
            DependencyGraphFormat.Json => RenderJson(graph),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };
    }

    public async Task ExportAsync(
        DependencyGraphFormat format,
        string path,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = Path.GetFullPath(path);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var contents = await RenderAsync(format, cancellationToken).ConfigureAwait(false);
        await File.WriteAllTextAsync(fullPath, contents, cancellationToken).ConfigureAwait(false);
    }

    private async Task<DependencyGraphDocument> CreateGraphAsync(
        CancellationToken cancellationToken)
    {
        await using var planningDiscovery = await moduleDiscoveryPlanner
            .DiscoverAsync(cancellationToken)
            .ConfigureAwait(false);
        var organizedModules = planningDiscovery.OrganizedModules;
        var ignoredModuleResolution = await ignoredModuleResultRegistrar
            .ResolveIgnoredModuleResultsAsync(
                organizedModules,
                planningDiscovery.DependencyRegistry,
                planningDiscovery.MetadataRegistry,
                planningDiscovery.OriginalModules)
            .ConfigureAwait(false);
        organizedModules = ignoredModuleResolution.OrganizedModules;
        ValidateRunnableModules(
            organizedModules,
            ignoredModuleResolution.UsedHistoryModuleTypes,
            planningDiscovery.DependencyRegistry,
            planningDiscovery.MetadataRegistry);
        var ignoredBeforeRunConditions = organizedModules.IgnoredModules
            .Select(ignored => ignored.Module)
            .ToHashSet<IModule>(ReferenceEqualityComparer.Instance);
        var conditionResolution = await ApplyRunConditionsAsync(
                organizedModules,
                planningDiscovery.MetadataRegistry,
                cancellationToken)
            .ConfigureAwait(false);
        var runConditionHistoryTypes = await ignoredModuleResultRegistrar
            .ResolveHistoryModuleTypesAsync(
                conditionResolution.OrganizedModules.IgnoredModules
                    .Select(ignored => ignored.Module)
                    .Where(module => !ignoredBeforeRunConditions.Contains(module)),
                planningDiscovery.OriginalModules)
            .ConfigureAwait(false);
        var usedHistoryModuleTypes = ignoredModuleResolution.UsedHistoryModuleTypes
            .Concat(runConditionHistoryTypes)
            .ToHashSet();
        var historyResolvedModuleTypes = ignoredModuleResolution.OrganizedModules.IgnoredModules
            .Select(ignored => ignored.Module.GetType())
            .Concat(conditionResolution.OrganizedModules.IgnoredModules
                .Select(ignored => ignored.Module.GetType()))
            .ToHashSet();
        var skipResolution = await CascadeRunConditionSkipsAsync(
                conditionResolution.OrganizedModules,
                usedHistoryModuleTypes,
                historyResolvedModuleTypes,
                planningDiscovery.OriginalModules,
                planningDiscovery.DependencyRegistry,
                planningDiscovery.MetadataRegistry,
                cancellationToken)
            .ConfigureAwait(false);
        organizedModules = skipResolution.OrganizedModules;
        planningDiscovery.DependencyChainProvider.Initialize(organizedModules.AllModules);
        var models = planningDiscovery.DependencyChainProvider.ModuleDependencyModels
            .OrderBy(model => GetModuleFullName(model.Module), StringComparer.Ordinal)
            .ToArray();
        var unresolvedSkipDecisions = PropagateUnresolvedSkipDecisions(
            models,
            conditionResolution.UnresolvedSkipDecisionTypes,
            skipResolution.SkippedModuleTypes,
            planningDiscovery.DependencyRegistry,
            planningDiscovery.MetadataRegistry);
        var ignoredModules = organizedModules.IgnoredModules.ToDictionary(
            ignored => ignored.Module.GetType());
        var states = models.ToDictionary(
            model => model.Module.GetType(),
            model =>
            {
                var moduleType = model.Module.GetType();
                ignoredModules.TryGetValue(moduleType, out var ignored);
                return skipResolution.SkippedModuleTypes.Contains(moduleType)
                    ? new GraphNodeExecutionState(true, ignored?.SkipDecision.Reason)
                    : unresolvedSkipDecisions.Contains(moduleType)
                        ? new GraphNodeExecutionState(null, null)
                        : new GraphNodeExecutionState(false, null);
            });

        return CreateDocument(
            organizedModules,
            models,
            states,
            planningDiscovery.MetadataRegistry);
    }

    private async Task<DependencyGraphDocument> CreateGraphAsync(
        PipelineSummary pipelineSummary,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var organizedModules = await moduleRetriever
            .GetOrganizedModules(cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        dependencyChainProvider.Initialize(organizedModules.AllModules);
        var models = dependencyChainProvider.ModuleDependencyModels
            .OrderBy(model => GetModuleFullName(model.Module), StringComparer.Ordinal)
            .ToArray();
        var resultsByType = pipelineSummary.Results
            .OfType<ModuleResult>()
            .Where(result => result.ModuleType is not null)
            .ToDictionary(result => result.ModuleType!);
        var states = models.ToDictionary(
            model => model.Module.GetType(),
            model =>
            {
                resultsByType.TryGetValue(model.Module.GetType(), out var result);
                return new GraphNodeExecutionState(
                    result?.ModuleStatus == Status.Skipped,
                    result?.SkipDecisionOrDefault?.Reason);
            });

        return CreateDocument(organizedModules, models, states, metadataRegistry);
    }

    private static DependencyGraphDocument CreateDocument(
        OrganizedModules organizedModules,
        IReadOnlyList<ModuleDependencyModel> models,
        IReadOnlyDictionary<Type, GraphNodeExecutionState> states,
        IModuleMetadataRegistry graphMetadataRegistry)
    {
        var estimatedDurations = organizedModules.RunnableModules.ToDictionary(
            runnable => runnable.Module.GetType(),
            runnable => runnable.EstimatedDuration);
        var identifiers = models
            .Select((model, index) => (model.Module, Identifier: $"n{index}"))
            .ToDictionary(item => item.Module.GetType(), item => item.Identifier);
        var nodes = models.Select(model =>
        {
            var moduleType = model.Module.GetType();
            estimatedDurations.TryGetValue(moduleType, out var estimatedDuration);
            var state = states[moduleType];
            return new DependencyGraphNode(
                identifiers[moduleType],
                moduleType.Name,
                GetModuleFullName(model.Module),
                graphMetadataRegistry.GetCategory(moduleType),
                state.Skipped,
                state.SkipReason,
                estimatedDuration);
        }).ToArray();
        var edges = models
            .SelectMany(dependent => dependent.IsDependentOn.Select(dependency =>
                new DependencyGraphEdge(
                    identifiers[dependency.Module.GetType()],
                    identifiers[dependent.Module.GetType()])))
            .OrderBy(edge => edge.From, StringComparer.Ordinal)
            .ThenBy(edge => edge.To, StringComparer.Ordinal)
            .ToArray();

        return new DependencyGraphDocument(nodes, edges);
    }

    private async Task<RunConditionSkipResolution> CascadeRunConditionSkipsAsync(
        OrganizedModules organizedModules,
        HashSet<Type> usedHistoryModuleTypes,
        HashSet<Type> historyResolvedModuleTypes,
        IReadOnlyDictionary<IModule, IModule> originalModules,
        IModuleDependencyRegistry graphDependencyRegistry,
        IModuleMetadataRegistry graphMetadataRegistry,
        CancellationToken cancellationToken)
    {
        var skippedModuleTypes = new HashSet<Type>();
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
                [.. organizedModules.AllModules],
                organizedModules.RunnableModules.Select(runnable => (IModule) runnable.Module),
                organizedModules.IgnoredModules,
                graphDependencyRegistry,
                graphMetadataRegistry,
                async pendingIgnoredModules =>
                {
                    var unresolvedModules = pendingIgnoredModules
                        .Select(ignored => ignored.Module)
                        .Where(module => historyResolvedModuleTypes.Add(module.GetType()))
                        .ToArray();
                    if (unresolvedModules.Length > 0)
                    {
                        var resolvedHistoryModuleTypes = await ignoredModuleResultRegistrar
                            .ResolveHistoryModuleTypesAsync(unresolvedModules, originalModules)
                            .ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();
                        usedHistoryModuleTypes.UnionWith(resolvedHistoryModuleTypes);
                    }

                    foreach (var ignoredModule in pendingIgnoredModules)
                    {
                        var moduleType = ignoredModule.Module.GetType();
                        if (!usedHistoryModuleTypes.Contains(moduleType))
                        {
                            skippedModuleTypes.Add(moduleType);
                        }
                    }
                },
                skippedModuleTypes.Contains,
                cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        var remainingModules = cascadeResult.RunnableModules.ToHashSet<IModule>(
            ReferenceEqualityComparer.Instance);

        return new RunConditionSkipResolution(
            new OrganizedModules(
                [.. organizedModules.RunnableModules.Where(runnable => remainingModules.Contains(runnable.Module))],
                cascadeResult.IgnoredModules),
            skippedModuleTypes);
    }

    private async Task<RunConditionResolution> ApplyRunConditionsAsync(
        OrganizedModules organizedModules,
        IModuleMetadataRegistry graphMetadataRegistry,
        CancellationToken cancellationToken)
    {
        var runnableModules = new List<RunnableModule>();
        var ignoredModules = organizedModules.IgnoredModules.ToList();
        var unresolvedSkipDecisionTypes = new HashSet<Type>();
        foreach (var runnableModule in organizedModules.RunnableModules)
        {
            var (shouldIgnore, skipDecision) = await moduleConditionHandler
                .ShouldIgnoreForPlanning(
                    runnableModule.Module,
                    graphMetadataRegistry,
                    cancellationToken)
                .ConfigureAwait(false);
            if (!shouldIgnore && runnableModule.Module.Configuration.SkipCondition is not null)
            {
                skipDecision = await EvaluateConfiguredSkipConditionAsync(
                        runnableModule.Module,
                        cancellationToken)
                    .ConfigureAwait(false);
                if (skipDecision is null)
                {
                    unresolvedSkipDecisionTypes.Add(runnableModule.Module.GetType());
                }
                else
                {
                    shouldIgnore = skipDecision.ShouldSkip;
                }
            }

            if (shouldIgnore)
            {
                ignoredModules.Add(new IgnoredModule(
                    runnableModule.Module,
                    skipDecision ?? SkipDecision.Skip("Module was ignored")));
            }
            else
            {
                runnableModules.Add(runnableModule);
            }
        }

        return new RunConditionResolution(
            new OrganizedModules(runnableModules, ignoredModules),
            unresolvedSkipDecisionTypes);
    }

    private async Task<SkipDecision?> EvaluateConfiguredSkipConditionAsync(
        IModule module,
        CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var scopedServices = scope.ServiceProvider;
        var configuration = module.Configuration;
        var executionContext = ExecutionContextFactory.Create(module, module.GetType());
        try
        {
            var moduleContext = new ModuleContext(
                scopedServices.GetRequiredService<IPipelineContext>(),
                module,
                executionContext,
                scopedServices.GetRequiredService<IInternalModuleLoggerProvider>().GetLogger(module.GetType()),
                mediator,
                estimatedTimeProvider,
                moduleResultAccessAllowed: false);
            try
            {
                using var planningResultAccess = PlanningModuleResultAccess.Enter();
                var planningCondition = configuration.SynchronousPlanningSkipCondition;
                return planningCondition is null
                    ? null
                    : await planningCondition(moduleContext, cancellationToken).ConfigureAwait(false);
            }
            catch (PlanningModuleResultUnavailableException)
            {
                return null;
            }
        }
        finally
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
        }
    }

    private static HashSet<Type> PropagateUnresolvedSkipDecisions(
        IReadOnlyList<ModuleDependencyModel> models,
        IReadOnlySet<Type> initiallyUnresolvedTypes,
        IReadOnlySet<Type> skippedModuleTypes,
        IModuleDependencyRegistry graphDependencyRegistry,
        IModuleMetadataRegistry graphMetadataRegistry)
    {
        var unresolvedTypes = initiallyUnresolvedTypes
            .Where(type => !skippedModuleTypes.Contains(type))
            .ToHashSet();
        var availableModuleTypes = models.Select(model => model.Module.GetType()).ToArray();
        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var model in models.Where(model =>
                         !skippedModuleTypes.Contains(model.Module.GetType())))
            {
                var moduleType = model.Module.GetType();
                if (unresolvedTypes.Contains(moduleType))
                {
                    continue;
                }

                var dependsOnUnresolved = ModuleDependencyResolver
                    .GetAllDependencies(
                        model.Module,
                        availableModuleTypes,
                        graphDependencyRegistry,
                        graphMetadataRegistry)
                    .Any(dependency => !dependency.Optional
                                       && unresolvedTypes.Contains(dependency.DependencyType));
                if (dependsOnUnresolved)
                {
                    changed = unresolvedTypes.Add(moduleType) || changed;
                }
            }
        }

        return unresolvedTypes;
    }

    private static void ValidateRunnableModules(
        OrganizedModules organizedModules,
        IReadOnlySet<Type> usedHistoryModuleTypes,
        IModuleDependencyRegistry graphDependencyRegistry,
        IModuleMetadataRegistry graphMetadataRegistry)
    {
        var runnableModules = organizedModules.RunnableModules
            .Select(runnableModule => (IModule) runnableModule.Module)
            .Concat(organizedModules.IgnoredModules
                .Select(ignoredModule => ignoredModule.Module)
                .Where(module => usedHistoryModuleTypes.Contains(module.GetType())))
            .ToList();

        ModuleDependencyValidator.Validate(
            runnableModules,
            graphDependencyRegistry,
            graphMetadataRegistry,
            usedHistoryModuleTypes);
    }

    private static string RenderMermaid(DependencyGraphDocument graph)
    {
        var builder = new StringBuilder("flowchart TD");
        foreach (var node in graph.Nodes)
        {
            builder.AppendLine();
            builder.Append("    ").Append(node.Id).Append("[\"")
                .Append(BuildLabel(node, "<br/>", EscapeMermaid))
                .Append("\"]");
        }

        foreach (var edge in graph.Edges)
        {
            builder.AppendLine();
            builder.Append("    ").Append(edge.From).Append(" --> ").Append(edge.To);
        }

        if (graph.Nodes.Any(node => node.Skipped is true))
        {
            builder.AppendLine();
            builder.AppendLine("    classDef skipped fill:#616161,color:#fff,stroke:#424242,stroke-dasharray: 5 5");
            builder.Append("    class ")
                .AppendJoin(',', graph.Nodes.Where(node => node.Skipped is true).Select(node => node.Id))
                .Append(" skipped");
        }

        return builder.ToString();
    }

    private static string RenderDot(DependencyGraphDocument graph)
    {
        var builder = new StringBuilder();
        builder.AppendLine("digraph ModularPipelines {");
        builder.AppendLine("    rankdir=LR;");
        foreach (var node in graph.Nodes)
        {
            builder.Append("    ").Append(node.Id)
                .Append(" [label=\"")
                .Append(BuildLabel(node, "\\n", EscapeDot))
                .Append('"');
            if (node.Skipped is true)
            {
                builder.Append(", style=\"filled,dashed\", fillcolor=\"#616161\", fontcolor=\"white\"");
            }

            builder.AppendLine("];");
        }

        foreach (var edge in graph.Edges)
        {
            builder.Append("    ").Append(edge.From).Append(" -> ").Append(edge.To).AppendLine(";");
        }

        builder.Append('}');
        return builder.ToString();
    }

    private static string RenderJson(DependencyGraphDocument graph)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        writer.WriteStartObject();
        writer.WriteStartArray("nodes");
        foreach (var node in graph.Nodes)
        {
            writer.WriteStartObject();
            writer.WriteString("id", node.Id);
            writer.WriteString("name", node.Name);
            writer.WriteString("fullName", node.FullName);
            writer.WriteString("category", node.Category);
            if (node.Skipped.HasValue)
            {
                writer.WriteBoolean("skipped", node.Skipped.Value);
            }
            else
            {
                writer.WriteNull("skipped");
            }
            writer.WriteString("skipReason", node.SkipReason);
            writer.WriteString("estimatedDuration", node.EstimatedDuration.ToString("c"));
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteStartArray("edges");
        foreach (var edge in graph.Edges)
        {
            writer.WriteStartObject();
            writer.WriteString("from", edge.From);
            writer.WriteString("to", edge.To);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static string BuildLabel(
        DependencyGraphNode node,
        string separator,
        Func<string, string> escape)
    {
        var parts = new List<string> { node.Name };
        if (!string.IsNullOrWhiteSpace(node.Category))
        {
            parts.Add($"Category: {node.Category}");
        }

        if (node.EstimatedDuration > TimeSpan.Zero)
        {
            parts.Add($"Estimated: {node.EstimatedDuration:c}");
        }

        if (node.Skipped is true)
        {
            parts.Add(string.IsNullOrWhiteSpace(node.SkipReason)
                ? "Skipped"
                : $"Skipped: {node.SkipReason}");
        }
        else if (node.Skipped is null)
        {
            parts.Add("Skip status: unresolved");
        }

        return string.Join(separator, parts.Select(escape));
    }

    private static string EscapeMermaid(string value) =>
        value.Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("\"", "&quot;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal)
            .Replace(">", "&gt;", StringComparison.Ordinal)
            .Replace("`", "&#96;", StringComparison.Ordinal)
            .Replace("\r\n", "<br/>", StringComparison.Ordinal)
            .Replace("\r", "<br/>", StringComparison.Ordinal)
            .Replace("\n", "<br/>", StringComparison.Ordinal);

    private static string EscapeDot(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\r\n", "\\n", StringComparison.Ordinal)
            .Replace("\r", "\\n", StringComparison.Ordinal)
            .Replace("\n", "\\n", StringComparison.Ordinal)
            .Replace("\"", "\\\"", StringComparison.Ordinal);

    private static string GetModuleFullName(IModule module) =>
        module.GetType().FullName ?? module.GetType().Name;

    private sealed record DependencyGraphDocument(
        IReadOnlyList<DependencyGraphNode> Nodes,
        IReadOnlyList<DependencyGraphEdge> Edges);

    private sealed record DependencyGraphNode(
        string Id,
        string Name,
        string FullName,
        string? Category,
        bool? Skipped,
        string? SkipReason,
        TimeSpan EstimatedDuration);

    private sealed record DependencyGraphEdge(string From, string To);

    private sealed record RunConditionResolution(
        OrganizedModules OrganizedModules,
        IReadOnlySet<Type> UnresolvedSkipDecisionTypes);

    private sealed record RunConditionSkipResolution(
        OrganizedModules OrganizedModules,
        IReadOnlySet<Type> SkippedModuleTypes);

    private sealed record GraphNodeExecutionState(bool? Skipped, string? SkipReason);
}
