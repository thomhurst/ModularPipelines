using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "list")]
public record GcloudAppVersionsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--hide-no-traffic")]
    public bool? HideNoTraffic { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}