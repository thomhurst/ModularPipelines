using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "configuration", "create")]
public record AzMaintenanceConfigurationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--expiration-date-time")]
    public string? ExpirationDateTime { get; set; }

    [CliOption("--extension-properties")]
    public string? ExtensionProperties { get; set; }

    [CliOption("--install-patches-linux-parameters")]
    public string? InstallPatchesLinuxParameters { get; set; }

    [CliOption("--install-patches-windows-parameters")]
    public string? InstallPatchesWindowsParameters { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maintenance-scope")]
    public string? MaintenanceScope { get; set; }

    [CliOption("--maintenance-window-recur-every")]
    public string? MaintenanceWindowRecurEvery { get; set; }

    [CliOption("--maintenance-window-start-date-time")]
    public string? MaintenanceWindowStartDateTime { get; set; }

    [CliOption("--maintenance-window-time-zone")]
    public string? MaintenanceWindowTimeZone { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--reboot-setting")]
    public string? RebootSetting { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }
}