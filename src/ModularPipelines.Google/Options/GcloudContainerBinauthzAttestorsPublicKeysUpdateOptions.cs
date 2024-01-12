using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "public-keys", "update")]
public record GcloudContainerBinauthzAttestorsPublicKeysUpdateOptions(
[property: PositionalArgument] string PublicKeyId,
[property: CommandSwitch("--attestor")] string Attestor
) : GcloudOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--pgp-public-key-file")]
    public string? PgpPublicKeyFile { get; set; }
}