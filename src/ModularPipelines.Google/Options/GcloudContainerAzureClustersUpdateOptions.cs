using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clusters", "update")]
public record GcloudContainerAzureClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client")]
    public string? Client { get; set; }

    [CliOption("--azure-application-id")]
    public string? AzureApplicationId { get; set; }

    [CliOption("--azure-tenant-id")]
    public string? AzureTenantId { get; set; }

    [CliFlag("--clear-client")]
    public bool? ClearClient { get; set; }

    [CliFlag("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }
}