using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Logging;

public class ModuleStateLoggingTests
{
    [Test]
    public async Task CompletedModule_UsesPlaceholderWhenThereAreNoLockKeys()
    {
        var logs = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton(logs);
                services.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<ModuleWithoutLocks>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(logs.ToString()).Contains("Module ModuleWithoutLocks completed after");
        await Assert.That(logs.ToString()).Contains("with lock keys: (none)");
    }

    private sealed class ModuleWithoutLocks : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
