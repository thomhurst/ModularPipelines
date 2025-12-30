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
    /// <summary>
    /// The default retry count used for testing retry behavior.
    /// </summary>
    private const int DefaultRetryCount = 3;

    /// <summary>
    /// The expected execution count after all retries complete (1 initial + 3 retries = 4).
    /// </summary>
    private const int ExpectedExecutionCountAfterRetries = 4;

    /// <summary>
    /// The expected execution count when no retries occur.
    /// </summary>
    private const int ExpectedSingleExecutionCount = 1;

    /// <summary>
    /// The timeout duration for the FailedModuleWithTimeout test module (in milliseconds).
    /// </summary>
    private const int ModuleTimeoutMs = 50;

    /// <summary>
    /// The delay duration for the FailedModuleWithTimeout test module (in milliseconds).
    /// Must be less than ModuleTimeoutMs to allow timeout to trigger.
    /// </summary>
    private const int ModuleDelayMs = 30;

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

            if (ExecutionCount != ExpectedExecutionCountAfterRetries)
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
                .WaitAndRetryAsync(DefaultRetryCount, _ => TimeSpan.Zero);
        }

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != ExpectedExecutionCountAfterRetries)
            {
                throw new Exception();
            }

            await Task.Yield();
            return null;
        }
    }

    private class FailedModuleWithTimeout : Module<IDictionary<string, object>?>, ITimeoutable
    {
        public TimeSpan Timeout => TimeSpan.FromMilliseconds(ModuleTimeoutMs);

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(ModuleDelayMs), cancellationToken);

            throw new Exception();
        }
    }

    [Test]
    public async Task When_Successful_Do_Not_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = DefaultRetryCount;
            })
            .AddModule<SuccessModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<SuccessModule>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SuccessModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedSingleExecutionCount);
            await Assert.That(result.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_Then_Retry()
    {
        var host = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = DefaultRetryCount;
            })
            .AddModule<FailedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var module = host.RootServices.GetServices<IModule>().OfType<FailedModule>().First();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
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
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
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
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedSingleExecutionCount);
            await Assert.That(result.Exception).IsNotNull();
        }
    }

    [Test]
    public async Task When_Retry_With_Timeout_Then_Honour_Overall_Timeout()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = DefaultRetryCount;
            })
            .AddModule<FailedModuleWithTimeout>()
            .ExecutePipelineAsync());
        await Assert.That(moduleFailedException?.InnerException).IsTypeOf<ModuleTimeoutException>();
    }
}
