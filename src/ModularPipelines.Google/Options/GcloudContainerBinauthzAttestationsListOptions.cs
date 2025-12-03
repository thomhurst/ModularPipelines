using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestations", "list")]
public record GcloudContainerBinauthzAttestationsListOptions : GcloudOptions
{
    [CliOption("--artifact-url")]
    public string? ArtifactUrl { get; set; }

    [CliOption("--attestor")]
    public string? Attestor { get; set; }

    [CliOption("--attestor-project")]
    public string? AttestorProject { get; set; }
}