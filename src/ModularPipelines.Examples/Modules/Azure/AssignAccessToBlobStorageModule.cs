using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.Authorization.Models;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionUserAssignedIdentityModule>]
public class AssignAccessToBlobStorageModule : Module<RoleAssignmentResource>
{
    protected override async Task<RoleAssignmentResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await GetModule<ProvisionUserAssignedIdentityModule>();
        
        var storageAccount = await GetModule<ProvisionBlobStorageAccountModule>();
        
        var roleAssignmentResource = await context.Azure().Provisioner.Security.RoleAssignment(
            storageAccount.Value!.Id,
            new RoleAssignmentCreateOrUpdateContent(WellKnownRoleDefinitions.BlobStorageOwnerDefinitionId, userAssignedIdentity.Value!.Data.PrincipalId!.Value)
        );
        
        return roleAssignmentResource.Value;
    }
}