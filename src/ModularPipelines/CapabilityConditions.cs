using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed;
using ModularPipelines.Engine.Attributes;

namespace ModularPipelines;

/// <summary>
/// Translates run conditions built from <see cref="ICapabilityCondition"/> into
/// <see cref="CapabilityRequirement"/> values. The condition handler uses this to decide which
/// conditions the distributed master defers to a worker, and the work publisher uses it to stamp
/// the requirement onto an assignment.
/// </summary>
internal static class CapabilityConditions
{
    private static readonly ConcurrentDictionary<Type, CapabilityRequirement?> ConditionRequirements = new();
    private static readonly ConcurrentDictionary<Type, CapabilityRequirement> DeclaredRequirements = new();

    /// <summary>
    /// Returns every capability requirement declared by a module type: explicit
    /// <see cref="RequiresCapabilityAttribute"/> and <see cref="RequiresAnyCapabilityAttribute"/>
    /// declarations, plus routes derived from capability run conditions.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="isConditionGroupSatisfied">
    /// Returns whether the master already satisfied a condition group locally, so it needs no route.
    /// </param>
    /// <param name="isConditionalRouteRequired">
    /// Returns whether the master evaluated every non-capability alternative of a condition group as
    /// false, so the group's conditional capability route becomes mandatory.
    /// </param>
    public static CapabilityRequirement GetModuleRequirement(
        Type moduleType,
        Func<Type, bool>? isConditionGroupSatisfied = null,
        Func<Type, bool>? isConditionalRouteRequired = null)
    {
        var requirement = GetDeclaredRequirement(moduleType);
        foreach (var (groupType, route) in GetConditionRoutes(moduleType))
        {
            if (isConditionGroupSatisfied?.Invoke(groupType) == true
                || (route.IsConditional && isConditionalRouteRequired?.Invoke(groupType) != true))
            {
                continue;
            }

            requirement = requirement.And(route.Requirement);
        }

        return requirement;
    }

    /// <summary>
    /// Returns the capability routes of a module's run conditions, keyed by condition group type.
    /// </summary>
    public static IEnumerable<(Type ConditionGroupType, CapabilityRoute Route)> GetConditionRoutes(Type moduleType)
    {
        var conditionAttributes = moduleType.GetCustomAttributes(inherit: true).OfType<IConditionAttribute>().ToArray();
        foreach (var attribute in conditionAttributes.Where(static attribute =>
                     attribute is not IGroupedConditionAttribute))
        {
            if (GetRoute(attribute) is { } route)
            {
                yield return (attribute.GetType(), route);
            }
        }

        foreach (var alternatives in conditionAttributes
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            if (GetRoute(alternatives) is { } route)
            {
                yield return (alternatives.Key, route);
            }
        }
    }

    /// <summary>
    /// Returns whether an attribute is built only from capability conditions.
    /// </summary>
    public static bool IsCapabilityOnly(IConditionAttribute attribute) =>
        GetRequirement(attribute.GetType(), attribute.Logic) is not null;

    /// <summary>
    /// Returns whether every non-capability alternative of a mixed <see cref="RunIfAnyAttribute"/> is
    /// planning-safe, so the master can evaluate all of them before deciding the capability route.
    /// </summary>
    public static bool HasOnlyPlanningLocalAlternatives(IConditionAttribute attribute) =>
        attribute is RunIfAnyAttribute
        && attribute.GetType().GetGenericArguments()
            .Where(static type => GetConditionRequirement(type) is null)
            .All(static type => typeof(IPlanningRunCondition).IsAssignableFrom(type));

