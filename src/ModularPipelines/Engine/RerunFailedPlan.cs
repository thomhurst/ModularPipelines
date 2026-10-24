using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

internal sealed class RerunFailedPlan(
    IOptions<PipelineOptions> options,
    IRunHistoryStore historyStore,
    IDependencyChainProvider dependencyChainProvider,
    IModuleResultHistoryProvider resultHistoryProvider,
    IPipelineContextProvider contextProvider,
    IExecutionLocationContext executionLocation,
    ILogger<RerunFailedPlan> logger,
    TimeProvider timeProvider)
{
    private readonly Dictionary<Type, IModuleResult> _results = [];

    public async Task InitializeAsync(OrganizedModules modules, CancellationToken cancellationToken)
    {
        if (!options.Value.RerunFailed)
        {
            return;
        }

        if (executionLocation.IsMaster || executionLocation.IsWorker)
        {
            throw new NotSupportedException("--rerun-failed currently requires in-process pipeline execution.");
        }

        if (options.Value.DisableModuleCache || options.Value.RunReport.HistoryRetention <= 0)
        {
            return;
        }

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30), timeProvider);
        using var readCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        try
        {
            var report = await FindCompatibleReportAsync(modules.AllModules, readCancellation.Token)
                .WaitAsync(readCancellation.Token).ConfigureAwait(false);
            if (report is null)
            {
                logger.LogInformation("No compatible previous run found; executing selected modules normally");
                return;
            }

            await RestoreSuccessfulResultsAsync(modules, report, readCancellation.Token).ConfigureAwait(false);
            InvalidateDependents();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            _results.Clear();
            logger.LogWarning(exception, "Could not prepare failed-module rerun; executing selected modules normally");
        }
    }

    public ModuleResult<T>? GetResult<T>(Module<T> module) =>
        _results.GetValueOrDefault(module.GetType()) as ModuleResult<T>.Success;

    private async Task RestoreSuccessfulResultsAsync(
        OrganizedModules modules,
        PipelineRunReport report,
        CancellationToken cancellationToken)
    {
        var previousModules = report.Modules.ToDictionary(static module => module.ModuleId);
        var context = contextProvider.GetModuleContext();
        foreach (var runnable in modules.RunnableModules)
        {
            var module = runnable.Module;
            var previous = previousModules[ModuleId.FromType(module.GetType())];
            if (!IsSuccessful(previous.Status))
            {
                continue;
            }

            var result = await resultHistoryProvider.TryGetAsync(module, context, cancellationToken)
                .WaitAsync(cancellationToken).ConfigureAwait(false);
            if (result is ModuleResult { HasSuccessfulValue: true } storedResult
                && IsSuccessful(result.Status)
                && result.StartTime == previous.ResultStartTime
                && result.EndTime == previous.ResultEndTime
                && MatchesModule(storedResult, module.GetType()))
            {
                _results.Add(module.GetType(), result);
            }
        }
    }

    private void InvalidateDependents()
    {
        // Any module that must execute invalidates its dependents, even when it succeeded last time.
        var pending = new Queue<ModuleDependencyModel>(dependencyChainProvider.ModuleDependencyModels
            .Where(model => !_results.ContainsKey(model.Module.GetType())));
        var visited = new HashSet<ModuleDependencyModel>();
        while (pending.TryDequeue(out var model))
        {
            if (!visited.Add(model))
            {
                continue;
            }

            _results.Remove(model.Module.GetType());
            foreach (var dependent in model.IsDependencyFor)
            {
                pending.Enqueue(dependent);
            }
        }
    }

    private async Task<PipelineRunReport?> FindCompatibleReportAsync(
        IReadOnlyList<IModule> modules,
        CancellationToken cancellationToken)
    {
        var identity = PipelineRunCompatibility.GetPipelineIdentity(options.Value.RunReport.PipelineIdentity, modules);
        var fingerprint = PipelineRunCompatibility.GetGraphFingerprint(dependencyChainProvider.ModuleDependencyModels);
        var moduleIds = modules.Select(static module => ModuleId.FromType(module.GetType())).ToHashSet();
        await foreach (var report in historyStore.GetRunsAsync(
                           new RunHistoryQuery { PipelineIdentity = identity, MaxRuns = int.MaxValue }, cancellationToken)
                           .ConfigureAwait(false))
        {
            if (report.SchemaVersion != PipelineRunReport.CurrentSchemaVersion
                || !string.Equals(report.PipelineIdentity, identity, StringComparison.Ordinal)
                || !string.Equals(report.GraphFingerprint, fingerprint, StringComparison.Ordinal)
                || report.Modules.Count != modules.Count
                || report.Modules.Select(static module => module.ModuleId).Distinct().Count() != modules.Count
                || !moduleIds.SetEquals(report.Modules.Select(static module => module.ModuleId)))
            {
                continue;
            }

            return report;
        }

        return null;
    }

    private static bool MatchesModule(ModuleResult result, Type moduleType) =>
        result.ModuleType is { } storedType
            ? storedType == moduleType
            : string.Equals(result.TypeName, moduleType.FullName, StringComparison.Ordinal)
              || string.Equals(result.TypeName, moduleType.AssemblyQualifiedName, StringComparison.Ordinal)
              || string.Equals(result.TypeName, ModuleTypeIdentifier.Get(moduleType), StringComparison.Ordinal);

    private static bool IsSuccessful(ModuleStatus status) =>
        status is ModuleStatus.Succeeded or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache;
}
