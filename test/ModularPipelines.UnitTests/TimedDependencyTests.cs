using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class TimedDependencyTests
{
    private static readonly TimeSpan LongModuleDelay = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan ShortModuleDelay = TimeSpan.FromMilliseconds(20);

    [Test]
    public async Task OneSecondModule_WillWaitForFiveSecondModule_ThenExecute()
    {
        var timeProvider = new FakeTimeProvider();

        var host = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<FiveSecondModule>()
            .AddModule<OneSecondModuleDependentOnFiveSecondModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var fiveSecondResult = resultRegistry.GetResult(typeof(FiveSecondModule))!;
        var oneSecondResult = resultRegistry.GetResult(typeof(OneSecondModuleDependentOnFiveSecondModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(oneSecondResult.ModuleDuration).IsGreaterThanOrEqualTo(ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-10)));

            await Assert.That(oneSecondResult.ModuleEnd).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleStart + LongModuleDelay + ShortModuleDelay.Add(TimeSpan.FromMilliseconds(-20)));

            await Assert.That(oneSecondResult.ModuleStart).IsGreaterThanOrEqualTo(fiveSecondResult.ModuleEnd);
        }
    }

    private class FiveSecondModule : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(LongModuleDelay, cancellationToken);
            return new Dictionary<string, object>();
        }
    }

    [ModularPipelines.Attributes.DependsOn<FiveSecondModule>]
    private class OneSecondModuleDependentOnFiveSecondModule : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ShortModuleDelay, cancellationToken);
            return new Dictionary<string, object>();
        }
    }
}