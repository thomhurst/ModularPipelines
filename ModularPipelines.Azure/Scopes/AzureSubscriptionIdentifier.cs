using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureSubscriptionIdentifier
(
    string SubscriptionId
) : AzureScope
{
    public AzureResourceGroupIdentifier GetResourceGroupIdentifier(string resourceGroupName) => new(SubscriptionId, resourceGroupName);
    public AzureResourceIdentifier GetResourceIdentifier(string resourceGroupName, string resourceName) => new(SubscriptionId, resourceGroupName, resourceName);
    public ResourceIdentifier ToSubscriptionIdentifier() => new($"/subscriptions/{SubscriptionId}");
    
    public static implicit operator AzureSubscriptionIdentifier(ResourceIdentifier resourceIdentifier) => new(resourceIdentifier.SubscriptionId!);
}