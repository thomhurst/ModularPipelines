namespace ModularPipelines.Tracing;

/// <summary>
/// Defines the stable activity-source, meter, tag, and instrument names emitted by ModularPipelines.
/// </summary>
public static class PipelineTelemetry
{
    /// <summary>Gets the pipeline activity source name.</summary>
    public const string PipelineSourceName = "ModularPipelines";

    /// <summary>Gets the module activity source name.</summary>
    public const string ModuleSourceName = "ModularPipelines.Modules";

    /// <summary>Gets the command activity source name.</summary>
    public const string CommandSourceName = "ModularPipelines.Commands";

    /// <summary>Gets the pipeline meter name.</summary>
    public const string MeterName = "ModularPipelines";

    /// <summary>Gets the pipeline-name tag key.</summary>
    public const string PipelineNameTag = "modular_pipelines.pipeline.name";

    /// <summary>Gets the pipeline-status tag key.</summary>
    public const string PipelineStatusTag = "modular_pipelines.pipeline.status";

    /// <summary>Gets the module-type tag key.</summary>
    public const string ModuleTypeTag = "modular_pipelines.module.type";

    /// <summary>Gets the fully qualified module-type tag key.</summary>
    public const string ModuleTypeFullNameTag = "modular_pipelines.module.type_full";

    /// <summary>Gets the module-status tag key.</summary>
    public const string ModuleStatusTag = "modular_pipelines.module.status";

    /// <summary>Gets the module-cache tag key.</summary>
    public const string ModuleCacheTag = "modular_pipelines.module.cache";

    /// <summary>Gets the exception-type tag key.</summary>
    public const string ExceptionTypeTag = "exception.type";

    /// <summary>Gets the exception-message tag key.</summary>
    public const string ExceptionMessageTag = "exception.message";

    /// <summary>Gets the command-tool tag key.</summary>
    public const string CommandToolTag = "process.executable.name";

    /// <summary>Gets the command-line tag key.</summary>
    public const string CommandInputTag = "process.command_line";

    /// <summary>Gets the command-exit-code tag key.</summary>
    public const string CommandExitCodeTag = "process.exit.code";

    /// <summary>Gets the command-duration tag key.</summary>
    public const string CommandDurationTag = "modular_pipelines.command.duration_ms";

    /// <summary>Gets the module-duration instrument name.</summary>
    public const string ModuleDurationMetric = "modular_pipelines.module.duration";

    /// <summary>Gets the failed-modules instrument name.</summary>
    public const string ModulesFailedMetric = "modular_pipelines.modules.failed";

    /// <summary>Gets the module-retries instrument name.</summary>
    public const string ModuleRetriesMetric = "modular_pipelines.module.retries";

    /// <summary>Gets the module-cache-hits instrument name.</summary>
    public const string ModuleCacheHitsMetric = "modular_pipelines.module.cache_hits";

    /// <summary>Gets the module-cache-misses instrument name.</summary>
    public const string ModuleCacheMissesMetric = "modular_pipelines.module.cache_misses";
}
