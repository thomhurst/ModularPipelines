using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

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

    // Uses optional dependency (default) - validation passes, but GetModule fails at runtime
    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2WithOptionalDep : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = await context.GetModule<Module1>();
            await Task.Yield();
            return true;
        }
    }

    // Uses required dependency - validation fails if Module1 is not registered
    [RequiresDependency<Module1>]
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
        // With Optional dependency (default), validation passes but GetModule fails at runtime
        // The exception is wrapped in ModuleFailedException
        await Assert.ThrowsAsync<ModuleFailedException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module2WithOptionalDep>()
                .ExecutePipelineAsync()
        );
    }

    [Test]
    public async Task Module_With_Required_Non_Registered_Dependency_Throws_ModuleNotRegisteredException()
    {
        // With Required dependency, validation fails and throws ModuleNotRegisteredException directly
        await Assert.ThrowsAsync<ModuleNotRegisteredException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module2WithRequiredDep>()
                .ExecutePipelineAsync()
        );
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
