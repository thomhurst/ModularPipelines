using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "update")]
public record GcloudComputeInstanceGroupsManagedUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--[no-]force-update-on-repair")]
    public string[]? NoForceUpdateOnRepair { get; set; }

    [CommandSwitch("--instance-redistribution-type")]
    public string? InstanceRedistributionType { get; set; }

    [CommandSwitch("--list-managed-instances-results")]
    public string? ListManagedInstancesResults { get; set; }

    [CommandSwitch("--remove-stateful-disks")]
    public string[]? RemoveStatefulDisks { get; set; }

    [CommandSwitch("--remove-stateful-external-ips")]
    public string[]? RemoveStatefulExternalIps { get; set; }

    [CommandSwitch("--remove-stateful-internal-ips")]
    public string[]? RemoveStatefulInternalIps { get; set; }

    [CommandSwitch("--stateful-disk")]
    public string[]? StatefulDisk { get; set; }

    [CommandSwitch("--stateful-external-ip")]
    public string[]? StatefulExternalIp { get; set; }

    [CommandSwitch("--stateful-internal-ip")]
    public string[]? StatefulInternalIp { get; set; }

    [CommandSwitch("--target-distribution-shape")]
    public string? TargetDistributionShape { get; set; }

    [BooleanCommandSwitch("--clear-autohealing")]
    public bool? ClearAutohealing { get; set; }

    [CommandSwitch("--initial-delay")]
    public string? InitialDelay { get; set; }

    [CommandSwitch("--health-check")]
    public string? HealthCheck { get; set; }

    [CommandSwitch("--http-health-check")]
    public string? HttpHealthCheck { get; set; }

    [CommandSwitch("--https-health-check")]
    public string? HttpsHealthCheck { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--update-policy-max-surge")]
    public string? UpdatePolicyMaxSurge { get; set; }

    [CommandSwitch("--update-policy-max-unavailable")]
    public string? UpdatePolicyMaxUnavailable { get; set; }

    [CommandSwitch("--update-policy-minimal-action")]
    public string? UpdatePolicyMinimalAction { get; set; }

    [BooleanCommandSwitch("none")]
    public bool? None { get; set; }

    [BooleanCommandSwitch("refresh")]
    public bool? Refresh { get; set; }

    [BooleanCommandSwitch("restart")]
    public bool? Restart { get; set; }

    [BooleanCommandSwitch("replace")]
    public bool? Replace { get; set; }

    [CommandSwitch("--update-policy-most-disruptive-action")]
    public string? UpdatePolicyMostDisruptiveAction { get; set; }

    [CommandSwitch("--update-policy-replacement-method")]
    public string? UpdatePolicyReplacementMethod { get; set; }

    [BooleanCommandSwitch("recreate")]
    public bool? Recreate { get; set; }

    [BooleanCommandSwitch("substitute")]
    public bool? Substitute { get; set; }

    [CommandSwitch("--update-policy-type")]
    public string? UpdatePolicyType { get; set; }

    [BooleanCommandSwitch("opportunistic")]
    public bool? Opportunistic { get; set; }

    [BooleanCommandSwitch("proactive")]
    public bool? Proactive { get; set; }
}