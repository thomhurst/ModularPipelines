using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-devices")]
public record AwsNetworkmanagerGetDevicesOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CliOption("--device-ids")]
    public string[]? DeviceIds { get; set; }

    [CliOption("--site-id")]
    public string? SiteId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}