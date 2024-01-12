using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "connectivity-tests", "update")]
public record GcloudNetworkManagementConnectivityTestsUpdateOptions(
[property: PositionalArgument] string ConnectivityTest
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-network")]
    public string? DestinationNetwork { get; set; }

    [CommandSwitch("--destination-port")]
    public string? DestinationPort { get; set; }

    [CommandSwitch("--destination-project")]
    public string? DestinationProject { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--other-projects")]
    public string[]? OtherProjects { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--source-network")]
    public string? SourceNetwork { get; set; }

    [CommandSwitch("--source-network-type")]
    public string? SourceNetworkType { get; set; }

    [CommandSwitch("--source-project")]
    public string? SourceProject { get; set; }

    [BooleanCommandSwitch("--clear-destination-cloud-sql-instance")]
    public bool? ClearDestinationCloudSqlInstance { get; set; }

    [CommandSwitch("--destination-cloud-sql-instance")]
    public string? DestinationCloudSqlInstance { get; set; }

    [BooleanCommandSwitch("--clear-destination-forwarding-rule")]
    public bool? ClearDestinationForwardingRule { get; set; }

    [CommandSwitch("--destination-forwarding-rule")]
    public string? DestinationForwardingRule { get; set; }

    [BooleanCommandSwitch("--clear-destination-gke-master-cluster")]
    public bool? ClearDestinationGkeMasterCluster { get; set; }

    [CommandSwitch("--destination-gke-master-cluster")]
    public string? DestinationGkeMasterCluster { get; set; }

    [BooleanCommandSwitch("--clear-destination-instance")]
    public bool? ClearDestinationInstance { get; set; }

    [CommandSwitch("--destination-instance")]
    public string? DestinationInstance { get; set; }

    [BooleanCommandSwitch("--clear-destination-ip-address")]
    public bool? ClearDestinationIpAddress { get; set; }

    [CommandSwitch("--destination-ip-address")]
    public string? DestinationIpAddress { get; set; }

    [BooleanCommandSwitch("--clear-source-app-engine-version")]
    public bool? ClearSourceAppEngineVersion { get; set; }

    [CommandSwitch("--source-app-engine-version")]
    public string? SourceAppEngineVersion { get; set; }

    [BooleanCommandSwitch("--clear-source-cloud-function")]
    public bool? ClearSourceCloudFunction { get; set; }

    [CommandSwitch("--source-cloud-function")]
    public string? SourceCloudFunction { get; set; }

    [BooleanCommandSwitch("--clear-source-cloud-run-revision")]
    public bool? ClearSourceCloudRunRevision { get; set; }

    [CommandSwitch("--source-cloud-run-revision")]
    public string? SourceCloudRunRevision { get; set; }

    [BooleanCommandSwitch("--clear-source-cloud-sql-instance")]
    public bool? ClearSourceCloudSqlInstance { get; set; }

    [CommandSwitch("--source-cloud-sql-instance")]
    public string? SourceCloudSqlInstance { get; set; }

    [BooleanCommandSwitch("--clear-source-gke-master-cluster")]
    public bool? ClearSourceGkeMasterCluster { get; set; }

    [CommandSwitch("--source-gke-master-cluster")]
    public string? SourceGkeMasterCluster { get; set; }

    [BooleanCommandSwitch("--clear-source-instance")]
    public bool? ClearSourceInstance { get; set; }

    [CommandSwitch("--source-instance")]
    public string? SourceInstance { get; set; }

    [BooleanCommandSwitch("--clear-source-ip-address")]
    public bool? ClearSourceIpAddress { get; set; }

    [CommandSwitch("--source-ip-address")]
    public string? SourceIpAddress { get; set; }
}