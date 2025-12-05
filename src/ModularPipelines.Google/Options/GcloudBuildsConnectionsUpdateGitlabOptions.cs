using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "update", "gitlab")]
public record GcloudBuildsConnectionsUpdateGitlabOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }

    [CliOption("--host-uri")]
    public string? HostUri { get; set; }

    [CliOption("--read-authorizer-token-secret-version")]
    public string? ReadAuthorizerTokenSecretVersion { get; set; }

    [CliOption("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CliOption("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}