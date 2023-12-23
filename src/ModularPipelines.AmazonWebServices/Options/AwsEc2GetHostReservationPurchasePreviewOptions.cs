using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-host-reservation-purchase-preview")]
public record AwsEc2GetHostReservationPurchasePreviewOptions(
[property: CommandSwitch("--host-id-set")] string[] HostIdSet,
[property: CommandSwitch("--offering-id")] string OfferingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}