using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "asymmetric-decrypt")]
public record GcloudKmsAsymmetricDecryptOptions(
[property: CliOption("--ciphertext-file")] string CiphertextFile,
[property: CliOption("--plaintext-file")] string PlaintextFile
) : GcloudOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--skip-integrity-verification")]
    public bool? SkipIntegrityVerification { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}