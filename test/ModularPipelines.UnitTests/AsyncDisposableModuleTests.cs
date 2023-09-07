using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class AsyncDisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await TestPipelineHostBuilder.Create()
            .AddModule<AsyncDisposableModule>()
            .ExecutePipelineAsync();

        Assert.That(modules.OfType<AsyncDisposableModule>().Single().IsDisposed, Is.True);
    }

    public class AsyncDisposableModule : Module, IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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
