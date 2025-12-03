using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "public-keys", "add")]
public record GcloudContainerBinauthzAttestorsPublicKeysAddOptions(
[property: CliOption("--attestor")] string Attestor,
[property: CliOption("--pgp-public-key-file")] string PgpPublicKeyFile,
[property: CliOption("--keyversion")] string Keyversion,
[property: CliOption("--keyversion-key")] string KeyversionKey,
[property: CliOption("--keyversion-keyring")] string KeyversionKeyring,
[property: CliOption("--keyversion-location")] string KeyversionLocation,
[property: CliOption("--keyversion-project")] string KeyversionProject,
[property: CliOption("--pkix-public-key-algorithm")] string PkixPublicKeyAlgorithm,
[property: CliOption("--pkix-public-key-file")] string PkixPublicKeyFile
) : GcloudOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--public-key-id-override")]
    public string? PublicKeyIdOverride { get; set; }
}