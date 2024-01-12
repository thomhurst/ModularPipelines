using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "remove-host-rule")]
public record GcloudComputeUrlMapsRemoveHostRuleOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--host")] string Host
) : GcloudOptions
{
    [BooleanCommandSwitch("--delete-orphaned-path-matcher")]
    public bool? DeleteOrphanedPathMatcher { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}