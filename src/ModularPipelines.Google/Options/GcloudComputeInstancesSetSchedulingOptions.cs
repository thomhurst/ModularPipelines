using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "set-scheduling")]
public record GcloudComputeInstancesSetSchedulingOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [BooleanCommandSwitch("--clear-min-node-cpu")]
    public bool? ClearMinNodeCpu { get; set; }

    [CommandSwitch("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CommandSwitch("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CommandSwitch("--min-node-cpu")]
    public string? MinNodeCpu { get; set; }

    [CommandSwitch("--[no-]preemptible")]
    public string[]? NoPreemptible { get; set; }

    [CommandSwitch("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CommandSwitch("--[no-]restart-on-failure")]
    public string[]? NoRestartOnFailure { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--clear-instance-termination-action")]
    public bool? ClearInstanceTerminationAction { get; set; }

    [CommandSwitch("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [BooleanCommandSwitch("DELETE")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("STOP")]
    public bool? Stop { get; set; }

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