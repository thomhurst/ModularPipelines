using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "restore")]
public record GcloudKmsKeysVersionsRestoreOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsRestoreOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsRestoreOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsRestoreOptionsVersion { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
