using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "update")]
public record GcloudComputeInstancesUpdateOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--[no-]deletion-protection")]
    public string[]? NoDeletionProtection { get; set; }

    [CliOption("--[no-]enable-display-device")]
    public string[]? NoEnableDisplayDevice { get; set; }

    [CliOption("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CliOption("--[no-]shielded-integrity-monitoring")]
    public string[]? NoShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-learn-integrity-policy")]
    public bool? ShieldedLearnIntegrityPolicy { get; set; }

    [CliOption("--[no-]shielded-secure-boot")]
    public string[]? NoShieldedSecureBoot { get; set; }

    [CliOption("--[no-]shielded-vtpm")]
    public string[]? NoShieldedVtpm { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-node-affinities")]
    public bool? ClearNodeAffinities { get; set; }

    [CliOption("--node")]
    public string? Node { get; set; }

    [CliOption("--node-affinity-file")]
    public string? NodeAffinityFile { get; set; }

    [CliFlag("key")]
    public bool? Key { get; set; }

    [CliFlag("operator")]
    public bool? Operator { get; set; }

    [CliFlag("values")]
    public bool? Values { get; set; }

    [CliOption("--node-group")]
    public string? NodeGroup { get; set; }
}