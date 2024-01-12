using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "add-host-rule")]
public record GcloudComputeUrlMapsAddHostRuleOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--hosts")] string[] Hosts,
[property: CommandSwitch("--path-matcher-name")] string PathMatcherName
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}