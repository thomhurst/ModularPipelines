using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "vulnerabilities", "load-vex")]
public record GcloudArtifactsVulnerabilitiesLoadVexOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--uri")] string Uri
) : GcloudOptions
{
    [CliOption("--project")]
    public new string? Project { get; set; }
}