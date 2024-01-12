using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "clusters", "create")]
public record GcloudWorkstationsClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [BooleanCommandSwitch("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }
}