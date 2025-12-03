using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "contents", "update")]
public record GcloudImmersiveStreamXrContentsUpdateOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Location,
[property: CliOption("--bucket")] string Bucket
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}