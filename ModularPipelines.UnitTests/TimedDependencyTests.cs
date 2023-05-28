using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class TimedDependencyTests
{
    [Test]
    public async Task OneSecondModule_WillWaitForFiveSecondModule_ThenExecute()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<FiveSecondModule>()
                    .AddModule<OneSecondModuleDependentOnFiveSecondModule>();
            })
            .ExecutePipelineAsync();

        var fiveSecondModule = modules.OfType<FiveSecondModule>().Single();
        var oneSecondModuleDependentOnFiveSecondModule = modules.OfType<OneSecondModuleDependentOnFiveSecondModule>().Single();
        
        Assert.Multiple(() =>
        {

            // 5 + 1
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.Duration, Is.GreaterThanOrEqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.EndTime, Is.GreaterThanOrEqualTo(fiveSecondModule.StartTime + TimeSpan.FromSeconds(6)));
            Assert.That(oneSecondModuleDependentOnFiveSecondModule.StartTime, Is.GreaterThanOrEqualTo(fiveSecondModule.EndTime));
        });
    }

    private class FiveSecondModule : Module
    {
        public FiveSecondModule(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return ModuleResult.Empty<IDictionary<string, object>>();
        }
    }

    [DependsOn<FiveSecondModule>]
    private class OneSecondModuleDependentOnFiveSecondModule : Module
    {
        public OneSecondModuleDependentOnFiveSecondModule(IModuleContext moduleContext) : base(moduleContext)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return ModuleResult.Empty<IDictionary<string, object>>();
        }
    }
}