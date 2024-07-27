using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Polly.Retry;

namespace ModularPipelines.UnitTests;

[Retry(3)]
public class RetryTests : TestBase
{
    private class SuccessModule : Module
    {
        internal int ExecutionCount;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return await NothingAsync();
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

            return await NothingAsync();
        }
    }

    private class FailedModuleWithCustomRetryPolicy : Module
    {
        protected override AsyncRetryPolicy<IDictionary<string, object>?> RetryPolicy
            => CreateRetryPolicy(3);

        internal int ExecutionCount;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;

            if (ExecutionCount != 4)
            {
                throw new Exception();
            }

            return await NothingAsync();
        }
    }

    private class FailedModuleWithTimeout : Module
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromMilliseconds(300);

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);

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
        
        await using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).Is.EqualTo(1);
            await Assert.That(module.Exception).Is.Null();
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).Is.EqualTo(4);
            await Assert.That(module.Exception).Is.Null();
        }
    }

    [Test]
    public async Task When_Error_With_Custom_RetryPolicy_Then_Retry()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<FailedModuleWithCustomRetryPolicy>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.Modules.OfType<FailedModuleWithCustomRetryPolicy>().First();
        
        await using (Assert.Multiple())
        {
            await Assert.That(module.ExecutionCount).Is.EqualTo(4);
            await Assert.That(module.Exception).Is.Null();
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(module?.ExecutionCount).Is.EqualTo(1);
            await Assert.That(module!.Exception).Is.Not.Null();
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
        await Assert.That(moduleFailedException?.InnerException).Is.TypeOf<ModuleTimeoutException>();
    }
}