using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Conditions;

namespace ModularPipelines.Attributes;

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

    private const string AlternativeCapabilityPrefix = "operating-system:";
    private static readonly string[] OperatingSystems = [Windows, Linux, MacOS];

    /// <summary>
    /// Returns the operating-system capabilities targeted by an all-platform condition.
    /// Mixed platform and non-platform conditions are evaluated locally and return no targets.
    /// </summary>
    public static IReadOnlyList<string> GetTargets(IConditionAttribute attribute)
    {
        if (attribute.Logic != ConditionLogic.All)
        {
            return [];
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return [];
        }

        HashSet<string>? supportedOperatingSystems = null;

        foreach (var conditionType in conditionTypes)
        {
            var conditionOperatingSystems = GetSupportedOperatingSystems(conditionType);
            if (conditionOperatingSystems is null)
            {
                return [];
            }

            if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = conditionOperatingSystems;
            }
            else
            {
                supportedOperatingSystems.IntersectWith(conditionOperatingSystems);
            }
        }

        if (supportedOperatingSystems is null || supportedOperatingSystems.Count == 0)
        {
            return [];
        }

        return [CreateCapability(supportedOperatingSystems)];
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
        Justification = "Condition types come from RunIfAll<T> generic arguments, whose new() constraint preserves a public parameterless constructor.")]
    private static HashSet<string>? GetSupportedOperatingSystems(Type conditionType)
    {
        var operatingSystem = GetOperatingSystem(conditionType);
        if (operatingSystem is not null)
        {
            return new HashSet<string>([operatingSystem], StringComparer.OrdinalIgnoreCase);
        }

        if (!typeof(ConditionGroup).IsAssignableFrom(conditionType)
            || Activator.CreateInstance(conditionType) is not ConditionGroup group
            || group.Conditions.Count == 0)
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
}
