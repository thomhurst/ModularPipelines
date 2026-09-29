using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.ServiceBus;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.PubSub;

public class AzureServiceBusProvisioner : BaseAzureProvisioner
{
    public AzureServiceBusProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<ServiceBusNamespaceResource>> NamespaceAsync(AzureResourceIdentifier azureResourceIdentifier, ServiceBusNamespaceData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaces()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<MigrationConfigurationResource>> MigrationConfigurationAsync(AzureResourceIdentifier azureResourceIdentifier, string queueName, MigrationConfigurationData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        return await serviceBus.Value.GetMigrationConfigurations()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusQueueResource>> QueueAsync(AzureResourceIdentifier azureResourceIdentifier, string queueName, ServiceBusQueueData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        return await serviceBus.Value.GetServiceBusQueues()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusTopicResource>> TopicAsync(AzureResourceIdentifier azureResourceIdentifier, string topicName, ServiceBusTopicData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(topicName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        return await serviceBus.Value.GetServiceBusTopics()
            .CreateOrUpdateAsync(WaitUntil.Completed, topicName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusSubscriptionResource>> SubscriptionAsync(AzureResourceIdentifier azureResourceIdentifier, string topicName, string subscriptionName, ServiceBusSubscriptionData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(topicName);
        ArgumentException.ThrowIfNullOrWhiteSpace(subscriptionName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName, cancellationToken).ConfigureAwait(false);

        return await topic.Value.GetServiceBusSubscriptions()
            .CreateOrUpdateAsync(WaitUntil.Completed, subscriptionName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusTopicAuthorizationRuleResource>> TopicAuthorizationRuleAsync(
        AzureResourceIdentifier azureResourceIdentifier, string topicName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(topicName);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationRuleName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName, cancellationToken).ConfigureAwait(false);

        return await topic.Value.GetServiceBusTopicAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusNamespaceAuthorizationRuleResource>> NamespaceAuthorizationRuleAsync(
        AzureResourceIdentifier azureResourceIdentifier, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationRuleName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        return await serviceBus.Value.GetServiceBusNamespaceAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ArmOperation<ServiceBusQueueAuthorizationRuleResource>> QueueAuthorizationRuleAsync(
        AzureResourceIdentifier azureResourceIdentifier, string queueName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentException.ThrowIfNullOrWhiteSpace(authorizationRuleName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken).ConfigureAwait(false);

        var queue = await serviceBus.Value.GetServiceBusQueueAsync(queueName, cancellationToken).ConfigureAwait(false);

        return await queue.Value.GetServiceBusQueueAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken).ConfigureAwait(false);
    }

    private async Task<Response<ServiceBusNamespaceResource>> GetServiceBusNamespace(AzureResourceIdentifier azureResourceIdentifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaceAsync(azureResourceIdentifier.ResourceName, cancellationToken).ConfigureAwait(false);
    }
}
