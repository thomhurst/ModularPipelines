using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "update")]
public record AzVmssUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--automatic-repairs-action")]
    public string? AutomaticRepairsAction { get; set; }

    [CliOption("--automatic-repairs-grace-period")]
    public string? AutomaticRepairsGracePeriod { get; set; }

    [CliOption("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CliOption("--custom-data")]
    public string? CustomData { get; set; }

    [CliOption("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CliFlag("--enable-automatic-repairs")]
    public bool? EnableAutomaticRepairs { get; set; }

    [CliFlag("--enable-cross-zone-upgrade")]
    public bool? EnableCrossZoneUpgrade { get; set; }

    [CliFlag("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [CliFlag("--enable-osimage-notification")]
    public bool? EnableOsimageNotification { get; set; }

    [CliFlag("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [CliFlag("--enable-spot-restore")]
    public bool? EnableSpotRestore { get; set; }

    [CliFlag("--enable-terminate-notification")]
    public bool? EnableTerminateNotification { get; set; }

    [CliFlag("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CliOption("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [CliFlag("--force-deletion")]
    public bool? ForceDeletion { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--max-batch-instance-percent")]
    public string? MaxBatchInstancePercent { get; set; }

    [CliOption("--max-price")]
    public string? MaxPrice { get; set; }

    [CliOption("--max-unhealthy-instance-percent")]
    public string? MaxUnhealthyInstancePercent { get; set; }

    [CliOption("--max-unhealthy-upgraded-instance-percent")]
    public string? MaxUnhealthyUpgradedInstancePercent { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pause-time-between-batches")]
    public string? PauseTimeBetweenBatches { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliFlag("--prioritize-unhealthy-instances")]
    public bool? PrioritizeUnhealthyInstances { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliFlag("--protect-from-scale-in")]
    public bool? ProtectFromScaleIn { get; set; }

    [CliFlag("--protect-from-scale-set-actions")]
    public bool? ProtectFromScaleSetActions { get; set; }

    [CliOption("--regular-priority-count")]
    public int? RegularPriorityCount { get; set; }

    [CliOption("--regular-priority-percentage")]
    public string? RegularPriorityPercentage { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scale-in-policy")]
    public string? ScaleInPolicy { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--spot-restore-timeout")]
    public string? SpotRestoreTimeout { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--terminate-notification-time")]
    public string? TerminateNotificationTime { get; set; }

    [CliFlag("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CliOption("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [CliOption("--vm-sku")]
    public string? VmSku { get; set; }
}