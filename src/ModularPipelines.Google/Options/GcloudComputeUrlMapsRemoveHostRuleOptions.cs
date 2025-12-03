using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "remove-host-rule")]
public record GcloudComputeUrlMapsRemoveHostRuleOptions(
[property: CliArgument] string UrlMap,
[property: CliOption("--host")] string Host
) : GcloudOptions
{
    [CliFlag("--delete-orphaned-path-matcher")]
    public bool? DeleteOrphanedPathMatcher { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}