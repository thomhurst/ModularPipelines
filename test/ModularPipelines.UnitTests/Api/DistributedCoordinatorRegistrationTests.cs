using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Api;

public class DistributedCoordinatorRegistrationTests
{
    [Test]
    public async Task Direct_Coordinator_Registration_Overrides_Earlier_Factory()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var builder = CreateDistributedBuilder()
            .AddDistributedCoordinatorFactory<TestCoordinatorFactory>();
        builder.Services.AddSingleton<IDistributedCoordinator>(coordinator);

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IDistributedCoordinator>())
            .IsSameReferenceAs(coordinator);
    }

    [Test]
    public async Task Coordinator_Factory_Overrides_Earlier_Direct_Registration()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var builder = CreateDistributedBuilder();
        builder.Services.AddSingleton<IDistributedCoordinator>(coordinator);
        builder.AddDistributedCoordinatorFactory<TestCoordinatorFactory>();

        await using var pipeline = await builder.BuildAsync();
        var factory = pipeline.Services.GetRequiredService<IDistributedCoordinatorFactory>();
        var resolvedCoordinator = pipeline.Services.GetRequiredService<IDistributedCoordinator>();
        _ = await resolvedCoordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(resolvedCoordinator).IsNotSameReferenceAs(coordinator);
            await Assert.That(((TestCoordinatorFactory) factory).CreateCount).IsEqualTo(1);
        }
    }

    private static PipelineBuilder CreateDistributedBuilder() =>
        TestPipelineBuilder.Create()
            .AddModule<TestModule>()
            .AddDistributedMode(options =>
            {
                options.TotalInstances = 2;
                options.InstanceIndex = 0;
            });

    public sealed class TestCoordinatorFactory : IDistributedCoordinatorFactory
    {
        public int CreateCount { get; private set; }

        public Task<IDistributedCoordinator> CreateAsync(CancellationToken cancellationToken)
        {
            CreateCount++;
            return Task.FromResult<IDistributedCoordinator>(new InMemoryDistributedCoordinator());
        }
    }

    private sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }
}
