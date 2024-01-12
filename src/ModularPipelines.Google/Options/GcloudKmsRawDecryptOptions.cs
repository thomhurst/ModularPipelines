using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "raw-decrypt")]
public record GcloudKmsRawDecryptOptions : GcloudOptions
{
    public GcloudKmsRawDecryptOptions(
        string ciphertextFile,
        string plaintextFile,
        string version
    )
    {
        CiphertextFile = ciphertextFile;
        PlaintextFile = plaintextFile;
        Version = version;
    }

    [CommandSwitch("--ciphertext-file")]
    public string CiphertextFile { get; set; }

    [CommandSwitch("--plaintext-file")]
    public string PlaintextFile { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--additional-authenticated-data-file")]
    public string? AdditionalAuthenticatedDataFile { get; set; }

    [CommandSwitch("--initialization-vector-file")]
    public string? InitializationVectorFile { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--skip-integrity-verification")]
    public bool? SkipIntegrityVerification { get; set; }
}
