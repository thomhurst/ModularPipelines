using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

internal sealed class BuildSystemLogIssueLoggerProvider : ILoggerProvider
{
    private static readonly string[] ExcludedCategoryPrefixes =
    [
        "Microsoft",
        "System",
    ];

    private readonly IBuildSystemFormatter _formatter;
    private readonly IBuildSystemCommandWriter _commandWriter;
    private readonly ConcurrentDictionary<string, byte> _reportedErrors =
        new(StringComparer.Ordinal);

    public BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatterProvider formatterProvider,
        IBuildSystemCommandWriter commandWriter)
        : this(formatterProvider.GetFormatter(), commandWriter)
    {
    }

    internal BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatter formatter,
        IBuildSystemCommandWriter commandWriter)
    {
        _formatter = formatter;
        _commandWriter = commandWriter;
    }

    public ILogger CreateLogger(string categoryName) =>
        new BuildSystemLogIssueLogger(
            _formatter,
            _commandWriter,
            _reportedErrors,
            IsIssueCategory(categoryName));

    public void Dispose()
    {
    }

    private static bool IsIssueCategory(string categoryName) =>
        !ExcludedCategoryPrefixes.Any(prefix =>
            categoryName.Equals(prefix, StringComparison.Ordinal)
            || categoryName.StartsWith($"{prefix}.", StringComparison.Ordinal));

    private sealed class BuildSystemLogIssueLogger(
        IBuildSystemFormatter formatter,
        IBuildSystemCommandWriter commandWriter,
        ConcurrentDictionary<string, byte> reportedErrors,
        bool isIssueCategory) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) =>
            isIssueCategory
            && logLevel is LogLevel.Warning or LogLevel.Error or LogLevel.Critical;

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
            if (exception is not null)
            {
                var rootException = GetRootException(exception);
                if (logLevel is LogLevel.Error or LogLevel.Critical
                    && !reportedErrors.TryAdd(rootException.ToString(), 0))
                {
                    return;
                }

                message = string.IsNullOrWhiteSpace(message)
                    ? rootException.Message
                    : $"{message}: {rootException.Message}";
            }

            var command = formatter.GetLogIssueCommand(logLevel, message);
            if (command is not null)
            {
                commandWriter.WriteLine(command);
            }
        }

        private static Exception GetRootException(Exception exception)
        {
            while (exception.InnerException is not null)
            {
                exception = exception.InnerException;
            }

            return exception;
        }
    }
}
