using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "create", "gitlab")]
public record GcloudBuildsConnectionsCreateGitlabOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--authorizer-token-secret-version")] string AuthorizerTokenSecretVersion,
[property: CommandSwitch("--read-authorizer-token-secret-version")] string ReadAuthorizerTokenSecretVersion,
[property: CommandSwitch("--webhook-secret-secret-version")] string WebhookSecretSecretVersion
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host-uri")]
    public string? HostUri { get; set; }

    [CommandSwitch("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CommandSwitch("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}