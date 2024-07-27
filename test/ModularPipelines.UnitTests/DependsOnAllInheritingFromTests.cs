using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class DependsOnAllInheritingFromTests : TestBase
{
    private abstract class BaseModule : Module
    {
    }
    
    private class Module1 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return await NothingAsync();
        }
    }

    [DependsOn<Module1>]
    private class Module2 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return await NothingAsync();
        }
    }

    [DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3 : BaseModule
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return await NothingAsync();
        }
    }

    [DependsOnAllModulesInheritingFrom<BaseModule>]
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
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        var module1 = pipelineSummary.GetModule<Module1>();
        var module2 = pipelineSummary.GetModule<Module2>();
        var module3 = pipelineSummary.GetModule<Module3>();
        var module4 = pipelineSummary.GetModule<Module4>();
        
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module1.StartTime.Add(TimeSpan.FromSeconds(1)));
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module1.EndTime);
        
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module2.StartTime.Add(TimeSpan.FromSeconds(1)));
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module2.EndTime);
        
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module3.StartTime.Add(TimeSpan.FromSeconds(1)));
        await Assert.That(module4.StartTime).Is.GreaterThanOrEqualTo(module3.EndTime);
    }
}