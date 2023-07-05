using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class AsyncDisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await PipelineHostBuilder.Create()
            .ConfigureServices((context, collection) =>
            {
                collection.AddModule<AsyncDisposableModule>();
            })
            .ExecutePipelineAsync();

        Assert.That(modules.OfType<AsyncDisposableModule>().Single().IsDisposed, Is.True);
    }

    public class AsyncDisposableModule : Module, IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Delay(100);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
