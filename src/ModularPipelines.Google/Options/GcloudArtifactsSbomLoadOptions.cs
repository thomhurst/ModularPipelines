using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "sbom", "load")]
public record GcloudArtifactsSbomLoadOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--uri")] string Uri
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--kms-key-version")]
    public string? KmsKeyVersion { get; set; }
}