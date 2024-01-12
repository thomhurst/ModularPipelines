using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "describe")]
public record GcloudSecretsVersionsDescribeOptions : GcloudOptions
{
    public GcloudSecretsVersionsDescribeOptions(
        string version,
        string secret
    )
    {
        GcloudSecretsVersionsDescribeOptionsVersion = version;
        Secret = secret;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudSecretsVersionsDescribeOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Secret { get; set; }
}
