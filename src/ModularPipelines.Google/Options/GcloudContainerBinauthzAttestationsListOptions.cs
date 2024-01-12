using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestations", "list")]
public record GcloudContainerBinauthzAttestationsListOptions : GcloudOptions
{
    [CommandSwitch("--artifact-url")]
    public string? ArtifactUrl { get; set; }

    [CommandSwitch("--attestor")]
    public string? Attestor { get; set; }

    [CommandSwitch("--attestor-project")]
    public string? AttestorProject { get; set; }
}