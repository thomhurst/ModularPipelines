using System.Diagnostics;

namespace ModularPipelines.Tracing;

/// <summary>
/// Provides Activity-based tracing for module execution.
/// </summary>
/// <remarks>
/// This class provides distributed tracing support using System.Diagnostics.Activity,
/// enabling integration with OpenTelemetry and other APM tools.
///
/// Phase 1 (current): Foundation - provides ActivitySource for module execution tracing
/// alongside existing AsyncLocal pattern for backward compatibility.
///
/// Future phases will gradually transition ambient context from AsyncLocal to Activity.
/// </remarks>
public static class ModuleActivityTracing
{
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

    /// <summary>
    /// The ActivitySource for ModularPipelines module execution.
    /// </summary>
    /// <remarks>
    /// Listeners can subscribe to this source to receive module execution spans.
    /// The source name follows the recommended namespace-based naming convention.
    /// </remarks>
    public static readonly ActivitySource Source = new(
        name: "ModularPipelines.Modules",
        version: "1.0.0");

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
        if (activity is null)
        {
            return;
        }

        activity.SetTag(ModuleStatusTag, "Failed");
        activity.SetTag(ExceptionTypeTag, exception.GetType().FullName);
        activity.SetTag(ExceptionMessageTag, exception.Message);
        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
    }
}
