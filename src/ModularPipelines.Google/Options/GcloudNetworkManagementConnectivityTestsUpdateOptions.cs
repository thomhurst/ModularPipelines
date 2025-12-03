using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "update")]
public record GcloudNetworkManagementConnectivityTestsUpdateOptions(
[property: CliArgument] string ConnectivityTest
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

    [CliFlag("--clear-destination-cloud-sql-instance")]
    public bool? ClearDestinationCloudSqlInstance { get; set; }

    [CliOption("--destination-cloud-sql-instance")]
    public string? DestinationCloudSqlInstance { get; set; }

    [CliFlag("--clear-destination-forwarding-rule")]
    public bool? ClearDestinationForwardingRule { get; set; }

    [CliOption("--destination-forwarding-rule")]
    public string? DestinationForwardingRule { get; set; }

    [CliFlag("--clear-destination-gke-master-cluster")]
    public bool? ClearDestinationGkeMasterCluster { get; set; }

    [CliOption("--destination-gke-master-cluster")]
    public string? DestinationGkeMasterCluster { get; set; }

    [CliFlag("--clear-destination-instance")]
    public bool? ClearDestinationInstance { get; set; }

    [CliOption("--destination-instance")]
    public string? DestinationInstance { get; set; }

    [CliFlag("--clear-destination-ip-address")]
    public bool? ClearDestinationIpAddress { get; set; }

    [CliOption("--destination-ip-address")]
    public string? DestinationIpAddress { get; set; }

    [CliFlag("--clear-source-app-engine-version")]
    public bool? ClearSourceAppEngineVersion { get; set; }

    [CliOption("--source-app-engine-version")]
    public string? SourceAppEngineVersion { get; set; }

    [CliFlag("--clear-source-cloud-function")]
    public bool? ClearSourceCloudFunction { get; set; }

    [CliOption("--source-cloud-function")]
    public string? SourceCloudFunction { get; set; }

    [CliFlag("--clear-source-cloud-run-revision")]
    public bool? ClearSourceCloudRunRevision { get; set; }

    [CliOption("--source-cloud-run-revision")]
    public string? SourceCloudRunRevision { get; set; }

    [CliFlag("--clear-source-cloud-sql-instance")]
    public bool? ClearSourceCloudSqlInstance { get; set; }

    [CliOption("--source-cloud-sql-instance")]
    public string? SourceCloudSqlInstance { get; set; }

    [CliFlag("--clear-source-gke-master-cluster")]
    public bool? ClearSourceGkeMasterCluster { get; set; }

    [CliOption("--source-gke-master-cluster")]
    public string? SourceGkeMasterCluster { get; set; }

    [CliFlag("--clear-source-instance")]
    public bool? ClearSourceInstance { get; set; }

    [CliOption("--source-instance")]
    public string? SourceInstance { get; set; }

    [CliFlag("--clear-source-ip-address")]
    public bool? ClearSourceIpAddress { get; set; }

    [CliOption("--source-ip-address")]
    public string? SourceIpAddress { get; set; }
}