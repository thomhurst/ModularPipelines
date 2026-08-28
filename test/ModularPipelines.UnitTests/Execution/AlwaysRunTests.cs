using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Execution;

public class AlwaysRunTests : TestBase
{
    public class MyModule1 : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.Attributes.DependsOn<MyModule1>]
    public class MyModule2 : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule2>]
    public class MyModule3 : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule3>]
    public class MyModule4 : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule1>]
    public class SuccessfulAlwaysRunModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [Test]
    public async Task AlwaysRunModules_Will_Run_Even_With_Exception()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<MyModule1>()
            .AddModule<MyModule2>()
            .AddModule<MyModule3>()
            .AddModule<MyModule4>()
            .BuildAsync();

        try
        {
            await host.RunAsync();
        }
        catch
        {
            // Expected - pipeline will fail
        }

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var result1 = resultRegistry.GetResult(typeof(MyModule1))!;
        var result2 = resultRegistry.GetResult(typeof(MyModule2))!;
        var result3 = resultRegistry.GetResult(typeof(MyModule3))!;
        var result4 = resultRegistry.GetResult(typeof(MyModule4))!;

        using (Assert.Multiple())
        {
            await Assert.That(result1.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result2.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result3.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result4.Status).IsNotEqualTo(ModuleStatus.NotStarted);
        }
    }

    [Test]
    public async Task WaitForAllModules_Returns_Summary_When_AlwaysRun_Dependency_Fails()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                ExecutionMode = ExecutionMode.WaitForAllModules,
                ThrowOnPipelineFailure = false,
            })
            .AddModule<MyModule1>()
            .AddModule<SuccessfulAlwaysRunModule>()
            .BuildAsync();

        var summary = await host.RunAsync();
        var alwaysRunResult = host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(SuccessfulAlwaysRunModule));

        using (Assert.Multiple())
        {
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(alwaysRunResult).IsNotNull();
            await Assert.That(alwaysRunResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task StopOnFirstException_Preserves_Primary_Failure_With_AlwaysRun_Dependency()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                ExecutionMode = ExecutionMode.StopOnFirstException,
                ThrowOnPipelineFailure = true,
            })
            .AddModule<MyModule1>()
            .AddModule<SuccessfulAlwaysRunModule>()
            .BuildAsync();

        await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());
    }
}
