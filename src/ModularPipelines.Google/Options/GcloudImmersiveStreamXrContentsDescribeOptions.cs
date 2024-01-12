using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "contents", "describe")]
public record GcloudImmersiveStreamXrContentsDescribeOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Location
) : GcloudOptions;