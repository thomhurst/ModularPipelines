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
    /// Returns the operating-system capabilities targeted by an all-platform condition.
    /// Mixed platform and non-platform conditions are evaluated locally and return no targets.
    /// </summary>
    public static IReadOnlyList<string> GetTargets(IConditionAttribute attribute)
    {
        var supportedOperatingSystems = GetSupportedOperatingSystems(attribute);

        if (supportedOperatingSystems is null || supportedOperatingSystems.Count == 0)
        {
            return [];
        }

        return [CreateCapability(supportedOperatingSystems)];
    }

    /// <summary>
    /// Returns whether all-platform attributes require mutually exclusive operating systems.
    /// </summary>
    public static bool HasImpossibleCombination(IEnumerable<IConditionAttribute> attributes)
    {
        HashSet<string>? supportedOperatingSystems = null;

        foreach (var attribute in attributes)
        {
            var attributeOperatingSystems = GetSupportedOperatingSystems(attribute);
            if (attributeOperatingSystems is null)
            {
                continue;
            }

            if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = attributeOperatingSystems;
            }
            else
            {
                supportedOperatingSystems.IntersectWith(attributeOperatingSystems);
            }
        }

        return supportedOperatingSystems is { Count: 0 };
    }

    /// <summary>
    /// Returns whether declared all-platform attributes require mutually exclusive operating systems
    /// without constructing condition attributes.
    /// </summary>
    public static bool HasImpossibleCombination(Type moduleType)
    {
        HashSet<string>? supportedOperatingSystems = null;
        var attributes = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(RunIfAllAttribute).IsAssignableFrom(type));

        foreach (var attribute in attributes)
        {
            var attributeOperatingSystems = GetSupportedOperatingSystems(attribute);
            if (attributeOperatingSystems is null)
            {
                continue;
            }

            if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = attributeOperatingSystems;
            }
            else
            {
                supportedOperatingSystems.IntersectWith(attributeOperatingSystems);
            }
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
        if (attribute.Logic != ConditionLogic.All)
        {
            return null;
        }

        if (attribute is IOperatingSystemConditionAttribute operatingSystemAttribute)
        {
            var operatingSystems = operatingSystemAttribute.OperatingSystems
                .Select(GetOperatingSystem)
                .ToArray();
            return operatingSystems.Any(operatingSystem => operatingSystem is null)
                ? null
                : new HashSet<string>(operatingSystems!, StringComparer.OrdinalIgnoreCase);
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return null;
        }

        HashSet<string>? supportedOperatingSystems = null;

        foreach (var conditionType in conditionTypes)
        {
            var conditionOperatingSystems = GetSupportedOperatingSystems(conditionType);
            if (conditionOperatingSystems is null)
            {
                return null;
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

        return supportedOperatingSystems;
    }

    private static HashSet<string>? GetSupportedOperatingSystems(CustomAttributeData attribute)
    {
        if (attribute.AttributeType == typeof(RunIfOperatingSystemAttribute))
        {
            return GetOperatingSystemsFromConstructor(attribute);
        }

        var conditionTypes = attribute.AttributeType.GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return null;
        }

        HashSet<string>? supportedOperatingSystems = null;
        foreach (var conditionType in conditionTypes)
        {
            var operatingSystem = GetOperatingSystem(conditionType);
            if (operatingSystem is null)
            {
                return null;
            }

            if (supportedOperatingSystems is null)
            {
                supportedOperatingSystems = new HashSet<string>(
                    [operatingSystem],
                    StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                supportedOperatingSystems.IntersectWith([operatingSystem]);
            }
        }

        return supportedOperatingSystems;
    }

    private static HashSet<string>? GetOperatingSystemsFromConstructor(CustomAttributeData attribute)
    {
        if (attribute.ConstructorArguments is not [{ Value: IReadOnlyCollection<CustomAttributeTypedArgument> values }])
        {
            return null;
        }

        var operatingSystems = values
            .Select(static value => value.Value is null
                ? OperatingSystemIdentifier.Unknown
                : (OperatingSystemIdentifier) Enum.ToObject(typeof(OperatingSystemIdentifier), value.Value))
            .Select(GetOperatingSystem)
            .ToArray();
        return operatingSystems.Length == 0 || operatingSystems.Any(static value => value is null)
            ? null
            : new HashSet<string>(operatingSystems!, StringComparer.OrdinalIgnoreCase);
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

    private static string? GetOperatingSystem(OperatingSystemIdentifier operatingSystem)
    {
        return operatingSystem switch
        {
            OperatingSystemIdentifier.Windows => Windows,
            OperatingSystemIdentifier.Linux => Linux,
            OperatingSystemIdentifier.MacOS => MacOS,
            OperatingSystemIdentifier.FreeBSD => FreeBSD,
            _ => null,
        };
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
