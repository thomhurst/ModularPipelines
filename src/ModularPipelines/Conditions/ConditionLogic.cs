namespace ModularPipelines.Conditions;

/// <summary>
/// Specifies how multiple conditions within a group are combined.
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

    /// <summary>
    /// Used for skip conditions - if condition returns true, module is skipped.
    /// </summary>
    Skip
}
