using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "create", "github")]
public record GcloudBuildsConnectionsCreateGithubOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }

    [CliOption("--app-installation-id")]
    public string? AppInstallationId { get; set; }
}