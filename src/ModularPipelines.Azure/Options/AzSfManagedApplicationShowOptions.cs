using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-application", "show")]
public record AzSfManagedApplicationShowOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CommandSwitch("--application-type-version")]
    public string? ApplicationTypeVersion { get; set; }

    [CommandSwitch("--close-duration")]
    public string? CloseDuration { get; set; }

    [BooleanCommandSwitch("--consider-warning-as-error")]
    public bool? ConsiderWarningAsError { get; set; }

    [CommandSwitch("--failure-action")]
    public string? FailureAction { get; set; }

    [BooleanCommandSwitch("--force-restart")]
    public bool? ForceRestart { get; set; }

    [CommandSwitch("--hc-retry-timeout")]
    public string? HcRetryTimeout { get; set; }

    [CommandSwitch("--hc-stable-duration")]
    public string? HcStableDuration { get; set; }

    [CommandSwitch("--hc-wait-duration")]
    public string? HcWaitDuration { get; set; }

    [CommandSwitch("--max-percent-unhealthy-apps")]
    public string? MaxPercentUnhealthyApps { get; set; }

    [CommandSwitch("--max-percent-unhealthy-partitions")]
    public string? MaxPercentUnhealthyPartitions { get; set; }

    [CommandSwitch("--max-percent-unhealthy-replicas")]
    public string? MaxPercentUnhealthyReplicas { get; set; }

    [CommandSwitch("--max-percent-unhealthy-services")]
    public string? MaxPercentUnhealthyServices { get; set; }

    [BooleanCommandSwitch("--recreate-application")]
    public bool? RecreateApplication { get; set; }

    [CommandSwitch("--rep-check-timeout")]
    public string? RepCheckTimeout { get; set; }

    [CommandSwitch("--service-type-health-policy-map")]
    public string? ServiceTypeHealthPolicyMap { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--ud-timeout")]
    public string? UdTimeout { get; set; }

    [CommandSwitch("--upgrade-mode")]
    public string? UpgradeMode { get; set; }

    [CommandSwitch("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}