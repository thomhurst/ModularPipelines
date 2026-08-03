using System.Diagnostics;
using System.Diagnostics.Metrics;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.Tracing;

/// <summary>
/// Provides tracing and metrics for pipeline, module, and command execution.
/// </summary>
/// <remarks>
/// This class provides distributed tracing support using System.Diagnostics.Activity,
/// enabling integration with OpenTelemetry and other APM tools.
///
/// Instrumentation uses only BCL <see cref="ActivitySource"/> and <see cref="Meter"/> APIs.
/// Consumers can subscribe directly or use the optional ModularPipelines.OpenTelemetry package.
/// </remarks>
public static class ModuleActivityTracing
{
    public const string PipelineSourceName = "ModularPipelines";
    public const string ModuleSourceName = "ModularPipelines.Modules";
    public const string CommandSourceName = "ModularPipelines.Commands";
    public const string MeterName = "ModularPipelines";

    public const string PipelineNameTag = "modular_pipelines.pipeline.name";
    public const string PipelineStatusTag = "modular_pipelines.pipeline.status";

    /// <summary>
    /// Tag key for the module type name.
    /// </summary>
    public const string ModuleTypeTag = "modular_pipelines.module.type";

    /// <summary>
    /// Tag key for the module type's full name (including namespace).
    /// </summary>
    public const string ModuleTypeFullNameTag = "modular_pipelines.module.type_full";

    /// <summary>
    /// Tag key for the module execution status.
    /// </summary>
    public const string ModuleStatusTag = "modular_pipelines.module.status";

    /// <summary>
    /// Tag key for exception type when a module fails.
    /// </summary>
    public const string ExceptionTypeTag = "exception.type";

    /// <summary>
    /// Tag key for exception message when a module fails.
    /// </summary>
    public const string ExceptionMessageTag = "exception.message";

    public const string CommandToolTag = "process.executable.name";
    public const string CommandInputTag = "process.command_line";
    public const string CommandExitCodeTag = "process.exit.code";
    public const string CommandDurationTag = "modular_pipelines.command.duration_ms";

    public const string ModuleDurationMetric = "modular_pipelines.module.duration";
    public const string ModulesFailedMetric = "modular_pipelines.modules.failed";
    public const string ModuleRetriesMetric = "modular_pipelines.module.retries";

    public static readonly Meter TelemetryMeter = new(MeterName, "1.0.0");

    private static readonly Histogram<double> ModuleDuration = TelemetryMeter.CreateHistogram<double>(
        ModuleDurationMetric,
        unit: "s",
        description: "Module execution duration in seconds.");

    private static readonly Counter<long> ModulesFailed = TelemetryMeter.CreateCounter<long>(
        ModulesFailedMetric,
        unit: "{module}",
        description: "Number of failed module executions.");

    private static readonly Counter<long> ModuleRetries = TelemetryMeter.CreateCounter<long>(
        ModuleRetriesMetric,
        unit: "{retry}",
        description: "Number of module retry attempts.");

    public static readonly ActivitySource PipelineSource = new(PipelineSourceName, "1.0.0");

    /// <summary>
    /// The ActivitySource for ModularPipelines module execution.
    /// </summary>
    /// <remarks>
    /// Listeners can subscribe to this source to receive module execution spans.
    /// The source name follows the recommended namespace-based naming convention.
    /// </remarks>
    public static readonly ActivitySource Source = new(
        name: ModuleSourceName,
        version: "1.0.0");

    public static readonly ActivitySource CommandSource = new(CommandSourceName, "1.0.0");

    internal static Activity? StartPipelineActivity(string? pipelineName)
    {
        var activity = PipelineSource.StartActivity("Pipeline.Run", ActivityKind.Internal);
        activity?.SetTag(
            PipelineNameTag,
            string.IsNullOrWhiteSpace(pipelineName) ? "ModularPipelines" : pipelineName);
        return activity;
    }

    internal static void RecordPipelineCompletion(Activity? activity, string status, bool failed)
    {
        activity?.SetTag(PipelineStatusTag, status);
        activity?.SetStatus(failed ? ActivityStatusCode.Error : ActivityStatusCode.Ok);
    }

    internal static void RecordPipelineFailure(
        Activity? activity,
        Exception exception,
        string obfuscatedMessage)
    {
        var status = exception switch
        {
            PipelineFailedException pipelineFailedException => pipelineFailedException.Summary.Status,
            PipelineCancelledException or OperationCanceledException => Status.PipelineTerminated,
            _ => Status.Failed,
        };
        activity?.SetTag(PipelineStatusTag, status.ToString());
        RecordException(activity, exception, obfuscatedMessage);
    }

    internal static Activity? StartCommandActivity(string tool)
    {
        var activity = CommandSource.StartActivity($"Command.{tool}", ActivityKind.Internal);
        activity?.SetTag(CommandToolTag, tool);
        return activity;
    }

    internal static void RecordCommandInput(Activity? activity, string obfuscatedCommandInput)
    {
        activity?.SetTag(CommandInputTag, obfuscatedCommandInput);
    }

    internal static void RecordCommandResult(Activity? activity, CommandResult result)
    {
        activity?.SetTag(CommandExitCodeTag, result.ExitCode);
        activity?.SetTag(CommandDurationTag, result.Duration.TotalMilliseconds);
        activity?.SetStatus(result.ExitCode == 0 ? ActivityStatusCode.Ok : ActivityStatusCode.Error);
    }

