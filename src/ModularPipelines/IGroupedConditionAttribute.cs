namespace ModularPipelines;

/// <summary>
/// Identifies condition attributes whose repeated alternatives should be evaluated as one group.
/// </summary>
public interface IGroupedConditionAttribute : IConditionAttribute
{
    /// <summary>
    /// Gets the type used to group alternative condition attributes.
    /// </summary>
    Type ConditionGroupType { get; }
}
