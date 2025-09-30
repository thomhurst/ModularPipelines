using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

/// <summary>
/// Manages collapsible logging sections for different build systems and output targets.
/// Provides both buffered logging (via ModuleLogger) and direct console output.
/// </summary>
/// <remarks>
/// This is a singleton service that creates a single scoped ModuleLogger instance
/// and reuses it throughout the application lifetime, avoiding unnecessary scope creation.
/// </remarks>
/// <example>
/// <code>
/// // Start a collapsible log group (buffered)
/// logging.StartConsoleLogGroup("Build Step");
/// // ... perform work ...
/// logging.EndConsoleLogGroup("Build Step");
///
/// // Direct console output (bypasses buffer)
/// logging.StartConsoleLogGroupDirectToConsole("Setup", LogLevel.Information);
/// logging.LogToConsoleDirect("Direct message");
/// logging.EndConsoleLogGroupDirectToConsole("Setup", LogLevel.Information);
/// </code>
/// </example>
internal class SmartCollapsableLogging : ICollapsableLogging, IInternalCollapsableLogging, IScopeDisposer
{
    private readonly ISmartCollapsableLoggingStringBlockProvider _smartCollapsableLoggingStringBlockProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ILogger<SmartCollapsableLogging> _logger;
    private readonly List<IServiceScope> _scopes = [];
    private readonly IModuleLogger _moduleLogger;
    private readonly IConsoleWriter _moduleLoggerConsoleWriter;

    public SmartCollapsableLogging(IServiceProvider serviceProvider,
        ISmartCollapsableLoggingStringBlockProvider smartCollapsableLoggingStringBlockProvider,
        IConsoleWriter consoleWriter,
        ILogger<SmartCollapsableLogging> logger)
    {
        _smartCollapsableLoggingStringBlockProvider = smartCollapsableLoggingStringBlockProvider;
        _consoleWriter = consoleWriter;
        _logger = logger;

        // Create a single scope for the lifetime of this singleton
        var scope = serviceProvider.CreateScope();
        _scopes.Add(scope);
        _moduleLogger = scope.ServiceProvider.GetRequiredService<IModuleLoggerProvider>().GetLogger();
        _moduleLoggerConsoleWriter = (IConsoleWriter)_moduleLogger;
    }

    public void StartConsoleLogGroup(string name) => StartGroup(name, _moduleLoggerConsoleWriter);

    public void StartConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            StartGroup(name, _consoleWriter);
        }
    }

    public void EndConsoleLogGroup(string name) => EndGroup(name, _moduleLoggerConsoleWriter);

    public void EndConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            EndGroup(name, _consoleWriter);
        }
    }

    public void LogToConsole(string value) => _moduleLoggerConsoleWriter.LogToConsole(value);

    public void LogToConsoleDirect(string value) => _consoleWriter.LogToConsole(value);

    public IEnumerable<IServiceScope> GetScopes() => _scopes;

    private bool IsEnabled(LogLevel logLevel)
    {
        try
        {
            return _logger.IsEnabled(logLevel);
        }
        catch
        {
            return true;
        }
    }

    private void StartGroup(string name, IConsoleWriter writer)
    {
        var startCommand = _smartCollapsableLoggingStringBlockProvider.GetStartConsoleLogGroup(name);
        if (startCommand != null)
        {
            writer.LogToConsole(startCommand);
        }
    }

    private void EndGroup(string name, IConsoleWriter writer)
    {
        var endCommand = _smartCollapsableLoggingStringBlockProvider.GetEndConsoleLogGroup(name);
        if (endCommand != null)
        {
            writer.LogToConsole(endCommand);
        }
    }
}