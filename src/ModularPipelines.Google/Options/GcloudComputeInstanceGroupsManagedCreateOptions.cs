using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "create")]
public record GcloudComputeInstanceGroupsManagedCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--size")] string Size,
[property: CliOption("--template")] string Template
) : GcloudOptions
{
    [CliOption("--base-instance-name")]
    public string? BaseInstanceName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--[no-]force-update-on-repair")]
    public string[]? NoForceUpdateOnRepair { get; set; }

    [CliOption("--initial-delay")]
    public string? InitialDelay { get; set; }

    [CliOption("--instance-redistribution-type")]
    public string? InstanceRedistributionType { get; set; }

    [CliOption("--list-managed-instances-results")]
    public string? ListManagedInstancesResults { get; set; }

    [CliOption("--stateful-disk")]
    public string[]? StatefulDisk { get; set; }

    [CliOption("--stateful-external-ip")]
    public string[]? StatefulExternalIp { get; set; }

    [CliOption("--stateful-internal-ip")]
    public string[]? StatefulInternalIp { get; set; }

    [CliOption("--target-distribution-shape")]
    public string? TargetDistributionShape { get; set; }

    [CliOption("--target-pool")]
    public string[]? TargetPool { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }

    [CliOption("--health-check")]
    public string? HealthCheck { get; set; }

    [CliOption("--http-health-check")]
    public string? HttpHealthCheck { get; set; }

    [CliOption("--https-health-check")]
    public string? HttpsHealthCheck { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--update-policy-max-surge")]
    public string? UpdatePolicyMaxSurge { get; set; }

    [CliOption("--update-policy-max-unavailable")]
    public string? UpdatePolicyMaxUnavailable { get; set; }

    [CliOption("--update-policy-minimal-action")]
    public string? UpdatePolicyMinimalAction { get; set; }

    [CliFlag("none")]
    public bool? None { get; set; }

    [CliFlag("refresh")]
    public bool? Refresh { get; set; }

    [CliFlag("restart")]
    public bool? Restart { get; set; }

    [CliFlag("replace")]
    public bool? Replace { get; set; }

    [CliOption("--update-policy-most-disruptive-action")]
    public string? UpdatePolicyMostDisruptiveAction { get; set; }

    [CliOption("--update-policy-replacement-method")]
    public string? UpdatePolicyReplacementMethod { get; set; }

    [CliFlag("recreate")]
    public bool? Recreate { get; set; }

    [CliFlag("substitute")]
    public bool? Substitute { get; set; }

    [CliOption("--update-policy-type")]
    public string? UpdatePolicyType { get; set; }

    [CliFlag("opportunistic")]
    public bool? Opportunistic { get; set; }

    [CliFlag("proactive")]
    public bool? Proactive { get; set; }
}