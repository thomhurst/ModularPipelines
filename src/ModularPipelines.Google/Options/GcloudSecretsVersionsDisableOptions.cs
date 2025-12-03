using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "disable")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudSecretsVersionsDisableOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Secret { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}
