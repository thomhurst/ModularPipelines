using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.Authorization.Models;
using Azure.ResourceManager.ManagedServiceIdentities;
using Azure.ResourceManager.Storage;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionUserAssignedIdentityModule>]
public class AssignAccessToBlobStorageModule : Module<RoleAssignmentResource>
{
    /// <inheritdoc/>
    protected override async Task<RoleAssignmentResource?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await context.GetModule<ProvisionUserAssignedIdentityModule>();

        var storageAccount = await context.GetModule<ProvisionBlobStorageAccountModule>();

        var roleAssignmentResource = await context.Azure().Provisioner.Security.RoleAssignment(
            storageAccount.ValueOrDefault!.Id,
            new RoleAssignmentCreateOrUpdateContent(WellKnownRoleDefinitions.BlobStorageOwnerDefinitionId, userAssignedIdentity.ValueOrDefault!.Data.PrincipalId!.Value)
        );

        return roleAssignmentResource.Value;
    }
}