namespace ModularPipelines.Azure.Scopes;

public record AzureResourceIdentifier
(
    string SubscriptionId,
    string ResourceGroupName,
    string ResourceName
) : AzureResourceGroupIdentifier(SubscriptionId, ResourceGroupName);