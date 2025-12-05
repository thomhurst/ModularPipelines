using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "update", "github")]
public record GcloudBuildsConnectionsUpdateGithubOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }
}