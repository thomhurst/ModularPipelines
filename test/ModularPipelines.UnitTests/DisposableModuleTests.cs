using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class DisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) => collection.Configure<PipelineOptions>(opt => opt.ShowProgressInConsole = false))
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<DisposableModule>();
            })
            .ExecutePipelineAsync();

        Assert.That(modules.OfType<DisposableModule>().Single().IsDisposed, Is.True);
    }

    public class DisposableModule : Module, IDisposable
    {
        public bool IsDisposed { get; private set; }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
