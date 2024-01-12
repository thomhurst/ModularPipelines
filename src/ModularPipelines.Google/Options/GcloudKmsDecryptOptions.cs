using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "decrypt")]
public record GcloudKmsDecryptOptions(
[property: CommandSwitch("--ciphertext-file")] string CiphertextFile,
[property: CommandSwitch("--plaintext-file")] string PlaintextFile
) : GcloudOptions
{
    [CommandSwitch("--additional-authenticated-data-file")]
    public string? AdditionalAuthenticatedDataFile { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--skip-integrity-verification")]
    public bool? SkipIntegrityVerification { get; set; }
}