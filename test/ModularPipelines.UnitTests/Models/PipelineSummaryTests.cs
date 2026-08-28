using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
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
}
