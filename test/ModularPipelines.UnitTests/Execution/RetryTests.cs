using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;

namespace ModularPipelines.UnitTests.Execution;

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

    private class SuccessModule : Module<bool>
    {
        internal int ExecutionCount;

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            await Task.Yield();
            return true;
        }
    }

    private class FailedModule : Module<bool>
    {
        internal int ExecutionCount;

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != ExpectedExecutionCountAfterRetries)
            {
                throw new Exception();
            }

            await Task.Yield();
            return true;
        }
    }

    private class FailedModuleWithCustomRetryPolicy : Module<string>
    {
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithRetryPolicy(Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(DefaultRetryCount, _ => TimeSpan.Zero))
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != ExpectedExecutionCountAfterRetries)
            {
                throw new Exception();
            }

            await Task.Yield();
            return "success";
        }
    }

    private class FailedModuleWithTimeout : Module<bool>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(ModuleTimeoutMs))
            .Build();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
            await Assert.That(result.ExceptionOrDefault).IsNull();
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
            await Assert.That(result.ExceptionOrDefault).IsNull();
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
            await Assert.That(result.ExceptionOrDefault).IsNull();
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
            await Assert.That(result.ExceptionOrDefault).IsNotNull();
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
