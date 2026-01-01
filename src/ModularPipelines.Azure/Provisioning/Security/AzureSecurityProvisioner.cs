using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.Authorization.Models;
using Azure.ResourceManager.ManagedServiceIdentities;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Security;

public class AzureSecurityProvisioner : BaseAzureProvisioner
{
    public AzureSecurityProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<UserAssignedIdentityResource>> UserAssignedIdentity(AzureResourceIdentifier azureResourceIdentifier, UserAssignedIdentityData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetUserAssignedIdentities()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<RoleAssignmentResource>> RoleAssignment(AzureResourceIdentifier azureResourceIdentifier, RoleAssignmentCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetRoleAssignments()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<RoleManagementPolicyAssignmentResource>> RoleManagementPolicyAssignment(AzureResourceIdentifier azureResourceIdentifier, RoleManagementPolicyAssignmentData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetRoleManagementPolicyAssignments()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<AuthorizationRoleDefinitionResource>> AuthorizationRoleDefinition(AzureScope scope, ResourceIdentifier roleDefinitionIdentifier, AuthorizationRoleDefinitionData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scope);
        ArgumentNullException.ThrowIfNull(roleDefinitionIdentifier);
        ArgumentNullException.ThrowIfNull(properties);

        var scopeResourceIdentifier = await ArmClient.GetResourceIdentifierAsync(scope);

        return await ArmClient.GetAuthorizationRoleDefinitions(scopeResourceIdentifier)
            .CreateOrUpdateAsync(WaitUntil.Completed, roleDefinitionIdentifier, properties, cancellationToken);
    }
}
