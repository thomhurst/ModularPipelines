using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "encrypt")]
public record GcloudKmsEncryptOptions(
[property: CliOption("--ciphertext-file")] string CiphertextFile,
[property: CliOption("--plaintext-file")] string PlaintextFile
) : GcloudOptions
{
    [CliOption("--additional-authenticated-data-file")]
    public string? AdditionalAuthenticatedDataFile { get; set; }

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