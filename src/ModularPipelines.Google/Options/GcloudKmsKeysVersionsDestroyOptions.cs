using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "destroy")]
public record GcloudKmsKeysVersionsDestroyOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDestroyOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDestroyOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsDestroyOptionsVersion { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
