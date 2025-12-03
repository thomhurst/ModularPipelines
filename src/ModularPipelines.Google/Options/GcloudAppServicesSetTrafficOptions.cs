using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "services", "set-traffic")]
public record GcloudAppServicesSetTrafficOptions(
[property: CliArgument] string Services,
[property: CliOption("--splits")] string[] Splits
) : GcloudOptions
{
    [CliFlag("--migrate")]
    public bool? Migrate { get; set; }

    [CliOption("--split-by")]
    public string? SplitBy { get; set; }
}