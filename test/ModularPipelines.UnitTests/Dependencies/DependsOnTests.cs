using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

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

#pragma warning disable CS0618 // Intentionally verifies validation of the legacy non-generic attribute.
    [ModularPipelines.Attributes.DependsOn(typeof(ModuleFailedException))]
#pragma warning restore CS0618
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
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task No_Exception_Thrown_When_Dependent_Module_Present_With_Optional()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module3>()
            .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Required_Dependency_Is_Auto_Registered_When_Missing()
    {
        // New behavior: Required dependencies are auto-registered if not present
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<Module2>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
        // Module1 should have been auto-registered
        await Assert.That(pipelineSummary.Modules.Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Optional_Dependency_Not_Auto_Registered_When_Missing()
    {
        // Optional dependencies are NOT auto-registered
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<Module3>()
            .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
        // Only Module3 should be registered (Module1 not auto-registered for optional dep)
        await Assert.That(pipelineSummary.Modules.Count()).IsEqualTo(1);
    }

    [Test]
    public async Task No_Exception_Thrown_When_Optional_Dependency_Missing_And_Get_If_Registered_Called()
    {
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<Module3WithGetIfRegistered>()
            .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Exception_Thrown_When_Optional_Dependency_Missing_And_Get_Module_Called()
    {
        // GetModule throws when module is not registered, even for optional deps
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .AddModule<Module3WithGet>()
                .RunAsync()).
            ThrowsException();
    }

    [Test]
    public async Task Depends_On_Self_Module_Throws_Exception()
    {
        var exception = await Assert.ThrowsAsync<PipelineValidationException>(
            async () => await TestPipelineBuilder.Create()
                .AddModule<DependsOnSelfModule>()
                .RunAsync());

        await Assert.That(exception!.ValidationResult.Errors.Single().Message)
            .IsEqualTo("Module 'DependsOnSelfModule' cannot reference itself. A module cannot depend on its own result.");
        await Assert.That(exception.InnerException).IsTypeOf<ModuleSelfDependencyException>();
    }

    [Test]
    public async Task Depends_On_Non_Module_Throws_Exception()
    {
        await Assert.That(async () => await TestPipelineBuilder.Create()
                .AddModule<DependsOnNonModule>()
                .RunAsync()).
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
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithOptionalDep>()
            .RunAsync();
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
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
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ModuleWithRequiredDep>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
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
        var pipelineSummary = await TestPipelineBuilder.Create()
            .AddModule<ModuleCheckingUnregisteredDep>()
            .RunAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }
}
