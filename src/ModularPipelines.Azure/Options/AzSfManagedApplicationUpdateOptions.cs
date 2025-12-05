using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-application", "update")]
public record AzSfManagedApplicationUpdateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CliOption("--application-type-version")]
    public string? ApplicationTypeVersion { get; set; }

    [CliOption("--close-duration")]
    public string? CloseDuration { get; set; }

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

    [CliOption("--max-percent-unhealthy-apps")]
    public string? MaxPercentUnhealthyApps { get; set; }

    [CliOption("--max-percent-unhealthy-partitions")]
    public string? MaxPercentUnhealthyPartitions { get; set; }

    [CliOption("--max-percent-unhealthy-replicas")]
    public string? MaxPercentUnhealthyReplicas { get; set; }

    [CliOption("--max-percent-unhealthy-services")]
    public string? MaxPercentUnhealthyServices { get; set; }

    [CliFlag("--recreate-application")]
    public bool? RecreateApplication { get; set; }

    [CliOption("--rep-check-timeout")]
    public string? RepCheckTimeout { get; set; }

    [CliOption("--service-type-health-policy-map")]
    public string? ServiceTypeHealthPolicyMap { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ud-timeout")]
    public string? UdTimeout { get; set; }

    [CliOption("--upgrade-mode")]
    public string? UpgradeMode { get; set; }

    [CliOption("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}