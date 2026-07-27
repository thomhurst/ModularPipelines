using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

internal sealed class BuildSystemLogIssueLoggerProvider : ILoggerProvider
{
    private readonly ILogger _logger;

    public BuildSystemLogIssueLoggerProvider(IBuildSystemFormatterProvider formatterProvider)
        : this(formatterProvider.GetFormatter(), () => System.Console.Out)
    {
    }

    internal BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatter formatter,
        Func<TextWriter> writerProvider)
    {
        _logger = new BuildSystemLogIssueLogger(formatter, writerProvider);
    }

    public ILogger CreateLogger(string categoryName) => _logger;

    public void Dispose()
    {
    }

    private sealed class BuildSystemLogIssueLogger(
        IBuildSystemFormatter formatter,
        Func<TextWriter> writerProvider) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel is LogLevel.Warning or LogLevel.Error or LogLevel.Critical;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> messageFormatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = messageFormatter(state, exception);
            var command = formatter.GetLogIssueCommand(logLevel, message);
            if (command is not null)
            {
                writerProvider().WriteLine(command);
            }
        }
    }
}
