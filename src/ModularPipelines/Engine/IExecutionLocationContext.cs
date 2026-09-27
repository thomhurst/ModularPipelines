using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IExecutionLocationContext
{
    bool IsMaster { get; }

    bool IsWorker { get; }

    bool ShouldDeferCapabilityConditions { get; }

    bool IsRoutingPrepared(IModule module);

    void MarkRoutingPrepared(IModule module);

    bool IsConditionGroupSatisfied(IModule module, Type conditionGroupType);

    void MarkConditionGroupSatisfied(IModule module, Type conditionGroupType);

    /// <summary>
    /// Records that the master evaluated every non-capability alternative of a condition group as false,
    /// so the module must be routed to a worker that satisfies the group's capability alternatives.
    /// </summary>
    void MarkConditionalRouteRequired(IModule module, Type conditionGroupType);

    bool IsConditionalRouteRequired(IModule module, Type conditionGroupType);

    IReadOnlyList<string> GetSatisfiedConditionGroupNames(IModule module);

    void RestoreSatisfiedConditionGroups(IModule module, IReadOnlyCollection<string> groupNames);
}
