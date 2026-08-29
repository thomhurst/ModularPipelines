using System.Runtime.CompilerServices;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

internal sealed class DistributedConditionRouting
{
    private readonly ConditionalWeakTable<IModule, HashSet<Type>> _locallySatisfiedGroups = new();

    public void MarkLocallySatisfied(IModule module, Type conditionGroupType)
    {
        var groups = _locallySatisfiedGroups.GetOrCreateValue(module);
        lock (groups)
        {
            groups.Add(conditionGroupType);
        }
    }

    public bool IsLocallySatisfied(IModule module, Type conditionGroupType)
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
}
