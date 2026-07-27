using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using Spectre.Console;

namespace ModularPipelines.Logging;

internal sealed class BuildSystemLogIssueLoggerProvider : ILoggerProvider
{
    private static readonly string[] ExcludedCategoryPrefixes =
    [
        "Microsoft",
        "System",
    ];

    private readonly IBuildSystemFormatter _formatter;
    private readonly IAnsiConsole _console;

    public BuildSystemLogIssueLoggerProvider(IBuildSystemFormatterProvider formatterProvider)
        : this(formatterProvider.GetFormatter(), ModularPipelines.Console.DelegatingAnsiConsole.Instance)
    {
    }

    internal BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatter formatter,
        IAnsiConsole console)
    {
        _formatter = formatter;
        _console = console;
    }

    public ILogger CreateLogger(string categoryName) =>
        new BuildSystemLogIssueLogger(
            _formatter,
            _console,
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
        IAnsiConsole console,
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
                message = $"{message}{Environment.NewLine}{exception}";
            }

            var command = formatter.GetLogIssueCommand(logLevel, message);
            if (command is not null)
            {
                console.Profile.Out.Writer.WriteLine(command);
            }
        }
    }
}
