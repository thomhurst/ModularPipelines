namespace ModularPipelines;

/// <summary>
/// Specifies what a satisfied <see cref="RunConditionAttribute"/> means for its module.
/// </summary>
public enum ConditionIntent
{
    /// <summary>
    /// The module runs only when the condition is satisfied.
    /// </summary>
    Run,

    /// <summary>
    /// The module is skipped when the condition is satisfied.
    /// </summary>
    Skip,
}
