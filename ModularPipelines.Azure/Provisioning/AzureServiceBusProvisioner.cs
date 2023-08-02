using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.ServiceBus;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning;

public class AzureServiceBusProvisioner : BaseAzureProvisioner
{
    public AzureServiceBusProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<ServiceBusNamespaceResource>> Namespace(AzureResourceIdentifier azureResourceIdentifier, ServiceBusNamespaceData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaces()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<MigrationConfigurationResource>> MigrationConfiguration(AzureResourceIdentifier azureResourceIdentifier, string queueName, MigrationConfigurationData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        return await serviceBus.Value.GetMigrationConfigurations()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties);
    }

    public async Task<ArmOperation<ServiceBusQueueResource>> Queue(AzureResourceIdentifier azureResourceIdentifier, string queueName, ServiceBusQueueData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        return await serviceBus.Value.GetServiceBusQueues()
            .CreateOrUpdateAsync(WaitUntil.Completed, queueName, properties);
    }

    public async Task<ArmOperation<ServiceBusTopicResource>> Topic(AzureResourceIdentifier azureResourceIdentifier, string topicName, ServiceBusTopicData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        return await serviceBus.Value.GetServiceBusTopics()
            .CreateOrUpdateAsync(WaitUntil.Completed, topicName, properties);
    }

    public async Task<ArmOperation<ServiceBusSubscriptionResource>> Subscription(AzureResourceIdentifier azureResourceIdentifier, string topicName, string subscriptionName, ServiceBusSubscriptionData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName);

        return await topic.Value.GetServiceBusSubscriptions()
            .CreateOrUpdateAsync(WaitUntil.Completed, subscriptionName, properties);
    }

    public async Task<ArmOperation<ServiceBusTopicAuthorizationRuleResource>> TopicAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string topicName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        var topic = await serviceBus.Value.GetServiceBusTopicAsync(topicName);

        return await topic.Value.GetServiceBusTopicAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties);
    }

    public async Task<ArmOperation<ServiceBusNamespaceAuthorizationRuleResource>> NamespaceAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        return await serviceBus.Value.GetServiceBusNamespaceAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties);
    }

    public async Task<ArmOperation<ServiceBusQueueAuthorizationRuleResource>> QueueAuthorizationRule(
        AzureResourceIdentifier azureResourceIdentifier, string queueName, string authorizationRuleName,
        ServiceBusAuthorizationRuleData properties)
    {
        var serviceBus = await GetServiceBusNamespace(azureResourceIdentifier);

        var queue = await serviceBus.Value.GetServiceBusQueueAsync(queueName);

        return await queue.Value.GetServiceBusQueueAuthorizationRules()
            .CreateOrUpdateAsync(WaitUntil.Completed, authorizationRuleName, properties);
    }

    private async Task<Response<ServiceBusNamespaceResource>> GetServiceBusNamespace(AzureResourceIdentifier azureResourceIdentifier)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetServiceBusNamespaceAsync(azureResourceIdentifier.ResourceName);
    }
}