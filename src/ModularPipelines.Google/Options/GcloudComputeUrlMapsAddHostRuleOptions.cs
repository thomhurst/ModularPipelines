using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "add-host-rule")]
public record GcloudComputeUrlMapsAddHostRuleOptions(
[property: CliArgument] string UrlMap,
[property: CliOption("--hosts")] string[] Hosts,
[property: CliOption("--path-matcher-name")] string PathMatcherName
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}