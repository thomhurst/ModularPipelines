using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "clusters", "create")]
public record GcloudWorkstationsClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliFlag("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }
}