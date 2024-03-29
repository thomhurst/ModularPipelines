using Azure.Core;
using Azure.ResourceManager.ManagedServiceIdentities;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

public class ProvisionUserAssignedIdentityModule : Module<UserAssignedIdentityResource>
{
    /// <inheritdoc/>
    protected override async Task<UserAssignedIdentityResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentityProvisionResponse = await context.Azure().Provisioner.Security.UserAssignedIdentity(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyUserIdentity"),
            new UserAssignedIdentityData(AzureLocation.UKSouth)
        );

        return userAssignedIdentityProvisionResponse.Value;
    }
}