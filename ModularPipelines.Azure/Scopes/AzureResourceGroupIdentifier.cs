using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureResourceGroupIdentifier
(
    string SubscriptionId,
    string ResourceGroupName
) : AzureSubscriptionIdentifier(SubscriptionId)
{
    public ResourceIdentifier ToResourceGroupIdentifier() => new($"{ToSubscriptionIdentifier()}/resourceGroups/{ResourceGroupName}");
}