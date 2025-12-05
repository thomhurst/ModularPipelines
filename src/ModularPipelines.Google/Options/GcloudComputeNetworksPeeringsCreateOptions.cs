using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "peerings", "create")]
public record GcloudComputeNetworksPeeringsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network,
[property: CliOption("--peer-network")] string PeerNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--auto-create-routes")]
    public bool? AutoCreateRoutes { get; set; }

    [CliFlag("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [CliFlag("--export-subnet-routes-with-public-ip")]
    public bool? ExportSubnetRoutesWithPublicIp { get; set; }

    [CliFlag("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [CliFlag("--import-subnet-routes-with-public-ip")]
    public bool? ImportSubnetRoutesWithPublicIp { get; set; }

    [CliOption("--peer-project")]
    public string? PeerProject { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }
}