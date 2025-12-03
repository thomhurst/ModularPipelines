using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "create", "gitlab")]
public record GcloudBuildsConnectionsCreateGitlabOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region,
[property: CliOption("--authorizer-token-secret-version")] string AuthorizerTokenSecretVersion,
[property: CliOption("--read-authorizer-token-secret-version")] string ReadAuthorizerTokenSecretVersion,
[property: CliOption("--webhook-secret-secret-version")] string WebhookSecretSecretVersion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host-uri")]
    public string? HostUri { get; set; }

    [CliOption("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CliOption("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}