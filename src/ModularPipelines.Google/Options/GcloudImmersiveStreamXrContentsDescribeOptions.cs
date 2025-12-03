using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "contents", "describe")]
public record GcloudImmersiveStreamXrContentsDescribeOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Location
) : GcloudOptions;