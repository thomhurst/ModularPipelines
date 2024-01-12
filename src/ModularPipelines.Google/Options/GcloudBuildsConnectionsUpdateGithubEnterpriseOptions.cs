using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "update", "github-enterprise")]
public record GcloudBuildsConnectionsUpdateGithubEnterpriseOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [CommandSwitch("--app-slug")]
    public string? AppSlug { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host-uri")]
    public string? HostUri { get; set; }

    [CommandSwitch("--private-key-secret-version")]
    public string? PrivateKeySecretVersion { get; set; }

    [CommandSwitch("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CommandSwitch("--ssl-ca-file")]
    public string? SslCaFile { get; set; }

    [CommandSwitch("--webhook-secret-secret-version")]
    public string? WebhookSecretSecretVersion { get; set; }
}