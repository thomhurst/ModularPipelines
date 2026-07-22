using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleExecutorLoggingTests
{
    [Test]
    public async Task Successful_Pipeline_Does_Not_Log_Cancellation()
    {
        var executorLogger = new RecordingLogger<ModuleExecutor>();
        var stateTrackerLogger = new RecordingLogger<ModuleStateTracker>();
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddSingleton<ILogger<ModuleExecutor>>(executorLogger);
        builder.Services.AddSingleton<ILogger<ModuleStateTracker>>(stateTrackerLogger);

        await builder
            .AddModule<SuccessfulModule>()
            .ExecutePipelineAsync();

        var cancellationWasLogged = executorLogger.Messages
            .Concat(stateTrackerLogger.Messages)
            .Any(message => message.Contains("cancell", StringComparison.OrdinalIgnoreCase));

        await Assert.That(cancellationWasLogged).IsFalse();
    }

    [Test]
    public async Task Failed_Pipeline_Logs_Cancellation_Cause()
    {
        var logger = new RecordingLogger<ModuleExecutor>();
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddSingleton<ILogger<ModuleExecutor>>(logger);

        await Assert.That(() => builder
                .AddModule<FailingModule>()
                .ExecutePipelineAsync())
            .ThrowsException();

        var failureCancellationWasLogged = logger.Messages.Any(message =>
            message.Contains("Module failure triggered cancellation", StringComparison.Ordinal));

        await Assert.That(failureCancellationWasLogged).IsTrue();
    }

    private sealed class SuccessfulModule : Module
    {
        protected override Task ExecuteModuleAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FailingModule : Module
    {
        protected override Task ExecuteModuleAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Expected test failure");
        }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public ConcurrentQueue<string> Messages { get; } = new();

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Enqueue(formatter(state, exception));
        }
    }
}
