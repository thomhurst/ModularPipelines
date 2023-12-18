using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "update")]
public record AzVmUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CommandSwitch("--disk-caching")]
    public string? DiskCaching { get; set; }

    [CommandSwitch("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [BooleanCommandSwitch("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [BooleanCommandSwitch("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [BooleanCommandSwitch("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CommandSwitch("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--host-group")]
    public string? HostGroup { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--max-price")]
    public string? MaxPrice { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-disk")]
    public string? OsDisk { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CommandSwitch("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }

    [CommandSwitch("--write-accelerator")]
    public string? WriteAccelerator { get; set; }
}

