using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "enable")]
public record GcloudKmsKeysVersionsEnableOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsEnableOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsEnableOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsEnableOptionsVersion { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
