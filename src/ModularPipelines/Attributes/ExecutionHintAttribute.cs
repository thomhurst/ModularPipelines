using ModularPipelines.Enums;

namespace ModularPipelines.Attributes;

/// <summary>
/// Provides a hint about the resource usage characteristics of a module.
/// The scheduler uses this to apply appropriate concurrency limits.
/// </summary>
/// <example>
/// <code>
/// [ExecutionHint(ExecutionType.CpuIntensive)]
/// public class CompilationModule : Module&lt;BuildResult&gt;
/// {
///     // Limited by ConcurrencyOptions.MaxCpuIntensiveModules
/// }
///
/// [ExecutionHint(ExecutionType.IoIntensive)]
/// public class DownloadDependenciesModule : Module&lt;DownloadResult&gt;
/// {
///     // Limited by ConcurrencyOptions.MaxIoIntensiveModules
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ExecutionHintAttribute : Attribute
{
    /// <summary>
    /// Gets the execution type hint for the module.
    /// </summary>
    public ExecutionType ExecutionType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionHintAttribute"/> class.
    /// </summary>
    /// <param name="executionType">The execution type hint for the module.</param>
    public ExecutionHintAttribute(ExecutionType executionType)
    {
        ExecutionType = executionType;
    }
}
