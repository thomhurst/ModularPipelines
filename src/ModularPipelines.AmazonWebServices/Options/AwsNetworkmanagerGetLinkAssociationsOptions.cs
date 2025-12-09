using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-link-associations")]
public record AwsNetworkmanagerGetLinkAssociationsOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}