using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "describe-input-device-thumbnail")]
public record AwsMedialiveDescribeInputDeviceThumbnailOptions(
[property: CliOption("--input-device-id")] string InputDeviceId,
[property: CliOption("--accept")] string Accept
) : AwsOptions;