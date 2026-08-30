using ModularPipelines.Reporting;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Execution;

public class ModuleHistoryTests
{
    [ModuleCategory("1")]
    private class SkipFromCategory : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [ModuleCategory("2")]
    private class RunnableCategoryModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [ModularPipelines.DependsOn<SkipFromCategory>]
    private class UsesCategoryDependency : Module<ModuleStatus>
    {
        protected internal override async Task<ModuleStatus> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return (await context.GetModule<SkipFromCategory>()).Status;
        }
    }

    private class SkipRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
        {
            return Task.FromResult(false);
        }
    }

    [RunIfAll<SkipRunCondition>]
    private class SkipFromRunCondition : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class SkipFromMethod : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Testing"));

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class NotFoundModuleRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext)
        {
            return Task.FromResult<ModuleResult<T>?>(null);
        }
    }

    private class GoodModuleRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext)
        {
            // Create a result using the module execution context
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private class CascadeDependentHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext)
        {
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext)
        {
            if (module.GetType() != typeof(UsesCategoryDependency))
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    [Test]
    public async Task Ignore_Category_Without_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_Without_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<RunnableCategoryModule>()
            .RunOnlyCategories("2")
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_Without_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_Without_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromMethod>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_NotFound_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_NotFound_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<RunnableCategoryModule>()
            .RunOnlyCategories("2")
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_NotFound_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Skip_From_Method_With_NotFound_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<NotFoundModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task Ignore_Category_With_Good_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task Required_Dependency_With_History_Does_Not_Skip_Dependent()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<UsesCategoryDependency>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildAsync();

        var summary = await host.RunAsync();

        var dependencyResult = await summary.Modules.OfType<SkipFromCategory>().Single();
        var dependentResult = await summary.Modules.OfType<UsesCategoryDependency>().Single();

        await Assert.That(dependencyResult.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
        await Assert.That(dependentResult.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(dependentResult.ValueOrDefault).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task Cascade_Skipped_Module_Uses_Its_Own_History()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<UsesCategoryDependency>()
            .IgnoreCategories("1")
            .AddResultsRepository<CascadeDependentHistoryRepository>()
            .BuildAsync();

        var summary = await host.RunAsync();

        var dependencyResult = await summary.Modules.OfType<SkipFromCategory>().Single();
        var dependentResult = await summary.Modules.OfType<UsesCategoryDependency>().Single();

        await Assert.That(dependencyResult.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(dependentResult.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task Ignore_By_Non_Runnable_Category_With_Good_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<RunnableCategoryModule>()
            .RunOnlyCategories("2")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromCategory))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task Skip_From_Run_Condition_With_Good_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromRunCondition>()
            .AddResultsRepository<GoodModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromRunCondition))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

    [Test]
    public async Task Skip_From_Method_With_Good_History_Repository()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipFromMethod>()
            .AddResultsRepository<GoodModuleRepository>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(SkipFromMethod))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.RestoredFromHistory);
    }

}
