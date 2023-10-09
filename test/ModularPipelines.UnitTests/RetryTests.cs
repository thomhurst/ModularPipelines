using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

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

        Assert.Multiple(() =>
        {
            Assert.That(module.ExecutionCount, Is.EqualTo(1));
            Assert.That(module.Exception, Is.Null);
        });
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

        Assert.Multiple(() =>
        {
            Assert.That(module.ExecutionCount, Is.EqualTo(4));
            Assert.That(module.Exception, Is.Null);
        });
    }

    [Test]
    public void When_Error_And_Zero_Retry_Count_Then_Do_Not_Retry()
    {
        var moduleFailedException = Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 0;
            })
            .AddModule<FailedModule>()
            .ExecutePipelineAsync());

        var module = moduleFailedException?.Module as FailedModule;

        Assert.Multiple(() =>
        {
            Assert.That(module?.ExecutionCount, Is.EqualTo(1));
            Assert.That(module!.Exception, Is.Not.Null);
        });
    }

    [Test]
    public void When_Retry_With_Timeout_Then_Honour_Overall_Timeout()
    {
        var moduleFailedException = Assert.ThrowsAsync<ModuleFailedException>(async () => await TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = 3;
            })
            .AddModule<FailedModuleWithTimeout>()
            .ExecutePipelineAsync());

        Assert.That(moduleFailedException?.InnerException, Is.TypeOf<OperationCanceledException>());
    }
}