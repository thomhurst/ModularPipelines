using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleEventContextTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    [Test]
    public async Task ModuleType_ReturnsCorrectType()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        await Assert.That(context.ModuleType).IsEqualTo(typeof(ModuleA));
    }

    [Test]
    public async Task ModuleName_ReturnsTypeName()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        await Assert.That(context.ModuleName).IsEqualTo("ModuleA");
    }

    [Test]
    public async Task GetService_ResolvesFromServiceProvider()
    {
        var module = new ModuleA();
        var services = new ServiceCollection();
        services.AddSingleton<string>("test-value");
        var serviceProvider = services.BuildServiceProvider();
        var context = CreateContext(module, serviceProvider: serviceProvider);

        var result = context.GetService<string>();
        await Assert.That(result).IsEqualTo("test-value");
    }

    [Test]
    public async Task GetMetadata_ReturnsValueFromRegistry()
    {
        var module = new ModuleA();
        var metadataRegistry = new ModuleMetadataRegistry();
        metadataRegistry.SetMetadata(typeof(ModuleA), "key", "value");
        var context = CreateContext(module, metadataRegistry: metadataRegistry);

        var result = context.GetMetadata<string>("key");
        await Assert.That(result).IsEqualTo("value");
    }

    [Test]
    public async Task RequestRetry_SetsRetryRequested()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.RequestRetry(TimeSpan.FromSeconds(5));

        await Assert.That(context.RetryRequested).IsTrue();
        await Assert.That(context.RetryDelay).IsEqualTo(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task SkipDependentModules_SetsFlag()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.SkipDependentModules("test reason");

        await Assert.That(context.ShouldSkipDependents).IsTrue();
        await Assert.That(context.SkipDependentsReason).IsEqualTo("test reason");
    }

    [Test]
    public async Task FailPipeline_SetsFlag()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.FailPipeline("test reason");

        await Assert.That(context.ShouldFailPipeline).IsTrue();
        await Assert.That(context.FailPipelineReason).IsEqualTo("test reason");
    }

    private static ModuleEventContext CreateContext(
        IModule module,
        IServiceProvider? serviceProvider = null,
        IModuleMetadataRegistry? metadataRegistry = null)
    {
        var pipelineContext = Mock.Of<IPipelineContext>();
        serviceProvider ??= new ServiceCollection().BuildServiceProvider();

        return new ModuleEventContext(
            module,
            module.GetType(),
            module.GetType().GetCustomAttributes(true).OfType<Attribute>().ToList(),
            DateTimeOffset.UtcNow,
            Status.Processing,
            pipelineContext,
            serviceProvider,
            metadataRegistry ?? new ModuleMetadataRegistry(),
            CancellationToken.None);
    }
}
