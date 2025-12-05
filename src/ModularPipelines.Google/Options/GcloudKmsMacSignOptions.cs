using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "mac-sign")]
public record GcloudKmsMacSignOptions(
[property: CliOption("--input-file")] string InputFile,
[property: CliOption("--signature-file")] string SignatureFile
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