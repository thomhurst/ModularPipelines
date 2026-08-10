using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
using NReco.Logging.File;
using Spectre.Console;
using File = ModularPipelines.FileSystem.File;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Logging;

public class ModuleLoggerTests
{
    private static readonly string RandomString = Guid.NewGuid().ToString();
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ((IConsoleWriter) context.Logger).LogToConsole(RandomString);

            ((IConsoleWriter) context.Logger).LogToConsole(new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    public class Module2 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation(new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    public class Module3 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("{Value}", new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task LogToConsole_Does_Not_Write_To_File_Logger()
    {
        // This test verifies that LogToConsole output goes to console buffers,
        // NOT to file loggers. The console output itself is verified implicitly
        // through the full integration tests.
        var file = File.GetNewTemporaryFilePath();

        var host = await TestPipelineBuilder.Create()
            .ConfigureServices(collection =>
            {
                collection.AddLogging(builder => { builder.AddFile(file); });
            })
            .AddModule<Module1>()
            .BuildAsync();

        await host.RunAsync();

        await host.DisposeAsync();

        // The key behavior: LogToConsole output should NOT appear in file logs
        await Assert.That(await file.ReadAsync()).DoesNotContain(RandomString);
    }

    [Test]
    [Arguments(typeof(Module2))]
    [Arguments(typeof(Module3))]
    public async Task Can_Obfuscate_Secret(Type moduleType)
    {
        var file = File.GetNewTemporaryFilePath();

        var pipelineBuilder = TestPipelineBuilder.Create();
        var host = await pipelineBuilder
            .ConfigureServices(collection =>
            {
                collection.Configure<MySecrets>(pipelineBuilder.Configuration);
                collection.AddLogging(builder => { builder.AddFile(file); });
                collection.AddSingleton(typeof(IModule), moduleType);
            })
            .SetLogLevel(LogLevel.Information)
            .BuildAsync();

        await host.RunAsync();

        await host.DisposeAsync();

        await Assert.That(await file.ReadAsync()).DoesNotContain("Secret Value!!!");
        await Assert.That(await file.ReadAsync()).Contains("**********");
    }

    [Test]
    public async Task Disposed_Logger_Is_Not_Rooted_By_ProcessExit()
    {
        var loggerReference = CreateDisposedLoggerReference();

        for (var attempt = 0; attempt < 10 && loggerReference.IsAlive; attempt++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            await Task.Delay(10);
        }

        await Assert.That(loggerReference.IsAlive).IsFalse();
    }

    [Test]
    public async Task Log_BuffersRawStateAndOriginalFormatter()
    {
        IBufferedLogEvent? bufferedLogEvent = null;
        var moduleOutputBuffer = new Mock<IModuleOutputBuffer>();
        moduleOutputBuffer
            .Setup(x => x.AddLogEvent(It.IsAny<IBufferedLogEvent>()))
            .Callback<IBufferedLogEvent>(logEvent => bufferedLogEvent = logEvent);
        var consoleCoordinator = CreateConsoleCoordinator(moduleOutputBuffer.Object);
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        defaultLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
        var formattedValuesObfuscator = new Mock<IFormattedLogValuesObfuscator>();
        formattedValuesObfuscator
            .Setup(x => x.TryObfuscateValues(It.IsAny<object>()))
            .Returns("sanitized-state");
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value?.Replace("secret", "***") ?? string.Empty);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            secretObfuscator.Object,
            formattedValuesObfuscator.Object,
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>());
        var originalState = new TestLogState("secret");

        logger.Log(
            LogLevel.Information,
            default,
            originalState,
            null,
            static (state, _) => $"value:{state.Value}");
        var captureLogger = new CaptureLogger();
        bufferedLogEvent!.WriteTo(captureLogger);

        await Assert.That(captureLogger.State).IsEqualTo("sanitized-state");
        await Assert.That(captureLogger.Message).IsEqualTo("value:***");
    }

    [Test]
    public async Task Log_DoesNotSerializeStateObfuscation()
    {
        var bothLogsEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLogs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var enteredCount = 0;
        var buffer = new ModuleOutputBuffer(typeof(ModuleLoggerTests));
        var consoleCoordinator = CreateConsoleCoordinator(buffer);
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        defaultLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
        var formattedValuesObfuscator = new Mock<IFormattedLogValuesObfuscator>();
        formattedValuesObfuscator
            .Setup(x => x.TryObfuscateValues(It.IsAny<object>()))
            .Callback(() =>
            {
                if (Interlocked.Increment(ref enteredCount) == 2)
                {
                    bothLogsEntered.TrySetResult();
                }

                releaseLogs.Task.GetAwaiter().GetResult();
            })
            .Returns((object state) => state);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            Mock.Of<ISecretObfuscator>(),
            formattedValuesObfuscator.Object,
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>());
        var firstLogTask = Task.Factory.StartNew(
            () => logger.LogInformation("first"),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        var secondLogTask = Task.Factory.StartNew(
            () => logger.LogInformation("second"),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

        try
        {
            await bothLogsEntered.Task.WaitAsync(TimeSpan.FromSeconds(1));
        }
        finally
        {
            releaseLogs.TrySetResult();
        }

        await Task.WhenAll(firstLogTask, secondLogTask);
        await Assert.That(buffer.HasOutput).IsTrue();
    }

    [Test]
    public async Task Dispose_DoesNotWaitForInProgressStateObfuscation()
    {
        var logEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLog = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var buffer = new ModuleOutputBuffer(typeof(ModuleLoggerTests));
        var consoleCoordinator = CreateConsoleCoordinator(buffer);
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        defaultLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
        var formattedValuesObfuscator = new Mock<IFormattedLogValuesObfuscator>();
        formattedValuesObfuscator
            .Setup(x => x.TryObfuscateValues(It.IsAny<object>()))
            .Callback(() =>
            {
                logEntered.TrySetResult();
                releaseLog.Task.GetAwaiter().GetResult();
            })
            .Returns((object state) => state);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            Mock.Of<ISecretObfuscator>(),
            formattedValuesObfuscator.Object,
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>());
        var logTask = Task.Run(() => logger.LogInformation("message"));

        await logEntered.Task.WaitAsync(TimeSpan.FromSeconds(1));
        var disposeStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var disposeTask = Task.Run(() =>
        {
            disposeStarted.TrySetResult();
            logger.Dispose();
        });

        try
        {
            await disposeStarted.Task.WaitAsync(TimeSpan.FromSeconds(1));
            await disposeTask.WaitAsync(TimeSpan.FromSeconds(1));
        }
        finally
        {
            releaseLog.TrySetResult();
        }

        await Task.WhenAll(logTask, disposeTask);
        await Assert.That(buffer.HasOutput).IsFalse();
        await Assert.That(buffer.IsComplete).IsTrue();
    }

    [Test]
    public async Task DisposeAsync_LogsObfuscatedModuleFailureWhenBufferedOutputFlushFails()
    {
        var moduleOutputBuffer = Mock.Of<IModuleOutputBuffer>();
        var consoleCoordinator = CreateConsoleCoordinator(moduleOutputBuffer);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                moduleOutputBuffer,
                typeof(ModuleLoggerTests),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IOException("flush failed"));
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        var moduleException = new InvalidOperationException("module secret-value");
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value?.Replace("secret-value", "**********") ?? string.Empty);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            secretObfuscator.Object,
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            outputCoordinator.Object);
        logger.SetException(moduleException);

        await logger.DisposeAsync();

        defaultLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("buffered output could not be flushed", StringComparison.Ordinal)),
            It.Is<Exception?>(exception => IsObfuscatedCopyOf(exception, moduleException)),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task DisposeAsync_LogsIndependentCancellationAsFlushFailure()
    {
        var moduleOutputBuffer = Mock.Of<IModuleOutputBuffer>();
        var consoleCoordinator = CreateConsoleCoordinator(moduleOutputBuffer);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        var providerCancellation = new OperationCanceledException("provider cancelled");
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                moduleOutputBuffer,
                typeof(ModuleLoggerTests),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(providerCancellation);
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            Mock.Of<ISecretObfuscator>(),
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            outputCoordinator.Object);

        await logger.DisposeAsync();

        defaultLogger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("Failed to flush module output", StringComparison.Ordinal)),
            providerCancellation,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task DeferredFlushFailure_LogsObfuscatedModuleFailureAfterDisposal()
    {
        var moduleOutputBuffer = new ModuleOutputBuffer(typeof(ModuleLoggerTests));
        var consoleCoordinator = CreateConsoleCoordinator(moduleOutputBuffer);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                moduleOutputBuffer,
                typeof(ModuleLoggerTests),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var defaultLogger = new Mock<ILogger<ModuleLoggerTests>>();
        var moduleException = new InvalidOperationException("module secret-value");
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) =>
                value?.Replace("secret-value", "**********") ?? string.Empty);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            defaultLogger.Object,
            secretObfuscator.Object,
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            outputCoordinator.Object);
        logger.SetException(moduleException);

        await logger.DisposeAsync();
        moduleOutputBuffer.ReportDeferredFlushFailure(new IOException("deferred flush failed"));

        defaultLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("buffered output could not be flushed", StringComparison.Ordinal)),
            It.Is<Exception?>(exception => IsObfuscatedCopyOf(exception, moduleException)),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    private static bool IsObfuscatedCopyOf(Exception? exception, Exception originalException)
    {
        return exception is IOriginalExceptionIdentity identity
               && ReferenceEquals(identity.OriginalException, originalException)
               && exception.ToString().Contains("**********", StringComparison.Ordinal)
               && !exception.ToString().Contains("secret-value", StringComparison.Ordinal);
    }

    [Test]
    public async Task Write_ReusesClearedRenderer()
    {
        var renderedLines = new List<string>();
        var moduleOutputBuffer = new Mock<IModuleOutputBuffer>();
        moduleOutputBuffer
            .Setup(x => x.WriteLine(It.IsAny<string>()))
            .Callback<string>(renderedLines.Add);
        var consoleCoordinator = CreateConsoleCoordinator(moduleOutputBuffer.Object);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var logger = new ModuleLogger<ModuleLoggerTests>(
            Mock.Of<ILogger<ModuleLoggerTests>>(),
            secretObfuscator.Object,
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>());

        logger.Write(new Markup("first"));
        logger.Write(new Markup("second"));

        await Assert.That(renderedLines).Count().IsEqualTo(2);
        await Assert.That(renderedLines[1]).Contains("second");
        await Assert.That(renderedLines[1]).DoesNotContain("first");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateDisposedLoggerReference()
    {
        var moduleOutputBuffer = Mock.Of<IModuleOutputBuffer>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.GetModuleBuffer(typeof(ModuleLoggerTests)))
            .Returns(moduleOutputBuffer);

        var logger = new ModuleLogger<ModuleLoggerTests>(
            Mock.Of<ILogger<ModuleLoggerTests>>(),
            Mock.Of<ISecretObfuscator>(),
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            Mock.Of<IOutputCoordinator>());

        logger.Dispose();
        return new WeakReference(logger);
    }

    private static Mock<IConsoleCoordinator> CreateConsoleCoordinator(IModuleOutputBuffer moduleOutputBuffer)
    {
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.GetModuleBuffer(typeof(ModuleLoggerTests)))
            .Returns(moduleOutputBuffer);
        return consoleCoordinator;
    }

    private readonly record struct TestLogState(string Value);

    private sealed class CaptureLogger : ILogger
    {
        public object? State { get; private set; }

        public string? Message { get; private set; }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            State = state;
            Message = formatter(state, exception);
        }
    }

    internal class MySecrets
    {
        [SecretValue] public string? Value1 { get; init; } = "Secret Value!!!";
    }
}
