using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class ModuleHistoryTests
{
    [ModuleCategory("1")]
    private class SkipFromCategory : IModule<IDictionary<string, object>?>
    {
        public Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
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
    private class SkipFromRunCondition : IModule<IDictionary<string, object>?>
    {
        public Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    [SkipRunCondition]
    private class UseHistoryTrueModule : IModule<IDictionary<string, object>?>, IHistoryAware
    {
        public Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context)
        {
            return true.AsTask();
        }

        public Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    private class SkipFromMethod : IModule<IDictionary<string, object>?>, ISkippable
    {
        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return SkipDecision.Skip("Testing").AsTask();
        }

        public Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    private class NotFoundModuleRepository : IModuleResultRepository
    {
        public Task SaveResultAsync<T>(IModule<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(IModule<T> module, IPipelineHookContext pipelineContext)
        {
            return Task.FromResult<ModuleResult<T>?>(null);
        }
    }

    private class GoodModuleRepository : IModuleResultRepository
    {
        public Task SaveResultAsync<T>(IModule<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(IModule<T> module, IPipelineHookContext pipelineContext)
        {
            // Create a result using the module execution context
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(new ModuleResult<T>(default(T?), executionContext));
        }
    }

    [Test]
    public async Task Ignore_Category_Without_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_Without_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_Without_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_Without_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_NotFound_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_NotFound_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_NotFound_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_With_NotFound_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_Good_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_Good_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .RunCategories("2")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_Good_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<GoodModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Skip_From_Method_With_Good_History_Repository()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<GoodModuleRepository>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Use_History_Without_Repository_Still_Skips()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<UseHistoryTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(UseHistoryTrueModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Skipped);
    }
}
