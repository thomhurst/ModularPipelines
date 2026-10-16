using Azure.Core;
using Azure.ResourceManager;
using ModularPipelines.Azure.Exceptions;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Extensions;

/// <summary>
/// Extension methods for <see cref="ArmClient"/>.
/// </summary>
public static class ArmClientExtensions
{
    /// <summary>
    /// Gets the <see cref="ResourceIdentifier"/> for the specified Azure scope.
    /// </summary>
    /// <param name="armClient">The ARM client.</param>
    /// <param name="azureScope">The Azure scope to resolve.</param>
    /// <param name="cancellationToken">Token used to cancel resource enumeration.</param>
    /// <returns>The resolved <see cref="ResourceIdentifier"/>.</returns>
    /// <exception cref="AzureResourceNotFoundException">
    /// Thrown when the specified <see cref="AzureResourceIdentifier"/> cannot be found in the resource group.
    /// </exception>
    /// <exception cref="UnsupportedAzureScopeException">
    /// Thrown when the <paramref name="azureScope"/> is of an unsupported type.
    /// </exception>
    public static async Task<ResourceIdentifier> GetResourceIdentifierAsync(this ArmClient armClient, AzureScope azureScope, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        switch (azureScope)
        {
            case AzureResourceIdentifier azureResourceIdentifier:
                {
                    await foreach (var genericResource in armClient
                                       .GetResourceGroupResource(azureResourceIdentifier.ToResourceGroupIdentifier())
                                       .GetGenericResourcesAsync(cancellationToken: cancellationToken)
                                       .WithCancellation(cancellationToken)
                                       .ConfigureAwait(false))
                    {
                        if (string.Equals(genericResource.Id.ResourceType.ToString(), azureResourceIdentifier.ResourceType, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(genericResource.Id.Name, azureResourceIdentifier.ResourceName, StringComparison.OrdinalIgnoreCase))
                        {
                            return genericResource.Id;
                        }
                    }

                    throw new AzureResourceNotFoundException(azureResourceIdentifier);
                }

            case AzureResourceGroupIdentifier azureResourceGroupIdentifier:
                return azureResourceGroupIdentifier.ToResourceGroupIdentifier();
            case AzureSubscriptionIdentifier azureSubscriptionIdentifier:
                return azureSubscriptionIdentifier.ToSubscriptionIdentifier();
            case AzureManagementGroupIdentifier azureManagementGroupIdentifier:
                return azureManagementGroupIdentifier.ToManagementGroupIdentifier();
            default:
                {
                    throw new UnsupportedAzureScopeException(azureScope);
                }
        }
    }
}
