using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Models;

public class PipelineSummaryTests
{
    [Test]
    public async Task Public_Contract_Uses_Sealed_Summary_And_Consistent_Timing()
    {
        var type = typeof(PipelineSummary);
        await Assert.That(type.IsSealed).IsTrue();
        await Assert.That(type.GetProperty("Status")).IsNull();
        foreach (var property in new[] { "StartTime", "EndTime", "Duration", "Succeeded" })
        {
            await Assert.That(type.GetProperty(property)).IsNotNull();
        }
    }

    private sealed class UnfinishedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>("unused");
    }

    [Test]
    public async Task Missing_Registry_Entries_Are_Unknown_Without_Fabricated_Failures()
    {
        var module = new UnfinishedModule();
        var resultRegistry = new ModuleResultRegistry();
        var metricsCollector = new Mock<IMetricsCollector>();
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider
            .Setup(x => x.GetMaxDegreeOfParallelism())
            .Returns(1);

        var factory = new PipelineSummaryFactory(
            resultRegistry,
            metricsCollector.Object,
            parallelLimitProvider.Object);

        var now = DateTimeOffset.UtcNow;
        var summary = factory.Create([module], TimeSpan.Zero, now, now);

        using (Assert.Multiple())
        {
            await Assert.That(summary.Modules).Count().IsEqualTo(1);
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(summary.Succeeded).IsFalse();
        }
    }

    [Test]
    public async Task Failures_And_IgnoredFailures_Partition_Failed_Results()
    {
        var succeeded = CreateResult(ModuleStatus.Succeeded, null);
        var failed = CreateResult(ModuleStatus.Failed, new InvalidOperationException("failed"));
        var timedOut = CreateResult(ModuleStatus.TimedOut, new TimeoutException("timed out"));
        var ignored = CreateResult(ModuleStatus.FailureIgnored, new InvalidOperationException("ignored"));
        var skipped = CreateResult(ModuleStatus.Skipped, null);
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary(
            [],
            [succeeded, failed, timedOut, ignored, skipped],
            TimeSpan.Zero,
            now,
            now);

        using (Assert.Multiple())
        {
            await Assert.That(summary.Failures).IsEquivalentTo([failed, timedOut]);
            await Assert.That(summary.IgnoredFailures).IsEquivalentTo([ignored]);
            await Assert.That(summary.Succeeded).IsFalse();
        }
    }

    [Test]
    public async Task Failures_Are_Empty_When_Only_Ignored_Failures_Exist()
    {
        var ignored = CreateResult(ModuleStatus.FailureIgnored, new InvalidOperationException("ignored"));
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary([], [ignored], TimeSpan.Zero, now, now);

        using (Assert.Multiple())
        {
            await Assert.That(summary.Failures).IsEmpty();
            await Assert.That(summary.IgnoredFailures).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(ModuleStatus.Succeeded, true)]
    [Arguments(ModuleStatus.Skipped, true)]
    [Arguments(ModuleStatus.RestoredFromCache, true)]
    [Arguments(ModuleStatus.RestoredFromHistory, true)]
    [Arguments(ModuleStatus.FailureIgnored, true)]
    [Arguments(ModuleStatus.Failed, false)]
    [Arguments(ModuleStatus.TimedOut, false)]
    [Arguments(ModuleStatus.Canceled, false)]
    [Arguments(ModuleStatus.DependencyFailed, false)]
    [Arguments(ModuleStatus.NotStarted, false)]
    [Arguments(ModuleStatus.Running, false)]
    [Arguments(ModuleStatus.Unknown, false)]
    public async Task Success_Requires_Accepted_Terminal_Results(ModuleStatus status, bool expected)
    {
        var result = CreateResult(status, status == ModuleStatus.FailureIgnored ? new Exception("ignored") : null);
        var start = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary([new UnfinishedModule()], [result], TimeSpan.FromSeconds(2), start, start.AddSeconds(2));

        await Assert.That(summary.Succeeded).IsEqualTo(expected);
        var json = System.Text.Json.JsonSerializer.Serialize(summary);
        var restored = System.Text.Json.JsonSerializer.Deserialize<PipelineSummary>(json)!;
        await Assert.That(restored.Succeeded).IsEqualTo(expected);
        await Assert.That(restored.StartTime).IsEqualTo(summary.StartTime);
        await Assert.That(restored.EndTime).IsEqualTo(summary.EndTime);
        await Assert.That(restored.Duration).IsEqualTo(summary.Duration);
        await Assert.That(json).DoesNotContain("\"Status\"");
    }

    [Test]
    public async Task Pipeline_Failure_Overrides_Successful_Module_Results()
    {
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary([new UnfinishedModule()], [CreateResult(ModuleStatus.Succeeded, null)], TimeSpan.Zero, now, now)
        {
            StatusOverride = ModuleStatus.Failed,
        };
        await Assert.That(summary.Succeeded).IsFalse();
    }

    [Test]
    [Arguments("{}")]
    [Arguments("{\"Status\":\"Failed\"}")]
    [Arguments("{\"Status\":\"Canceled\"}")]
    public async Task Missing_Serialized_Success_Does_Not_Imply_Success(string json)
    {
        var summary = System.Text.Json.JsonSerializer.Deserialize<PipelineSummary>(json)!;
        await Assert.That(summary.Succeeded).IsFalse();
    }

    [Test]
    public async Task Cancellation_With_Engine_Exception_Is_Not_Successful()
    {
        var now = DateTimeOffset.UtcNow;
        var canceled = CreateResult(ModuleStatus.Canceled, new OperationCanceledException());
        var summary = new PipelineSummary([new UnfinishedModule()], [canceled], TimeSpan.Zero, now, now);
        await Assert.That(summary.Succeeded).IsFalse();
        await Assert.That(summary.Failures).IsEquivalentTo([canceled]);
    }

    private static IModuleResult CreateResult(ModuleStatus status, Exception? exception)
    {
        var result = new Mock<IModuleResult>();
        result.SetupGet(x => x.Status).Returns(status);
        result.SetupGet(x => x.ExceptionOrDefault).Returns(exception);
        return result.Object;
    }
}
