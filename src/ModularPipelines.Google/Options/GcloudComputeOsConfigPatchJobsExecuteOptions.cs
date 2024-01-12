using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-jobs", "execute")]
public record GcloudComputeOsConfigPatchJobsExecuteOptions(
[property: BooleanCommandSwitch("--instance-filter-all")] bool InstanceFilterAll,
[property: CommandSwitch("--instance-filter-group-labels")] IEnumerable<KeyValue> InstanceFilterGroupLabels,
[property: CommandSwitch("--instance-filter-name-prefixes")] string[] InstanceFilterNamePrefixes,
[property: CommandSwitch("--instance-filter-names")] string[] InstanceFilterNames,
[property: CommandSwitch("--instance-filter-zones")] string[] InstanceFilterZones
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [BooleanCommandSwitch("--mig-instances-allowed")]
    public bool? MigInstancesAllowed { get; set; }

    [CommandSwitch("--reboot-config")]
    public string? RebootConfig { get; set; }

    [BooleanCommandSwitch("--apt-dist")]
    public bool? AptDist { get; set; }

    [CommandSwitch("--apt-excludes")]
    public string[]? AptExcludes { get; set; }

    [CommandSwitch("--apt-exclusive-packages")]
    public string[]? AptExclusivePackages { get; set; }

    [CommandSwitch("--post-patch-linux-executable")]
    public string? PostPatchLinuxExecutable { get; set; }

    [CommandSwitch("--post-patch-linux-success-codes")]
    public string[]? PostPatchLinuxSuccessCodes { get; set; }

    [CommandSwitch("--post-patch-windows-executable")]
    public string? PostPatchWindowsExecutable { get; set; }

    [CommandSwitch("--post-patch-windows-success-codes")]
    public string[]? PostPatchWindowsSuccessCodes { get; set; }

    [CommandSwitch("--pre-patch-linux-executable")]
    public string? PrePatchLinuxExecutable { get; set; }

    [CommandSwitch("--pre-patch-linux-success-codes")]
    public string[]? PrePatchLinuxSuccessCodes { get; set; }

    [CommandSwitch("--pre-patch-windows-executable")]
    public string? PrePatchWindowsExecutable { get; set; }

    [CommandSwitch("--pre-patch-windows-success-codes")]
    public string[]? PrePatchWindowsSuccessCodes { get; set; }

    [CommandSwitch("--rollout-mode")]
    public string? RolloutMode { get; set; }

    [BooleanCommandSwitch("concurrent-zones")]
    public bool? ConcurrentZones { get; set; }

    [BooleanCommandSwitch("zone-by-zone")]
    public bool? ZoneByZone { get; set; }

    [CommandSwitch("--rollout-disruption-budget")]
    public string? RolloutDisruptionBudget { get; set; }

    [CommandSwitch("--rollout-disruption-budget-percent")]
    public string? RolloutDisruptionBudgetPercent { get; set; }

    [CommandSwitch("--windows-exclusive-patches")]
    public string[]? WindowsExclusivePatches { get; set; }

    [CommandSwitch("--windows-classifications")]
    public string[]? WindowsClassifications { get; set; }

    [CommandSwitch("--windows-excludes")]
    public string[]? WindowsExcludes { get; set; }

    [CommandSwitch("--yum-exclusive-packages")]
    public string[]? YumExclusivePackages { get; set; }

    [CommandSwitch("--yum-excludes")]
    public string[]? YumExcludes { get; set; }

    [BooleanCommandSwitch("--yum-minimal")]
    public bool? YumMinimal { get; set; }

    [BooleanCommandSwitch("--yum-security")]
    public bool? YumSecurity { get; set; }

    [CommandSwitch("--zypper-exclusive-patches")]
    public string[]? ZypperExclusivePatches { get; set; }

    [CommandSwitch("--zypper-categories")]
    public string[]? ZypperCategories { get; set; }

    [CommandSwitch("--zypper-excludes")]
    public string[]? ZypperExcludes { get; set; }

    [CommandSwitch("--zypper-severities")]
    public string[]? ZypperSeverities { get; set; }

    [BooleanCommandSwitch("--zypper-with-optional")]
    public bool? ZypperWithOptional { get; set; }

    [BooleanCommandSwitch("--zypper-with-update")]
    public bool? ZypperWithUpdate { get; set; }
}