using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Dependencies;

public class ModuleNotRegisteredExceptionTests : TestBase
{
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    // Uses optional dependency - validation passes, but GetModule fails at runtime
    [ModularPipelines.Attributes.DependsOn<Module1>(Optional = true)]
    private class Module2WithOptionalDep : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
        }
    }

    // Uses required dependency (default) - Module1 will be auto-registered
    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2WithRequiredDep : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task Module_Getting_Non_Registered_Module_With_Optional_Dep_Throws_ModuleFailedException()
    {
        // With Optional dependency, validation passes but GetModule fails at runtime
        // The exception is wrapped in ModuleFailedException
        await Assert.ThrowsAsync<ModuleFailedException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module2WithOptionalDep>()
                .ExecutePipelineAsync()
        );
    }

    [Test]
    public async Task Module_With_Required_Dependency_Auto_Registers_Missing_Module()
    {
        // With Required dependency (default), missing modules are auto-registered
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<Module2WithRequiredDep>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        // Module1 was auto-registered
        await Assert.That(pipelineSummary.Modules.Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Module_Getting_Registered_Module_Does_Not_Throw_Exception()
    {
        await Assert.That(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module1>()
                .AddModule<Module2WithOptionalDep>()
                .ExecutePipelineAsync()
        ).ThrowsNothing();
    }
}
