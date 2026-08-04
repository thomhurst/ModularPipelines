using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal sealed class PipelineRunReportFactory(
    ICommandExecutionCounter commandExecutionCounter,
    ISecretObfuscator secretObfuscator)
{
    public PipelineRunReport Create(
        PipelineSummary summary,
        PipelineRunReport? previousReport,
        string pipelineIdentity,
        Exception? pipelineException = null,
        string? runId = null)
    {
        var uniqueModuleTypeNames = summary.Modules
            .GroupBy(static module => ModuleTypeIdentifier.Get(module.GetType()), StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .Select(static group => group.Key)
            .ToHashSet(StringComparer.Ordinal);
        var resultsByRuntimeType = summary.Results
            .OfType<ModuleResult>()
            .Where(static result => result.ModuleType is not null)
            .GroupBy(static result => result.ModuleType!)
            .ToDictionary(static group => group.Key, static group => (IModuleResult) group.First());
        var resultsByType = summary.Results
            .Select(static result => (Result: result, TypeName: GetResultTypeName(result)))
            .Where(item => item.TypeName is not null && uniqueModuleTypeNames.Contains(item.TypeName))
            .GroupBy(static item => item.TypeName!, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.First().Result,
                StringComparer.Ordinal);
        var uniqueModuleNames = summary.Modules
            .GroupBy(static module => module.GetType().Name, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .Select(static group => group.Key)
            .ToHashSet(StringComparer.Ordinal);
        var resultsByName = summary.Results
            .Where(result => uniqueModuleNames.Contains(result.ModuleName))
            .GroupBy(static result => result.ModuleName, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
        var timelinesByType = summary.ModuleTimelines?
            .GroupBy(
                static timeline => string.IsNullOrWhiteSpace(timeline.RuntimeModuleTypeName)
                    ? timeline.ModuleTypeName
                    : timeline.RuntimeModuleTypeName,
                StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal)
            ?? new Dictionary<string, ModuleTimeline>(StringComparer.Ordinal);
        var previousByType = previousReport?.Modules
            .GroupBy(static module => module.ModuleTypeName, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .ToDictionary(
                static group => group.Key,
                static group => group.First(),
                StringComparer.Ordinal)
            ?? new Dictionary<string, ModuleRunReport>(StringComparer.Ordinal);

        var modules = summary.Modules
            .Select(module => CreateModuleReport(
                module.GetType(),
                resultsByRuntimeType,
                resultsByType,
                resultsByName,
                timelinesByType,
                uniqueModuleTypeNames,
                previousByType))
            .ToArray();
        var status = pipelineException is null ? summary.Status : Status.Failed;
        TimeSpan? previousTotalDuration = status == Status.Successful
                                          && previousReport?.Status == Status.Successful
            ? previousReport.TotalDuration
            : null;

        return new PipelineRunReport
        {
            RunId = runId ?? Guid.NewGuid().ToString("N"),
            PipelineIdentity = pipelineIdentity,
            Status = status,
            Start = summary.Start,
            End = summary.End,
            TotalDuration = summary.TotalDuration,
            PreviousTotalDuration = previousTotalDuration,
            TotalDurationDelta = previousTotalDuration is null
                ? null
                : summary.TotalDuration - previousTotalDuration.Value,
            Metrics = summary.Metrics,
            Exception = CreateExceptionDetails(pipelineException),
            Modules = modules,
            CommandCount = commandExecutionCounter.TotalCount,
            UnattributedCommandCount = commandExecutionCounter.UnattributedCount,
        };
    }

    public PipelineRunReport WithCorrelation(
        PipelineRunReport report,
        RunReportEnrichmentContext context) =>
        report with
        {
            Correlation = new RunCorrelation
            {
                GitSha = Obfuscate(context.GitSha),
                GitBranch = Obfuscate(context.GitBranch),
                Hostname = Obfuscate(context.Hostname),
                CiRunUrl = Obfuscate(context.CiRunUrl),
                BuildSystem = context.BuildSystem,
            },
        };

    private string? Obfuscate(string? value) =>
        value is null ? null : secretObfuscator.Obfuscate(value, null);

    private ModuleRunReport CreateModuleReport(
        Type moduleType,
        IReadOnlyDictionary<Type, IModuleResult> resultsByRuntimeType,
        IReadOnlyDictionary<string, IModuleResult> resultsByType,
        IReadOnlyDictionary<string, IModuleResult> resultsByName,
        IReadOnlyDictionary<string, ModuleTimeline> timelinesByType,
        IReadOnlySet<string> uniqueModuleTypeNames,
        IReadOnlyDictionary<string, ModuleRunReport> previousByType)
    {
        var typeName = ModuleTypeIdentifier.Get(moduleType);
        var runtimeTypeName = ModuleTypeIdentifier.GetRuntime(moduleType);
        var result = GetResult(
            moduleType,
            typeName,
            resultsByRuntimeType,
            resultsByType,
            resultsByName);
        var timeline = timelinesByType.GetValueOrDefault(runtimeTypeName);
        if (timeline is null && uniqueModuleTypeNames.Contains(typeName))
        {
            timeline = timelinesByType.GetValueOrDefault(typeName);
        }
        var previous = uniqueModuleTypeNames.Contains(typeName)
            ? previousByType.GetValueOrDefault(typeName)
            : null;
        var current = CreateCurrentReportValues(result, timeline);
        var previousDuration = GetPreviousDuration(current.Status, current.DurationMeasured, previous);
        return new ModuleRunReport
        {
            ModuleName = moduleType.Name,
            ModuleTypeName = typeName,
            Status = current.Status,
            Duration = current.Duration,
            DurationMeasured = current.DurationMeasured,
            Start = current.Start,
            End = current.End,
            SkipReason = GetSkipReason(result),
            Exception = CreateExceptionDetails(result?.ExceptionOrDefault),
            CommandCount = commandExecutionCounter.GetCount(moduleType),
            PreviousDuration = previousDuration,
            DurationDelta = previousDuration.HasValue
                ? current.Duration - previousDuration.Value
                : null,
        };
    }

    private static IModuleResult? GetResult(
        Type moduleType,
        string typeName,
        IReadOnlyDictionary<Type, IModuleResult> resultsByRuntimeType,
        IReadOnlyDictionary<string, IModuleResult> resultsByType,
        IReadOnlyDictionary<string, IModuleResult> resultsByName)
    {
        return resultsByRuntimeType.TryGetValue(moduleType, out var runtimeResult)
            ? runtimeResult
            : resultsByType.TryGetValue(typeName, out var result)
            ? result
            : resultsByName.GetValueOrDefault(moduleType.Name);
    }

    private static CurrentModuleReportValues CreateCurrentReportValues(
        IModuleResult? result,
        ModuleTimeline? timeline)
    {
        var status = result?.ModuleStatus ?? timeline?.Status ?? Status.Unknown;
        if (timeline is { StartTime: { } start, EndTime: { } end })
        {
            return new CurrentModuleReportValues(
                status,
                end - start,
                DurationMeasured: true,
                start,
                end);
        }

        return new CurrentModuleReportValues(
            status,
            result?.ModuleDuration ?? TimeSpan.Zero,
            DurationMeasured: false,
            Start: null,
            End: null);
    }

    private static TimeSpan? GetPreviousDuration(
        Status status,
        bool durationMeasured,
        ModuleRunReport? previous) =>
        status == Status.Successful
        && durationMeasured
        && previous is { Status: Status.Successful, DurationMeasured: true }
            ? previous.Duration
            : null;

    private string? GetSkipReason(IModuleResult? result) =>
        result?.SkipDecisionOrDefault?.Reason is { } skipReason
            ? secretObfuscator.Obfuscate(skipReason, null)
            : null;

    private static string? GetResultTypeName(IModuleResult result) =>
        result is ModuleResult { ModuleType: { } moduleType }
            ? ModuleTypeIdentifier.Get(moduleType)
            : result.ModuleTypeName;

    public RunReportExceptionDetails? CreateExceptionDetails(Exception? exception)
    {
        var filteredException = exception as IFilteredRunReportException;
        return exception is null
            ? null
            : new RunReportExceptionDetails
            {
                Type = filteredException?.TypeName
                    ?? exception.GetType().FullName
                    ?? exception.GetType().Name,
                Message = secretObfuscator.Obfuscate(exception.Message, null),
                StackTrace = (filteredException?.OriginalStackTrace ?? exception.StackTrace) is not { } stackTrace
                    ? null
                    : secretObfuscator.Obfuscate(stackTrace, null),
                InnerException = CreateExceptionDetails(exception.InnerException),
                InnerExceptions = exception is AggregateException aggregateException
                    ? aggregateException.InnerExceptions
                        .Select(CreateExceptionDetails)
                        .OfType<RunReportExceptionDetails>()
                        .ToArray()
                    : [],
            };
    }

    private sealed record CurrentModuleReportValues(
        Status Status,
        TimeSpan Duration,
        bool DurationMeasured,
        DateTimeOffset? Start,
        DateTimeOffset? End);
}
