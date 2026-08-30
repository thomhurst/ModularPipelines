using System.Runtime.CompilerServices;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

internal sealed class DistributedConditionRouting
{
    private readonly ConditionalWeakTable<IModule, HashSet<Type>> _locallySatisfiedGroups = new();
    private readonly ConditionalWeakTable<IModule, object> _preparedModules = new();

    public bool IsPrepared(IModule module) => _preparedModules.TryGetValue(module, out _);

    public void MarkPrepared(IModule module) =>
        _preparedModules.GetValue(module, static _ => new object());

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

    public IReadOnlyList<string> GetLocallySatisfiedGroupNames(IModule module)
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

    public void RestoreLocallySatisfiedGroups(
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
                MarkLocallySatisfied(module, groupType);
            }
        }
    }

    private static string GetGroupName(Type groupType) =>
        groupType.AssemblyQualifiedName ?? groupType.FullName ?? groupType.Name;
}
