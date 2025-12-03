using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-jobs", "execute")]
public record GcloudComputeOsConfigPatchJobsExecuteOptions(
[property: CliFlag("--instance-filter-all")] bool InstanceFilterAll,
[property: CliOption("--instance-filter-group-labels")] IEnumerable<KeyValue> InstanceFilterGroupLabels,
[property: CliOption("--instance-filter-name-prefixes")] string[] InstanceFilterNamePrefixes,
[property: CliOption("--instance-filter-names")] string[] InstanceFilterNames,
[property: CliOption("--instance-filter-zones")] string[] InstanceFilterZones
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliFlag("--mig-instances-allowed")]
    public bool? MigInstancesAllowed { get; set; }

    [CliOption("--reboot-config")]
    public string? RebootConfig { get; set; }

    [CliFlag("--apt-dist")]
    public bool? AptDist { get; set; }

    [CliOption("--apt-excludes")]
    public string[]? AptExcludes { get; set; }

    [CliOption("--apt-exclusive-packages")]
    public string[]? AptExclusivePackages { get; set; }

    [CliOption("--post-patch-linux-executable")]
    public string? PostPatchLinuxExecutable { get; set; }

    [CliOption("--post-patch-linux-success-codes")]
    public string[]? PostPatchLinuxSuccessCodes { get; set; }

    [CliOption("--post-patch-windows-executable")]
    public string? PostPatchWindowsExecutable { get; set; }

    [CliOption("--post-patch-windows-success-codes")]
    public string[]? PostPatchWindowsSuccessCodes { get; set; }

    [CliOption("--pre-patch-linux-executable")]
    public string? PrePatchLinuxExecutable { get; set; }

    [CliOption("--pre-patch-linux-success-codes")]
    public string[]? PrePatchLinuxSuccessCodes { get; set; }

    [CliOption("--pre-patch-windows-executable")]
    public string? PrePatchWindowsExecutable { get; set; }

    [CliOption("--pre-patch-windows-success-codes")]
    public string[]? PrePatchWindowsSuccessCodes { get; set; }

    [CliOption("--rollout-mode")]
    public string? RolloutMode { get; set; }

    [CliFlag("concurrent-zones")]
    public bool? ConcurrentZones { get; set; }

    [CliFlag("zone-by-zone")]
    public bool? ZoneByZone { get; set; }

    [CliOption("--rollout-disruption-budget")]
    public string? RolloutDisruptionBudget { get; set; }

    [CliOption("--rollout-disruption-budget-percent")]
    public string? RolloutDisruptionBudgetPercent { get; set; }

    [CliOption("--windows-exclusive-patches")]
    public string[]? WindowsExclusivePatches { get; set; }

    [CliOption("--windows-classifications")]
    public string[]? WindowsClassifications { get; set; }

    [CliOption("--windows-excludes")]
    public string[]? WindowsExcludes { get; set; }

    [CliOption("--yum-exclusive-packages")]
    public string[]? YumExclusivePackages { get; set; }

    [CliOption("--yum-excludes")]
    public string[]? YumExcludes { get; set; }

    [CliFlag("--yum-minimal")]
    public bool? YumMinimal { get; set; }

    [CliFlag("--yum-security")]
    public bool? YumSecurity { get; set; }

    [CliOption("--zypper-exclusive-patches")]
    public string[]? ZypperExclusivePatches { get; set; }

    [CliOption("--zypper-categories")]
    public string[]? ZypperCategories { get; set; }

    [CliOption("--zypper-excludes")]
    public string[]? ZypperExcludes { get; set; }

    [CliOption("--zypper-severities")]
    public string[]? ZypperSeverities { get; set; }

    [CliFlag("--zypper-with-optional")]
    public bool? ZypperWithOptional { get; set; }

    [CliFlag("--zypper-with-update")]
    public bool? ZypperWithUpdate { get; set; }
}