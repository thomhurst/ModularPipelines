using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "update", "gitlab")]
public record GcloudBuildsConnectionsUpdateGitlabOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }

    [CommandSwitch("--host-uri")]
    public string? HostUri { get; set; }

    [CommandSwitch("--read-authorizer-token-secret-version")]
    public string? ReadAuthorizerTokenSecretVersion { get; set; }

    [CommandSwitch("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CommandSwitch("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}