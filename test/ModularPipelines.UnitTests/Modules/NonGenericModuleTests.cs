using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Modules;

public class NonGenericModuleTests : TestBase
{
    [Test]
    public async Task Executes_And_Produces_None_Result()
    {
        var module = await RunModule<TestModule>();
        var result = await module;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(1);
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(result.ValueOrDefault).IsEqualTo(None.Value);
            await Assert.That(((IModule) module).ResultType).IsEqualTo(typeof(None));
        }
    }

    private sealed class TestModule : Module
    {
        public int ExecutionCount { get; private set; }

        protected override async Task ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            ExecutionCount++;
        }
    }
}
