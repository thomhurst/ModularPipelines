using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "update")]
public record AzVmssUpdateOptions(
[property: CommandSwitch("--instance-ids")] string InstanceIds,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--automatic-repairs-action")]
    public string? AutomaticRepairsAction { get; set; }

    [CommandSwitch("--automatic-repairs-grace-period")]
    public string? AutomaticRepairsGracePeriod { get; set; }

    [CommandSwitch("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CommandSwitch("--custom-data")]
    public string? CustomData { get; set; }

    [CommandSwitch("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [BooleanCommandSwitch("--enable-automatic-repairs")]
    public bool? EnableAutomaticRepairs { get; set; }

    [BooleanCommandSwitch("--enable-cross-zone-upgrade")]
    public bool? EnableCrossZoneUpgrade { get; set; }

    [BooleanCommandSwitch("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [BooleanCommandSwitch("--enable-osimage-notification")]
    public bool? EnableOsimageNotification { get; set; }

    [BooleanCommandSwitch("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [BooleanCommandSwitch("--enable-spot-restore")]
    public bool? EnableSpotRestore { get; set; }

    [BooleanCommandSwitch("--enable-terminate-notification")]
    public bool? EnableTerminateNotification { get; set; }

    [BooleanCommandSwitch("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CommandSwitch("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [BooleanCommandSwitch("--force-deletion")]
    public bool? ForceDeletion { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--max-batch-instance-percent")]
    public string? MaxBatchInstancePercent { get; set; }

    [CommandSwitch("--max-price")]
    public string? MaxPrice { get; set; }

    [CommandSwitch("--max-unhealthy-instance-percent")]
    public string? MaxUnhealthyInstancePercent { get; set; }

    [CommandSwitch("--max-unhealthy-upgraded-instance-percent")]
    public string? MaxUnhealthyUpgradedInstancePercent { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pause-time-between-batches")]
    public string? PauseTimeBetweenBatches { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [BooleanCommandSwitch("--prioritize-unhealthy-instances")]
    public bool? PrioritizeUnhealthyInstances { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [BooleanCommandSwitch("--protect-from-scale-in")]
    public bool? ProtectFromScaleIn { get; set; }

    [BooleanCommandSwitch("--protect-from-scale-set-actions")]
    public bool? ProtectFromScaleSetActions { get; set; }

    [CommandSwitch("--regular-priority-count")]
    public int? RegularPriorityCount { get; set; }

    [CommandSwitch("--regular-priority-percentage")]
    public string? RegularPriorityPercentage { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--scale-in-policy")]
    public string? ScaleInPolicy { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--spot-restore-timeout")]
    public string? SpotRestoreTimeout { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--terminate-notification-time")]
    public string? TerminateNotificationTime { get; set; }

    [BooleanCommandSwitch("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CommandSwitch("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [CommandSwitch("--vm-sku")]
    public string? VmSku { get; set; }
}