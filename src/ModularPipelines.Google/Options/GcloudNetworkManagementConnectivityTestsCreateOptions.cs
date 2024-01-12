using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "connectivity-tests", "create")]
public record GcloudNetworkManagementConnectivityTestsCreateOptions(
[property: PositionalArgument] string ConnectivityTest,
[property: CommandSwitch("--destination-cloud-sql-instance")] string DestinationCloudSqlInstance,
[property: CommandSwitch("--destination-forwarding-rule")] string DestinationForwardingRule,
[property: CommandSwitch("--destination-gke-master-cluster")] string DestinationGkeMasterCluster,
[property: CommandSwitch("--destination-instance")] string DestinationInstance,
[property: CommandSwitch("--destination-ip-address")] string DestinationIpAddress,
[property: CommandSwitch("--source-app-engine-version")] string SourceAppEngineVersion,
[property: CommandSwitch("--source-cloud-function")] string SourceCloudFunction,
[property: CommandSwitch("--source-cloud-run-revision")] string SourceCloudRunRevision,
[property: CommandSwitch("--source-cloud-sql-instance")] string SourceCloudSqlInstance,
[property: CommandSwitch("--source-gke-master-cluster")] string SourceGkeMasterCluster,
[property: CommandSwitch("--source-instance")] string SourceInstance,
[property: CommandSwitch("--source-ip-address")] string SourceIpAddress
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
}