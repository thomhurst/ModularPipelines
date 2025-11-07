using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class TimedDependencyTests
{
    // Reduced delays from 5 seconds / 1 second to 100ms / 20ms for 50x faster test execution
    private static readonly TimeSpan LongModuleDelay = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan ShortModuleDelay = TimeSpan.FromMilliseconds(20);

    [Test]
    public async Task OneSecondModule_WillWaitForFiveSecondModule_ThenExecute()
    {
        var timeProvider = new FakeTimeProvider();

        var pipelineSummary = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<FiveSecondModule>()
            .AddModule<OneSecondModuleDependentOnFiveSecondModule>()
            .ExecutePipelineAsync();

        var fiveSecondModule = pipelineSummary.Modules.OfType<FiveSecondModule>().Single();
        var oneSecondModuleDependentOnFiveSecondModule = pipelineSummary.Modules.OfType<OneSecondModuleDependentOnFiveSecondModule>().Single();

        var fiveSecondResult = await fiveSecondModule;
        var oneSecondModuleDependentOnFiveSecondResult = await oneSecondModuleDependentOnFiveSecondModule;

        using (Assert.Multiple())
        {
            // Verify dependent module duration is at least the short delay (with tolerance for timing variations)
            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.Duration).IsGreaterThanOrEqualTo(ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-10)));
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleDuration).IsGreaterThanOrEqualTo(ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-10)));

            // Verify dependent module ends after first module started + both delays
            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.EndTime).IsGreaterThanOrEqualTo(fiveSecondModule.StartTime + LongModuleDelay + ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-20)));
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleEnd).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleStart + LongModuleDelay + ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-20)));

            // Verify dependent module starts after first module ends
            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.StartTime).IsGreaterThanOrEqualTo(fiveSecondModule.EndTime);
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleStart).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleEnd);
        }
    }

    private class FiveSecondModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(LongModuleDelay, cancellationToken);
            return new Dictionary<string, object>();
        }
    }

    [ModularPipelines.Attributes.DependsOn<FiveSecondModule>]
    private class OneSecondModuleDependentOnFiveSecondModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ShortModuleDelay, cancellationToken);
            return new Dictionary<string, object>();
        }
    }
}