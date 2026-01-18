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

    [ModularPipelines.Attributes.DependsOn<Module1>]  // Required by default
    private class Module2 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]  // Optional - won't auto-register
    private class Module3 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
    private class Module3WithGetIfRegistered : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModuleIfRegistered<Module1>();
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
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
    public async Task No_Exception_Thrown_When_Dependent_Module_Present_With_Optional()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module3>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Required_Dependency_Is_Auto_Registered_When_Missing()
    {
        // New behavior: Required dependencies are auto-registered if not present
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        // Module1 should have been auto-registered
        await Assert.That(pipelineSummary.Modules.Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Optional_Dependency_Not_Auto_Registered_When_Missing()
    {
        // Optional dependencies are NOT auto-registered
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module3>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        // Only Module3 should be registered (Module1 not auto-registered for optional dep)
        await Assert.That(pipelineSummary.Modules.Count()).IsEqualTo(1);
    }

    [Test]
    public async Task No_Exception_Thrown_When_Optional_Dependency_Missing_And_Get_If_Registered_Called()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module3WithGetIfRegistered>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Exception_Thrown_When_Optional_Dependency_Missing_And_Get_Module_Called()
    {
        // GetModule throws when module is not registered, even for optional deps
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

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
    private class ModuleWithOptionalDep : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task Optional_Dependency_Works_When_Missing()
    {
        // Optional deps don't require the module to be present
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithOptionalDep>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]  // Required by default
    private class ModuleWithRequiredDep : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task Required_Dependency_Auto_Registers_Missing_Module()
    {
        // Required dependencies get auto-registered
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithRequiredDep>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        // Module1 was auto-registered
        var module1 = pipelineSummary.Modules.OfType<Module1>().SingleOrDefault();
        await Assert.That(module1).IsNotNull();
    }

    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
    private class ModuleCheckingUnregisteredDep : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var dep = context.GetModuleIfRegistered<Module1>();
            await Task.Yield();
            return dep == null;  // Should be null since Module1 is not registered (optional dep)
        }
    }

    [Test]
    public async Task Optional_Dependency_Returns_Null_When_GetModuleIfRegistered_Called_On_Unregistered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleCheckingUnregisteredDep>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }
}
