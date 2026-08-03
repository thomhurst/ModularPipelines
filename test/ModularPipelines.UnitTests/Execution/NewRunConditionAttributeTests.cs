using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Execution;

/// <summary>
/// Tests for the new run condition attributes: RunIfAll, RunIfAny, and SkipIf.
/// </summary>
public class NewRunConditionAttributeTests : TestBase
{
    private static readonly AsyncLocal<ConditionState?> AsyncConditionState = new();

    private static ConditionState CurrentConditionState =>
        AsyncConditionState.Value ??= new ConditionState();

    private static CancellationTokenSource? ConditionCancellationTokenSource
    {
        get => CurrentConditionState.CancellationTokenSource;
        set => CurrentConditionState.CancellationTokenSource = value;
    }

    private static bool SubsequentConditionWasEvaluated
    {
        get => CurrentConditionState.SubsequentConditionWasEvaluated;
        set => CurrentConditionState.SubsequentConditionWasEvaluated = value;
    }

    private static bool DependencyWasExecuted
    {
        get => CurrentConditionState.DependencyWasExecuted;
        set => CurrentConditionState.DependencyWasExecuted = value;
    }

    private sealed class ConditionState
    {
        public CancellationTokenSource? CancellationTokenSource { get; set; }

        public bool SubsequentConditionWasEvaluated { get; set; }

        public bool DependencyWasExecuted { get; set; }
    }

    #region Test Conditions

