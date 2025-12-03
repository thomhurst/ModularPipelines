using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "destroy")]
public record GcloudKmsKeysVersionsDestroyOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDestroyOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDestroyOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsDestroyOptionsVersion { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
