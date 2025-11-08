using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;

namespace ModularPipelines.UnitTests;

public class RetryTests : TestBase
{
    private class SuccessModule : Module
    {
        internal int ExecutionCount;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            await Task.CompletedTask;
            return null;
        }
    }

    private class FailedModule : Module
    {
        internal int ExecutionCount;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != 4)
            {
                throw new Exception();
            }

            await Task.CompletedTask;
            return null;
        }
    }

    private class FailedModuleWithCustomRetryPolicy : Module, IModuleRetryPolicy
    {
        public IAsyncPolicy GetRetryPolicy() =>
            Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(i * i * 100));

        internal int ExecutionCount;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != 4)
            {
                throw new Exception();
            }

            await Task.CompletedTask;
            return null;
        }
    }

    private class FailedModuleWithTimeout : Module, IModuleTimeout
    {
        // Reduced timeout from 300ms to 50ms for faster test execution
        public TimeSpan GetTimeout() => TimeSpan.FromMilliseconds(50);

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 200ms to 30ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(30), cancellationToken);

            throw new Exception();
        }
    }

    [Test]
    public async Task When_Successful_Do_Not_Retry()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<SuccessModule>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.Modules.OfType<SuccessModule>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(1);
            await Assert.That(module.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_Then_Retry()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<FailedModule>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.Modules.OfType<FailedModule>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(4);
            await Assert.That(module.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_With_Custom_RetryPolicy_Then_Retry()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<FailedModuleWithCustomRetryPolicy>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.Modules.OfType<FailedModuleWithCustomRetryPolicy>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).IsEqualTo(4);
            await Assert.That(module.Exception).IsNull();
        }
    }

    [Test]
    public async Task When_Error_And_Zero_Retry_Count_Then_Do_Not_Retry()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 0;
            })
            .AddModule<FailedModule>()
            .ExecutePipelineAsync());

        var module = moduleFailedException?.Module as FailedModule;

        using (Assert.Multiple())
        {
            await Assert.That(module?.ExecutionCount).IsEqualTo(1);
            await Assert.That(module!.Exception).IsNotNull();
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