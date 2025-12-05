using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "public-keys", "update")]
public record GcloudContainerBinauthzAttestorsPublicKeysUpdateOptions(
[property: CliArgument] string PublicKeyId,
[property: CliOption("--attestor")] string Attestor
) : GcloudOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--pgp-public-key-file")]
    public string? PgpPublicKeyFile { get; set; }
}