using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "transfer-input-device")]
public record AwsMedialiveTransferInputDeviceOptions(
[property: CliOption("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CliOption("--target-customer-id")]
    public string? TargetCustomerId { get; set; }

    [CliOption("--target-region")]
    public string? TargetRegion { get; set; }

    [CliOption("--transfer-message")]
    public string? TransferMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}