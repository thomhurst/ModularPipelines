using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "create")]
public record GcloudContainerBinauthzAttestorsCreateOptions(
[property: PositionalArgument] string Attestor,
[property: CommandSwitch("--attestation-authority-note")] string AttestationAuthorityNote,
[property: CommandSwitch("--attestation-authority-note-project")] string AttestationAuthorityNoteProject
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}