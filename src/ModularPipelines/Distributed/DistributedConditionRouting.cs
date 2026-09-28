using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed;

internal sealed class DistributedConditionRouting(
    IOptions<DistributedOptions> options,
    RoleDetector roleDetector) : IExecutionLocationContext
{
    private readonly DistributedOptions _options = options.Value;
    private readonly RoleDetector _roleDetector = roleDetector;
    private readonly ConditionalWeakTable<IModule, HashSet<Type>> _locallySatisfiedGroups = [];
    private readonly ConditionalWeakTable<IModule, StrongBox<FormulaValue>> _preparedConditionValues = [];
    private readonly ConditionalWeakTable<IModule, object> _preparedModules = [];

    public bool IsMaster => IsDistributedExecution
                            && _roleDetector.DetectRole() == DistributedRole.Master;

    public bool IsWorker => IsDistributedExecution
                            && _roleDetector.DetectRole() == DistributedRole.Worker;

    // The role queries stay pure so run reporting and ignored-result handling always see the
    // real cross-process role. Only capability condition deferral is suppressed while
    // the master locally executes an assignment it already routed to itself; otherwise that
    // module would be deferred a second time.
    public bool ShouldDeferCapabilityConditions => IsMaster
                                                        && !DistributedAssignmentExecutionScope.IsActive;

    private bool IsDistributedExecution => _options.Enabled
                                           && _options.TotalInstances > 1;

    public bool IsRoutingPrepared(IModule module) => _preparedModules.TryGetValue(module, out _);

    public void MarkRoutingPrepared(IModule module) =>
        _preparedModules.GetValue(module, static _ => new object());

    public void MarkConditionGroupSatisfied(IModule module, Type conditionGroupType)
    {
        var groups = _locallySatisfiedGroups.GetOrCreateValue(module);
        lock (groups)
        {
            groups.Add(conditionGroupType);
        }
    }

    public bool IsConditionGroupSatisfied(IModule module, Type conditionGroupType)
    {
        if (!_locallySatisfiedGroups.TryGetValue(module, out var groups))
        {
            return false;
        }

        lock (groups)
        {
            return groups.Contains(conditionGroupType);
        }
    }

    public void SetPreparedConditionValue(IModule module, FormulaValue value) =>
        _preparedConditionValues.AddOrUpdate(module, new StrongBox<FormulaValue>(value));

    public bool TryGetPreparedConditionValue(IModule module, out FormulaValue value)
    {
        if (_preparedConditionValues.TryGetValue(module, out var box))
        {
            value = box.Value;
            return true;
        }

        value = default;
        return false;
    }

    public IReadOnlyList<string> GetSatisfiedConditionGroupNames(IModule module)
    {
        if (!_locallySatisfiedGroups.TryGetValue(module, out var groups))
        {
            return [];
        }

        lock (groups)
        {
            return groups
                .Select(GetGroupName)
                .Order(StringComparer.Ordinal)
                .ToArray();
        }
    }

    public void RestoreSatisfiedConditionGroups(
        IModule module,
        IReadOnlyCollection<string> groupNames)
    {
        if (groupNames.Count == 0)
        {
            return;
        }

        var groupsByName = module.GetType()
            .GetCustomAttributes(inherit: true)
            .OfType<IConditionAttribute>()
            .Select(static attribute => attribute is IGroupedConditionAttribute groupedAttribute
                ? groupedAttribute.ConditionGroupType
                : attribute.GetType())
            .Distinct()
            .ToDictionary(GetGroupName, StringComparer.Ordinal);
        foreach (var groupName in groupNames)
        {
            if (groupsByName.TryGetValue(groupName, out var groupType))
            {
                MarkConditionGroupSatisfied(module, groupType);
            }
        }
    }

    // Version-independent so a master and worker built from different package versions of the
    // same pipeline still agree on group names.
    private static string GetGroupName(Type groupType) => StableTypeName.Get(groupType);
}
