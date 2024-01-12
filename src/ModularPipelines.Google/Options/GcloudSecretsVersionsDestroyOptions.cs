using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "destroy")]
public record GcloudSecretsVersionsDestroyOptions : GcloudOptions
{
    public GcloudSecretsVersionsDestroyOptions(
        string version,
        string secret
    )
    {
        GcloudSecretsVersionsDestroyOptionsVersion = version;
        Secret = secret;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudSecretsVersionsDestroyOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Secret { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}
