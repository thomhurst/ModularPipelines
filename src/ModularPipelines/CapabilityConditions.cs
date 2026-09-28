using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
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
    /// Returns the capability requirement of a module type without evaluating any ordinary condition:
    /// explicit <see cref="RequiresCapabilityAttribute"/> and <see cref="RequiresAnyCapabilityAttribute"/>
    /// declarations plus the capabilities its run conditions need whatever the ordinary conditions return.
    /// Returns <c>null</c> when no worker can satisfy them.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="isConditionGroupSatisfied">
    /// Returns whether the master already satisfied a condition group locally, so it needs no route.
    /// </param>
    public static CapabilityRequirement? GetModuleRequirement(
        Type moduleType,
        Func<Type, bool>? isConditionGroupSatisfied = null)
    {
        var formula = ConditionFormula.ForModule(
            moduleType.GetCustomAttributes(inherit: true).OfType<RunConditionAttribute>(),
            isConditionGroupSatisfied);
        return Combine(moduleType, formula?.Evaluate(static _ => true) ?? FormulaValue.True);
    }

    /// <summary>
    /// Combines a module's declared requirement with the value of its condition formula, or returns
    /// <c>null</c> when no worker can satisfy both.
    /// </summary>
    public static CapabilityRequirement? Combine(Type moduleType, FormulaValue conditionValue) =>
        conditionValue.RequirementOrNone is { } conditionRequirement
        && GetDeclaredRequirement(moduleType).And(conditionRequirement) is { IsSatisfiable: true } requirement
            ? requirement
            : null;

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
    /// An attribute whose ordinary conditions can satisfy it without a capability returns a conditional
    /// route for its capability branches.
    /// </summary>
    public static CapabilityRoute? GetRoute(RunConditionAttribute attribute)
    {
        var requirement = GetRequirement(attribute);
        if (requirement is not null)
        {
            return new CapabilityRoute(requirement, IsConditional: false);
        }

        // Mixed alternatives, such as RunIfAny<OnLinux, OnCI> or a ConditionGroup of OnGpu OR OnCI, still
        // route to their capability branches when the ordinary branches turn out false.
        return ConditionFormula.ForAttribute(attribute)?.Evaluate(static _ => null) is
        { Kind: FormulaValueKind.Requirement, Requirement: { } routable }
            ? new CapabilityRoute(routable, IsConditional: true)
            : null;
    }

    /// <summary>
    /// Returns the route for one group of alternative conditions, or <c>null</c> when no alternative
    /// has a satisfiable capability condition. Non-capability alternatives make the route conditional.
    /// </summary>
    public static CapabilityRoute? GetRoute(IEnumerable<RunConditionAttribute> alternatives)
    {
        var requirements = alternatives
            .Select(static attribute => GetRequirement(attribute))
            .ToArray();
        var routable = OrAll(requirements);
        return routable is { IsSatisfiable: true }
            ? new CapabilityRoute(routable, IsConditional: requirements.Any(static requirement => requirement is null))
            : null;
    }

    /// <summary>
    /// Returns whether an attribute has a capability condition that some worker could satisfy.
    /// </summary>
    public static bool IsRoutable(RunConditionAttribute attribute) =>
        GetRoute(attribute) is { Requirement.IsSatisfiable: true };

    /// <summary>
    /// Returns whether the combined routes of required attributes could be satisfied by some worker.
    /// </summary>
    public static bool HasRoutableRequirement(IEnumerable<RunConditionAttribute> attributes)
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
    /// routing a mixed <see cref="RunIfAnyAttribute{T1,T2}"/> condition to a worker.
    /// </summary>
    public static IReadOnlyList<Type> GetLocalAlternatives(RunConditionAttribute attribute)
    {
        var attributeType = attribute.GetType();
        if (BuiltInConditionAttributes.GetRunLogic(attributeType) != ConditionLogic.Any)
        {
            return [];
        }

        var conditionTypes = BuiltInConditionAttributes.GetConditionTypes(attributeType);
        if (conditionTypes.All(static type => GetConditionRequirement(type) is null))
        {
            return [];
        }

        return conditionTypes
            .Where(static type => GetConditionRequirement(type) is null
                                  && typeof(IPlanningSafe).IsAssignableFrom(type))
            .ToArray();
    }

    /// <summary>
    /// Returns whether declared capabilities and required capability conditions can never be satisfied
    /// by one worker, without constructing condition attributes.
    /// </summary>
    /// <remarks>
    /// Built-in generic run attributes are read from their type arguments without construction; each is its
    /// own requirement. Planning-safe custom attributes are constructed to read their intent and group key, so
    /// alternatives sharing a group key contribute the union of their capability conditions.
    /// </remarks>
    public static bool HasImpossibleCombination(Type moduleType)
    {
        var builtInAttributes = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => BuiltInConditionAttributes.GetIntent(type) == ConditionIntent.Run);

        // Declared capability attributes can conflict with each other or with run conditions.
        var combined = GetDeclaredRequirement(moduleType);
        foreach (var attribute in builtInAttributes)
        {
            if (GetRequirement(attribute.AttributeType) is { } requirement)
            {
                combined = combined.And(requirement);
            }
        }

        var planningSafeCustomAttributes = CustomAttributeMetadata.GetApplicable(
                moduleType,
                static type => typeof(RunConditionAttribute).IsAssignableFrom(type)
                               && typeof(IPlanningSafe).IsAssignableFrom(type)
                               && !BuiltInConditionAttributes.IsBuiltIn(type))
            .Select(CustomAttributeMetadata.Create<RunConditionAttribute>)
            .Where(static attribute => attribute.Intent == ConditionIntent.Run);
        foreach (var attributesInGroup in planningSafeCustomAttributes
                     .GroupBy(static attribute => attribute.GroupKey ?? attribute.GetType()))
        {
            var requirements = attributesInGroup.Select(GetRequirement).ToArray();
            if (requirements.All(static requirement => requirement is not null))
            {
                combined = combined.And(OrAll(requirements)!);
            }
        }

        return !combined.IsSatisfiable;
    }

    private static CapabilityRequirement? GetRequirement(RunConditionAttribute attribute)
    {
        if (attribute.Intent != ConditionIntent.Run)
        {
            return null;
        }

        var conditionTypes = BuiltInConditionAttributes.GetMemberConditionTypes(attribute.GetType());
        return conditionTypes.Length == 0
            ? null
            : Combine(conditionTypes.Select(GetConditionRequirement), attribute.Logic);
    }

    private static CapabilityRequirement? GetRequirement(Type attributeType)
    {
        if (BuiltInConditionAttributes.GetRunLogic(attributeType) is not { } logic)
        {
            return null;
        }

        return Combine(
            BuiltInConditionAttributes.GetConditionTypes(attributeType).Select(GetConditionRequirement),
            logic);
    }

    /// <summary>
    /// Combines member requirements into a necessary capability requirement, or <c>null</c> when the
    /// members impose none. AND keeps the capabilities of its capability members, because every member
    /// must hold; the selected worker evaluates the other members. OR has a requirement only when every
    /// alternative has one, because a non-capability alternative could hold on any worker.
    /// </summary>
    private static CapabilityRequirement? Combine(IEnumerable<CapabilityRequirement?> requirements, ConditionLogic logic)
    {
        var requirementArray = requirements.ToArray();
        if (logic == ConditionLogic.All)
        {
            var capabilityRequirements = requirementArray.OfType<CapabilityRequirement>().ToArray();
            return capabilityRequirements.Length == 0
                ? null
                : capabilityRequirements.Aggregate(CapabilityRequirement.None, static (left, right) => left.And(right));
        }

        return requirementArray.Any(static requirement => requirement is null) ? null : OrAll(requirementArray);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIf<T...> or RunIfAny<T...> generic arguments, whose new() constraints preserve a public parameterless constructor.")]
    private static CapabilityRequirement? GetConditionRequirement(Type conditionType) =>
        ConditionRequirements.GetOrAdd(conditionType, static type =>
            typeof(IPlanningSafe).IsAssignableFrom(type)
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

        if (condition is not (ConditionGroup group and IPlanningSafe)
            || group.Logic is not (ConditionLogic.All or ConditionLogic.Any)
            || group.Conditions.Count == 0)
        {
            return null;
        }

        return Combine(group.Conditions.Select(GetConditionRequirement), group.Logic);
    }

    private static CapabilityRequirement? OrAll(IEnumerable<CapabilityRequirement?> requirements) =>
        requirements
            .OfType<CapabilityRequirement>()
            .Aggregate((CapabilityRequirement?) null, static (left, right) => left?.Or(right) ?? right);

    internal sealed record CapabilityRoute(
        CapabilityRequirement Requirement,
        bool IsConditional);
}
