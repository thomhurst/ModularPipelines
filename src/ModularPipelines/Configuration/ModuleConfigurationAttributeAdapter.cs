using System.Collections.Frozen;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Configuration;

/// <summary>
/// Converts attribute and legacy module metadata into the canonical configuration model.
/// </summary>
internal static class ModuleConfigurationAttributeAdapter
{
    public static ModuleConfiguration Apply(
        Type moduleType,
        ModuleConfiguration configured,
        IReadOnlySet<string> declaredTags,
        string? declaredCategory,
        IReadOnlyList<DeclaredDependency> declaredDependencies)
    {
        var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        tags.UnionWith(moduleType.GetCustomAttributes<ModuleTagAttribute>(inherit: true).Select(attribute => attribute.Tag));
        tags.UnionWith(declaredTags);
        tags.UnionWith(configured.Tags);

        var attributeDependencies = moduleType
            .GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>()
            .Select(attribute => attribute.Optional
                ? DeclaredDependency.Optional(attribute.Type)
                : DeclaredDependency.Required(attribute.Type));

        var dependencies = attributeDependencies
            .Concat(declaredDependencies)
            .Concat(configured.Dependencies)
            .GroupBy(dependency => dependency.ModuleType)
            .Select(group => group.Any(dependency => !dependency.IsOptional)
                ? group.First(dependency => !dependency.IsOptional)
                : group.First())
            .ToArray();

        var notInParallel = moduleType.GetCustomAttribute<NotInParallelAttribute>(inherit: true);
        var priority = moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true);
        var executionHint = moduleType.GetCustomAttribute<ExecutionHintAttribute>(inherit: true);
        var category = configured.Category
            ?? declaredCategory
            ?? moduleType.GetCustomAttribute<ModuleCategoryAttribute>(inherit: true)?.Category;
        if (configured.ParallelConstraintKeys is null
            && notInParallel is null
            && configured.Priority is null
            && priority is null
            && configured.ExecutionType is null
            && executionHint is null
            && tags.Count == 0
            && category is null
            && dependencies.Length == 0)
        {
            return configured;
        }

        return new ModuleConfiguration
        {
            SkipCondition = configured.SkipCondition,
            Timeout = configured.Timeout,
            RetryPolicyFactory = configured.RetryPolicyFactory,
            IgnoreFailuresCondition = configured.IgnoreFailuresCondition,
            AlwaysRun = configured.AlwaysRun,
            OnBeforeExecute = configured.OnBeforeExecute,
            OnAfterExecute = configured.OnAfterExecute,
            ParallelConstraintKeys = configured.ParallelConstraintKeys
                ?? notInParallel?.ConstraintKeys,
            Priority = configured.Priority ?? priority?.Priority,
            ExecutionType = configured.ExecutionType ?? executionHint?.ExecutionType,
            Tags = tags.ToFrozenSet(StringComparer.OrdinalIgnoreCase),
            Category = category,
            Dependencies = dependencies,
        };
    }
}
