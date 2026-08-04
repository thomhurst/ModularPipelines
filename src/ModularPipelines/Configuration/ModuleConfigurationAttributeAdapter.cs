using System.Collections.Frozen;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Configuration;

/// <summary>
/// Converts attribute and module metadata into the canonical configuration model.
/// </summary>
internal static class ModuleConfigurationAttributeAdapter
{
    public static ModuleConfiguration Apply(
        Type moduleType,
        ModuleConfiguration configured)
    {
        var tags = MergeTags(moduleType, configured.Tags);
        var dependencies = MergeDependencies(moduleType, configured.Dependencies);
        var notInParallel = moduleType.GetCustomAttribute<NotInParallelAttribute>(inherit: true);
        var priority = moduleType.GetCustomAttribute<PriorityAttribute>(inherit: true);
        var executionHint = moduleType.GetCustomAttribute<ExecutionHintAttribute>(inherit: true);
        var cacheInputPatterns = moduleType
            .GetCustomAttributes<CacheInputsAttribute>(inherit: true)
            .SelectMany(attribute => attribute.Patterns)
            .Concat(configured.CacheInputPatterns)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var category = configured.Category
            ?? moduleType.GetCustomAttribute<ModuleCategoryAttribute>(inherit: true)?.Category;
        if (IsUnchanged(configured, notInParallel, priority, executionHint, tags, category, cacheInputPatterns, dependencies))
        {
            return configured;
        }

        return new ModuleConfiguration
        {
            SkipCondition = configured.SkipCondition,
            PlanningSkipCondition = configured.PlanningSkipCondition,
            SynchronousPlanningSkipCondition = configured.SynchronousPlanningSkipCondition,
            Timeout = configured.Timeout,
            RetryConfiguration = configured.RetryConfiguration,
            AdvancedRetryPolicyFactory = configured.AdvancedRetryPolicyFactory,
            IgnoreFailuresCondition = configured.IgnoreFailuresCondition,
            AlwaysRun = configured.AlwaysRun,
            ParallelConstraintKeys = configured.ParallelConstraintKeys
                ?? notInParallel?.ConstraintKeys.ToArray(),
            Priority = configured.Priority ?? priority?.Priority,
            ExecutionType = configured.ExecutionType ?? executionHint?.ExecutionType,
            Tags = tags,
            Category = category,
            CacheInputPatterns = cacheInputPatterns,
            CacheKeyParts = configured.CacheKeyParts,
            CacheEnvironmentVariables = configured.CacheEnvironmentVariables,
            CacheAssemblyVersionKey = configured.CacheAssemblyVersionKey,
            Dependencies = dependencies,
        };
    }

    private static IReadOnlySet<string> MergeTags(
        Type moduleType,
        IReadOnlySet<string> configuredTags)
    {
        return moduleType
            .GetCustomAttributes<ModuleTagAttribute>(inherit: true)
            .Select(attribute => attribute.Tag)
            .Concat(configuredTags)
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);
    }

    private static DeclaredDependency[] MergeDependencies(
        Type moduleType,
        IReadOnlyList<DeclaredDependency> configuredDependencies)
    {
        var attributeDependencies = ModuleDependencyResolver
            .GetDependencies(moduleType)
            .Select(dependency => dependency.Optional
                ? DeclaredDependency.Optional(dependency.DependencyType)
                : DeclaredDependency.Required(dependency.DependencyType));

        return attributeDependencies
            .Concat(configuredDependencies)
            .GroupBy(dependency => dependency.ModuleType)

            // Conflicting declarations use the strictest scheduling contract:
            // a required dependency wins over an optional declaration.
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
        IReadOnlyList<string> cacheInputPatterns,
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
               && cacheInputPatterns.Count == 0
               && dependencies.Count == 0;
    }
}
