using ModularPipelines.Console;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineOutputCoordinatorTests
{
    [Test]
    public async Task Dispose_SchedulesBuffersCreatedByRetainedWriteFlush()
    {
        var events = new List<string>();
        var retainedBuffer = new Mock<IModuleOutputBuffer>();
        retainedBuffer.SetupGet(x => x.ModuleType).Returns(typeof(PipelineOutputCoordinatorTests));
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator.Setup(x => x.FlushPendingWritesAsync())
            .Callback(() => events.Add("retained"))
            .ReturnsAsync([retainedBuffer.Object]);
        consoleCoordinator.Setup(x => x.FlushModuleOutput()).Callback(() => events.Add("unattributed"));

        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                retainedBuffer.Object,
                typeof(PipelineOutputCoordinatorTests),
                It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("scheduled"))
            .Returns(Task.CompletedTask);
        outputCoordinator.Setup(x => x.FlushDeferredAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("deferred"))
            .Returns(Task.CompletedTask);

        var coordinator = new PipelineOutputCoordinator(
            new RecordingProgressExecutor(events),
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            outputCoordinator.Object);
        var scope = await coordinator.InitializeAsync();

        await scope.DisposeAsync();

        await Assert.That(string.Join(",", events))
            .IsEqualTo("retained,scheduled,progress,deferred,unattributed");
        outputCoordinator.VerifyAll();
    }

    private sealed class RecordingProgressExecutor(List<string> events) : IPrintProgressExecutor
    {
        public Task<IPrintProgressExecutor> InitializeAsync() => Task.FromResult<IPrintProgressExecutor>(this);

        public ValueTask DisposeAsync()
        {
            events.Add("progress");
            return ValueTask.CompletedTask;
        }
    }
}
