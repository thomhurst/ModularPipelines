using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "import")]
public record GcloudKmsKeysVersionsImportOptions(
[property: CliOption("--algorithm")] string Algorithm,
[property: CliOption("--import-job")] string ImportJob
) : GcloudOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [CliOption("--target-key-file")]
    public string? TargetKeyFile { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--wrapped-key-file")]
    public string? WrappedKeyFile { get; set; }
}