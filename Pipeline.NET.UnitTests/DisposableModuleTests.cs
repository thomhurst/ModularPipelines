using Pipeline.NET.Context;
using Pipeline.NET.Host;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.UnitTests;

public class DisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await PipelineHostBuilder.Create()
            .AddModule<DisposableModule>()
            .ExecutePipelineAsync();
        
        Assert.That(modules.OfType<DisposableModule>().Single().IsDisposed, Is.True);
    }
    
    public class DisposableModule : Module, IDisposable
    {
        public bool IsDisposed { get; private set; }
        public DisposableModule(IModuleContext context) : base(context)
        {
        }

        protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
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