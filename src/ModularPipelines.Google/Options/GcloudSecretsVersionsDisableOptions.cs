using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "disable")]
public record GcloudSecretsVersionsDisableOptions : GcloudOptions
{
    public GcloudSecretsVersionsDisableOptions(
        string version,
        string secret
    )
    {
        GcloudSecretsVersionsDisableOptionsVersion = version;
        Secret = secret;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudSecretsVersionsDisableOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Secret { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}
