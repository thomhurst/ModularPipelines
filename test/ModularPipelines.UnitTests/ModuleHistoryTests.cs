using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class ModuleHistoryTests
{
    [ModuleCategory("1")]
    private class SkipFromCategory : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    private class SkipRunConditionAttribute : RunConditionAttribute
    {
        public override Task<bool> Condition(IPipelineHookContext pipelineContext)
        {
            return false.AsTask();
        }
    }

    [SkipRunCondition]
    private class SkipFromRunCondition : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    [SkipRunCondition]
    private class UseHistoryTrueModule : Module
    {
        protected internal override Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context)
        {
            return true.AsTask();
        }

        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    private class SkipFromMethod : Module
    {
        protected internal override Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return SkipDecision.Skip("Testing").AsTask();
        }

        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    private class NotFoundModuleRepository : IModuleResultRepository
    {
        public Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineHookContext pipelineContext)
        {
            return Task.FromResult<ModuleResult<T>?>(null);
        }
    }

    private class GoodModuleRepository : IModuleResultRepository
    {
        public Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module, IPipelineHookContext pipelineContext)
        {
            return Task.FromResult<ModuleResult<T>?>(new ModuleResult<T>(default(T?), module));
        }
    }

    [Test]
    public async Task Ignore_Category_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Skip_From_Method_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Use_History_Without_Repository_Still_Skips()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<UseHistoryTrueModule>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules[0].Status).IsEqualTo(Status.Skipped);
    }
}