using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class AsyncDisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<AsyncDisposableModule>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.OfType<AsyncDisposableModule>().Single().IsDisposed).IsTrue();
    }

    public class AsyncDisposableModule : Module<bool>, IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 100ms to 1ms for faster test execution
            await Task.Delay(1, cancellationToken);
            return true;
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            // Reduced delay from 100ms to 1ms for faster test execution
            await Task.Delay(1);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}