using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class TimedDependencyTests
{
    [Test]
    public async Task OneSecondModule_WillWaitForFiveSecondModule_ThenExecute()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<FiveSecondModule>()
            .AddModule<OneSecondModuleDependentOnFiveSecondModule>()
            .ExecutePipelineAsync();

        var fiveSecondModule = pipelineSummary.Modules.OfType<FiveSecondModule>().Single();
        var oneSecondModuleDependentOnFiveSecondModule = pipelineSummary.Modules.OfType<OneSecondModuleDependentOnFiveSecondModule>().Single();

        var fiveSecondResult = await fiveSecondModule;
        var oneSecondModuleDependentOnFiveSecondResult = await oneSecondModuleDependentOnFiveSecondModule;
        
        using (Assert.Multiple())
        {
            // 5 + 1
            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.Duration).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(900));
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleDuration).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(900));

            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.EndTime).IsGreaterThanOrEqualTo(fiveSecondModule.StartTime + TimeSpan.FromMilliseconds(5900));
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleEnd).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleStart + TimeSpan.FromMilliseconds(5900));

            await Assert.That(oneSecondModuleDependentOnFiveSecondModule.StartTime).IsGreaterThanOrEqualTo(fiveSecondModule.EndTime);
            await Assert.That(oneSecondModuleDependentOnFiveSecondResult.ModuleStart).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleEnd);
        }
    }

    private class FiveSecondModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return new Dictionary<string, object>();
        }
    }

    [DependsOn<FiveSecondModule>]
    private class OneSecondModuleDependentOnFiveSecondModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return new Dictionary<string, object>();
        }
    }
}