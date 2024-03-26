using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

internal class SmartCollapsableLogging : ICollapsableLogging, IInternalCollapsableLogging
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly ISmartCollapsableLoggingStringBlockProvider _smartCollapsableLoggingStringBlockProvider;
    private readonly IConsoleWriter _consoleWriter;

    private IModuleLogger ModuleLogger => _moduleLoggerProvider.GetLogger();

    public SmartCollapsableLogging(IServiceProvider serviceProvider,
        ISmartCollapsableLoggingStringBlockProvider smartCollapsableLoggingStringBlockProvider,
        IConsoleWriter consoleWriter)
    {
        _moduleLoggerProvider = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IModuleLoggerProvider>();
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