using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "enable")]
public record GcloudKmsKeysVersionsEnableOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsEnableOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsEnableOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsEnableOptionsVersion { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
