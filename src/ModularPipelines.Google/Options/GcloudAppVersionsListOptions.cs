using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "versions", "list")]
public record GcloudAppVersionsListOptions : GcloudOptions
{
    [CliFlag("--hide-no-traffic")]
    public bool? HideNoTraffic { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}