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
        var tags = MergeTags(moduleType, declaredTags, configured.Tags);
        var dependencies = MergeDependencies(moduleType, declaredDependencies, configured.Dependencies);
        var notInParallel = moduleType.GetCustomAttribute<NotInParallelAttribute>(inherit: true);
        var priority = moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true);
        var executionHint = moduleType.GetCustomAttribute<ExecutionHintAttribute>(inherit: true);
        var category = configured.Category
            ?? declaredCategory
            ?? moduleType.GetCustomAttribute<ModuleCategoryAttribute>(inherit: true)?.Category;
        if (IsUnchanged(configured, notInParallel, priority, executionHint, tags, category, dependencies))
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
            Tags = tags,
            Category = category,
            Dependencies = dependencies,
        };
    }

    private static IReadOnlySet<string> MergeTags(
        Type moduleType,
        IReadOnlySet<string> declaredTags,
        IReadOnlySet<string> configuredTags)
    {
        return moduleType
            .GetCustomAttributes<ModuleTagAttribute>(inherit: true)
            .Select(attribute => attribute.Tag)
            .Concat(declaredTags)
            .Concat(configuredTags)
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);
    }

    private static DeclaredDependency[] MergeDependencies(
        Type moduleType,
        IReadOnlyList<DeclaredDependency> declaredDependencies,
        IReadOnlyList<DeclaredDependency> configuredDependencies)
    {
        var attributeDependencies = moduleType
            .GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>()
            .Select(attribute => attribute.Optional
                ? DeclaredDependency.Optional(attribute.Type)
                : DeclaredDependency.Required(attribute.Type));

        return attributeDependencies
            .Concat(declaredDependencies)
            .Concat(configuredDependencies)
            .GroupBy(dependency => dependency.ModuleType)

            // Conflicting declarations use the strictest scheduling contract:
            // a required/conditional dependency wins over an optional/lazy declaration.
            .Select(group => group.Any(dependency => !dependency.IsOptional)
                ? group.First(dependency => !dependency.IsOptional)
                : group.First())
            .ToArray();
    }

    private static bool IsUnchanged(
        ModuleConfiguration configured,
        NotInParallelAttribute? notInParallel,
        PriorityAttribute? priority,
        ExecutionHintAttribute? executionHint,
        IReadOnlySet<string> tags,
        string? category,
        IReadOnlyList<DeclaredDependency> dependencies)
    {
        return configured.ParallelConstraintKeys is null
               && notInParallel is null
               && configured.Priority is null
               && priority is null
               && configured.ExecutionType is null
               && executionHint is null
               && tags.Count == 0
               && category is null
               && dependencies.Count == 0;
    }
}
