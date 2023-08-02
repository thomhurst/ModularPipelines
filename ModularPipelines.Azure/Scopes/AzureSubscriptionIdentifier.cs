using Azure.Core;

namespace ModularPipelines.Azure.Scopes;

public record AzureSubscriptionIdentifier
(
    string SubscriptionId
) : AzureScope
{
    public ResourceIdentifier ToSubscriptionIdentifier() => new($"/subscriptions/{SubscriptionId}");
}