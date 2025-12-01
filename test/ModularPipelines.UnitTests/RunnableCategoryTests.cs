using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class RunnableCategoryTests : TestBase
{
    [ModuleCategory("Run1")]
    private class RunnableModule1 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModuleCategory("Run2")]
    private class RunnableModule2 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModuleCategory("Run1")]
    private class RunnableModule3 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModuleCategory("NoRun1")]
    private class NonRunnableModule1 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModuleCategory("NoRun2")]
    private class NonRunnableModule2 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class OtherModule3 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [Test]
    public async Task When_RunCategories_Specified_Then_Expected_Modules_Run()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .RunCategories("Run1", "Run2")
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        using (Assert.Multiple())
        {
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule1))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule2))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule3))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule1))!.ModuleStatus).IsEqualTo(Status.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule2))!.ModuleStatus).IsEqualTo(Status.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(OtherModule3))!.ModuleStatus).IsEqualTo(Status.Skipped);
        }
    }

    [Test]
    public async Task When_IgnoreCategories_Specified_Then_Expected_Modules_Run()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunnableModule1>()
            .AddModule<RunnableModule2>()
            .AddModule<NonRunnableModule1>()
            .AddModule<NonRunnableModule2>()
            .AddModule<RunnableModule3>()
            .AddModule<OtherModule3>()
            .IgnoreCategories("NoRun1", "NoRun2")
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        using (Assert.Multiple())
        {
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule1))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule2))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(RunnableModule3))!.ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule1))!.ModuleStatus).IsEqualTo(Status.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(NonRunnableModule2))!.ModuleStatus).IsEqualTo(Status.Skipped);
            await Assert.That(resultRegistry.GetResult(typeof(OtherModule3))!.ModuleStatus).IsEqualTo(Status.Successful);
        }
    }
}