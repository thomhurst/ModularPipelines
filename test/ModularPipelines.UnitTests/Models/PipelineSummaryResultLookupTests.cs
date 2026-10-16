using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Models;

public class PipelineSummaryResultLookupTests
{
    [Test]
    public async Task Pipeline_Results_Are_Available_By_Exact_Module_Type()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<First.ValueModule>()
            .AddModule<Second.ValueModule>()
            .RunAsync();

        await Assert.That(summary.GetResult<First.ValueModule, int>().Value).IsEqualTo(1);
        await Assert.That(summary.GetResult<Second.ValueModule, int>().Value).IsEqualTo(2);
    }

    [Test]
    public async Task Lookup_Distinguishes_Module_Types_With_The_Same_Name_And_Result_Type()
    {
        var first = ModuleResult<int>.CreateSuccess(1, CreateContext<First.ValueModule>());
        var second = ModuleResult<int>.CreateSuccess(2, CreateContext<Second.ValueModule>());
        var summary = CreateSummary(first, second);

        await Assert.That(summary.GetResult<First.ValueModule, int>()).IsSameReferenceAs(first);
        await Assert.That(summary.GetResult<Second.ValueModule, int>()).IsSameReferenceAs(second);
        await Assert.That(summary.TryGetResult<Second.ValueModule, int>(out var result)).IsTrue();
        await Assert.That(result).IsSameReferenceAs(second);
    }

    [Test]
    [Arguments(false, ModuleStatus.Failed)]
    [Arguments(true, ModuleStatus.Failed)]
    [Arguments(false, ModuleStatus.FailureIgnored)]
    [Arguments(true, ModuleStatus.FailureIgnored)]
    public async Task Lookup_Returns_Typed_Failure_And_Preserves_Metadata(bool typed, ModuleStatus status)
    {
        var exception = new InvalidOperationException("Module failed");
        var context = CreateContext<First.ValueModule>(status);
        ModuleResult failure = typed
            ? ModuleResult<int>.CreateFailure(exception, context)
            : ModuleResult.CreateFailure(exception, context);
        failure = failure with { WorkerId = new WorkerId("worker-1") };
        var summary = CreateSummary(failure);

        await Assert.That(summary.TryGetResult<First.ValueModule, int>(out var result)).IsTrue();
        await Assert.That(result).IsTypeOf<ModuleResult<int>.Failure>();
        await Assert.That(result!.ExceptionOrDefault).IsSameReferenceAs(exception);
        await Assert.That(summary.GetResult<First.ValueModule, int>()).IsEqualTo(result);
        await Assert.That(result.Name).IsEqualTo(failure.Name);
        await Assert.That(result.TypeName).IsEqualTo(failure.TypeName);
        await Assert.That(result.ModuleType).IsEqualTo(failure.ModuleType);
        await Assert.That(result.Status).IsEqualTo(failure.Status);
        await Assert.That(result.StartTime).IsEqualTo(failure.StartTime);
        await Assert.That(result.EndTime).IsEqualTo(failure.EndTime);
        await Assert.That(result.Duration).IsEqualTo(failure.Duration);
        await Assert.That(result.WorkerId).IsEqualTo(failure.WorkerId);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Lookup_Returns_Typed_Skipped_Result(bool typed)
    {
        var decision = SkipDecision.Skip("No changes");
        var context = CreateContext<First.ValueModule>(ModuleStatus.Skipped);
        ModuleResult skipped = typed
            ? ModuleResult<int>.CreateSkipped(decision, context)
            : ModuleResult.CreateSkipped(decision, context);
        var summary = CreateSummary(skipped);

        await Assert.That(summary.TryGetResult<First.ValueModule, int>(out var result)).IsTrue();
        await Assert.That(result).IsTypeOf<ModuleResult<int>.Skipped>();
        await Assert.That(result!.SkipDecisionOrDefault).IsSameReferenceAs(decision);
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(summary.GetResult<First.ValueModule, int>()).IsEqualTo(result);
    }

    [Test]
    public async Task Missing_Result_Returns_Null_And_Get_Throws_With_Module_Type()
    {
        var summary = CreateSummary(ModuleResult<int>.CreateSuccess(1, CreateContext<First.ValueModule>()));

        await Assert.That(summary.TryGetResult<Second.ValueModule, int>(out var result)).IsFalse();
        await Assert.That(result).IsNull();
        var exception = await Assert.That(() => summary.GetResult<Second.ValueModule, int>())
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains(typeof(Second.ValueModule).FullName!);
    }

    [Test]
    public async Task Lookup_Uses_Fully_Qualified_TypeName_When_Runtime_Type_Is_Unavailable()
    {
        var original = ModuleResult<int>.CreateSuccess(42, CreateContext<First.ValueModule>());
        var result = original with { ModuleType = null };
        var summary = CreateSummary(result);

        await Assert.That(summary.GetResult<First.ValueModule, int>()).IsSameReferenceAs(result);
        await Assert.That(summary.TryGetResult<Second.ValueModule, int>(out _)).IsFalse();
    }

    [Test]
    public async Task Lookup_Does_Not_Fall_Back_To_Display_Name_Or_Override_Runtime_Type()
    {
        var original = ModuleResult<int>.CreateSuccess(42, CreateContext<First.ValueModule>());
        var unidentified = original with { ModuleType = null, TypeName = null };
        var summary = CreateSummary(unidentified);
        await Assert.That(summary.TryGetResult<First.ValueModule, int>(out _)).IsFalse();

        var conflicting = original with { TypeName = typeof(Second.ValueModule).FullName };
        summary = CreateSummary(conflicting);
        await Assert.That(summary.GetResult<First.ValueModule, int>()).IsSameReferenceAs(conflicting);
        await Assert.That(summary.TryGetResult<Second.ValueModule, int>(out _)).IsFalse();
    }

    [Test]
    public async Task Deserialized_Summary_Has_No_Typed_Results()
    {
        var summary = CreateSummary(ModuleResult<int>.CreateSuccess(42, CreateContext<First.ValueModule>()));
        var restored = JsonSerializer.Deserialize<PipelineSummary>(JsonSerializer.Serialize(summary))!;

        await Assert.That(restored.Modules).IsEmpty();
        await Assert.That(restored.TryGetResult<First.ValueModule, int>(out var result)).IsFalse();
        await Assert.That(result).IsNull();
        await Assert.That(() => restored.GetResult<First.ValueModule, int>()).Throws<InvalidOperationException>();
    }

    private static PipelineSummary CreateSummary(params IModuleResult[] results)
        => new([], results, TimeSpan.Zero, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

    private static ModuleExecutionContext CreateContext<TModule>(ModuleStatus status = ModuleStatus.Succeeded)
        where TModule : Module<int>, new()
        => new(new TModule(), typeof(TModule))
        {
            Status = status,
            StartTime = DateTimeOffset.UtcNow.AddSeconds(-1),
            EndTime = DateTimeOffset.UtcNow,
        };

    private static class First
    {
        public sealed class ValueModule : Module<int>
        {
            protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                => Task.FromResult(1);
        }
    }

    private static class Second
    {
        public sealed class ValueModule : Module<int>
        {
            protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                => Task.FromResult(2);
        }
    }
}
