namespace ModularPipelines.Attributes;

/// <summary>
/// Single source of truth mapping the OS-only mandatory run conditions
/// (<see cref="RunOnWindowsOnlyAttribute"/>, <see cref="RunOnLinuxOnlyAttribute"/>,
/// <see cref="RunOnMacOSOnlyAttribute"/>) to their operating-system capability identifier.
/// Both the condition handler (which decides whether to defer an OS condition to a worker)
/// and the distributed work publisher (which stamps the OS capability onto an assignment)
/// consume this, so the two paths cannot drift as new operating systems are added.
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
    /// Returns the operating-system capability an OS-only mandatory condition targets, or
    /// <see langword="null"/> if the attribute is not an OS-only condition. Pattern matching
    /// means subclasses of the OS-only attributes are classified by their base operating system.
    /// </summary>
#pragma warning disable CS0618 // MandatoryRunConditionAttribute is the legacy base type these OS conditions derive from.
    public static string? GetTarget(MandatoryRunConditionAttribute attribute) => attribute switch
#pragma warning restore CS0618
    {
        RunOnWindowsOnlyAttribute => Windows,
        RunOnLinuxOnlyAttribute => Linux,
        RunOnMacOSOnlyAttribute => MacOS,
        _ => null,
    };
}
