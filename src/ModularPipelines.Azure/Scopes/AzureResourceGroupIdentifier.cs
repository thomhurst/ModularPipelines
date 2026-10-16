using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureResourceGroupIdentifier
(
    string SubscriptionId,
    string ResourceGroupName
) : AzureSubscriptionIdentifier(SubscriptionId)
{
    public AzureResourceIdentifier GetResourceIdentifier(string resourceType, string resourceName) => new(SubscriptionId, ResourceGroupName, resourceType, resourceName);

    public ResourceIdentifier ToResourceGroupIdentifier() => new($"{ToSubscriptionIdentifier()}/resourceGroups/{ResourceGroupName}");

    public static implicit operator AzureResourceGroupIdentifier(ResourceIdentifier resourceIdentifier) =>
        new(
            resourceIdentifier.SubscriptionId!,
            resourceIdentifier.ResourceGroupName!
        );
}