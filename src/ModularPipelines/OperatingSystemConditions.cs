using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Engine.Attributes;

namespace ModularPipelines;

/// <summary>
/// Maps platform run conditions to distributed operating-system capability identifiers.
/// Both the condition handler (which decides whether to defer an OS condition to a worker)
/// and the distributed work publisher (which stamps the OS capability onto an assignment)
/// consume this mapping.
/// </summary>
internal static class OperatingSystemConditions
{
    /// <summary>Capability identifier for Windows-only modules.</summary>
    public const string Windows = "windows";

    /// <summary>Capability identifier for Linux-only modules.</summary>
    public const string Linux = "linux";

    /// <summary>Capability identifier for macOS-only modules.</summary>
    public const string MacOS = "macos";

    /// <summary>Capability identifier for FreeBSD-only modules.</summary>
    public const string FreeBSD = "freebsd";

    private const string AlternativeCapabilityPrefix = "operating-system:";
    private static readonly string[] OperatingSystems = [Windows, Linux, MacOS, FreeBSD];

    /// <summary>
    /// Returns the operating-system capabilities targeted by a platform condition.
    /// Mixed <see cref="RunIfAnyAttribute"/> conditions return targets for their platform alternatives.
    /// </summary>
    public static IReadOnlyList<string> GetTargets(IConditionAttribute attribute)
    {
        var supportedOperatingSystems = GetRoute(attribute)?.OperatingSystems;

        if (supportedOperatingSystems is null || supportedOperatingSystems.Count == 0)
        {
            return [];
        }

        return [CreateCapability(supportedOperatingSystems)];
    }

