using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "update")]
public record GcloudContainerAttachedClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliFlag("--clear-admin-groups")]
    public bool? ClearAdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliFlag("--clear-admin-users")]
    public bool? ClearAdminUsers { get; set; }

    [CliFlag("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliOption("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CliOption("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}