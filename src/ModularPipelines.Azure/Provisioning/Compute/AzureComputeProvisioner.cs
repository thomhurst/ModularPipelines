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

    public async Task<ArmOperation<AppServicePlanResource>> AppServicePlan(AzureResourceIdentifier azureResourceIdentifier, AppServicePlanData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServicePlans()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<WebSiteResource>> WebSite(AzureResourceIdentifier azureResourceIdentifier, WebSiteData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetWebSites()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<WebSiteSlotResource>> WebSiteSlot(AzureResourceIdentifier azureResourceIdentifier, WebSiteData properties, CancellationToken cancellationToken = default)
    {
        var website = await GetResourceGroup(azureResourceIdentifier).GetWebSiteAsync(azureResourceIdentifier.ResourceName, cancellationToken);

        return await website.Value.GetWebSiteSlots()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<SiteFunctionResource>> WebSiteDeployment(AzureResourceIdentifier azureResourceIdentifier, FunctionEnvelopeData properties, CancellationToken cancellationToken = default)
    {
        var website = await GetResourceGroup(azureResourceIdentifier).GetWebSiteAsync(azureResourceIdentifier.ResourceName, cancellationToken);

        return await website.Value.GetSiteFunctions()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<StaticSiteResource>> StaticSite(AzureResourceIdentifier azureResourceIdentifier, StaticSiteData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetStaticSites()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<AppServiceDomainResource>> AppServiceDomain(AzureResourceIdentifier azureResourceIdentifier, AppServiceDomainData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServiceDomains()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }

    public async Task<ArmOperation<AppServiceEnvironmentResource>> AppServiceEnvironment(AzureResourceIdentifier azureResourceIdentifier, AppServiceEnvironmentData properties, CancellationToken cancellationToken = default)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetAppServiceEnvironments()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }
}