    /// <summary>
    /// Returns planning-safe non-platform alternatives that the master can evaluate before
    /// routing a mixed <see cref="RunIfAnyAttribute"/> condition to an operating-system worker.
    /// </summary>
    public static IReadOnlyList<Type> GetLocalAlternatives(IConditionAttribute attribute)
    {
        if (attribute is not RunIfAnyAttribute)
        {
            return [];
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (!conditionTypes.Any(type => GetSupportedOperatingSystems(type) is not null))
        {
            return [];
        }

        return conditionTypes
            .Where(type => GetSupportedOperatingSystems(type) is null
                           && typeof(IPlanningRunCondition).IsAssignableFrom(type))
            .ToArray();
    }

    /// <summary>
    /// Returns the union capability targeted by one group of alternative conditions.
    /// Non-platform alternatives are evaluated on the distributed master and do not
    /// remove the routing constraint supplied by platform alternatives.
    /// </summary>
    public static IReadOnlyList<string> GetTargets(
        IEnumerable<IGroupedConditionAttribute> alternatives)
    {
        var supportedOperatingSystems = GetRoute(alternatives)?.OperatingSystems;

        return supportedOperatingSystems is null or { Count: 0 }
            ? []
            : [CreateCapability(supportedOperatingSystems)];
    }

    public static OperatingSystemRoute? GetRoute(IConditionAttribute attribute)
    {
        var supportedOperatingSystems = GetSupportedOperatingSystems(attribute);
        if (supportedOperatingSystems is not null)
        {
            return new OperatingSystemRoute(supportedOperatingSystems, IsConditional: false);
        }

        if (attribute is not RunIfAnyAttribute)
        {
            return null;
        }

        var routableOperatingSystems = GetRoutableOperatingSystems(
            attribute.GetType().GetGenericArguments());
        return routableOperatingSystems.Count == 0
            ? null
            : new OperatingSystemRoute(routableOperatingSystems, IsConditional: true);
    }

    public static OperatingSystemRoute? GetRoute(
        IEnumerable<IGroupedConditionAttribute> alternatives)
    {
        var alternativeArray = alternatives.ToArray();
        var supportedOperatingSystems = GetSupportedOperatingSystemsForAlternatives(alternativeArray);
        if (supportedOperatingSystems is not null)
        {
            return supportedOperatingSystems.Count == 0
                ? null
                : new OperatingSystemRoute(supportedOperatingSystems, IsConditional: false);
        }

        var routableOperatingSystems = GetRoutableOperatingSystemsForAlternatives(alternativeArray);
        return routableOperatingSystems.Count == 0
            ? null
            : new OperatingSystemRoute(routableOperatingSystems, IsConditional: true);
    }

    public static string GetCapability(IEnumerable<string> operatingSystems) =>
        CreateCapability(operatingSystems);

    public static bool TryGetCapabilityRoute(
        string capability,
        [NotNullWhen(true)] out OperatingSystemRoute? route)
    {
        var operatingSystems = capability.StartsWith(
            AlternativeCapabilityPrefix,
            StringComparison.OrdinalIgnoreCase)
            ? capability[AlternativeCapabilityPrefix.Length..].Split('|')
            : [capability];
        if (operatingSystems.Length == 0
            || operatingSystems.Any(operatingSystem =>
                !OperatingSystems.Contains(operatingSystem, StringComparer.OrdinalIgnoreCase)))
        {
            route = null;
            return false;
        }

        route = new OperatingSystemRoute(
            operatingSystems.ToHashSet(StringComparer.OrdinalIgnoreCase),
            IsConditional: false);
        return true;
    }

    /// <summary>
    /// Returns the intersection of all required operating-system routes.
    /// </summary>
    public static IReadOnlySet<string>? GetRequiredOperatingSystems(
        IEnumerable<IConditionAttribute> attributes)
    {
        HashSet<string>? requiredOperatingSystems = null;
        foreach (var attribute in attributes)
        {
            var route = GetRoute(attribute);
            if (route is null)
            {
                continue;
            }

            IntersectConstraint(
                ref requiredOperatingSystems,
                new HashSet<string>(route.OperatingSystems, StringComparer.OrdinalIgnoreCase));
        }

        return requiredOperatingSystems;
    }

    /// <summary>
    /// Returns whether required platform attributes have no operating system in common.
    /// </summary>
    public static bool HasImpossibleCombination(IEnumerable<IConditionAttribute> attributes)
    {
        HashSet<string>? supportedOperatingSystems = null;
        var conditionAttributes = attributes.ToArray();

        foreach (var attribute in conditionAttributes.Where(static attribute =>
                     attribute is not IGroupedConditionAttribute))
        {
            IntersectConstraint(
                ref supportedOperatingSystems,
                GetSupportedOperatingSystems(attribute));
        }

        foreach (var alternatives in conditionAttributes
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            IntersectConstraint(
                ref supportedOperatingSystems,
                GetSupportedOperatingSystemsForAlternatives(alternatives));
        }

        return supportedOperatingSystems is { Count: 0 };
    }

    /// <summary>
    /// Returns whether declared required platform attributes have no operating system in common
    /// without constructing condition attributes.
    /// </summary>
    public static bool HasImpossibleCombination(Type moduleType)
    {
        HashSet<string>? supportedOperatingSystems = null;
        var attributes = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(RunIfAttribute).IsAssignableFrom(type)
                           || typeof(RunIfAllAttribute).IsAssignableFrom(type)
                           || typeof(RunIfAnyAttribute).IsAssignableFrom(type));

        foreach (var attribute in attributes.Where(static attribute =>
                     !typeof(IGroupedConditionAttribute).IsAssignableFrom(attribute.AttributeType)))
        {
            IntersectConstraint(
                ref supportedOperatingSystems,
                GetSupportedOperatingSystems(attribute));
        }

        foreach (var alternatives in attributes
                     .Where(static attribute =>
                         typeof(IGroupedConditionAttribute).IsAssignableFrom(attribute.AttributeType)
                         && typeof(IPlanningConditionAttribute).IsAssignableFrom(attribute.AttributeType))
                     .GroupBy(static attribute =>
                         CustomAttributeMetadata.Create<IGroupedConditionAttribute>(attribute)
                             .ConditionGroupType))
        {
            IntersectConstraint(
                ref supportedOperatingSystems,
                GetSupportedOperatingSystemsForAlternatives(alternatives));
        }

        return supportedOperatingSystems is { Count: 0 };
    }

