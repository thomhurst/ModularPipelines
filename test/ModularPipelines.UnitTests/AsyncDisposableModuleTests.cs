using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class AsyncDisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<AsyncDisposableModule>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.OfType<AsyncDisposableModule>().Single().IsDisposed).Is.True();
    }

    public class AsyncDisposableModule : Module, IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await Task.Delay(100);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}