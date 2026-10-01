using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Execution;

/// <summary>
/// Covers the single run-condition seam: <see cref="IRunCondition"/>, <see cref="RunConditionAttribute"/>,
/// <see cref="IPlanningSafe"/>, and the builder condition methods.
/// </summary>
public class RunConditionSeamTests
{
    private static readonly AsyncLocal<List<CancellationToken>?> ObservedTokens = new();

    private static List<CancellationToken> Tokens => ObservedTokens.Value ??= [];

    private sealed class AlwaysTrue : IRunCondition, IPlanningSafe
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    private sealed class AlwaysFalse : IRunCondition, IPlanningSafe
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }

    private sealed class RuntimeOnlyFalse : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }

    private sealed class TokenCapture : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            Tokens.Add(cancellationToken);
            return Task.FromResult(true);
        }
    }

    private sealed class TokenCaptureGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new TokenCapture()];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    private sealed class AnyGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new AlwaysFalse(), new AlwaysTrue()];

        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    private sealed class AllGroup : ConditionGroup
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new AlwaysTrue(), new AlwaysFalse()];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class PlanningSafeValueAttribute(bool result) : RunConditionAttribute(ConditionIntent.Run), IPlanningSafe
    {
        public override string ConditionNames => $"PlanningSafeValue({result})";

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(result);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class RuntimeOnlyValueAttribute(bool result) : RunConditionAttribute(ConditionIntent.Run)
    {
        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(result);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class BranchAlternativeAttribute(bool result) : RunConditionAttribute(ConditionIntent.Run)
    {
        public override Type? GroupKey => typeof(BranchAlternativeAttribute);

        public override string ConditionNames => $"BranchAlternative({result})";

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(result);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class SkipValueAttribute(bool result) : RunConditionAttribute(ConditionIntent.Skip)
    {
        public override Type? GroupKey => typeof(SkipValueAttribute);

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(result);
    }

    [PlanningSafeValue(false)]
    private sealed class PlanningSafeAttributeModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RuntimeOnlyValue(false)]
    private sealed class RuntimeOnlyAttributeModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<AlwaysTrue, AlwaysTrue>]
    private sealed class RunIfAllTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<AlwaysTrue, AlwaysFalse>]
    private sealed class RunIfOneFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [BranchAlternative(false)]
    [BranchAlternative(true)]
    private sealed class OneAlternativeTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [BranchAlternative(false)]
    [BranchAlternative(false)]
    private sealed class NoAlternativeTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [SkipValue(false)]
    [SkipValue(true)]
    private sealed class GroupedSkipModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class WithRunIfFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithRunIf<AlwaysFalse>();
    }

    private sealed class WithRunIfTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithRunIf(new AlwaysTrue());
    }

    private sealed class WithSkipIfTrueModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipIf<AlwaysTrue>();
    }

    private sealed class WithSkipIfInstanceFalseModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipIf(new AlwaysFalse());
    }

    private static async Task<IModuleResult> RunAsync<TModule>()
        where TModule : class, IModule
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModule<TModule>()
            .BuildAsync();
        await host.RunAsync();
        return host.Services.GetRequiredService<IModuleResultRegistry>().GetResult(typeof(TModule))!;
    }

    private static async Task<JsonElement> PlanAsync<TModule>()
        where TModule : class, IModule
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<TModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        using var document = JsonDocument.Parse(await exporter.RenderAsync(DependencyGraphFormat.Json));
        return document.RootElement.GetProperty("nodes").EnumerateArray().Single().Clone();
    }

    [Test]
    public async Task ThirdParty_PlanningSafe_Attribute_Is_Evaluated_During_Planning()
    {
        var node = await PlanAsync<PlanningSafeAttributeModule>();

        using (Assert.Multiple())
        {
            await Assert.That(node.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(node.GetProperty("skipReason").GetString())
                .IsEqualTo("RunIf<PlanningSafeValue(False)> not satisfied");
        }
    }

    [Test]
    public async Task ThirdParty_Attribute_Without_Marker_Stays_Unresolved_During_Planning()
    {
        var node = await PlanAsync<RuntimeOnlyAttributeModule>();

        await Assert.That(node.GetProperty("skipped").ValueKind).IsEqualTo(JsonValueKind.Null);
    }

    [Test]
    public async Task CancellationToken_Reaches_Condition_Through_RunIf_Attribute()
    {
        Tokens.Clear();
        using var cancellationTokenSource = new CancellationTokenSource();

        await new RunIfAttribute<TokenCapture>().EvaluateAsync(
            Mock.Of<IPipelineContext>(),
            cancellationTokenSource.Token);

        await Assert.That(Tokens).IsEquivalentTo([cancellationTokenSource.Token]);
    }

    [Test]
    public async Task CancellationToken_Reaches_Condition_Through_ConditionGroup()
    {
        Tokens.Clear();
        using var cancellationTokenSource = new CancellationTokenSource();

        await new TokenCaptureGroup().EvaluateAsync(Mock.Of<IPipelineContext>(), cancellationTokenSource.Token);

        await Assert.That(Tokens).IsEquivalentTo([cancellationTokenSource.Token]);
    }

    [Test]
    public async Task CancellationToken_Reaches_Condition_Through_WithRunIf()
    {
        Tokens.Clear();
        using var cancellationTokenSource = new CancellationTokenSource();
        var configuration = new ModuleConfigurationBuilder().WithRunIf(new TokenCapture()).Build();

        var decision = await configuration.SkipCondition!(Mock.Of<IModuleContext>(), cancellationTokenSource.Token);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsFalse();
            await Assert.That(Tokens).IsEquivalentTo([cancellationTokenSource.Token]);
        }
    }

    [Test]
    public async Task WithRunIf_Throws_When_Token_Is_Already_Canceled()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        var configuration = new ModuleConfigurationBuilder().WithRunIf<AlwaysTrue>().Build();

        await Assert.That(async () =>
                await configuration.SkipCondition!(Mock.Of<IModuleContext>(), cancellationTokenSource.Token))
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task WithRunIf_False_Skips_With_RunIf_Reason()
    {
        var result = await RunAsync<WithRunIfFalseModule>();

        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(result.SkipDecisionOrDefault!.Reason).IsEqualTo("RunIf<AlwaysFalse> not satisfied");
        }
    }

    [Test]
    public async Task WithRunIf_Instance_True_Runs()
    {
        var result = await RunAsync<WithRunIfTrueModule>();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task WithSkipIf_True_Skips_With_SkipIf_Reason()
    {
        var result = await RunAsync<WithSkipIfTrueModule>();

        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(result.SkipDecisionOrDefault!.Reason).IsEqualTo("SkipIf<AlwaysTrue> returned true");
        }
    }

    [Test]
    public async Task WithSkipIf_Instance_False_Runs()
    {
        var result = await RunAsync<WithSkipIfInstanceFalseModule>();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task WithRunIf_PlanningSafe_Condition_Resolves_During_Planning()
    {
        var configuration = new ModuleConfigurationBuilder().WithRunIf<AlwaysFalse>().Build();

        var decision = await configuration.PlanningSkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        await Assert.That(decision?.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task WithRunIf_RuntimeOnly_Condition_Stays_Unresolved_During_Planning()
    {
        var configuration = new ModuleConfigurationBuilder().WithRunIf(new RuntimeOnlyFalse()).Build();

        var decision = await configuration.PlanningSkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        await Assert.That(decision).IsNull();
    }

    [Test]
    public async Task Builder_Condition_Methods_Reject_Null()
    {
        var builder = new ModuleConfigurationBuilder();

        using (Assert.Multiple())
        {
            await Assert.That(() => builder.WithRunIf(null!)).Throws<ArgumentNullException>();
            await Assert.That(() => builder.WithSkipIf(null!)).Throws<ArgumentNullException>();
        }
    }

    [Test]
    public async Task RunIf_With_Multiple_Conditions_Requires_All()
    {
        var allTrue = await RunAsync<RunIfAllTrueModule>();
        var oneFalse = await RunAsync<RunIfOneFalseModule>();

        using (Assert.Multiple())
        {
            await Assert.That(allTrue.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(oneFalse.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(oneFalse.SkipDecisionOrDefault!.Reason)
                .IsEqualTo("RunIf<AlwaysTrue, AlwaysFalse> not satisfied");
        }
    }

    [Test]
    public async Task Attributes_Sharing_A_GroupKey_Are_Alternatives()
    {
        var oneTrue = await RunAsync<OneAlternativeTrueModule>();
        var noneTrue = await RunAsync<NoAlternativeTrueModule>();

        using (Assert.Multiple())
        {
            await Assert.That(oneTrue.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(noneTrue.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(noneTrue.SkipDecisionOrDefault!.Reason).Contains("No grouped run conditions were met");
        }
    }

    [Test]
    public async Task GroupKey_Is_Ignored_For_Skip_Intent()
    {
        var result = await RunAsync<GroupedSkipModule>();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task ConditionGroup_Combines_Members_With_Its_Logic()
    {
        var context = Mock.Of<IPipelineContext>();

        using (Assert.Multiple())
        {
            await Assert.That(await new AnyGroup().EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new AllGroup().EvaluateAsync(context, CancellationToken.None)).IsFalse();
        }
    }

    [Test]
    public async Task RunConditionAttribute_Rejects_Unknown_Intent()
    {
        await Assert.That(() => new InvalidIntentAttribute()).Throws<ArgumentOutOfRangeException>();
    }

    private sealed class InvalidIntentAttribute() : RunConditionAttribute((ConditionIntent) 42)
    {
        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
