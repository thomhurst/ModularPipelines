namespace ModularPipelines;

/// <summary>
/// Specifies how the members of a <see cref="ConditionGroup"/> are combined.
/// </summary>
public enum ConditionLogic
{
    /// <summary>
    /// All conditions must return true (AND logic).
    /// </summary>
    All,

    /// <summary>
    /// At least one condition must return true (OR logic).
    /// </summary>
    Any,
}
