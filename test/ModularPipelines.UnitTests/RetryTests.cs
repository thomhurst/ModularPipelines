using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;

namespace ModularPipelines.UnitTests;

public class RetryTests : TestBase
{
    private class SuccessModule : Module<IDictionary<string, object>?>
    {
        internal int ExecutionCount;

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            await Task.Yield();
            return null;
        }
    }

    private class FailedModule : Module<IDictionary<string, object>?>
    {
        internal int ExecutionCount;

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != 4)
            {
                throw new Exception();
            }

            await Task.Yield();
            return null;
        }
    }

    private class FailedModuleWithCustomRetryPolicy : Module<IDictionary<string, object>?>, IRetryable<IDictionary<string, object>?>
    {
        internal int ExecutionCount;

        public AsyncRetryPolicy<IDictionary<string, object>?> GetRetryPolicy(IPipelineContext context)
        {
            return Policy<IDictionary<string, object>?>
                .Handle<Exception>()
                .WaitAndRetryAsync(3, _ => TimeSpan.Zero);
        }

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != 4)
            {
                throw new Exception();
            }

            await Task.Yield();
            return null;
        }
    }

    private class FailedModuleWithTimeout : Module<IDictionary<string, object>?>, ITimeoutable
    {
        // Reduced timeout from 300ms to 50ms for faster test execution
        public TimeSpan Timeout => TimeSpan.FromMilliseconds(50);

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 200ms to 30ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(30), cancellationToken);

            throw new Exception();
        }
    }

    [Test]
    public async Task When_Successful_Do_Not_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<SuccessModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<SuccessModule>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SuccessModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(1);
            await Assert.That(result.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_Then_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<FailedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<FailedModule>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(4);
            await Assert.That(result.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_With_Custom_RetryPolicy_Then_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<FailedModuleWithCustomRetryPolicy>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<FailedModuleWithCustomRetryPolicy>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModuleWithCustomRetryPolicy))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(4);
            await Assert.That(result.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_And_Zero_Retry_Count_Then_Do_Not_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 0;
            })
            .AddModule<FailedModule>()
            .BuildHostAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await host.ExecutePipelineAsync());

        var module = host.RootServices.GetServices<IModule>().OfType<FailedModule>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(1);
            await Assert.That(result.Exception).IsNotNull();
        }
    }

    [Test]
    public async Task When_Retry_With_Timeout_Then_Honour_Overall_Timeout()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<FailedModuleWithTimeout>()
            .ExecutePipelineAsync());
        await Assert.That(moduleFailedException?.InnerException).IsTypeOf<ModuleTimeoutException>();
    }
}
