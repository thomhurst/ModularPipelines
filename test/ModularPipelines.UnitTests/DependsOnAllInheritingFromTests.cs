using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class DependsOnAllInheritingFromTests : TestBase
{
    // Reduced delay from 1 second to 50ms for faster test execution
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(50);

    private abstract class BaseModule : Module
    {
    }

    private class Module1 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return await NothingAsync();
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return await NothingAsync();
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return await NothingAsync();
        }
    }

    [ModularPipelines.Attributes.DependsOnAllModulesInheritingFrom<BaseModule>]
    private class Module4 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Present()
    {
        var timeProvider = new FakeTimeProvider();

        var pipelineSummary = await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        var module1 = pipelineSummary.GetModule<Module1>();
        var module2 = pipelineSummary.GetModule<Module2>();
        var module3 = pipelineSummary.GetModule<Module3>();
        var module4 = pipelineSummary.GetModule<Module4>();

        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module1.StartTime.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module1.EndTime);

        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module2.StartTime.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module2.EndTime);

        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module3.StartTime.Add(ModuleDelay.Add(TimeSpan.FromMilliseconds(-25))));
        await Assert.That(module4.StartTime).IsGreaterThanOrEqualTo(module3.EndTime);
    }
}