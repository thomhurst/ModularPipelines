using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "get-public-key")]
public record GcloudKmsKeysVersionsGetPublicKeyOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsGetPublicKeyOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsGetPublicKeyOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsGetPublicKeyOptionsVersion { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }
}
