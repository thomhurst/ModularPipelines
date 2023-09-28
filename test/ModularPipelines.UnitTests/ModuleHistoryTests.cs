﻿using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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
        public override Task<bool> Condition(IPipelineContext pipelineContext)
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
        protected override Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context)
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
        protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
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
        public Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module)
        {
            return Task.FromResult<ModuleResult<T>?>(null);
        }
    }

    private class GoodModuleRepository : IModuleResultRepository
    {
        public Task SaveResultAsync<T>(ModuleBase module, ModuleResult<T> moduleResult)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(ModuleBase module)
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

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Skip_From_Run_Condition_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Skip_From_Method_Without_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Ignore_Category_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Skip_From_Method_With_NotFound_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }

    [Test]
    public async Task Ignore_Category_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.UsedHistory));
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.UsedHistory));
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.UsedHistory));
    }

    [Test]
    public async Task Skip_From_Method_With_Good_History_Repository()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<GoodModuleRepository>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.UsedHistory));
    }

    [Test]
    public async Task Use_History_Without_Repository_Still_Skips()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<UseHistoryTrueModule>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules[0].Status, Is.EqualTo(Status.Skipped));
    }
}