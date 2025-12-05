using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "sbom", "list")]
public record GcloudArtifactsSbomListOptions : GcloudOptions
{
    [CliOption("--dependency")]
    public string? Dependency { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-prefix")]
    public string? ResourcePrefix { get; set; }
}