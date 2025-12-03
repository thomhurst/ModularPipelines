using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "peerings", "update")]
public record GcloudComputeNetworksPeeringsUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [CliFlag("--export-subnet-routes-with-public-ip")]
    public bool? ExportSubnetRoutesWithPublicIp { get; set; }

    [CliFlag("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [CliFlag("--import-subnet-routes-with-public-ip")]
    public bool? ImportSubnetRoutesWithPublicIp { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }
}