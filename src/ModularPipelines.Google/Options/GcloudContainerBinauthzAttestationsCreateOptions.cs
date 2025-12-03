using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestations", "create")]
public record GcloudContainerBinauthzAttestationsCreateOptions(
[property: CliOption("--artifact-url")] string ArtifactUrl,
[property: CliOption("--public-key-id")] string PublicKeyId,
[property: CliOption("--signature-file")] string SignatureFile
) : GcloudOptions
{
    [CliOption("--payload-file")]
    public string? PayloadFile { get; set; }

    [CliOption("--note")]
    public string? Note { get; set; }

    [CliOption("--note-project")]
    public string? NoteProject { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }

    [CliOption("--attestor")]
    public string? Attestor { get; set; }

    [CliOption("--attestor-project")]
    public string? AttestorProject { get; set; }
}