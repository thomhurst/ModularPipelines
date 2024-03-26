using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

internal class SmartCollapsableLogging : ICollapsableLogging, IInternalCollapsableLogging, IScopeDisposer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISmartCollapsableLoggingStringBlockProvider _smartCollapsableLoggingStringBlockProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly List<IServiceScope> _scopes = new();

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
        IConsoleWriter consoleWriter)
    {
        _serviceProvider = serviceProvider;
        _smartCollapsableLoggingStringBlockProvider = smartCollapsableLoggingStringBlockProvider;
        _consoleWriter = consoleWriter;
    }

    public void StartConsoleLogGroup(string name)
    {
        StartGroup(name, ModuleLogger);
    }

    public void StartConsoleLogGroupDirectToConsole(string name)
    {
        StartGroup(name, _consoleWriter);
    }

    public void EndConsoleLogGroup(string name)
    {
        EndGroup(name, ModuleLogger);
    }

    public void EndConsoleLogGroupDirectToConsole(string name)
    {
        EndGroup(name, _consoleWriter);
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