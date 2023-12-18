using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "application", "list")]
public record AzSfApplicationListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CommandSwitch("--application-type-version")]
    public string? ApplicationTypeVersion { get; set; }

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

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--max-porcent-unhealthy-apps")]
    public string? MaxPorcentUnhealthyApps { get; set; }

    [CommandSwitch("--max-porcent-unhealthy-partitions")]
    public string? MaxPorcentUnhealthyPartitions { get; set; }

    [CommandSwitch("--max-porcent-unhealthy-replicas")]
    public string? MaxPorcentUnhealthyReplicas { get; set; }

    [CommandSwitch("--max-porcent-unhealthy-services")]
    public string? MaxPorcentUnhealthyServices { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--rep-check-timeout")]
    public string? RepCheckTimeout { get; set; }

    [CommandSwitch("--service-type-health-policy-map")]
    public string? ServiceTypeHealthPolicyMap { get; set; }

    [CommandSwitch("--ud-timeout")]
    public string? UdTimeout { get; set; }

    [CommandSwitch("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}