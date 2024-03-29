using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ModuleNotInitializedTests : TestBase
{
    private class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class ModuleNotInitializedModule : Module
    {
        private readonly Module1 _module1;

        public ModuleNotInitializedModule()
        {
            _module1 = GetModule<Module1>();
        }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    [Test]
    public async Task ThrowsException()
    {
        await Assert.ThrowsAsync<ModuleNotInitializedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<Module1>()
                .AddModule<ModuleNotInitializedModule>()
                .BuildHostAsync()
        );
    }
}