using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

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
            
            return scope
                .ServiceProvider
                .GetRequiredService<IModuleLoggerProvider>()
                .GetLogger();
        }
    }

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

    public void StartConsoleLogGroup(string name)
    {
        StartGroup(name, ModuleLogger);
    }

    public void StartConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            StartGroup(name, _consoleWriter);
        }
    }

    public void EndConsoleLogGroup(string name)
    {
        EndGroup(name, ModuleLogger);
    }

    public void EndConsoleLogGroupDirectToConsole(string name, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            EndGroup(name, _consoleWriter);
        }
    }

    public void LogToConsole(string value)
    {
        LogToConsole(value, ModuleLogger);
    }

    public void LogToConsoleDirect(string value)
    {
        LogToConsole(value, _consoleWriter);
    }

    public IEnumerable<IServiceScope> GetScopes()
    {
        return _scopes;
    }

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
        var startConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetStartConsoleLogGroup(name);

        if (startConsoleLogGroup != null)
        {
            writer.LogToConsole(startConsoleLogGroup);
        }
    }

    private void EndGroup(string name, IConsoleWriter writer)
    {
        var endConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetEndConsoleLogGroup(name);

        if (endConsoleLogGroup != null)
        {
            writer.LogToConsole(endConsoleLogGroup);
        }
    }

    private void LogToConsole(string value, IConsoleWriter writer)
    {
        writer.LogToConsole(value);
    }
}