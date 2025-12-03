using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "access")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudSecretsVersionsAccessOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Secret { get; set; }

    [CliOption("--out-file")]
    public string? OutFile { get; set; }
}
