﻿using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine.Executors;

internal class PrintModuleOutputExecutor : IPrintModuleOutputExecutor
{
    private readonly IModuleLoggerContainer _moduleLoggerContainer;

    public PrintModuleOutputExecutor(IModuleLoggerContainer moduleLoggerContainer)
    {
        _moduleLoggerContainer = moduleLoggerContainer;
    }
    
    [StackTraceHidden]
    public async Task ExecuteAndPrintModuleOutput(Func<Task> executeDelegate)
    {
        try
        {
            await executeDelegate();
        }
        finally
        {
            _moduleLoggerContainer.PrintAllLoggers();
        }
    }
}