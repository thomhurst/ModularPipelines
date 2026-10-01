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
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Unknown);
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
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Failed);
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

    private static IModuleResult CreateResult(ModuleStatus status, Exception? exception)
    {
        var result = new Mock<IModuleResult>();
        result.SetupGet(x => x.Status).Returns(status);
        result.SetupGet(x => x.ExceptionOrDefault).Returns(exception);
        return result.Object;
    }
}
