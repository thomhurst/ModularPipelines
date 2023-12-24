using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "describe-input-device-thumbnail")]
public record AwsMedialiveDescribeInputDeviceThumbnailOptions(
[property: CommandSwitch("--input-device-id")] string InputDeviceId,
[property: CommandSwitch("--accept")] string Accept
) : AwsOptions;