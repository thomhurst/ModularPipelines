using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "public-keys", "add")]
public record GcloudContainerBinauthzAttestorsPublicKeysAddOptions(
[property: CommandSwitch("--attestor")] string Attestor,
[property: CommandSwitch("--pgp-public-key-file")] string PgpPublicKeyFile,
[property: CommandSwitch("--keyversion")] string Keyversion,
[property: CommandSwitch("--keyversion-key")] string KeyversionKey,
[property: CommandSwitch("--keyversion-keyring")] string KeyversionKeyring,
[property: CommandSwitch("--keyversion-location")] string KeyversionLocation,
[property: CommandSwitch("--keyversion-project")] string KeyversionProject,
[property: CommandSwitch("--pkix-public-key-algorithm")] string PkixPublicKeyAlgorithm,
[property: CommandSwitch("--pkix-public-key-file")] string PkixPublicKeyFile
) : GcloudOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--public-key-id-override")]
    public string? PublicKeyIdOverride { get; set; }
}