using System.Reflection;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Provisioning.Compute;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.UnitTests;

public class AzureComputeProvisionerContractTests
{
    [Test]
    public async Task ProvisionerAsyncMethods_FollowAsyncContract()
    {
        var methods = typeof(IAzureProvisioner).Assembly
            .GetTypes()
            .Where(type => type.Namespace?.StartsWith(
                "ModularPipelines.Azure.Provisioning",
                StringComparison.Ordinal) == true)
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(method => typeof(Task).IsAssignableFrom(method.ReturnType))
            .ToArray();

        await Assert.That(methods).IsNotEmpty();
        await Assert.That(methods.All(method => method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
        await Assert.That(methods.All(
            method => method.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken))).IsTrue();
    }

    [Test]
    public async Task AppServiceDomainAsync_Preserves_AppService_Sdk_Contract()
    {
        var method = typeof(AzureComputeProvisioner).GetMethod(
            nameof(AzureComputeProvisioner.AppServiceDomainAsync),
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
