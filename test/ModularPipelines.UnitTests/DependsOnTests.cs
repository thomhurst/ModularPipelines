using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class DependsOnTests : TestBase
{
    private class Module1 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3 : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3WithGetIfRegistered : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModuleIfRegistered<Module1, IDictionary<string, object>?>();
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3WithGet : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<Module1, IDictionary<string, object>?>();
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependsOnSelfModule>]
    private class DependsOnSelfModule : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<Module1, IDictionary<string, object>?>();
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn(typeof(ModuleFailedException))]
    private class DependsOnNonModule : IModule<IDictionary<string, object>?>
    {
        public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<Module1, IDictionary<string, object>?>();
            await Task.Yield();
            return null;
        }
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Present()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Present2()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module3>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Exception_Thrown_When_Dependent_Module_Missing_And_No_Ignore_On_Attribute()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<Module2>()
                .ExecutePipelineAsync())
            .ThrowsException();
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Missing_And_Ignore_On_Attribute()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module3>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Missing_And_Get_If_Registered_Called()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module3WithGetIfRegistered>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Exception_Thrown_When_Dependent_Module_Missing_And_Get_Module_Called()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<Module3WithGet>()
                .ExecutePipelineAsync()).
            ThrowsException();
    }

    [Test]
    public async Task Depends_On_Self_Module_Throws_Exception()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<DependsOnSelfModule>()
                .ExecutePipelineAsync()).
            Throws<ModuleReferencingSelfException>();
    }

    [Test]
    public async Task Depends_On_Non_Module_Throws_Exception()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<DependsOnNonModule>()
                .ExecutePipelineAsync()).
            ThrowsException()
            .And.HasMessageEqualTo("ModularPipelines.Exceptions.ModuleFailedException is not a Module (does not implement IModule)");
    }
}