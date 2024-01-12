using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "invalidate-cdn-cache")]
public record GcloudComputeUrlMapsInvalidateCdnCacheOptions(
[property: PositionalArgument] string Urlmap,
[property: CommandSwitch("--path")] string Path
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}