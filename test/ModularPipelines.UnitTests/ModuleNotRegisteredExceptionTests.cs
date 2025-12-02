using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ModuleNotRegisteredExceptionTests : TestBase
{
    private class Module1 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            _ = context.GetModule<Module1, IDictionary<string, object>?>();
            await Task.Yield();
            return null;
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
