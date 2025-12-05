using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "enable")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudSecretsVersionsEnableOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Secret { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}
