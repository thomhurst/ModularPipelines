using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "log")]
public record GcloudBuildsLogOptions(
[property: CliArgument] string Build
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--stream")]
    public bool? Stream { get; set; }
}