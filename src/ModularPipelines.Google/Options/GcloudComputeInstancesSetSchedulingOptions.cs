using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "set-scheduling")]
public record GcloudComputeInstancesSetSchedulingOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliFlag("--clear-min-node-cpu")]
    public bool? ClearMinNodeCpu { get; set; }

    [CliOption("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CliOption("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CliOption("--min-node-cpu")]
    public string? MinNodeCpu { get; set; }

    [CliOption("--[no-]preemptible")]
    public string[]? NoPreemptible { get; set; }

    [CliOption("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CliOption("--[no-]restart-on-failure")]
    public string[]? NoRestartOnFailure { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--clear-instance-termination-action")]
    public bool? ClearInstanceTerminationAction { get; set; }

    [CliOption("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CliFlag("DELETE")]
    public bool? Delete { get; set; }

    [CliFlag("STOP")]
    public bool? Stop { get; set; }

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