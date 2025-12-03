using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "describe")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudSecretsVersionsDescribeOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Secret { get; set; }
}
