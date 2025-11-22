using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

/// <summary>
/// Manages collapsible logging sections for different build systems and output targets.
/// Provides both buffered logging (via ModuleLogger) and direct console output.
/// </summary>
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
    private readonly IServiceProvider _serviceProvider;
    private readonly ISmartCollapsableLoggingStringBlockProvider _smartCollapsableLoggingStringBlockProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ILogger<SmartCollapsableLogging> _logger;
    private readonly List<IServiceScope> _scopes = [];

    private IModuleLogger ModuleLogger
    {
        get
        {
            var scope = _serviceProvider.CreateScope();
            _scopes.Add(scope);
            return scope.ServiceProvider.GetRequiredService<IModuleLoggerProvider>().GetLogger();
        }
    }

    private IConsoleWriter ModuleLoggerConsoleWriter => (IConsoleWriter)ModuleLogger;

    public SmartCollapsableLogging(IServiceProvider serviceProvider,
        ISmartCollapsableLoggingStringBlockProvider smartCollapsableLoggingStringBlockProvider,
        IConsoleWriter consoleWriter,
        ILogger<SmartCollapsableLogging> logger)
    {
        _serviceProvider = serviceProvider;
        _smartCollapsableLoggingStringBlockProvider = smartCollapsableLoggingStringBlockProvider;
        _consoleWriter = consoleWriter;
        _logger = logger;
    }

    public void StartConsoleLogGroup(string name) => StartGroup(name, ModuleLoggerConsoleWriter);

    public void StartConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            StartGroup(name, _consoleWriter);
        }
    }

    public void EndConsoleLogGroup(string name) => EndGroup(name, ModuleLoggerConsoleWriter);

    public void EndConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            EndGroup(name, _consoleWriter);
        }
    }

    public void LogToConsole(string value) => ModuleLoggerConsoleWriter.LogToConsole(value);

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
