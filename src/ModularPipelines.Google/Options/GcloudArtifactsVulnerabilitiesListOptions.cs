using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "vulnerabilities", "list")]
public record GcloudArtifactsVulnerabilitiesListOptions(
[property: CliArgument] string Uri
) : GcloudOptions
{
    [CliOption("--occurrence-filter")]
    public string? OccurrenceFilter { get; set; }
}