using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "create")]
public record GcloudContainerBinauthzAttestorsCreateOptions(
[property: CliArgument] string Attestor,
[property: CliOption("--attestation-authority-note")] string AttestationAuthorityNote,
[property: CliOption("--attestation-authority-note-project")] string AttestationAuthorityNoteProject
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}