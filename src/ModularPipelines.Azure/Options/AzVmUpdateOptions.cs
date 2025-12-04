using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "update")]
public record AzVmUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CliOption("--disk-caching")]
    public string? DiskCaching { get; set; }

    [CliOption("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CliFlag("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [CliFlag("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [CliFlag("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CliOption("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--host-group")]
    public string? HostGroup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--max-price")]
    public string? MaxPrice { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-disk")]
    public string? OsDisk { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CliOption("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }

    [CliOption("--write-accelerator")]
    public string? WriteAccelerator { get; set; }
}