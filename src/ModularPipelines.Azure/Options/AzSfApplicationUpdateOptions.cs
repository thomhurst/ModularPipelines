using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "application", "update")]
public record AzSfApplicationUpdateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CliOption("--application-type-version")]
    public string? ApplicationTypeVersion { get; set; }

    [CliFlag("--consider-warning-as-error")]
    public bool? ConsiderWarningAsError { get; set; }

    [CliOption("--failure-action")]
    public string? FailureAction { get; set; }

    [CliFlag("--force-restart")]
    public bool? ForceRestart { get; set; }

    [CliOption("--hc-retry-timeout")]
    public string? HcRetryTimeout { get; set; }

    [CliOption("--hc-stable-duration")]
    public string? HcStableDuration { get; set; }

    [CliOption("--hc-wait-duration")]
    public string? HcWaitDuration { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--max-porcent-unhealthy-apps")]
    public string? MaxPorcentUnhealthyApps { get; set; }

    [CliOption("--max-porcent-unhealthy-partitions")]
    public string? MaxPorcentUnhealthyPartitions { get; set; }

    [CliOption("--max-porcent-unhealthy-replicas")]
    public string? MaxPorcentUnhealthyReplicas { get; set; }

    [CliOption("--max-porcent-unhealthy-services")]
    public string? MaxPorcentUnhealthyServices { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }

    [CliOption("--rep-check-timeout")]
    public string? RepCheckTimeout { get; set; }

    [CliOption("--service-type-health-policy-map")]
    public string? ServiceTypeHealthPolicyMap { get; set; }

    [CliOption("--ud-timeout")]
    public string? UdTimeout { get; set; }

    [CliOption("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}