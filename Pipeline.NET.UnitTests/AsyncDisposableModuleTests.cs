using Pipeline.NET.Context;
using Pipeline.NET.Host;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.UnitTests;

public class AsyncDisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await PipelineHostBuilder.Create()
            .AddModule<AsyncDisposableModule>()
            .ExecutePipelineAsync();
        
        Assert.That(modules.OfType<AsyncDisposableModule>().Single().IsDisposed, Is.True);
    }
    
    public class AsyncDisposableModule : Module, IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }
        public AsyncDisposableModule(IModuleContext context) : base(context)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
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