namespace ModularPipelines.Engine;

/// <summary>
/// Cached metadata for module behavior interfaces.
/// </summary>
/// <remarks>
/// This caches the result of runtime type checks for behavior interfaces
/// to avoid repeated <c>is</c> checks during module execution.
///
/// Behavior interfaces:
/// - ISkippable: Module can be skipped based on conditions
/// - IHookable: Module has before/after execution hooks
/// - ITimeoutable: Module has a timeout configuration
/// - IRetryable: Module has a retry policy
/// - IIgnoreFailures: Module failures are non-fatal
/// - IAlwaysRun: Module runs regardless of pipeline cancellation
/// </remarks>
internal sealed record ModuleBehaviorMetadata
{
    /// <summary>
    /// Gets a value indicating whether the module implements ISkippable.
    /// </summary>
    public bool IsSkippable { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module implements IHookable.
    /// </summary>
    public bool IsHookable { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module implements ITimeoutable.
    /// </summary>
    public bool IsTimeoutable { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module implements IRetryable.
    /// </summary>
    public bool IsRetryable { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module implements IIgnoreFailures.
    /// </summary>
    public bool IsIgnoreFailures { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module implements IAlwaysRun.
    /// </summary>
    public bool IsAlwaysRun { get; init; }

    /// <summary>
    /// Creates behavior metadata from a module type by checking implemented interfaces.
    /// </summary>
    /// <param name="moduleType">The module type to analyze.</param>
    /// <returns>Metadata containing cached behavior flags.</returns>
    public static ModuleBehaviorMetadata FromType(Type moduleType)
    {
        var interfaces = moduleType.GetInterfaces();
        var interfaceSet = new HashSet<string>(interfaces.Select(i => i.Name));

        return new ModuleBehaviorMetadata
        {
            IsSkippable = interfaceSet.Contains("ISkippable"),
            IsHookable = interfaceSet.Contains("IHookable"),
            IsTimeoutable = interfaceSet.Contains("ITimeoutable"),
            IsRetryable = interfaces.Any(i => i.IsGenericType && i.Name.StartsWith("IRetryable`")),
            IsIgnoreFailures = interfaceSet.Contains("IIgnoreFailures"),
            IsAlwaysRun = interfaceSet.Contains("IAlwaysRun"),
        };
    }
}
