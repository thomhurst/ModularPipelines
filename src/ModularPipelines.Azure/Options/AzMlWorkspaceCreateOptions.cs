using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "workspace", "create")]
public record AzMlWorkspaceCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CliOption("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enable-data-isolation")]
    public bool? EnableDataIsolation { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--image-build-compute")]
    public string? ImageBuildCompute { get; set; }

    [CliOption("--key-vault")]
    public string? KeyVault { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-network")]
    public string? ManagedNetwork { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--update-dependent-resources")]
    public bool? UpdateDependentResources { get; set; }

    [CliOption("--workspace-hub")]
    public string? WorkspaceHub { get; set; }
}