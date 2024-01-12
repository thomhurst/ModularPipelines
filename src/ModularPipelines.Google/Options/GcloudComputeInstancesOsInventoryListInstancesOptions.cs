using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "os-inventory", "list-instances")]
public record GcloudComputeInstancesOsInventoryListInstancesOptions : GcloudOptions
{
    [CommandSwitch("--inventory-filter")]
    public string? InventoryFilter { get; set; }

    [CommandSwitch("--kernel-version")]
    public string? KernelVersion { get; set; }

    [CommandSwitch("--os-shortname")]
    public string? OsShortname { get; set; }

    [CommandSwitch("--os-version")]
    public string? OsVersion { get; set; }

    [CommandSwitch("--package-name")]
    public string? PackageName { get; set; }

    [CommandSwitch("--package-version")]
    public string? PackageVersion { get; set; }
}