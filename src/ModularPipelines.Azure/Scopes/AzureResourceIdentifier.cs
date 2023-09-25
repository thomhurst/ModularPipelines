using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureResourceIdentifier
(
    string SubscriptionId,
    string ResourceGroupName,
    string ResourceName
) : AzureResourceGroupIdentifier(SubscriptionId, ResourceGroupName)
{
    public static implicit operator AzureResourceIdentifier(ResourceIdentifier resourceIdentifier) =>
        new(
            resourceIdentifier.SubscriptionId!,
            resourceIdentifier.ResourceGroupName!,
            resourceIdentifier.Name
        );
}