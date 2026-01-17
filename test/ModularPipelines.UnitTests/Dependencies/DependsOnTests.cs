using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Dependencies;

public class DependsOnTests : TestBase
{
    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = false)]
    private class Module2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3WithGetIfRegistered : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModuleIfRegistered<Module1>();
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(IgnoreIfNotRegistered = true)]
    private class Module3WithGet : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependsOnSelfModule>]
    private class DependsOnSelfModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn(typeof(ModuleFailedException))]
    private class DependsOnNonModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
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
    public async Task Exception_Thrown_When_Dependent_Module_Missing_And_Optional_False()
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
            Throws<InvalidModuleTypeException>()
            .And.HasMessageEqualTo("ModularPipelines.Exceptions.ModuleFailedException is not a Module (does not implement IModule)");
    }

    [Test]
    public async Task Optional_Dependency_Default_True_No_Exception_When_Missing()
    {
        // New default: Optional = true, so missing dependencies don't throw
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithOptionalDep>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]  // No Optional = false, so it's optional by default
    private class ModuleWithOptionalDep : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task RequiresDependency_Throws_When_Missing()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
                .AddModule<ModuleWithRequiredDep>()
                .ExecutePipelineAsync())
            .ThrowsException();
    }

    [RequiresDependency<Module1>]
    private class ModuleWithRequiredDep : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}