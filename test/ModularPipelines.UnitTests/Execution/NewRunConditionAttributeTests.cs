using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Execution;

/// <summary>
/// Tests for the new run condition attributes: RunIfAll, RunIfAny, and SkipIf.
/// </summary>
public class NewRunConditionAttributeTests : TestBase
{
    #region Test Conditions

    private class AlwaysTrue : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineHookContext context)
            => Task.FromResult(true);
    }

    private class AlwaysFalse : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineHookContext context)
            => Task.FromResult(false);
    }

    private class TrueConditionGroup : ConditionGroup
    {
        public override IRunCondition[] Conditions => [new AlwaysTrue()];
        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    private class FalseConditionGroup : ConditionGroup
    {
        public override IRunCondition[] Conditions => [new AlwaysFalse()];
        public override ConditionLogic Logic => ConditionLogic.All;
    }

    #endregion

    #region Test Modules

    // No conditions - should run
    private class NoConditionsModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAll with single true condition - should run
    [RunIfAll<AlwaysTrue>]
    private class RunIfAllTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAll with single false condition - should skip
    [RunIfAll<AlwaysFalse>]
    private class RunIfAllFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAll with two conditions, one false - should skip
    [RunIfAll<AlwaysTrue, AlwaysFalse>]
    private class RunIfAllMixedModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAny with single true condition - should run
    [RunIfAny<AlwaysTrue>]
    private class RunIfAnyTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAny with single false condition - should skip
    [RunIfAny<AlwaysFalse>]
    private class RunIfAnyFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // RunIfAny with two conditions, one true - should run
    [RunIfAny<AlwaysFalse, AlwaysTrue>]
    private class RunIfAnyMixedModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // SkipIf with true condition - should skip
    [SkipIf<AlwaysTrue>]
    private class SkipIfTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // SkipIf with false condition - should run
    [SkipIf<AlwaysFalse>]
    private class SkipIfFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // Combined: RunIfAll + SkipIf - SkipIf should be evaluated first
    [RunIfAll<AlwaysTrue>]
    [SkipIf<AlwaysTrue>]
    private class CombinedSkipAndRunModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // Multiple RunIfAll attributes - all must pass (AND between attributes)
    [RunIfAll<AlwaysTrue>]
    [RunIfAll<AlwaysTrue>]
    private class MultipleRunIfAllTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // Multiple RunIfAll attributes - one fails (AND between attributes)
    [RunIfAll<AlwaysTrue>]
    [RunIfAll<AlwaysFalse>]
    private class MultipleRunIfAllMixedModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // ConditionGroup test
    [RunIfAny<TrueConditionGroup>]
    private class ConditionGroupTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAll<FalseConditionGroup>]
    private class ConditionGroupFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    #endregion

    #region Tests

    [Test]
    public async Task NoConditions_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<NoConditionsModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(NoConditionsModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAll_SingleTrueCondition_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAllTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAll_SingleFalseCondition_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAllFalseModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAll_MixedConditions_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAllMixedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAny_SingleTrueCondition_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAnyTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAny_SingleFalseCondition_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAnyFalseModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAny_MixedConditions_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<RunIfAnyMixedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task SkipIf_TrueCondition_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipIfTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkipIfTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task SkipIf_FalseCondition_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkipIfFalseModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkipIfFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task SkipIf_EvaluatedBeforeRunIfAll_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<CombinedSkipAndRunModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(CombinedSkipAndRunModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task MultipleRunIfAll_AllTrue_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<MultipleRunIfAllTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(MultipleRunIfAllTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task MultipleRunIfAll_OneFails_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<MultipleRunIfAllMixedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(MultipleRunIfAllMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task ConditionGroup_TrueGroup_ShouldRun()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<ConditionGroupTrueModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(ConditionGroupTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task ConditionGroup_FalseGroup_ShouldSkip()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<ConditionGroupFalseModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(ConditionGroupFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    #endregion
}
