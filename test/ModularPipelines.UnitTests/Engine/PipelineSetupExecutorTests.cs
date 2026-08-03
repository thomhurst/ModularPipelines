using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel(nameof(PipelineSetupExecutorTests))]
public class PipelineSetupExecutorTests
{
    [AttributeUsage(AttributeTargets.Class)]
    private sealed class CountingAttribute : Attribute
    {
        public CountingAttribute()
        {
            InstanceCount++;
        }

        public static int InstanceCount { get; set; }
    }

    [Counting]
    private sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(TestModule));
    }

    [Test]
    public async Task OnModuleReadyAsync_WithoutHooks_DoesNotCreateAttributes()
    {
        var executor = new PipelineSetupExecutor(
            [],
            [],
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleAttributeEventService());
        var module = new TestModule();
        CountingAttribute.InstanceCount = 0;

        await executor.OnModuleReadyAsync(new ModuleState(module, module.GetType()));

        await Assert.That(CountingAttribute.InstanceCount).IsEqualTo(0);
    }

    [Test]
    public async Task OnModuleReadyAsync_WithHook_UsesCachedAttributeInstances()
    {
        var attributeEventService = new ModuleAttributeEventService();
        IReadOnlyList<Attribute>? receiverAttributes = null;
        var receiver = new Mock<IModuleEventReceiver>();
        receiver.Setup(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()))
            .Callback<IModuleHookContext>(context => receiverAttributes = context.ModuleAttributes)
            .Returns(Task.CompletedTask);
        var executor = new PipelineSetupExecutor(
            [],
            [receiver.Object],
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            attributeEventService);
        var module = new TestModule();

        await executor.OnModuleReadyAsync(new ModuleState(module, module.GetType()));

        await Assert.That(ReferenceEquals(
                receiverAttributes,
                attributeEventService.GetAttributes(module.GetType())))
            .IsTrue();
    }
}
