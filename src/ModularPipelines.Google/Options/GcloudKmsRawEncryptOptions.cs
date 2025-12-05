using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "raw-encrypt")]
public record GcloudKmsRawEncryptOptions : GcloudOptions
{
    public GcloudKmsRawEncryptOptions(
        string ciphertextFile,
        string plaintextFile,
        string version
    )
    {
        CiphertextFile = ciphertextFile;
        PlaintextFile = plaintextFile;
        Version = version;
    }

    [CliOption("--ciphertext-file")]
    public string CiphertextFile { get; set; }

    [CliOption("--plaintext-file")]
    public string PlaintextFile { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliOption("--additional-authenticated-data-file")]
    public string? AdditionalAuthenticatedDataFile { get; set; }

    [CliOption("--initialization-vector-file")]
    public string? InitializationVectorFile { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--skip-integrity-verification")]
    public bool? SkipIntegrityVerification { get; set; }
}
