using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "instances", "describe")]
public record GcloudImmersiveStreamXrInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;