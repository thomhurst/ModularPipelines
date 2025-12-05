using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace-hub", "create")]
public record AzMlWorkspaceHubCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--additional-workspace-storage-accounts")]
    public int? AdditionalWorkspaceStorageAccounts { get; set; }

    [CliOption("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CliOption("--default-workspace-resource-group")]
    public string? DefaultWorkspaceResourceGroup { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

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
}