    /// <summary>
    /// Returns the requirement declared by <see cref="RequiresCapabilityAttribute"/> and
    /// <see cref="RequiresAnyCapabilityAttribute"/> on a module type. Unlike run conditions, these
    /// attributes do not evaluate themselves, so the pipeline checks them against local capabilities.
    /// </summary>
    public static CapabilityRequirement GetDeclaredRequirement(Type moduleType) =>
        DeclaredRequirements.GetOrAdd(moduleType, static type =>
        {
            // Only construct capability attributes: planning must not construct unrelated attributes.
            var requirement = CapabilityRequirement.AllOf(
            [
                .. type.GetCustomAttributes<RequiresCapabilityAttribute>(inherit: true)
                    .SelectMany(static attribute => attribute.Capabilities)
                    .Select(static name => new Capability(name)),
            ]);

            foreach (var attribute in type.GetCustomAttributes<RequiresAnyCapabilityAttribute>(inherit: true))
            {
                requirement = requirement.And(CapabilityRequirement.AnyOf(
                    [.. attribute.Capabilities.Select(static name => new Capability(name))]));
            }

            return requirement;
        });

    /// <summary>
    /// Returns the route for one condition attribute, or <c>null</c> when it has no capability condition.
    /// A mixed <see cref="RunIfAnyAttribute"/> returns a conditional route for its capability alternatives.
    /// </summary>
    public static CapabilityRoute? GetRoute(IConditionAttribute attribute)
    {
        var requirement = GetRequirement(attribute.GetType(), attribute.Logic);
        if (requirement is not null)
        {
            return new CapabilityRoute(requirement, IsConditional: false);
        }

        if (attribute is not RunIfAnyAttribute)
        {
            return null;
        }

        var routable = OrAll(attribute.GetType().GetGenericArguments().Select(GetConditionRequirement));
        return routable is { IsSatisfiable: true }
            ? new CapabilityRoute(routable, IsConditional: true)
            : null;
    }

    /// <summary>
    /// Returns the route for one group of alternative conditions, or <c>null</c> when no alternative
    /// has a satisfiable capability condition. Non-capability alternatives make the route conditional.
    /// </summary>
    public static CapabilityRoute? GetRoute(IEnumerable<IGroupedConditionAttribute> alternatives)
    {
        var requirements = alternatives
            .Select(static attribute => GetRequirement(attribute.GetType(), attribute.Logic))
            .ToArray();
        var routable = OrAll(requirements);
        return routable is { IsSatisfiable: true }
            ? new CapabilityRoute(routable, IsConditional: requirements.Any(static requirement => requirement is null))
            : null;
    }

    /// <summary>
    /// Returns whether an attribute has a capability condition that some worker could satisfy.
    /// </summary>
    public static bool IsRoutable(IConditionAttribute attribute) =>
        GetRoute(attribute) is { Requirement.IsSatisfiable: true };

    /// <summary>
    /// Returns whether the combined routes of required attributes could be satisfied by some worker.
    /// </summary>
    public static bool HasRoutableRequirement(IEnumerable<IConditionAttribute> attributes)
    {
        CapabilityRequirement? combined = null;
        foreach (var route in attributes.Select(GetRoute).OfType<CapabilityRoute>())
        {
            combined = combined?.And(route.Requirement) ?? route.Requirement;
        }

        return combined is { IsSatisfiable: true };
    }

