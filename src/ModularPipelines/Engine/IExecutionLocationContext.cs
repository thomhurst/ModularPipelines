using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IExecutionLocationContext
{
    bool IsMaster { get; }

    bool IsWorker { get; }

    bool ShouldDeferOperatingSystemConditions { get; }

    bool IsRoutingPrepared(IModule module);

    void MarkRoutingPrepared(IModule module);

    bool IsConditionGroupSatisfied(IModule module, Type conditionGroupType);

    void MarkConditionGroupSatisfied(IModule module, Type conditionGroupType);

    IReadOnlyList<string> GetSatisfiedConditionGroupNames(IModule module);

    void RestoreSatisfiedConditionGroups(IModule module, IReadOnlyCollection<string> groupNames);
}