    private class AlwaysTrue : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
            => Task.FromResult(true);
    }

    private class AlwaysFalse : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
            => Task.FromResult(false);
    }

    private class TrueConditionGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new AlwaysTrue()];
        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    private class FalseConditionGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new AlwaysFalse()];
        public override ConditionLogic Logic => ConditionLogic.All;
    }

    private class CancellationConditionGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new CancelDuringEvaluation(), new TrackEvaluation()];
        public override ConditionLogic Logic => ConditionLogic.Any;
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

    [SkipIf<AlwaysFalse>]
    private class AttributeAndFluentConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override ModularPipelines.Configuration.ModuleConfiguration Configure() => ModularPipelines.Configuration.ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("Fluent condition"))
            .Build();
    }

    private class CancelDuringEvaluation : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
        {
            ConditionCancellationTokenSource!.Cancel();
            return Task.FromResult(false);
        }
    }

    private class TrackEvaluation : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
        {
            SubsequentConditionWasEvaluated = true;
            return Task.FromResult(true);
        }
    }

    private class ThrowOnConstruction : IRunCondition
    {
        public ThrowOnConstruction() => throw new InvalidOperationException("Condition should not be constructed");

        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    private class ConditionDependencyModule : SimpleTestModule<bool>
    {
        protected override bool Result
        {
            get
            {
                DependencyWasExecuted = true;
                return true;
            }
        }
    }

    private class DependencyCompletedCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
            => Task.FromResult(DependencyWasExecuted);
    }

    [SkipIf<DependencyCompletedCondition>]
    [ModularPipelines.Attributes.DependsOn<ConditionDependencyModule>]
    private class ConditionAfterDependencyModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipIf<CancelDuringEvaluation>]
    [RunIfAll<TrackEvaluation>]
    private class CancellationAwareConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipIf<CancelDuringEvaluation, TrackEvaluation>]
    private class CancellationAwareGroupedAttributeModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAll<CancellationConditionGroup>]
    private class CancellationAwareConditionGroupModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAll<TrackEvaluation>]
    private class DiscoveryCancellationModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    #endregion

    #region Tests

    [Test]
    public async Task NoConditions_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<NoConditionsModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(NoConditionsModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAll_SingleTrueCondition_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAllTrueModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAll_SingleFalseCondition_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAllFalseModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAll_MixedConditions_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAllMixedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAllMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAny_SingleTrueCondition_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAnyTrueModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAny_SingleFalseCondition_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAnyFalseModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task RunIfAny_MixedConditions_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<RunIfAnyMixedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(RunIfAnyMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task SkipIf_TrueCondition_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipIfTrueModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkipIfTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task SkipIf_FalseCondition_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkipIfFalseModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkipIfFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task RunIfAny_DoesNotConstructConditionsAfterTrueResult()
    {
        var result = await new RunIfAnyAttribute<AlwaysTrue, ThrowOnConstruction>()
            .EvaluateAsync(Mock.Of<IPipelineContext>());

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task RunIfAll_DoesNotConstructConditionsAfterFalseResult()
    {
        var result = await new RunIfAllAttribute<AlwaysFalse, ThrowOnConstruction>()
            .EvaluateAsync(Mock.Of<IPipelineContext>());

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task SkipIf_DoesNotConstructConditionsAfterTrueResult()
    {
        var result = await new SkipIfAttribute<AlwaysTrue, ThrowOnConstruction>()
            .EvaluateAsync(Mock.Of<IPipelineContext>());

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task SkipIf_EvaluatedBeforeRunIfAll_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<CombinedSkipAndRunModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(CombinedSkipAndRunModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task MultipleRunIfAll_AllTrue_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<MultipleRunIfAllTrueModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(MultipleRunIfAllTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task MultipleRunIfAll_OneFails_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<MultipleRunIfAllMixedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(MultipleRunIfAllMixedModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task ConditionGroup_TrueGroup_ShouldRun()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<ConditionGroupTrueModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(ConditionGroupTrueModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task ConditionGroup_FalseGroup_ShouldSkip()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<ConditionGroupFalseModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(ConditionGroupFalseModule))!;
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
    }

    [Test]
    public async Task Attribute_And_Fluent_Conditions_Use_One_Skip_Pipeline()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<AttributeAndFluentConditionModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(AttributeAndFluentConditionModule))!;
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
            await Assert.That(moduleResult.SkipDecisionOrDefault!.Reason).IsEqualTo("Fluent condition");
        }
    }

    [Test]
    public async Task Attribute_Condition_Is_Evaluated_After_Dependencies()
    {
        DependencyWasExecuted = false;
        var host = await TestPipelineBuilder.Create()
            .AddModule<ConditionDependencyModule>()
            .AddModule<ConditionAfterDependencyModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(ConditionAfterDependencyModule))!;
        using (Assert.Multiple())
        {
            await Assert.That(DependencyWasExecuted).IsTrue();
            await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Skipped);
        }
    }

    [Test]
    public async Task Cancellation_Is_Checked_Between_Attribute_Conditions()
    {
        using var setupTokenSource = new CancellationTokenSource();
        ConditionCancellationTokenSource = setupTokenSource;

        var host = await TestPipelineBuilder.Create()
            .AddModule<CancellationAwareConditionModule>()
            .BuildAsync();
        var module = new CancellationAwareConditionModule();
        var conditionHandler = host.Services.GetRequiredService<IModuleConditionHandler>();
        using var cancellationTokenSource = new CancellationTokenSource();
        ConditionCancellationTokenSource = cancellationTokenSource;
        SubsequentConditionWasEvaluated = false;

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            conditionHandler.ShouldIgnore(module, cancellationTokenSource.Token));
        await Assert.That(SubsequentConditionWasEvaluated).IsFalse();

        using var retryTokenSource = new CancellationTokenSource();
        ConditionCancellationTokenSource = retryTokenSource;

        var retryResult = await conditionHandler.ShouldIgnore(module);

        await Assert.That(retryResult.ShouldIgnore).IsFalse();
        await Assert.That(SubsequentConditionWasEvaluated).IsTrue();
    }

    [Test]
    public async Task Cancellation_Is_Checked_Within_Grouped_Attribute()
    {
        await AssertGroupedCancellation<CancellationAwareGroupedAttributeModule>();
    }

    [Test]
    public async Task Cancellation_Is_Checked_Within_Condition_Group()
    {
        await AssertGroupedCancellation<CancellationAwareConditionGroupModule>();
    }

    private static async Task AssertGroupedCancellation<TModule>()
        where TModule : class, IModule, new()
    {
        using var setupTokenSource = new CancellationTokenSource();
        ConditionCancellationTokenSource = setupTokenSource;
        var host = await TestPipelineBuilder.Create()
            .AddModule<TModule>()
            .BuildAsync();
        var module = new TModule();
        var conditionHandler = host.Services.GetRequiredService<IModuleConditionHandler>();
        using var cancellationTokenSource = new CancellationTokenSource();
        ConditionCancellationTokenSource = cancellationTokenSource;
        SubsequentConditionWasEvaluated = false;

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            conditionHandler.ShouldIgnore(module, cancellationTokenSource.Token));
        await Assert.That(SubsequentConditionWasEvaluated).IsFalse();
    }

    [Test]
    public async Task Pipeline_Cancellation_Is_Propagated_To_Discovery()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        SubsequentConditionWasEvaluated = false;

        await Assert.ThrowsAsync<OperationCanceledException>(() => TestPipelineBuilder.Create()
            .AddModule<DiscoveryCancellationModule>()
            .ExecutePipelineAsync(cancellationTokenSource.Token));

        await Assert.That(SubsequentConditionWasEvaluated).IsFalse();
    }

    #endregion
}
