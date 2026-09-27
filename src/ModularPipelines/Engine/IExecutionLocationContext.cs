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
    /// Records the module's condition formula as evaluated by the master while preparing routing.
    /// </summary>
    void SetPreparedConditionValue(IModule module, FormulaValue value);

    bool TryGetPreparedConditionValue(IModule module, out FormulaValue value);

    IReadOnlyList<string> GetSatisfiedConditionGroupNames(IModule module);

    void RestoreSatisfiedConditionGroups(IModule module, IReadOnlyCollection<string> groupNames);
}
