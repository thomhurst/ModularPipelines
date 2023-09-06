using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class DisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var modules = await TestPipelineHostBuilder.Create()
            .AddModule<DisposableModule>()
            .ExecutePipelineAsync();

        Assert.That(modules.OfType<DisposableModule>().Single().IsDisposed, Is.True);
    }

    public class DisposableModule : Module, IDisposable
    {
        public bool IsDisposed { get; private set; }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
