using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.TrafficManager;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Compute;

public class AzureTrafficAndLoadBalancerProvisioner : BaseAzureProvisioner
{
    public AzureTrafficAndLoadBalancerProvisioner(ArmClient armClient) : base(armClient)
    {
    }
    
    public async Task<ArmOperation<TrafficManagerProfileResource>> TrafficManagerProfile(AzureResourceIdentifier azureResourceIdentifier, TrafficManagerProfileData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetTrafficManagerProfiles()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
    
    public async Task<ArmOperation<LoadBalancerResource>> LoadBalancer(AzureResourceIdentifier azureResourceIdentifier, LoadBalancerData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetLoadBalancers()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
    
    public async Task<ArmOperation<ApplicationGatewayResource>> ApplicationGateway(AzureResourceIdentifier azureResourceIdentifier, ApplicationGatewayData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetApplicationGateways()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}