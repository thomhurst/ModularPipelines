using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "asymmetric-sign")]
public record GcloudKmsAsymmetricSignOptions(
[property: CommandSwitch("--input-file")] string InputFile,
[property: CommandSwitch("--signature-file")] string SignatureFile
) : GcloudOptions
{
    [CommandSwitch("--digest-algorithm")]
    public string? DigestAlgorithm { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--skip-integrity-verification")]
    public bool? SkipIntegrityVerification { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}