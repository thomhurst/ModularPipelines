using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "create", "github-enterprise")]
public record GcloudBuildsConnectionsCreateGithubEnterpriseOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region,
[property: CliOption("--host-uri")] string HostUri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--app-slug")]
    public string? AppSlug { get; set; }

    [CliOption("--private-key-secret-version")]
    public string? PrivateKeySecretVersion { get; set; }

    [CliOption("--webhook-secret-secret-version")]
    public string? WebhookSecretSecretVersion { get; set; }

    [CliOption("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [CliOption("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CliOption("--ssl-ca-file")]
    public string? SslCaFile { get; set; }
}