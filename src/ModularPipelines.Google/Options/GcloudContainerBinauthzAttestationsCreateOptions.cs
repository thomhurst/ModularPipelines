using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestations", "create")]
public record GcloudContainerBinauthzAttestationsCreateOptions(
[property: CommandSwitch("--artifact-url")] string ArtifactUrl,
[property: CommandSwitch("--public-key-id")] string PublicKeyId,
[property: CommandSwitch("--signature-file")] string SignatureFile
) : GcloudOptions
{
    [CommandSwitch("--payload-file")]
    public string? PayloadFile { get; set; }

    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [CommandSwitch("--note-project")]
    public string? NoteProject { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--attestor")]
    public string? Attestor { get; set; }

    [CommandSwitch("--attestor-project")]
    public string? AttestorProject { get; set; }
}