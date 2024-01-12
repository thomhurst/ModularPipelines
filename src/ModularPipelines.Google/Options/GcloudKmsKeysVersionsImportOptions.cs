using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "import")]
public record GcloudKmsKeysVersionsImportOptions(
[property: CommandSwitch("--algorithm")] string Algorithm,
[property: CommandSwitch("--import-job")] string ImportJob
) : GcloudOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [CommandSwitch("--target-key-file")]
    public string? TargetKeyFile { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--wrapped-key-file")]
    public string? WrappedKeyFile { get; set; }
}