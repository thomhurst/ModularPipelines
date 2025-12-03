using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "register")]
public record GcloudContainerAttachedClustersRegisterOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--distribution")] string Distribution,
[property: CliOption("--fleet-project")] string FleetProject,
[property: CliOption("--platform-version")] string PlatformVersion,
[property: CliOption("--context")] string Context,
[property: CliOption("--kubeconfig")] string Kubeconfig,
[property: CliFlag("--has-private-issuer")] bool HasPrivateIssuer,
[property: CliOption("--issuer-url")] string IssuerUrl
) : GcloudOptions
{
    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CliOption("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}