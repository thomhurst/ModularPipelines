using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace", "create")]
public record AzMlWorkspaceCreateOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--adb-workspace")]
    public string? AdbWorkspace { get; set; }

    [CommandSwitch("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CommandSwitch("--cmk-keyvault")]
    public string? CmkKeyvault { get; set; }

    [CommandSwitch("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [BooleanCommandSwitch("--exist-ok")]
    public bool? ExistOk { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [BooleanCommandSwitch("--hbi-workspace")]
    public bool? HbiWorkspace { get; set; }

    [CommandSwitch("--keyvault")]
    public string? Keyvault { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--pe-auto-approval")]
    public bool? PeAutoApproval { get; set; }

    [CommandSwitch("--pe-name")]
    public string? PeName { get; set; }

    [CommandSwitch("--pe-resource-group")]
    public string? PeResourceGroup { get; set; }

    [CommandSwitch("--pe-subnet-name")]
    public string? PeSubnetName { get; set; }

    [CommandSwitch("--pe-subscription-id")]
    public string? PeSubscriptionId { get; set; }

    [CommandSwitch("--pe-vnet-name")]
    public string? PeVnetName { get; set; }

    [CommandSwitch("--primary-user-assigned-identity")]
    public string? PrimaryUserAssignedIdentity { get; set; }

    [CommandSwitch("--resource-cmk-uri")]
    public string? ResourceCmkUri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--system_datastores_auth_mode")]
    public string? SystemDatastoresAuthMode { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identity-for-cmk-encryption")]
    public string? UserAssignedIdentityForCmkEncryption { get; set; }

    [CommandSwitch("--v1-legacy-mode")]
    public string? V1LegacyMode { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}