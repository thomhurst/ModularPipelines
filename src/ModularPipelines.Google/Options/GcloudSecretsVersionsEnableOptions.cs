using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "enable")]
public record GcloudSecretsVersionsEnableOptions : GcloudOptions
{
    public GcloudSecretsVersionsEnableOptions(
        string version,
        string secret
    )
    {
        GcloudSecretsVersionsEnableOptionsVersion = version;
        Secret = secret;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudSecretsVersionsEnableOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Secret { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}
