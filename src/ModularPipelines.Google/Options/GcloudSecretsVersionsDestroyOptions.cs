using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "destroy")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudSecretsVersionsDestroyOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Secret { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}
