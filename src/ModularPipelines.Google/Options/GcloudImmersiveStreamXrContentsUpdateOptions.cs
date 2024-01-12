using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("immersive-stream", "xr", "contents", "update")]
public record GcloudImmersiveStreamXrContentsUpdateOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--bucket")] string Bucket
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}