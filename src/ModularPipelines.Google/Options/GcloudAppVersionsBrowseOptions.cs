using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "versions", "browse")]
public record GcloudAppVersionsBrowseOptions(
[property: CliArgument] string Versions
) : GcloudOptions
{
    [CliFlag("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}