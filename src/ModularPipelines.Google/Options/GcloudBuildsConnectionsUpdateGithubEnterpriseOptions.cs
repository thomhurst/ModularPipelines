using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "update", "github-enterprise")]
public record GcloudBuildsConnectionsUpdateGithubEnterpriseOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [CliOption("--app-slug")]
    public string? AppSlug { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host-uri")]
    public string? HostUri { get; set; }

    [CliOption("--private-key-secret-version")]
    public string? PrivateKeySecretVersion { get; set; }

    [CliOption("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CliOption("--ssl-ca-file")]
    public string? SslCaFile { get; set; }

    [CliOption("--webhook-secret-secret-version")]
    public string? WebhookSecretSecretVersion { get; set; }
}