    /// <summary>
    /// Returns planning-safe non-capability alternatives that the master can evaluate before
    /// routing a mixed <see cref="RunIfAnyAttribute"/> condition to a worker.
    /// </summary>
    public static IReadOnlyList<Type> GetLocalAlternatives(IConditionAttribute attribute)
    {
        if (attribute is not RunIfAnyAttribute)
        {
            return [];
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (conditionTypes.All(static type => GetConditionRequirement(type) is null))
        {
            return [];
        }

        return conditionTypes
            .Where(static type => GetConditionRequirement(type) is null
                                  && typeof(IPlanningRunCondition).IsAssignableFrom(type))
            .ToArray();
    }

    /// <summary>
    /// Returns whether declared capabilities and required capability conditions can never be satisfied
    /// by one worker, without constructing condition attributes.
    /// </summary>
    public static bool HasImpossibleCombination(Type moduleType)
    {
        var attributes = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(RunIfAttribute).IsAssignableFrom(type)
                           || typeof(RunIfAllAttribute).IsAssignableFrom(type)
                           || typeof(RunIfAnyAttribute).IsAssignableFrom(type));

        // Declared capability attributes can conflict with each other or with run conditions.
        var combined = GetDeclaredRequirement(moduleType);
        foreach (var attribute in attributes.Where(static attribute =>
                     !typeof(IGroupedConditionAttribute).IsAssignableFrom(attribute.AttributeType)))
        {
            if (GetRequirement(attribute.AttributeType) is { } requirement)
            {
                combined = combined.And(requirement);
            }
        }

        foreach (var alternatives in attributes
                     .Where(static attribute =>
                         typeof(IGroupedConditionAttribute).IsAssignableFrom(attribute.AttributeType)
                         && typeof(IPlanningConditionAttribute).IsAssignableFrom(attribute.AttributeType))
                     .GroupBy(static attribute =>
                         CustomAttributeMetadata.Create<IGroupedConditionAttribute>(attribute)
                             .ConditionGroupType))
        {
            var requirements = alternatives
                .Select(static attribute => GetRequirement(attribute.AttributeType))
                .ToArray();
            if (requirements.All(static requirement => requirement is not null))
            {
                combined = combined.And(OrAll(requirements)!);
            }
        }

        return !combined.IsSatisfiable;
    }

    private static CapabilityRequirement? GetRequirement(Type attributeType) =>
        GetRequirement(
            attributeType,
            typeof(RunIfAnyAttribute).IsAssignableFrom(attributeType) ? ConditionLogic.Any : ConditionLogic.All);

    private static CapabilityRequirement? GetRequirement(Type attributeType, ConditionLogic logic)
    {
        if (logic is not (ConditionLogic.All or ConditionLogic.Any))
        {
            return null;
        }

        var conditionTypes = attributeType.GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return null;
        }

        var requirements = conditionTypes.Select(GetConditionRequirement).ToArray();
        if (requirements.Any(static requirement => requirement is null))
        {
            return null;
        }

        return logic == ConditionLogic.All
            ? requirements.Aggregate(CapabilityRequirement.None, static (left, right) => left.And(right!))
            : OrAll(requirements);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIf<T>, RunIfAll<T...>, or RunIfAny<T...> generic arguments, whose new() constraints preserve a public parameterless constructor.")]
    private static CapabilityRequirement? GetConditionRequirement(Type conditionType) =>
        ConditionRequirements.GetOrAdd(conditionType, static type =>
            typeof(IPlanningRunCondition).IsAssignableFrom(type)
            && (typeof(ICapabilityCondition).IsAssignableFrom(type) || typeof(ConditionGroup).IsAssignableFrom(type))
            && Activator.CreateInstance(type) is IRunCondition condition
                ? GetConditionRequirement(condition)
                : null);

    private static CapabilityRequirement? GetConditionRequirement(IRunCondition condition)
    {
        if (condition is ICapabilityCondition capabilityCondition)
        {
            return CapabilityRequirement.AllOf(capabilityCondition.Capability);
        }

        if (condition is not (ConditionGroup group and IPlanningRunCondition)
            || group.Logic is not (ConditionLogic.All or ConditionLogic.Any)
            || group.Conditions.Count == 0)
        {
            return null;
        }

        var requirements = group.Conditions.Select(GetConditionRequirement).ToArray();
        if (requirements.Any(static requirement => requirement is null))
        {
            return null;
        }

        return group.Logic == ConditionLogic.All
            ? requirements.Aggregate(CapabilityRequirement.None, static (left, right) => left.And(right!))
            : OrAll(requirements);
    }

    private static CapabilityRequirement? OrAll(IEnumerable<CapabilityRequirement?> requirements) =>
        requirements
            .OfType<CapabilityRequirement>()
            .Aggregate((CapabilityRequirement?) null, static (left, right) => left?.Or(right) ?? right);

    internal sealed record CapabilityRoute(
        CapabilityRequirement Requirement,
        bool IsConditional);
}
