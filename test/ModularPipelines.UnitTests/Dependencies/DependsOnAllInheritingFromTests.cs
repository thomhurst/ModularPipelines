using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class DependsOnAllInheritingFromTests : TestBase
{
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(50);

    private abstract class BaseModule : Module<bool>
    {
        protected internal abstract override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
    }

    // Generic base module for testing open generic type dependencies (Issue #1337)
    private abstract class GenericBaseModule<T> : Module<T>;

    private class GenericModule1 : GenericBaseModule<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return 42;
        }
    }

    private class GenericModule2 : GenericBaseModule<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return "test";
        }
    }

    // This module depends on all modules inheriting from the OPEN generic type GenericBaseModule<>
    [DependsOnAllModulesInheritingFrom(typeof(GenericBaseModule<>))]
    private class DependsOnOpenGenericModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    private class Module1 : BaseModule
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : BaseModule
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
    private class Module3 : BaseModule
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOnAllModulesInheritingFrom<BaseModule>]
    private class Module4 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
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

    [Test]
    public async Task DependsOnAllModulesInheritingFrom_Works_With_Open_Generic_Types()
    {
        // Regression test for Issue #1337
        // DependsOnAllModulesInheritingFrom(typeof(GenericBaseModule<>)) should wait for
        // GenericModule1 : GenericBaseModule<int> and GenericModule2 : GenericBaseModule<string>
        var timeProvider = new FakeTimeProvider();

        var host = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<GenericModule1>()
            .AddModule<GenericModule2>()
            .AddModule<DependsOnOpenGenericModule>()
            .BuildHostAsync();

        var pipelineSummary = await host.ExecutePipelineAsync();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var result1 = resultRegistry.GetResult(typeof(GenericModule1))!;
        var result2 = resultRegistry.GetResult(typeof(GenericModule2))!;
        var dependentResult = resultRegistry.GetResult(typeof(DependsOnOpenGenericModule))!;

        // The dependent module should start AFTER both generic modules complete
        await Assert.That(dependentResult.ModuleStart).IsGreaterThanOrEqualTo(result1.ModuleEnd);
        await Assert.That(dependentResult.ModuleStart).IsGreaterThanOrEqualTo(result2.ModuleEnd);
    }
}
