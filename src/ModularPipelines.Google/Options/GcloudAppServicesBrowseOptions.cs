using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "services", "browse")]
public record GcloudAppServicesBrowseOptions(
[property: CliArgument] string Services
) : GcloudOptions
{
    [CliFlag("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}