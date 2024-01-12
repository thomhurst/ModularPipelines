using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "create", "github-enterprise")]
public record GcloudBuildsConnectionsCreateGithubEnterpriseOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--host-uri")] string HostUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--app-slug")]
    public string? AppSlug { get; set; }

    [CommandSwitch("--private-key-secret-version")]
    public string? PrivateKeySecretVersion { get; set; }

    [CommandSwitch("--webhook-secret-secret-version")]
    public string? WebhookSecretSecretVersion { get; set; }

    [CommandSwitch("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [CommandSwitch("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CommandSwitch("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}