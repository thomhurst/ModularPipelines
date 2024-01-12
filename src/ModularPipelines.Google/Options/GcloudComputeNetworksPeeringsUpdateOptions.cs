using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "peerings", "update")]
public record GcloudComputeNetworksPeeringsUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--export-subnet-routes-with-public-ip")]
    public bool? ExportSubnetRoutesWithPublicIp { get; set; }

    [BooleanCommandSwitch("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--import-subnet-routes-with-public-ip")]
    public bool? ImportSubnetRoutesWithPublicIp { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }
}