using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class DependsOnAllInheritingFromTests : TestBase
{
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(50);

    private abstract class BaseModule : IModule<IDictionary<string, object>?>
    {
        public abstract Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
    }

    private class Module1 : BaseModule
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : BaseModule
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3 : BaseModule
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOnAllModulesInheritingFrom<BaseModule>]
    private class Module4 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Present()
    {
        var timeProvider = new FakeTimeProvider();

        var host = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .BuildHostAsync();

        var pipelineSummary = await host.ExecutePipelineAsync();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var result1 = resultRegistry.GetResult(typeof(Module1))!;
        var result2 = resultRegistry.GetResult(typeof(Module2))!;
        var result3 = resultRegistry.GetResult(typeof(Module3))!;
        var result4 = resultRegistry.GetResult(typeof(Module4))!;

        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result1.ModuleStart.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result1.ModuleEnd);

        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result2.ModuleStart.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result2.ModuleEnd);

        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result3.ModuleStart.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(result4.ModuleStart).IsGreaterThanOrEqualTo(result3.ModuleEnd);
    }
}
