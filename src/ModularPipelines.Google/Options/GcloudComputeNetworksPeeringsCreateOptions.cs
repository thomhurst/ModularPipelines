using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "peerings", "create")]
public record GcloudComputeNetworksPeeringsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--peer-network")] string PeerNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--auto-create-routes")]
    public bool? AutoCreateRoutes { get; set; }

    [BooleanCommandSwitch("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--export-subnet-routes-with-public-ip")]
    public bool? ExportSubnetRoutesWithPublicIp { get; set; }

    [BooleanCommandSwitch("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--import-subnet-routes-with-public-ip")]
    public bool? ImportSubnetRoutesWithPublicIp { get; set; }

    [CommandSwitch("--peer-project")]
    public string? PeerProject { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }
}