using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clusters", "update")]
public record GcloudContainerAzureClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client")]
    public string? Client { get; set; }

    [CommandSwitch("--azure-application-id")]
    public string? AzureApplicationId { get; set; }

    [CommandSwitch("--azure-tenant-id")]
    public string? AzureTenantId { get; set; }

    [BooleanCommandSwitch("--clear-client")]
    public bool? ClearClient { get; set; }

    [BooleanCommandSwitch("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }
}