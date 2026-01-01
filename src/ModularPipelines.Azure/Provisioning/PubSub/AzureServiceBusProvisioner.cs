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

    public async Task<ArmOperation<ServiceBusNamespaceResource>> Namespace(AzureResourceIdentifier azureResourceIdentifier, ServiceBusNamespaceData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaces()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<MigrationConfigurationResource>> MigrationConfiguration(AzureResourceIdentifier azureResourceIdentifier, string queueName, MigrationConfigurationData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        return await serviceBus.Value.GetMigrationConfigurations()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusQueueResource>> Queue(AzureResourceIdentifier azureResourceIdentifier, string queueName, ServiceBusQueueData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        return await serviceBus.Value.GetServiceBusQueues()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusTopicResource>> Topic(AzureResourceIdentifier azureResourceIdentifier, string topicName, ServiceBusTopicData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        return await serviceBus.Value.GetServiceBusTopics()
            .CreateOrUpdateAsync(WaitUntil.Completed, topicName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusSubscriptionResource>> Subscription(AzureResourceIdentifier azureResourceIdentifier, string topicName, string subscriptionName, ServiceBusSubscriptionData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName, cancellationToken);

        return await topic.Value.GetServiceBusSubscriptions()
            .CreateOrUpdateAsync(WaitUntil.Completed, subscriptionName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusTopicAuthorizationRuleResource>> TopicAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string topicName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName, cancellationToken);

        return await topic.Value.GetServiceBusTopicAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusNamespaceAuthorizationRuleResource>> NamespaceAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        return await serviceBus.Value.GetServiceBusNamespaceAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken);
    }

    public async Task<ArmOperation<ServiceBusQueueAuthorizationRuleResource>> QueueAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string queueName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties, CancellationToken cancellationToken = default)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier, cancellationToken);

        var queue = await serviceBus.Value.GetServiceBusQueueAsync(queueName, cancellationToken);

        return await queue.Value.GetServiceBusQueueAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties, cancellationToken);
    }

    private async Task<Response<ServiceBusNamespaceResource>> GetServiceBusNamespace(AzureResourceIdentifier azureResourceIdentifier, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaceAsync(azureResourceIdentifier.ResourceName, cancellationToken);
    }
}
