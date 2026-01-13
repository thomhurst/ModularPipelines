using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class DisposableModuleTests
{
    [Test]
    public async Task SuccessfullyDisposed()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DisposableModule>()
            .ExecutePipelineAsync();
        await Assert.That(pipelineSummary.Modules.OfType<DisposableModule>().Single().IsDisposed).IsTrue();
    }

    public class DisposableModule : Module<bool>, IDisposable
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
        public void Dispose()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}