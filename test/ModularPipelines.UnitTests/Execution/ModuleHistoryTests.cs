using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

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

    [ModularPipelines.Attributes.DependsOn<SkipFromCategory>]
    private class UsesCategoryDependency : Module<Status>
    {
        protected internal override async Task<Status> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return (await context.GetModule<SkipFromCategory>()).ModuleStatus;
        }
    }

#pragma warning disable CS0618 // This test exercises history behavior through the legacy run-condition path.
    private class SkipRunConditionAttribute : RunConditionAttribute
    {
        public override Task<bool> Condition(IPipelineContext pipelineContext)
        {
            return false.AsTask();
        }
    }
#pragma warning restore CS0618

    [SkipRunCondition]
    private class SkipFromRunCondition : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class SkipFromMethod : Module<bool>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(() => SkipDecision.Skip("Testing"))
            .Build();

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
    public async Task Required_Dependency_With_History_Does_Not_Skip_Dependent()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<UsesCategoryDependency>()
            .IgnoreCategories("1")
            .AddResultsRepository<GoodModuleRepository>()
            .BuildHostAsync();

        var summary = await host.ExecutePipelineAsync();

        var dependencyResult = await summary.Modules.OfType<SkipFromCategory>().Single();
        var dependentResult = await summary.Modules.OfType<UsesCategoryDependency>().Single();

        await Assert.That(dependencyResult.ModuleStatus).IsEqualTo(Status.UsedHistory);
        await Assert.That(dependentResult.ModuleStatus).IsEqualTo(Status.Successful);
        await Assert.That(dependentResult.ValueOrDefault).IsEqualTo(Status.UsedHistory);
    }

    [Test]
    public async Task Cascade_Skipped_Module_Uses_Its_Own_History()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipFromCategory>()
            .AddModule<UsesCategoryDependency>()
            .IgnoreCategories("1")
            .AddResultsRepository<CascadeDependentHistoryRepository>()
            .BuildHostAsync();

        var summary = await host.ExecutePipelineAsync();

        var dependencyResult = await summary.Modules.OfType<SkipFromCategory>().Single();
        var dependentResult = await summary.Modules.OfType<UsesCategoryDependency>().Single();

        await Assert.That(dependencyResult.ModuleStatus).IsEqualTo(Status.Skipped);
        await Assert.That(dependentResult.ModuleStatus).IsEqualTo(Status.UsedHistory);
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

}