    internal static void RecordCommandFailure(
        Activity? activity,
        Exception exception,
        string obfuscatedMessage)
    {
        if (exception is CommandException commandException)
        {
            activity?.SetTag(CommandExitCodeTag, commandException.Result.ExitCode);
            activity?.SetTag(CommandDurationTag, commandException.Result.Duration.TotalMilliseconds);
        }

        RecordException(activity, exception, obfuscatedMessage);
    }

    internal static void RecordModuleMetrics(
        Type moduleType,
        string status,
        TimeSpan duration)
    {
        var tags = new TagList
        {
            { ModuleTypeTag, moduleType.Name },
            { ModuleTypeFullNameTag, moduleType.FullName },
            { ModuleStatusTag, status },
        };

        ModuleDuration.Record(duration.TotalSeconds, tags);
        if (status is "Failed" or "TimedOut")
        {
            ModulesFailed.Add(1, tags);
        }
    }

    internal static void RecordModuleRetries(Type moduleType, int retryCount)
    {
        if (retryCount <= 0)
        {
            return;
        }

        ModuleRetries.Add(
            retryCount,
            new TagList
            {
                { ModuleTypeTag, moduleType.Name },
                { ModuleTypeFullNameTag, moduleType.FullName },
            });
    }

    /// <summary>
    /// Starts a new Activity for module execution.
    /// </summary>
    /// <param name="moduleType">The type of the module being executed.</param>
    /// <returns>The started Activity, or null if no listeners are registered.</returns>
    public static Activity? StartModuleActivity(Type moduleType)
    {
        var activity = Source.StartActivity(
            name: $"Module.{moduleType.Name}",
            kind: ActivityKind.Internal);

        if (activity is not null)
        {
            activity.SetTag(ModuleTypeTag, moduleType.Name);
            activity.SetTag(ModuleTypeFullNameTag, moduleType.FullName);
        }

        return activity;
    }

    /// <summary>
    /// Records a successful module completion on the activity.
    /// </summary>
    /// <param name="activity">The activity to update.</param>
    public static void RecordSuccess(Activity? activity)
    {
        activity?.SetTag(ModuleStatusTag, "Successful");
        activity?.SetStatus(ActivityStatusCode.Ok);
    }

    internal static void RecordUsedHistory(Activity? activity)
    {
        activity?.SetTag(ModuleStatusTag, "UsedHistory");
        activity?.SetStatus(ActivityStatusCode.Ok, "Module result restored from history");
    }

    internal static void RecordPipelineTerminated(Activity? activity)
    {
        activity?.SetTag(ModuleStatusTag, "PipelineTerminated");
        activity?.SetStatus(ActivityStatusCode.Error, "Module terminated because the pipeline failed");
    }

    internal static void RecordTimedOut(
        Activity? activity,
        Exception exception,
        string obfuscatedMessage)
    {
        activity?.SetTag(ModuleStatusTag, "TimedOut");
        RecordException(activity, exception, obfuscatedMessage);
    }

    /// <summary>
    /// Records a skipped module on the activity.
    /// </summary>
    /// <param name="activity">The activity to update.</param>
    public static void RecordSkipped(Activity? activity)
    {
        activity?.SetTag(ModuleStatusTag, "Skipped");
        activity?.SetStatus(ActivityStatusCode.Ok, "Module was skipped");
    }

    /// <summary>
    /// Records a failed module execution on the activity.
    /// </summary>
    /// <param name="activity">The activity to update.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    public static void RecordFailure(Activity? activity, Exception exception)
    {
        RecordFailure(activity, exception, "Module execution failed");
    }

    internal static void RecordFailure(
        Activity? activity,
        Exception exception,
        string obfuscatedMessage)
    {
        activity?.SetTag(ModuleStatusTag, "Failed");
        RecordException(activity, exception, obfuscatedMessage);
    }

    /// <summary>
    /// Gets the current module name from the current Activity or AsyncLocal context.
    /// </summary>
    /// <returns>The module name, or null if no module context is available.</returns>
    /// <remarks>
    /// This method first checks the Activity (for OpenTelemetry integration), then falls back
    /// to the AsyncLocal context (AmbientModuleContext) which is always available during module execution.
    /// </remarks>
    public static string? GetCurrentModuleName()
    {
        // First try Activity (for OpenTelemetry integration)
        var activityModuleName = Activity.Current?.GetTagItem(ModuleTypeTag) as string;
        if (activityModuleName != null)
        {
            return activityModuleName;
        }

        // Fall back to AsyncLocal context (always available during module execution)
        return AmbientModuleContext.CurrentModuleName;
    }

    /// <summary>
    /// Gets the current Activity ID, if available.
    /// </summary>
    /// <returns>The activity ID, or null if no activity is active.</returns>
    public static string? GetCurrentActivityId()
    {
        return Activity.Current?.Id;
    }

    private static void RecordException(
        Activity? activity,
        Exception exception,
        string obfuscatedMessage)
    {
        activity?.SetTag(ExceptionTypeTag, exception.GetType().FullName);
        activity?.SetTag(ExceptionMessageTag, obfuscatedMessage);
        activity?.SetStatus(ActivityStatusCode.Error, obfuscatedMessage);
    }
}
