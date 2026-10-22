using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Registration;

public class PipelineBuilderLifecycleTests
{
    [Test]
    [Arguments("build", "build")]
    [Arguments("build", "validate")]
    [Arguments("build", "run")]
    [Arguments("validate", "build")]
    [Arguments("validate", "validate")]
    [Arguments("validate", "run")]
    [Arguments("run", "build")]
    public async Task ReusingBuilderReportsConsumedBuilder(string firstOperation, string secondOperation)
    {
        var builder = TestPipelineBuilder.Create().AddModule<TestModule>();
        await StartAsync(builder, firstOperation);

        await AssertConsumedAsync(builder, secondOperation);
    }

    [Test]
    public async Task FailedBuildConsumesBuilder()
    {
        var builder = TestPipelineBuilder.Create();
        await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await AssertConsumedAsync(builder, "build");
    }

    [Test]
    public async Task FailedValidationConsumesBuilder()
    {
        var builder = TestPipelineBuilder.Create();
        var result = await builder.ValidateAsync();
        await Assert.That(result.HasErrors).IsTrue();

        await AssertConsumedAsync(builder, "run");
    }

    [Test]
    [Arguments("build")]
    [Arguments("validate")]
    [Arguments("run")]
    public async Task PreCanceledStartupDoesNotConsumeBuilder(string operation)
    {
        var builder = TestPipelineBuilder.Create().AddModule<TestModule>();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => StartAsync(builder, operation, cancellation.Token));

        await using var pipeline = await builder.BuildAsync();
        await Assert.That(pipeline).IsNotNull();
    }

    [Test]
    public async Task CancellationAfterStartupConsumesBuilder()
    {
        var builder = TestPipelineBuilder.Create().AddModule<TestModule>();
        using var cancellation = new CancellationTokenSource();

        await Assert.ThrowsAsync<OperationCanceledException>(() => builder.BuildAsync((_, token) =>
        {
            cancellation.Cancel();
            token.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }, cancellation.Token));

        await AssertConsumedAsync(builder, "build");
    }

    [Test]
    public async Task ConcurrentStartupReportsConsumedBuilder()
    {
        var builder = TestPipelineBuilder.Create().AddModule<TestModule>();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var startup = builder.BuildAsync(async (_, token) =>
        {
            started.SetResult();
            await release.Task.WaitAsync(token);
        });

        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(30));
            await AssertConsumedAsync(builder, "validate");
        }
        finally
        {
            release.TrySetResult();
            await using var pipeline = await startup;
        }
    }

    private static async Task AssertConsumedAsync(PipelineBuilder builder, string operation)
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => StartAsync(builder, operation));
        await Assert.That(exception!.Message).Contains("PipelineBuilder has already been consumed");
        await Assert.That(exception.Message).Contains("Pipeline.CreateBuilder");
    }

    private static async Task StartAsync(PipelineBuilder builder, string operation, CancellationToken cancellationToken = default)
    {
        switch (operation)
        {
            case "build":
                await using (var pipeline = await builder.BuildAsync(cancellationToken))
                {
                }

                break;
            case "validate":
                await builder.ValidateAsync(cancellationToken);
                break;
            case "run":
                await builder.RunAsync(cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

    private sealed class TestModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
