using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-devices", "claim-devices-by-claim-code")]
public record AwsIot1clickDevicesClaimDevicesByClaimCodeOptions(
[property: CliOption("--claim-code")] string ClaimCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}