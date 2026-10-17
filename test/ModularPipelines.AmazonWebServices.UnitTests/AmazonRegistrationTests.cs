using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.AmazonWebServices.Extensions;
using ModularPipelines.AmazonWebServices.Services;
using ModularPipelines.Context;
using Moq;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AmazonRegistrationTests
{
    [Test]
    public async Task Registration_Is_Idempotent_And_Scoped()
    {
        var services = new ServiceCollection();
        services.RegisterAmazonContext();
        services.RegisterAmazonContext();

        foreach (var serviceType in new[] { typeof(IAmazon), typeof(IAmazonProvisioner), typeof(IS3) })
        {
            var descriptor = services.Single(service => service.ServiceType == serviceType);
            await Assert.That(descriptor.Lifetime).IsEqualTo(ServiceLifetime.Scoped);
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Supplied_Client_Resolves_Complete_Graph_And_Context_Within_Each_Scope(bool useClientExtension)
    {
        using var client = new TrackingS3Client();
        var services = new ServiceCollection();
        services.AddSingleton(Mock.Of<ICommandContext>());
        if (useClientExtension)
        {
            await Assert.That(services.AddS3Client(client)).IsSameReferenceAs(services);
        }
        else
        {
            services.AddSingleton<AmazonS3Client>(client);
            services.RegisterAmazonContext();
        }

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        IAmazon firstAmazon;

        using (var scope = provider.CreateScope())
        {
            firstAmazon = scope.ServiceProvider.GetRequiredService<IAmazon>();
            await AssertGraph(scope.ServiceProvider, firstAmazon, client);
            var servicesContext = new Mock<IServicesContext>(MockBehavior.Strict);
            servicesContext.Setup(context => context.GetRequiredService<IAmazon>()).Returns(firstAmazon);
            var context = new Mock<IPipelineContext>(MockBehavior.Strict);
            context.SetupGet(context => context.Services).Returns(servicesContext.Object);
            await Assert.That(context.Object.Amazon()).IsSameReferenceAs(firstAmazon);
        }

        await Assert.That(client.DisposeCount).IsEqualTo(0);
        using (var scope = provider.CreateScope())
        {
            var secondAmazon = scope.ServiceProvider.GetRequiredService<IAmazon>();
            await AssertGraph(scope.ServiceProvider, secondAmazon, client);
            await Assert.That(secondAmazon).IsNotSameReferenceAs(firstAmazon);
            await Assert.That(secondAmazon.Provisioner).IsNotSameReferenceAs(firstAmazon.Provisioner);
            await Assert.That(secondAmazon.Provisioner.S3).IsNotSameReferenceAs(firstAmazon.Provisioner.S3);
        }

        provider.Dispose();
        await Assert.That(client.DisposeCount).IsEqualTo(0);
    }

    [Test]
    public async Task Directly_Constructed_Wrapper_Still_Owns_Client()
    {
        var client = new TrackingS3Client();
        var s3 = new S3(client);
        s3.Dispose();
        s3.Dispose();

        await Assert.That(client.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Registration_Preserves_Custom_Services()
    {
        var amazon = Mock.Of<IAmazon>();
        var provisioner = Mock.Of<IAmazonProvisioner>();
        var s3 = Mock.Of<IS3>();
        var services = new ServiceCollection();
        services.AddScoped(_ => amazon);
        services.AddScoped(_ => provisioner);
        services.AddScoped(_ => s3);
        services.RegisterAmazonContext();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        await Assert.That(scope.ServiceProvider.GetRequiredService<IAmazon>()).IsSameReferenceAs(amazon);
        await Assert.That(scope.ServiceProvider.GetRequiredService<IAmazonProvisioner>()).IsSameReferenceAs(provisioner);
        await Assert.That(scope.ServiceProvider.GetRequiredService<IS3>()).IsSameReferenceAs(s3);
    }

    private static async Task AssertGraph(IServiceProvider services, IAmazon amazon, AmazonS3Client client)
    {
        await Assert.That(services.GetRequiredService<IAmazon>()).IsSameReferenceAs(amazon);
        await Assert.That(amazon.Provisioner).IsSameReferenceAs(services.GetRequiredService<IAmazonProvisioner>());
        await Assert.That(amazon.Provisioner.S3).IsSameReferenceAs(services.GetRequiredService<IS3>());
        await Assert.That(amazon.AwsCli).IsSameReferenceAs(services.GetRequiredService<IAws>());
        await Assert.That(services.GetRequiredService<AmazonS3Client>()).IsSameReferenceAs(client);
    }

    private sealed class TrackingS3Client() : AmazonS3Client(new AnonymousAWSCredentials(), new AmazonS3Config
    {
        ServiceURL = "http://127.0.0.1:1",
    })
    {
        public int DisposeCount { get; private set; }

        protected override void Dispose(bool disposing)
        {
            DisposeCount++;
            base.Dispose(disposing);
        }
    }
}
