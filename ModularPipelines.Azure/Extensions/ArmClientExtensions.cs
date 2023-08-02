using Azure.Core;
using Azure.ResourceManager;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Extensions;

public static class ArmClientExtensions
{
    public static async Task<ResourceIdentifier> GetResourceIdentifierAsync(this ArmClient armClient, AzureScope azureScope)
    {
        switch (azureScope)
        {
            case AzureResourceIdentifier azureResourceIdentifier:
            {
                await foreach (var page in armClient
                                   .GetResourceGroupResource(azureResourceIdentifier.ToResourceGroupIdentifier())
                                   .GetGenericResourcesAsync()
                                   .AsPages())
                {
                    foreach (var genericResource in page.Values)
                    {
                        if (genericResource.Id.Name == azureResourceIdentifier.ResourceName)
                        {
                            return genericResource.Id;
                        }   
                    }
                }

                throw new Exception($"Unknown Resource: {azureResourceIdentifier}");
            }
            case AzureResourceGroupIdentifier azureResourceGroupIdentifier:
                return azureResourceGroupIdentifier.ToResourceGroupIdentifier();
            case AzureSubscriptionIdentifier azureSubscriptionIdentifier:
                return azureSubscriptionIdentifier.ToSubscriptionIdentifier();
            case AzureManagementGroupIdentifier azureManagementGroupIdentifier:
                return azureManagementGroupIdentifier.ToManagementGroupIdentifier();
            default:
            {
                throw new Exception($"Unknown Resource: {azureScope}");
            }
        }
    }
}