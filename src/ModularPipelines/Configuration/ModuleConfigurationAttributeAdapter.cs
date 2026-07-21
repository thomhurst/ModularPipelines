using System.Collections.Frozen;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
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
        var skipCondition = CombineSkipConditions(moduleType, configured.SkipCondition);

        if (skipCondition == configured.SkipCondition
            && configured.ParallelConstraintKeys is null
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
            SkipCondition = skipCondition,
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

#pragma warning disable CS0618 // Legacy conditions are adapted here to preserve compatibility.
    private static Func<IModuleContext, Task<SkipDecision>>? CombineSkipConditions(
        Type moduleType,
        Func<IModuleContext, Task<SkipDecision>>? configuredCondition)
    {
        var conditionAttributes = moduleType
            .GetCustomAttributes(inherit: true)
            .OfType<IConditionAttribute>()
            .ToArray();
        var mandatoryLegacyConditions = moduleType
            .GetCustomAttributes<MandatoryRunConditionAttribute>(inherit: true)
            .ToArray();
        var optionalLegacyConditions = moduleType
            .GetCustomAttributes<RunConditionAttribute>(inherit: true)
            .Except(mandatoryLegacyConditions)
            .ToArray();

        if (conditionAttributes.Length == 0
            && mandatoryLegacyConditions.Length == 0
            && optionalLegacyConditions.Length == 0)
        {
            return configuredCondition;
        }

        return async context =>
        {
            var pipelineContext = (IPipelineHookContext) context;

            foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Skip))
            {
                if (await attribute.EvaluateAsync(pipelineContext).ConfigureAwait(false))
                {
                    return SkipDecision.Skip($"SkipIf<{attribute.ConditionNames}> returned true");
                }
            }

            foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.All))
            {
                if (!await attribute.EvaluateAsync(pipelineContext).ConfigureAwait(false))
                {
                    return SkipDecision.Skip($"RunIfAll<{attribute.ConditionNames}> not satisfied");
                }
            }

            foreach (var attribute in conditionAttributes.Where(attribute => attribute.Logic == ConditionLogic.Any))
            {
                if (!await attribute.EvaluateAsync(pipelineContext).ConfigureAwait(false))
                {
                    return SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied");
                }
            }

            foreach (var attribute in mandatoryLegacyConditions)
            {
                if (!await attribute.Condition(pipelineContext).ConfigureAwait(false))
                {
                    return SkipDecision.Skip($"A condition to run this module has not been met - {attribute.GetType().Name}");
                }
            }

            if (optionalLegacyConditions.Length > 0)
            {
                var anyConditionMet = false;
                foreach (var attribute in optionalLegacyConditions)
                {
                    if (await attribute.Condition(pipelineContext).ConfigureAwait(false))
                    {
                        anyConditionMet = true;
                        break;
                    }
                }

                if (!anyConditionMet)
                {
                    var names = optionalLegacyConditions.Select(attribute =>
                        attribute.GetType().Name.Replace("Attribute", string.Empty, StringComparison.OrdinalIgnoreCase));
                    return SkipDecision.Skip($"No run conditions were met: {string.Join(", ", names)}");
                }
            }

            if (configuredCondition is null)
            {
                return SkipDecision.DoNotSkip;
            }

            var decision = await configuredCondition(context).ConfigureAwait(false);
            return decision.ShouldSkip && decision.Reason is null
                ? SkipDecision.Skip("Configured skip condition returned true")
                : decision;
        };
    }
#pragma warning restore CS0618
}
