using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

internal class SmartCollapsableLogging : ICollapsableLogging, IInternalCollapsableLogging
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly ISmartCollapsableLoggingStringBlockProvider _smartCollapsableLoggingStringBlockProvider;

    private IModuleLogger ModuleLogger => _moduleLoggerProvider.GetLogger();
    
    public SmartCollapsableLogging(IModuleLoggerProvider moduleLoggerProvider,
        ISmartCollapsableLoggingStringBlockProvider smartCollapsableLoggingStringBlockProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _smartCollapsableLoggingStringBlockProvider = smartCollapsableLoggingStringBlockProvider;
    }
    
    public void LogToConsole(string value)
    {
        ModuleLogger.LogToConsole(value);
    }

    public void StartConsoleLogGroup(string name)
    {
        var startConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetStartConsoleLogGroup(name);
        
        if (startConsoleLogGroup != null)
        {
            ModuleLogger.LogToConsole(startConsoleLogGroup);
        }
    }

    public void EndConsoleLogGroup(string name)
    {
        var endConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetEndConsoleLogGroup(name);
        
        if (endConsoleLogGroup != null)
        {
            ModuleLogger.LogToConsole(endConsoleLogGroup);
        }
    }

    public void StartConsoleLogGroupInternal(string name)
    {
        var startConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetStartConsoleLogGroup(name);
        
        if (startConsoleLogGroup != null)
        {
            Console.WriteLine(startConsoleLogGroup);
        }
    }

    public void EndConsoleLogGroupInternal(string name)
    {
        var endConsoleLogGroup = _smartCollapsableLoggingStringBlockProvider.GetEndConsoleLogGroup(name);
        
        if (endConsoleLogGroup != null)
        {
            Console.WriteLine(endConsoleLogGroup);
        }
    }

    public void LogToConsoleInternal(string value)
    {
        Console.WriteLine(value);
    }
}