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
        if (conditionTypes.Length == 0 || conditionTypes.Any(type => GetTarget(type) is null))
        {
            return [];
        }

        return conditionTypes.Select(type => GetTarget(type)!).Distinct().ToArray();
    }

    private static string? GetTarget(Type conditionType)
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
}
