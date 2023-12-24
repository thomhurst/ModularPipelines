using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot1click-devices", "claim-devices-by-claim-code")]
public record AwsIot1clickDevicesClaimDevicesByClaimCodeOptions(
[property: CommandSwitch("--claim-code")] string ClaimCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}