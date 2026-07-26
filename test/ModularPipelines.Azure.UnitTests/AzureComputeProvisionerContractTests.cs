using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using ModularPipelines.Azure.Provisioning.Compute;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.UnitTests;

public class AzureComputeProvisionerContractTests
{
    [Test]
    public async Task AppServiceDomain_Preserves_AppService_Sdk_Contract()
    {
        var method = typeof(AzureComputeProvisioner).GetMethod(
            nameof(AzureComputeProvisioner.AppServiceDomain),
            [
                typeof(AzureResourceIdentifier),
                typeof(AppServiceDomainData),
                typeof(CancellationToken),
            ]);

        await Assert.That(method).IsNotNull();
        await Assert.That(method!.ReturnType)
            .IsEqualTo(typeof(Task<ArmOperation<AppServiceDomainResource>>));
    }
}
