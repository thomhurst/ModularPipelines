using System.Runtime.CompilerServices;
using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Secrets;

namespace ModularPipelines.Logging;

internal sealed class BuildSystemLogIssueLoggerProvider : ILoggerProvider
{
    private static readonly string[] ExcludedCategoryPrefixes =
    [
        "Microsoft",
        "System",
    ];
    private static readonly object ReportedErrorMarker = new();

    private readonly IBuildSystemFormatter _formatter;
    private readonly IBuildSystemCommandWriter _commandWriter;
    private readonly Lazy<ISecretObfuscator> _secretObfuscator;
    private readonly Lazy<ISpectreConsoleLoggerControl?> _loggerControl;
    // Identity deduplicates propagated wrappers without merging independent failures that
    // share diagnostics. Weak keys let completed failures leave with their exception graph.
    private readonly ConditionalWeakTable<Exception, object> _reportedErrors = [];

    public BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatterProvider formatterProvider,
        IBuildSystemCommandWriter commandWriter,
        IServiceProvider serviceProvider)
        : this(formatterProvider.GetFormatter(), commandWriter,
            new Lazy<ISecretObfuscator>(serviceProvider.GetRequiredService<ISecretObfuscator>),
            new Lazy<ISpectreConsoleLoggerControl?>(serviceProvider.GetService<ISpectreConsoleLoggerControl>))
    {
    }

    internal BuildSystemLogIssueLoggerProvider(
        IBuildSystemFormatter formatter,
        IBuildSystemCommandWriter commandWriter,
        Lazy<ISecretObfuscator> secretObfuscator,
        Lazy<ISpectreConsoleLoggerControl?> loggerControl)
    {
        _formatter = formatter;
        _commandWriter = commandWriter;
        // SecretProvider needs ILoggerFactory, which constructs this provider. Resolve only when logging.
        _secretObfuscator = secretObfuscator;
        _loggerControl = loggerControl;
    }

    public ILogger CreateLogger(string categoryName) =>
        new BuildSystemLogIssueLogger(
            _formatter,
            _commandWriter,
            _secretObfuscator,
            _loggerControl,
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
        IBuildSystemFormatter buildSystemFormatter,
        IBuildSystemCommandWriter commandWriter,
        Lazy<ISecretObfuscator> secretObfuscator,
        Lazy<ISpectreConsoleLoggerControl?> loggerControl,
        ConditionalWeakTable<Exception, object> reportedErrors,
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
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel) || ModuleLogEvents.IsBuildIssueSuppressed(eventId))
            {
                return;
            }

            var message = FormatIssueMessage(logLevel, formatter(state, exception), exception);
            if (message is null)
            {
                return;
            }

            var command = buildSystemFormatter.GetLogIssueCommand(logLevel, secretObfuscator.Value.Obfuscate(message, null));
            if (command is null)
            {
                return;
            }

            // Module output groups render under this lock; a command written outside it
            // could land inside another module's group.
            lock (loggerControl.Value?.SynchronizationLock ?? commandWriter)
            {
                commandWriter.WriteLine(command);
            }
        }

        private string? FormatIssueMessage(
            LogLevel logLevel,
            string message,
            Exception? exception)
        {
            if (exception is null)
            {
                return message;
            }

            var rootException = exception.GetBaseException();
            var errorIdentity = rootException is IOriginalExceptionIdentity identity
                ? identity.OriginalException
                : rootException;
            if (logLevel is LogLevel.Error or LogLevel.Critical
                && !reportedErrors.TryAdd(errorIdentity, ReportedErrorMarker))
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(message)
                ? rootException.Message
                : $"{message}: {rootException.Message}";
        }
    }
}
