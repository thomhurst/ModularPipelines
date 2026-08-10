using Kevlar;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

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
    /// The timeout duration for timeout test modules (in milliseconds).
    /// </summary>
    private const int ModuleTimeoutMs = 250;

    /// <summary>
    /// The retry back-off duration for the FailedModuleWithTimeout test module (in milliseconds).
    /// </summary>
    private const int RetryDelayMs = 750;

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

    private sealed class RetryableTestException : Exception
    {
    }

    private class FailedModuleWithFilteredRetries : Module<bool>
    {
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithRetry(
                DefaultRetryCount,
                TimeSpan.Zero,
                exception => exception is RetryableTestException)
            .Build();

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            return ExecutionCount == ExpectedExecutionCountAfterRetries
                ? Task.FromResult(true)
                : Task.FromException<bool>(new RetryableTestException());
        }
    }

    private class FailedModuleWithRejectedRetry : Module<bool>
    {
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithRetry(
                DefaultRetryCount,
                TimeSpan.Zero,
                exception => exception is RetryableTestException)
            .Build();

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return Task.FromException<bool>(new InvalidOperationException());
        }
    }

    private class FailedModuleWithCustomRetryPolicy : Module<string>
    {
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .Advanced
            .WithShield(Shield
                .When<Exception>()
                .Retry(DefaultRetryCount, Backoff.None))
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
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(ModuleTimeoutMs))
            .Advanced
            .WithShield(Shield
                .When<Exception>()
                .Retry(DefaultRetryCount, Backoff.Constant(TimeSpan.FromMilliseconds(RetryDelayMs))))
            .Build();

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return Task.FromException<bool>(new Exception());
        }
    }

    private class CancellableModuleWithTimeout : Module<bool>
    {
        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(ModuleTimeoutMs))
            .WithRetry(DefaultRetryCount, TimeSpan.Zero)
            .Build();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return true;
        }
    }

    [Test]
    public async Task When_Successful_Do_Not_Retry()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options with
            {
                DefaultRetryCount = DefaultRetryCount,
            })
            .AddModule<SuccessModule>()
            .BuildAsync();

        await host.RunAsync();

        var module = host.Services.GetServices<IModule>().OfType<SuccessModule>().First();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
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
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options with
            {
                DefaultRetryCount = DefaultRetryCount,
            })
            .AddModule<FailedModule>()
            .BuildAsync();

        await host.RunAsync();

        var module = host.Services.GetServices<IModule>().OfType<FailedModule>().First();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
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
        var host = await TestPipelineBuilder.Create()
            .AddModule<FailedModuleWithCustomRetryPolicy>()
            .BuildAsync();

        await host.RunAsync();

        var module = host.Services.GetServices<IModule>().OfType<FailedModuleWithCustomRetryPolicy>().First();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModuleWithCustomRetryPolicy))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
            await Assert.That(result.ExceptionOrDefault).IsNull();
        }
    }

    [Test]
    public async Task When_Error_Matches_Filter_Then_Retry()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<FailedModuleWithFilteredRetries>()
            .BuildAsync();

        await host.RunAsync();

        var module = host.Services.GetServices<IModule>().OfType<FailedModuleWithFilteredRetries>().Single();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModuleWithFilteredRetries))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
            await Assert.That(result.ExceptionOrDefault).IsNull();
        }
    }

    [Test]
    public async Task When_Error_DoesNotMatch_Filter_Then_DoNotRetry()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<FailedModuleWithRejectedRetry>()
            .BuildAsync();

        await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());

        var module = host.Services.GetServices<IModule>().OfType<FailedModuleWithRejectedRetry>().Single();
        await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedSingleExecutionCount);
    }

    [Test]
    public async Task When_Error_And_Zero_Retry_Count_Then_Do_Not_Retry()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options with
            {
                DefaultRetryCount = 0,
            })
            .AddModule<FailedModule>()
            .BuildAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await host.RunAsync());

        var module = host.Services.GetServices<IModule>().OfType<FailedModule>().First();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(FailedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedSingleExecutionCount);
            await Assert.That(result.ExceptionOrDefault).IsNotNull();
        }
    }

    private class NonCancellableModuleWithTimeout : Module<bool>
    {
        private readonly TaskCompletionSource<bool> _completion =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        internal int ExecutionCount;

        internal int RetryCallbackCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(50))
            .Advanced
            .WithRetryPolicy(Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    DefaultRetryCount,
                    _ => TimeSpan.FromMinutes(1),
                    (_, _, _, _) => RetryCallbackCount++))
            .Build();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return await _completion.Task;
        }

        internal void Complete() => _completion.TrySetResult(true);
    }

    private class CancelledDuringRetryModule : Module<bool>
    {
        private readonly TaskCompletionSource _secondAttemptStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        internal Task SecondAttemptStarted => _secondAttemptStarted.Task;

        internal int ExecutionCount;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(2))
            .WithRetry(1, TimeSpan.FromMilliseconds(250))
            .Build();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            if (ExecutionCount == 1)
            {
                throw new RetryableTestException();
            }

            _secondAttemptStarted.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return true;
        }
    }

    [Test]
    public async Task When_Retry_Backoff_Exceeds_Timeout_Then_All_Attempts_Run()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<FailedModuleWithTimeout>()
            .BuildAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await host.RunAsync());

        var module = host.Services.GetServices<IModule>().OfType<FailedModuleWithTimeout>().Single();
        var timeoutException = moduleFailedException?.InnerException as ModuleTimeoutException;
        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
            await Assert.That(timeoutException).IsNull();
        }
    }

    [Test]
    public async Task When_Retry_Timeouts_During_Module_Then_Report_Token_Respected()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<CancellableModuleWithTimeout>()
            .BuildAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());

        var module = host.Services.GetServices<IModule>().OfType<CancellableModuleWithTimeout>().Single();
        var timeoutException = moduleFailedException?.InnerException as ModuleTimeoutException;
        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedExecutionCountAfterRetries);
            await Assert.That(timeoutException).IsNotNull();
            await Assert.That(timeoutException!.WasCancellationTokenRespected).IsTrue();
        }
    }

    [Test]
    public async Task When_Timed_Out_Attempt_Remains_Active_Then_Do_Not_Retry()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<NonCancellableModuleWithTimeout>()
            .BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<NonCancellableModuleWithTimeout>().Single();

        try
        {
            var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(
                () => host.RunAsync().WaitAsync(TimeSpan.FromSeconds(10)));
            var timeoutException = moduleFailedException?.InnerException as ModuleTimeoutException;

            using (Assert.Multiple())
            {
                await Assert.That(module.ExecutionCount).IsEqualTo(ExpectedSingleExecutionCount);
                await Assert.That(module.RetryCallbackCount).IsEqualTo(1);
                await Assert.That(timeoutException).IsNotNull();
                await Assert.That(timeoutException!.WasCancellationTokenRespected).IsFalse();
            }
        }
        finally
        {
            module.Complete();
        }
    }

    [Test]
    public async Task When_Cancelled_During_Later_Attempt_Then_Report_PipelineTerminated()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var host = await TestPipelineBuilder.Create()
            .AddModule<CancelledDuringRetryModule>()
            .BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<CancelledDuringRetryModule>().Single();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var pipelineTask = host.RunAsync(cancellationTokenSource.Token);

        await module.SecondAttemptStarted.WaitAsync(TimeSpan.FromSeconds(5));
        cancellationTokenSource.Cancel();

        await pipelineTask;

        var result = resultRegistry.GetResult(typeof(CancelledDuringRetryModule));
        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(2);
            await Assert.That(result).IsNotNull();
            await Assert.That(result!.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
        }
    }
}
