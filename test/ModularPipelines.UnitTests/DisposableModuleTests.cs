using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class DisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DisposableModule>()
            .ExecutePipelineAsync();

        Assert.That(pipelineSummary.Modules.OfType<DisposableModule>().Single().IsDisposed, Is.True);
    }

    public class DisposableModule : Module, IDisposable
    {
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}