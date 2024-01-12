using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "remove-path-matcher")]
public record GcloudComputeUrlMapsRemovePathMatcherOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--path-matcher-name")] string PathMatcherName
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}