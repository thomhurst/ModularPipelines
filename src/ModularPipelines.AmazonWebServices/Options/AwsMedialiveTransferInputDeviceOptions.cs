using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "transfer-input-device")]
public record AwsMedialiveTransferInputDeviceOptions(
[property: CommandSwitch("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CommandSwitch("--target-customer-id")]
    public string? TargetCustomerId { get; set; }

    [CommandSwitch("--target-region")]
    public string? TargetRegion { get; set; }

    [CommandSwitch("--transfer-message")]
    public string? TransferMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}