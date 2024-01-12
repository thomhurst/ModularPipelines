using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "access")]
public record GcloudSecretsVersionsAccessOptions : GcloudOptions
{
    public GcloudSecretsVersionsAccessOptions(
        string version,
        string secret
    )
    {
        GcloudSecretsVersionsAccessOptionsVersion = version;
        Secret = secret;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudSecretsVersionsAccessOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Secret { get; set; }

    [CommandSwitch("--out-file")]
    public string? OutFile { get; set; }
}
