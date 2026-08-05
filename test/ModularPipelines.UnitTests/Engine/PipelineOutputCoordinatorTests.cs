using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineOutputCoordinatorTests
{
    private sealed class OptionsTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [Test]
    public async Task PipelineBuilder_CopiesModuleOutputFlushSettings()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        var expectedInterval = TimeSpan.FromSeconds(17);
        const int expectedThreshold = 23;
        builder.ConfigurePipelineOptions(options => options with
        {
            Console = options.Console with
            {
                ModuleOutputFlushInterval = expectedInterval,
                ModuleOutputFlushThreshold = expectedThreshold,
            },
        });

        await using var pipeline = await builder.BuildAsync();
        var runtimeOptions = pipeline.Services
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<PipelineOptions>>()
            .Value;

        await Assert.That(runtimeOptions.Console.ModuleOutputFlushInterval).IsEqualTo(expectedInterval);
        await Assert.That(runtimeOptions.Console.ModuleOutputFlushThreshold).IsEqualTo(expectedThreshold);
    }

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
        consoleCoordinator.Setup(x => x.FlushModuleOutputAsync())
            .Callback(() => events.Add("unattributed"))
            .Returns(Task.CompletedTask);

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
        outputCoordinator.Setup(x => x.WaitForPendingFlushesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("quiesced"))
            .Returns(Task.CompletedTask);

        var coordinator = new PipelineOutputCoordinator(
            new RecordingProgressExecutor(events),
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            outputCoordinator.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ModuleOutputFlushInterval = TimeSpan.Zero },
            }),
            Mock.Of<ILogger<PipelineOutputCoordinator>>());
        var scope = await coordinator.InitializeAsync();

        await scope.DisposeAsync();

        await Assert.That(string.Join(",", events))
            .IsEqualTo("quiesced,retained,scheduled,progress,deferred,unattributed");
        outputCoordinator.VerifyAll();
    }

    [Test]
    public async Task Dispose_FlushesDeferredOutputWhenEarlierTeardownFails()
    {
        var events = new List<string>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.FlushPendingWritesAsync())
            .Callback(() => events.Add("retained"))
            .ThrowsAsync(new InvalidOperationException("retained flush failed"));
        consoleCoordinator
            .Setup(x => x.FlushModuleOutputAsync())
            .Callback(() => events.Add("unattributed"))
            .Returns(Task.CompletedTask);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.WaitForPendingFlushesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("quiesced"))
            .Returns(Task.CompletedTask);
        outputCoordinator
            .Setup(x => x.FlushDeferredAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("deferred"))
            .Returns(Task.CompletedTask);
        var coordinator = new PipelineOutputCoordinator(
            new RecordingProgressExecutor(events),
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            outputCoordinator.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ModuleOutputFlushInterval = TimeSpan.Zero },
            }),
            Mock.Of<ILogger<PipelineOutputCoordinator>>());
        var scope = await coordinator.InitializeAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await scope.DisposeAsync());

        await Assert.That(string.Join(",", events))
            .IsEqualTo("quiesced,retained,progress,deferred,unattributed");
    }

    [Test]
    public async Task Dispose_AggregatesTeardownFailuresInExecutionOrder()
    {
        var events = new List<string>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.FlushPendingWritesAsync())
            .Callback(() => events.Add("retained"))
            .ThrowsAsync(new InvalidOperationException("retained flush failed"));
        consoleCoordinator
            .Setup(x => x.FlushModuleOutputAsync())
            .Callback(() => events.Add("unattributed"))
            .ThrowsAsync(new IOException("unattributed flush failed"));
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.WaitForPendingFlushesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("quiesced"))
            .Returns(Task.CompletedTask);
        outputCoordinator
            .Setup(x => x.FlushDeferredAsync(It.IsAny<CancellationToken>()))
            .Callback(() => events.Add("deferred"))
            .ThrowsAsync(new ApplicationException("deferred flush failed"));
        var coordinator = new PipelineOutputCoordinator(
            new RecordingProgressExecutor(events),
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            outputCoordinator.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ModuleOutputFlushInterval = TimeSpan.Zero },
            }),
            Mock.Of<ILogger<PipelineOutputCoordinator>>());
        var scope = await coordinator.InitializeAsync();

        var exception = await Assert.ThrowsAsync<AggregateException>(async () =>
            await scope.DisposeAsync());

        await Assert.That(string.Join(",", events))
            .IsEqualTo("quiesced,retained,progress,deferred,unattributed");
        await Assert.That(exception!.InnerExceptions.Select(inner => inner.Message))
            .IsEquivalentTo([
                "retained flush failed",
                "deferred flush failed",
                "unattributed flush failed",
            ]);
    }

    [Test]
    public async Task RunningScope_PeriodicallyFlushesInProgressOutput()
    {
        var flushObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.FlushInProgressModuleOutputAsync(It.IsAny<CancellationToken>()))
            .Callback(() => flushObserved.TrySetResult())
            .Returns(Task.CompletedTask);
        consoleCoordinator
            .Setup(x => x.FlushPendingWritesAsync())
            .ReturnsAsync([]);
        consoleCoordinator
            .Setup(x => x.FlushModuleOutputAsync())
            .Returns(Task.CompletedTask);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.FlushDeferredAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        outputCoordinator
            .Setup(x => x.WaitForPendingFlushesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var coordinator = new PipelineOutputCoordinator(
            new RecordingProgressExecutor([]),
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            outputCoordinator.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions
                {
                    ModuleOutputFlushInterval = TimeSpan.FromMilliseconds(10),
                },
            }),
            Mock.Of<ILogger<PipelineOutputCoordinator>>());

        var scope = await coordinator.InitializeAsync();
        await flushObserved.Task.WaitAsync(TimeSpan.FromSeconds(1));
        await scope.DisposeAsync();

        consoleCoordinator.Verify(
            x => x.FlushInProgressModuleOutputAsync(It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Test]
    [Arguments(-1L)]
    [Arguments((long) uint.MaxValue)]
    public async Task Initialize_InvalidLiveFlushIntervalThrowsImmediately(long milliseconds)
    {
        var events = new List<string>();
        var progressExecutor = new Mock<IPrintProgressExecutor>();
        progressExecutor
            .Setup(x => x.InitializeAsync())
            .Callback(() => events.Add("progress"))
            .ReturnsAsync(progressExecutor.Object);
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.Install())
            .Callback(() => events.Add("install"));
        consoleCoordinator
            .Setup(x => x.EnableOutputBuffering())
            .Callback(() => events.Add("buffer"));
        var coordinator = new PipelineOutputCoordinator(
            progressExecutor.Object,
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions
                {
                    ModuleOutputFlushInterval = TimeSpan.FromMilliseconds(milliseconds),
                },
            }),
            Mock.Of<ILogger<PipelineOutputCoordinator>>());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await coordinator.InitializeAsync());
        await Assert.That(events).IsEmpty();
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
