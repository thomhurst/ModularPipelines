using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "create")]
public record GcloudNetworkManagementConnectivityTestsCreateOptions(
[property: CliArgument] string ConnectivityTest,
[property: CliOption("--destination-cloud-sql-instance")] string DestinationCloudSqlInstance,
[property: CliOption("--destination-forwarding-rule")] string DestinationForwardingRule,
[property: CliOption("--destination-gke-master-cluster")] string DestinationGkeMasterCluster,
[property: CliOption("--destination-instance")] string DestinationInstance,
[property: CliOption("--destination-ip-address")] string DestinationIpAddress,
[property: CliOption("--source-app-engine-version")] string SourceAppEngineVersion,
[property: CliOption("--source-cloud-function")] string SourceCloudFunction,
[property: CliOption("--source-cloud-run-revision")] string SourceCloudRunRevision,
[property: CliOption("--source-cloud-sql-instance")] string SourceCloudSqlInstance,
[property: CliOption("--source-gke-master-cluster")] string SourceGkeMasterCluster,
[property: CliOption("--source-instance")] string SourceInstance,
[property: CliOption("--source-ip-address")] string SourceIpAddress
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-network")]
    public string? DestinationNetwork { get; set; }

    [CliOption("--destination-port")]
    public string? DestinationPort { get; set; }

    [CliOption("--destination-project")]
    public string? DestinationProject { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--other-projects")]
    public string[]? OtherProjects { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--source-network")]
    public string? SourceNetwork { get; set; }

    [CliOption("--source-network-type")]
    public string? SourceNetworkType { get; set; }

    [CliOption("--source-project")]
    public string? SourceProject { get; set; }
}