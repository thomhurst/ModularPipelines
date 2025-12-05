using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-host-reservation-purchase-preview")]
public record AwsEc2GetHostReservationPurchasePreviewOptions(
[property: CliOption("--host-id-set")] string[] HostIdSet,
[property: CliOption("--offering-id")] string OfferingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}