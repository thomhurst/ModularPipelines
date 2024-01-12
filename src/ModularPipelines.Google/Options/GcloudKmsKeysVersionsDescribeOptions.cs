using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "describe")]
public record GcloudKmsKeysVersionsDescribeOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDescribeOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDescribeOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsDescribeOptionsVersion { get; set; }

    [CommandSwitch("--attestation-file")]
    public string? AttestationFile { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
