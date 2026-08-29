using System.Text;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class PipelineRunReportFactory(
    ICommandExecutionCounter commandExecutionCounter,
    ISecretObfuscator secretObfuscator,
    IModuleOutputExcerptProvider? outputExcerptProvider = null,
    IOptions<PipelineOptions>? pipelineOptions = null,
    ISecretProvider? secretProvider = null)
{
    private static readonly Encoding Utf8 = new UTF8Encoding(
        encoderShouldEmitUTF8Identifier: false,
        throwOnInvalidBytes: false);

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
            .Where(result => uniqueModuleNames.Contains(result.Name))
            .GroupBy(static result => result.Name, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
        var timelinesByType = summary.ModuleTimelines?
            .ToFirstByKeyDictionary(
                static timeline => string.IsNullOrWhiteSpace(timeline.RuntimeModuleTypeName)
                    ? timeline.ModuleTypeName
                    : timeline.RuntimeModuleTypeName)
            ?? new Dictionary<string, ModuleTimeline>(StringComparer.Ordinal);
        var previousByType = previousReport?.Modules
            .ToUniqueByKeyDictionary(static module => module.ModuleTypeName)
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
        var status = pipelineException is null ? summary.Status : ModuleStatus.Failed;
        var previousTotalDuration = GetPreviousTotalDuration(status, previousReport);

        return new PipelineRunReport
        {
            RunId = runId ?? Guid.NewGuid().ToString("N"),
            PipelineIdentity = pipelineIdentity,
            Status = status,
            Start = summary.Start,
            End = summary.End,
            TotalDuration = summary.TotalDuration,
            PreviousEnd = GetPreviousEnd(previousReport, previousTotalDuration, modules),
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

    private static TimeSpan? GetPreviousTotalDuration(ModuleStatus status, PipelineRunReport? previousReport) =>
        status == ModuleStatus.Succeeded && previousReport?.Status == ModuleStatus.Succeeded
            ? previousReport.TotalDuration
            : null;

    private static DateTimeOffset? GetPreviousEnd(
        PipelineRunReport? previousReport,
        TimeSpan? previousTotalDuration,
        IReadOnlyCollection<ModuleRunReport> modules) =>
        previousTotalDuration.HasValue || modules.Any(static module => module.DurationDelta.HasValue)
            ? previousReport?.End
            : null;

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
            Output = CreateOutputExcerpt(moduleType),
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
        var status = result?.Status ?? timeline?.Status ?? ModuleStatus.Unknown;
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
            result?.Duration ?? TimeSpan.Zero,
            DurationMeasured: false,
            Start: null,
            End: null);
    }

    private static TimeSpan? GetPreviousDuration(
        ModuleStatus status,
        bool durationMeasured,
        ModuleRunReport? previous) =>
        status == ModuleStatus.Succeeded
        && durationMeasured
        && previous is { Status: ModuleStatus.Succeeded, DurationMeasured: true }
            ? previous.Duration
            : null;

    private string? GetSkipReason(IModuleResult? result) =>
        result?.SkipDecisionOrDefault?.Reason is { } skipReason
            ? secretObfuscator.Obfuscate(skipReason, null)
            : null;

    private ModuleOutputExcerpt? CreateOutputExcerpt(Type moduleType)
    {
        var excerpt = outputExcerptProvider?.GetModuleOutputExcerpt(moduleType);
        if (excerpt is null)
        {
            return null;
        }

        if (excerpt.SecretPatternsVersion is { } version)
        {
            var currentVersion = secretProvider?.Version;
            // Detect a pattern change while the already-masked excerpt is handed off.
            return currentVersion == version
                   && secretProvider?.Version == currentVersion
                ? excerpt
                : null;
        }

        var secretPatternsVersion = secretProvider?.Version;
        var maskedStdout = ObfuscateOptional(excerpt.StdoutTail);
        var maskedStderr = ObfuscateOptional(excerpt.StderrTail);
        var maximumBytes = pipelineOptions?.Value.RunReport.MaxOutputBytesPerModule ?? int.MaxValue;
        var stdoutBudget = Math.Min(GetByteCount(excerpt.StdoutTail), maximumBytes);
        var stderrBudget = Math.Min(GetByteCount(excerpt.StderrTail), maximumBytes - stdoutBudget);
        var stdoutTail = GetUtf8Tail(maskedStdout, stdoutBudget);
        var stderrTail = GetUtf8Tail(maskedStderr, stderrBudget);
        var additionallyTruncatedBytes = Math.Max(
            0,
            GetByteCount(maskedStdout)
            + GetByteCount(maskedStderr)
            - GetByteCount(stdoutTail)
            - GetByteCount(stderrTail));

        var finalExcerpt = excerpt with
        {
            StdoutTail = stdoutTail,
            StderrTail = stderrTail,
            TruncatedBytes = excerpt.TruncatedBytes + additionallyTruncatedBytes,
            SecretPatternsVersion = secretPatternsVersion,
        };

        return secretProvider?.Version == secretPatternsVersion ? finalExcerpt : null;
    }

    internal PipelineRunReport RemoveStaleOutputExcerpts(PipelineRunReport report)
    {
        if (secretProvider is null || report.Modules.All(static module => module.Output is null))
        {
            return report;
        }

        var currentVersion = secretProvider.Version;
        var modules = report.Modules
            .Select(module => module.Output?.SecretPatternsVersion == currentVersion
                ? module
                : module with { Output = null })
            .ToArray();

        if (secretProvider.Version != currentVersion)
        {
            modules = modules
                .Select(static module => module with { Output = null })
                .ToArray();
        }

        return report with { Modules = modules };
    }

    internal PipelineRunReport RemoveOutputExcerpts(PipelineRunReport report) =>
        report with
        {
            Modules = report.Modules
                .Select(static module => module with { Output = null })
                .ToArray(),
        };

    internal string SerializeWithValidatedOutputExcerpts(PipelineRunReport report) =>
        CreateValidatedSerialization(report).Contents;

    internal Task WriteWithValidatedOutputExcerptsAsync(
        string path,
        PipelineRunReport report,
        CancellationToken cancellationToken = default) =>
        WriteWithValidatedOutputExcerptsAsync(
            path,
            report,
            static (temporaryPath, contents, token) =>
                File.WriteAllTextAsync(temporaryPath, contents, token),
            cancellationToken);

    internal Task WriteWithValidatedOutputExcerptsAsync(
        string path,
        PipelineRunReport report,
        Func<string, string, CancellationToken, Task> writeAsync,
        CancellationToken cancellationToken = default)
    {
        var serialization = CreateValidatedSerialization(report);
        return AtomicFileWriter.WriteAllTextAsync(
            path,
            serialization.Contents,
            writeAsync,
            () => serialization.SecretPatternsVersion is { } version
                  && secretProvider?.Version != version
                ? RunReportJsonSerializer.Serialize(RemoveOutputExcerpts(report))
                : null,
            serialization.SecretPatternsVersion is { } version
                ? action => secretProvider?.TryExecuteIfVersionCurrent(version, action) == true
                : null,
            cancellationToken);
    }

    private ValidatedSerialization CreateValidatedSerialization(PipelineRunReport report)
    {
        var validatedReport = RemoveStaleOutputExcerpts(report);
        var serializedReport = RunReportJsonSerializer.Serialize(validatedReport);
        var outputExcerptCount = validatedReport.Modules.Count(static module => module.Output is not null);
        if (outputExcerptCount == 0)
        {
            return new ValidatedSerialization(serializedReport, null);
        }

        var revalidatedReport = RemoveStaleOutputExcerpts(validatedReport);
        var excerptsRemainCurrent = revalidatedReport.Modules.Count(static module => module.Output is not null)
                                    == outputExcerptCount;
        return new ValidatedSerialization(
            excerptsRemainCurrent
                ? serializedReport
                : RunReportJsonSerializer.Serialize(revalidatedReport),
            revalidatedReport.Modules
                .Select(static module => module.Output?.SecretPatternsVersion)
                .FirstOrDefault(static version => version is not null));
    }

    private sealed record ValidatedSerialization(string Contents, long? SecretPatternsVersion);

    private static int GetByteCount(string? value) => Utf8.GetByteCount(value ?? string.Empty);

    private static string? GetUtf8Tail(string? value, int maximumBytes) =>
        value is null ? null : ModuleOutputExcerptBuffer.GetUtf8Tail(value, maximumBytes);

    private string? ObfuscateOptional(string? value) =>
        value is null ? null : secretObfuscator.Obfuscate(value, null);

    private static string? GetResultTypeName(IModuleResult result) =>
        result is ModuleResult { ModuleType: { } moduleType }
            ? ModuleTypeIdentifier.Get(moduleType)
            : result.TypeName;

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
        ModuleStatus Status,
        TimeSpan Duration,
        bool DurationMeasured,
        DateTimeOffset? Start,
        DateTimeOffset? End);
}
