using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "update")]
public record GcloudComputeInstancesUpdateOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--[no-]deletion-protection")]
    public string[]? NoDeletionProtection { get; set; }

    [CommandSwitch("--[no-]enable-display-device")]
    public string[]? NoEnableDisplayDevice { get; set; }

    [CommandSwitch("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CommandSwitch("--[no-]shielded-integrity-monitoring")]
    public string[]? NoShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-learn-integrity-policy")]
    public bool? ShieldedLearnIntegrityPolicy { get; set; }

    [CommandSwitch("--[no-]shielded-secure-boot")]
    public string[]? NoShieldedSecureBoot { get; set; }

    [CommandSwitch("--[no-]shielded-vtpm")]
    public string[]? NoShieldedVtpm { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-node-affinities")]
    public bool? ClearNodeAffinities { get; set; }

    [CommandSwitch("--node")]
    public string? Node { get; set; }

    [CommandSwitch("--node-affinity-file")]
    public string? NodeAffinityFile { get; set; }

    [BooleanCommandSwitch("key")]
    public bool? Key { get; set; }

    [BooleanCommandSwitch("operator")]
    public bool? Operator { get; set; }

    [BooleanCommandSwitch("values")]
    public bool? Values { get; set; }

    [CommandSwitch("--node-group")]
    public string? NodeGroup { get; set; }
}