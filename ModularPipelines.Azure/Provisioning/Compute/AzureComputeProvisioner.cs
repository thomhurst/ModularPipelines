using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Compute;

public class AzureComputeProvisioner : BaseAzureProvisioner
{
    public AzureComputeProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<AppServicePlanResource>> AppServicePlan(AzureResourceIdentifier azureResourceIdentifier, AppServicePlanData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServicePlans()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<WebSiteResource>> WebSite(AzureResourceIdentifier azureResourceIdentifier, WebSiteData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetWebSites()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<WebSiteSlotResource>> WebSiteSlot(AzureResourceIdentifier azureResourceIdentifier, WebSiteData properties)
    {
        var website = await GetResourceGroup(azureResourceIdentifier).GetWebSiteAsync(azureResourceIdentifier.ResourceName);

        return await website.Value.GetWebSiteSlots()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<SiteFunctionResource>> WebSiteDeployment(AzureResourceIdentifier azureResourceIdentifier, FunctionEnvelopeData properties)
    {
        var website = await GetResourceGroup(azureResourceIdentifier).GetWebSiteAsync(azureResourceIdentifier.ResourceName);

        return await website.Value.GetSiteFunctions()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<StaticSiteResource>> StaticSite(AzureResourceIdentifier azureResourceIdentifier, StaticSiteData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetStaticSites()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }


    public async Task<ArmOperation<AppServiceDomainResource>> AppServiceDomain(AzureResourceIdentifier azureResourceIdentifier, AppServiceDomainData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServiceDomains()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }

    public async Task<ArmOperation<AppServiceEnvironmentResource>> AppServiceEnvironment(AzureResourceIdentifier azureResourceIdentifier, AppServiceEnvironmentData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServiceEnvironments()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}