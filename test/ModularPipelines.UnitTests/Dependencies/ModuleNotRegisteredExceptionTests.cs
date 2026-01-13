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
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<Module1, bool>();
            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task Module_Getting_Non_Registered_Module_Throws_Exception()
    {
        await Assert.ThrowsAsync<ModuleNotRegisteredException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module2>()
                .ExecutePipelineAsync()
        );
    }

    [Test]
    public async Task Module_Getting_Registered_Module_Does_Not_Throw_Exception()
    {
        await Assert.That(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<Module1>()
                .AddModule<Module2>()
                .ExecutePipelineAsync()
        ).ThrowsNothing();
    }
}