    /// <summary>
    /// Returns every capability automatically satisfied by a worker on the specified operating system.
    /// </summary>
    public static IReadOnlyList<string> GetWorkerCapabilities(string operatingSystem)
    {
        if (!OperatingSystems.Contains(operatingSystem, StringComparer.OrdinalIgnoreCase))
        {
            return [];
        }

        var capabilities = new List<string> { operatingSystem };
        var currentOperatingSystemIndex = Array.FindIndex(
            OperatingSystems,
            candidate => string.Equals(candidate, operatingSystem, StringComparison.OrdinalIgnoreCase));

        for (var mask = 1; mask < 1 << OperatingSystems.Length; mask++)
        {
            if ((mask & (1 << currentOperatingSystemIndex)) == 0
                || System.Numerics.BitOperations.PopCount((uint) mask) < 2)
            {
                continue;
            }

            capabilities.Add(CreateCapability(
                OperatingSystems.Where((_, index) => (mask & (1 << index)) != 0)));
        }

        return capabilities;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Stateful OS attributes expose identifiers directly; generic conditions have a new() constraint preserving a public parameterless constructor.")]
    private static HashSet<string>? GetSupportedOperatingSystems(IConditionAttribute attribute)
    {
        if (attribute.Logic is not (ConditionLogic.All or ConditionLogic.Any))
        {
            return null;
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return null;
        }

        var supportedOperatingSystems = attribute.Logic == ConditionLogic.All
            ? null
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var conditionType in conditionTypes)
        {
            var conditionOperatingSystems = GetSupportedOperatingSystems(conditionType);
            if (conditionOperatingSystems is null)
            {
                return null;
            }

            if (attribute.Logic == ConditionLogic.Any)
            {
                supportedOperatingSystems!.UnionWith(conditionOperatingSystems);
            }
            else if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = conditionOperatingSystems;
            }
            else
            {
                supportedOperatingSystems.IntersectWith(conditionOperatingSystems);
            }
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string>? GetSupportedOperatingSystems(CustomAttributeData attribute)
    {
        var conditionTypes = attribute.AttributeType.GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return null;
        }

        var useUnion = typeof(RunIfAnyAttribute).IsAssignableFrom(attribute.AttributeType);
        var supportedOperatingSystems = useUnion
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : null;
        foreach (var conditionType in conditionTypes)
        {
            var conditionOperatingSystems = GetSupportedOperatingSystems(conditionType);
            if (conditionOperatingSystems is null)
            {
                return null;
            }

            if (useUnion)
            {
                supportedOperatingSystems!.UnionWith(conditionOperatingSystems);
            }
            else if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = conditionOperatingSystems;
            }
            else
            {
                supportedOperatingSystems.IntersectWith(conditionOperatingSystems);
            }
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string>? GetSupportedOperatingSystemsForAlternatives(
        IEnumerable<IConditionAttribute> alternatives)
    {
        var supportedOperatingSystems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var alternative in alternatives)
        {
            var alternativeOperatingSystems = GetSupportedOperatingSystems(alternative);
            if (alternativeOperatingSystems is null)
            {
                return null;
            }

            supportedOperatingSystems.UnionWith(alternativeOperatingSystems);
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string> GetRoutableOperatingSystemsForAlternatives(
        IEnumerable<IConditionAttribute> alternatives)
    {
        var supportedOperatingSystems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var alternative in alternatives)
        {
            var alternativeOperatingSystems = GetSupportedOperatingSystems(alternative);
            if (alternativeOperatingSystems is not null)
            {
                supportedOperatingSystems.UnionWith(alternativeOperatingSystems);
            }
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string> GetRoutableOperatingSystems(IEnumerable<Type> conditionTypes)
    {
        var supportedOperatingSystems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var conditionType in conditionTypes)
        {
            var conditionOperatingSystems = GetSupportedOperatingSystems(conditionType);
            if (conditionOperatingSystems is not null)
            {
                supportedOperatingSystems.UnionWith(conditionOperatingSystems);
            }
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string>? GetSupportedOperatingSystemsForAlternatives(
        IEnumerable<CustomAttributeData> alternatives)
    {
        var supportedOperatingSystems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var alternative in alternatives)
        {
            var alternativeOperatingSystems = GetSupportedOperatingSystems(alternative);
            if (alternativeOperatingSystems is null)
            {
                return null;
            }

            supportedOperatingSystems.UnionWith(alternativeOperatingSystems);
        }

        return supportedOperatingSystems;
    }

    private static void IntersectConstraint(
        ref HashSet<string>? supportedOperatingSystems,
        HashSet<string>? constraint)
    {
        if (constraint is null)
        {
            return;
        }

        if (supportedOperatingSystems is null)
        {
            supportedOperatingSystems = constraint;
            return;
        }

        supportedOperatingSystems.IntersectWith(constraint);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIf<T>, RunIfAll<T...>, or RunIfAny<T...> generic arguments, whose new() constraints preserve a public parameterless constructor.")]
    private static HashSet<string>? GetSupportedOperatingSystems(
        Type conditionType)
    {
        var operatingSystem = GetOperatingSystem(conditionType);
        if (operatingSystem is not null)
        {
            return new HashSet<string>([operatingSystem], StringComparer.OrdinalIgnoreCase);
        }

        if (!typeof(ConditionGroup).IsAssignableFrom(conditionType)
            || !typeof(IPlanningRunCondition).IsAssignableFrom(conditionType)
            || Activator.CreateInstance(conditionType) is not ConditionGroup group)
        {
            return null;
        }

        return GetSupportedOperatingSystems(group);
    }

    private static HashSet<string>? GetSupportedOperatingSystems(ConditionGroup group)
    {
        if (group.Conditions.Count == 0)
        {
            return null;
        }

        var conditionOperatingSystems = group.Conditions
            .Select(condition => GetSupportedOperatingSystems(condition.GetType()))
            .ToArray();
        if (conditionOperatingSystems.Any(targets => targets is null))
        {
            return null;
        }

        var supportedOperatingSystems = group.Logic == ConditionLogic.All
            ? new HashSet<string>(OperatingSystems, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var targets in conditionOperatingSystems)
        {
            if (group.Logic == ConditionLogic.All)
            {
                supportedOperatingSystems.IntersectWith(targets!);
            }
            else
            {
                supportedOperatingSystems.UnionWith(targets!);
            }
        }

        return supportedOperatingSystems;
    }

    private static string? GetOperatingSystem(Type conditionType)
    {
        if (conditionType == typeof(OnWindows))
        {
            return Windows;
        }

        if (conditionType == typeof(OnLinux))
        {
            return Linux;
        }

        if (conditionType == typeof(OnFreeBSD))
        {
            return FreeBSD;
        }

        return conditionType == typeof(OnMacOS) ? MacOS : null;
    }

    private static string CreateCapability(IEnumerable<string> operatingSystems)
    {
        var orderedOperatingSystems = OperatingSystems
            .Where(candidate => operatingSystems.Contains(candidate, StringComparer.OrdinalIgnoreCase))
            .ToArray();

        return orderedOperatingSystems.Length == 1
            ? orderedOperatingSystems[0]
            : AlternativeCapabilityPrefix + string.Join('|', orderedOperatingSystems);
    }

    internal sealed record OperatingSystemRoute(
        IReadOnlySet<string> OperatingSystems,
        bool IsConditional);
}
